using AutoMapper;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using FavProducts.Command;
using FavProducts.Core.Command;
using FavProducts.Core.Configuration;
using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using FavProducts.Data;
using FavProducts.Data.Repository.MySQL;
using FavProducts.Rest.Extensions;
using FavProducts.Service;
using FavProducts.Service.Read;
using FavProducts.Service.Write;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using SwaggerOptions = FavProducts.Core.Configuration.SwaggerOptions;

namespace FavProducts.Rest
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = config.Build();

            Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        protected IConfigurationRoot Configuration { get; }
        protected ILogger Logger { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<DataContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("SQL.Write"));
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var jwtSettings = new JwtSettings();
            Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
            
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true,
                    };
                });

            services.AddAuthorization();
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "FavProducts API", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0] }
                };

                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                x.AddSecurityRequirement(security);
            });

            var container = new Container().WithDependencyInjectionAdapter(services);

            #region [ Configuration ]

            container.UseInstance(Configuration);

            var apiEndpoints = new ApiEndpoints();
            Configuration.GetSection(nameof(ApiEndpoints)).Bind(apiEndpoints);
            container.UseInstance(apiEndpoints);
            services.AddSingleton(apiEndpoints);

            var pagination = new Pagination();
            Configuration.GetSection(nameof(Pagination)).Bind(pagination);
            container.UseInstance(pagination);
            services.AddSingleton(pagination);

            var redisCacheSettings = new CacheSettings();
            Configuration.GetSection(nameof(CacheSettings)).Bind(redisCacheSettings);
            container.UseInstance(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (redisCacheSettings.Enabled)
            {
                services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
                services.AddSingleton<ICacheService, CacheService>();
            }

            #endregion

            #region [ Logging ]

            container.UseInstance(Logger);

            #endregion

            #region [ Command ]

            container.Register<IPersonDeleteCommand, PersonDeleteCommand>();
            container.Register<IPersonGetCommand, PersonGetCommand>();
            container.Register<IPersonListCommand, PersonListCommand>();
            container.Register<IPersonPostCommand, PersonPostCommand>();
            container.Register<IPersonPutCommand, PersonPutCommand>();

            container.Register<IProductPostCommand, ProductPostCommand>();
            container.Register<IProductListCommand, ProductListCommand>();

            #endregion

            #region [ Repository ]

            string provider = Configuration.GetConnectionString("SQL.Provider");
            string sqlRead = Configuration.GetConnectionString("SQL.Read");
            string sqlWrite = Configuration.GetConnectionString("SQL.Write");

            container.UseInstance<IPersonRepository>(new PersonRepository(sqlRead, provider), serviceKey: "Read");
            container.UseInstance<IPersonRepository>(new PersonRepository(sqlWrite, provider), serviceKey: "Write");

            container.UseInstance<IProductRepository>(new ProductRepository(sqlRead, provider), serviceKey: "Read");
            container.UseInstance<IProductRepository>(new ProductRepository(sqlWrite, provider), serviceKey: "Write");

            #endregion

            #region [ Service ]

            var clientConfiguration = new PerBaseUrlFlurlClientFactory().ConfigureClient(apiEndpoints.Product, (c) => c.Headers.Add("Accept", "application/json"));

            container.UseInstance<IFlurlClientFactory>(clientConfiguration);
            container.UseInstance<IFlurlClient>(container.Resolve<IFlurlClientFactory>().Get(apiEndpoints.Product), serviceKey: "FlurlClient.Product");

            container.Register<IPersonService, PersonService>();

            int pageSize = pagination.Size;

            container.Register<IPersonReadService, PersonReadService>(Made.Of(() => new PersonReadService(Arg.Of<IPersonRepository>("Read"), pageSize)));
            container.Register<IPersonWriteService, PersonWriteService>(Made.Of(() => new PersonWriteService(Arg.Of<IPersonRepository>("Write"))));

            container.Register<IProductService, ProductService>();

            int maxAttempts = apiEndpoints.MaxAttempts;

            container.Register<IProductReadService, ProductReadService>(Made.Of(() => new ProductReadService(Arg.Of<IProductRepository>("Read"), Arg.Of<IFlurlClient>("FlurlClient.Product"), maxAttempts, Arg.Of<ILogger>())));
            container.Register<IProductWriteService, ProductWriteService>(Made.Of(() => new ProductWriteService(Arg.Of<IProductRepository>("Write"))));

            container.Register<ICacheService, CacheService>(Made.Of(() => new CacheService(Arg.Of<IDistributedCache>(), Arg.Of<ILogger>())));

            container.Register<IUserService, UserService>();

            #endregion

            var serviceProvider = container.Resolve<IServiceProvider>();

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ConfigureCustomExceptionMiddleware();

            app.UseAuthentication();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseMvc();
        }
    }
}
