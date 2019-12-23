using System;

namespace FavProducts.Domain
{
    public class Person : Entity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}