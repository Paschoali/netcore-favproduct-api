using FavProducts.Command;
using FavProducts.Core.Rest.Transport;
using FavProducts.Core.Services;
using FavProducts.Domain;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FavProducts.UnitTest.Command
{
    public class PersonListCommandTest
    {
        private readonly PersonListCommand _sutPersonListCommand;
        private readonly Mock<IPersonService> _mockPersonService;

        public PersonListCommandTest()
        {
            _mockPersonService = new Mock<IPersonService>();

            var mockLogger = new Mock<Serilog.ILogger>()
            {
                DefaultValue = DefaultValue.Mock
            };

            _sutPersonListCommand = new PersonListCommand(_mockPersonService.Object, mockLogger.Object);
        }

        public class Execute : PersonListCommandTest
        {
            protected IEnumerable<Person> PersonList;
            protected int PageSize;
            protected int PageNumber;
            protected PersonListRequest Request;
            protected IEnumerable<Person> Response;

            public Execute()
            {
                PersonList = new List<Person>();
                PageSize = 10;
                PageNumber = 1;

                _mockPersonService
                    .Setup(x => x.ListAsync(PageNumber))
                    .ReturnsAsync(PersonList);
            }
        }

        public class Given_An_Invalid_Request : Execute
        {
            [Fact]
            public async Task Should_Throw_Argument_Null_Exception()
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => _sutPersonListCommand.ListAsync(null));
            }
        }

        public class Given_A_Valid_Request : Execute
        {
            public Given_A_Valid_Request()
            {
                Request = new PersonListRequest
                {
                    Page = 1
                };
            }

            [Fact]
            public async Task Should_Return_Success_Response()
            {
                Response = await _sutPersonListCommand.ListAsync(Request.Page);
                Response.Should().NotBeNull();
            }
        }
    }
}