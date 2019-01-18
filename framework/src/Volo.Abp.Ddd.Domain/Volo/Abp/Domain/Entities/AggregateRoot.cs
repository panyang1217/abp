<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, IAggregateRoot, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _domainEvents = new Collection<object>();

        protected virtual void AddDomainEvent(object eventData)
        {
            _domainEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IGeneratesDomainEvents
    {
        private readonly ICollection<object> _domainEvents = new Collection<object>();

        protected AggregateRoot()
        {
            
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {

        }

        protected virtual void AddDomainEvent(object eventData)
        {
            _domainEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Volo.Abp.Domain.Entities
{
    [Serializable]
    public abstract class AggregateRoot : Entity, 
        IAggregateRoot,
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ExtraProperties = new Dictionary<string, object>();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public virtual IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, 
        IAggregateRoot<TKey>, 
        IGeneratesDomainEvents, 
        IHasExtraProperties,
        IHasConcurrencyStamp
    {
        public Dictionary<string, object> ExtraProperties { get; protected set; }

        [DisableAuditing]
        public string ConcurrencyStamp { get; set; }

        private readonly ICollection<object> _localEvents = new Collection<object>();
        private readonly ICollection<object> _distributedEvents = new Collection<object>();

        protected AggregateRoot()
        {
            ExtraProperties = new Dictionary<string, object>();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
            ExtraProperties = new Dictionary<string, object>();
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual void AddLocalEvent(object eventData)
        {
            _localEvents.Add(eventData);
        }

        protected virtual void AddDistributedEvent(object eventData)
        {
            _distributedEvents.Add(eventData);
        }

        public virtual IEnumerable<object> GetLocalEvents()
        {
            return _localEvents;
        }

        public virtual IEnumerable<object> GetDistributedEvents()
        {
            return _distributedEvents;
        }

        public virtual void ClearLocalEvents()
        {
            _localEvents.Clear();
        }

        public virtual void ClearDistributedEvents()
        {
            _distributedEvents.Clear();
        }
    }
>>>>>>> upstream/master
}