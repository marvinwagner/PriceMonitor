using FluentValidation;
using FluentValidation.Results;
using PriceMonitor.Core.Messages;
using System;
using System.Collections.Generic;

namespace PriceMonitor.Core.Domain
{
    public abstract class Entity<T> where T : class
    {
        public Guid Id { get; private set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected ValidationResult Validate(T entity, AbstractValidator<T> validator)
            => validator.Validate(entity);

        public void AddEvent(Event @event)
        {
            _notifications ??= new List<Event>();
            _notifications.Add(@event);
        }

        public void RemoveEvent(Event @event)
        {
            _notifications?.Remove(@event);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }

        public virtual bool IsValid()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() * 907 + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
    }
}
