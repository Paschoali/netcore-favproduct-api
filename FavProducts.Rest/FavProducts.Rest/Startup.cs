using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using FavProducts.Command;
using FavProducts.Core.Command;
using FavProducts.Core.Configuration;
using FavProducts.Core.Repository;
using FavProducts.Core.Services;
using FavProducts.Data.Repository.MySQL;
using FavProducts.Rest.Extensions;
using FavProducts.Service;
using FavProducts.Service.Read;
using FavProducts.Service.Write;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FavProductsAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var container = new Container().WithDependencyInjectionAdapter(services);

            #region [ Configuration ]

            container.UseInstance(Configuration);

            var apiEndpoints = new ApiEndpoints();
            Configuration.GetSection(nameof(ApiEndpoints)).Bind(apiEndpoints);
            container.UseInstance(apiEndpoints);

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

            container.Register<IPersonReadService, PersonReadService>(Made.Of(() => new PersonReadService(Arg.Of<IPersonRepository>("Read"))));
            container.Register<IPersonWriteService, PersonWriteService>(Made.Of(() => new PersonWriteService(Arg.Of<IPersonRepository>("Write"))));

            container.Register<IProductService, ProductService>();

            container.Register<IProductReadService, ProductReadService>(Made.Of(() => new ProductReadService(Arg.Of<IProductRepository>("Read"), Arg.Of<IFlurlClient>("FlurlClient.Product"), 3)));
            container.Register<IProductWriteService, ProductWriteService>(Made.Of(() => new ProductWriteService(Arg.Of<IProductRepository>("Write"))));

            container.Register<ICacheService, CacheService>(Made.Of(() => new CacheService(Arg.Of<IDistributedCache>())));

            #endregion

            var serviceProvider = container.Resolve<IServiceProvider>();

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.ConfigureCustomExceptionMiddleware();
            app.UseMvc();
        }
    }
}
