using FavProducts.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FavProducts.UnitTest.Command
{
    public class PersonGetCommandTest
    {
        private readonly PersonGetCommand _sutPersonGetCommand;
        private readonly Mock<IPersonService> _mockPersonService;

        public PersonGetCommandTest()
        {
            _mockPersonService = new Mock<IPersonService>();

            var mockLogger = new Mock<Serilog.ILogger>()
            {
                DefaultValue = DefaultValue.Mock
            };

            _sutPersonGetCommand = new PersonGetCommand(_mockPersonService.Object, mockLogger.Object);
        }

        public class Execute : PersonGetCommandTest
        {
            protected Person Person;
            protected Guid PersonId;
            protected Person Response;

            public Execute()
            {
                Person = new Person();
                PersonId = Guid.Parse("f2722f20-1478-4f30-ac6c-c148bb5c3dfb");

                _mockPersonService
                    .Setup(x => x.GetAsync(PersonId))
                    .ReturnsAsync(Person);
            }
        }

        public class Given_An_Invalid_Request : Execute
        {
            [Fact]
            public async Task Should_Throw_Argument_Null_Exception()
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _sutPersonGetCommand.ExecuteAsync(Guid.Empty));
            }
        }

        public class Given_A_Valid_Request : Execute
        {
            [Fact]
            public async Task Should_Return_Success_Response()
            {
                Response = await _sutPersonGetCommand.ExecuteAsync(PersonId);
                Response.Should().NotBeNull();
            }

            [Fact]
            public async Task Should_Return_Null_Response()
            {
                Response = await _sutPersonGetCommand.ExecuteAsync(Guid.NewGuid());
                Response.Should().BeNull();
            }
        }
    }
}