using System;

namespace FavProducts.Domain
{
    public abstract class Entity<T> : IEquatable<Entity<T>>
    {
        public T Id { get; set; }

        public bool Equals(Entity<T> other) => other != null && Id.Equals(other.Id);

        public bool IsEmpty => Id.Equals(default(T));
    }
}