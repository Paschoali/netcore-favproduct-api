﻿using System;
using FavProducts.Core.Command;
using FavProducts.Core.Services;
using FavProducts.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace FavProducts.Command
{
    public class PersonListCommand : IPersonListCommand
    {
        private readonly IPersonService _personService;
        private readonly ILogger _logger;

        public PersonListCommand(IPersonService personService, ILogger logger)
        {
            _personService = personService;
            _logger = logger.ForContext<PersonListCommand>();
        }

        public async Task<IEnumerable<Person>> ListAsync(int? pageNumber)
        {
            if (pageNumber != null)
                return await _personService.ListAsync(pageNumber.Value);

            _logger.Error($"Page number cannot be null");
            throw new ArgumentNullException($"Page number cannot be null");
        }
    }
}