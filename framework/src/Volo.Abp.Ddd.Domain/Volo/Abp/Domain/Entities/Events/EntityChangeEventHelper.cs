<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public class EntityChangeEventHelper : IEntityChangeEventHelper, ITransientDependency
    {
        public IEventBus EventBus { get; set; }

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EntityChangeEventHelper(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            EventBus = NullEventBus.Instance;
        }

        public async Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            await TriggerEventsInternalAsync(changeReport);

            if (changeReport.IsEmpty() || _unitOfWorkManager.Current == null)
            {
                return;
            }

            await _unitOfWorkManager.Current.SaveChangesAsync();
        }

        public virtual async Task TriggerEntityCreatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityCreatingEventData<>), entity, true);
        }

        public async Task TriggerEntityCreatedEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityCreatedEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityCreatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityCreatedEventData<>), entity, false);
        }

        public virtual async Task TriggerEntityUpdatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityUpdatingEventData<>), entity, true);
        }

        public async Task TriggerEntityUpdatedEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityUpdatedEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityUpdatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityUpdatedEventData<>), entity, false);
        }

        public virtual async Task TriggerEntityDeletingEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityDeletingEventData<>), entity, true);
        }

        public async Task TriggerEntityDeletedEventAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityDeletedEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityDeletedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(typeof(EntityDeletedEventData<>), entity, false);
        }

        protected virtual async Task TriggerEventsInternalAsync(EntityChangeReport changeReport)
        {
            await TriggerEntityChangeEvents(changeReport.ChangedEntities);
            await TriggerDomainEvents(changeReport.DomainEvents);
        }

        protected virtual async Task TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (var changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        await TriggerEntityCreatingEventAsync(changedEntity.Entity);
                        await TriggerEntityCreatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        await TriggerEntityUpdatingEventAsync(changedEntity.Entity);
                        await TriggerEntityUpdatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        await TriggerEntityDeletingEventAsync(changedEntity.Entity);
                        await TriggerEntityDeletedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    default:
                        throw new AbpException("Unknown EntityChangeType: " + changedEntity.ChangeType);
                }
            }
        }

        protected virtual async Task TriggerDomainEvents(List<DomainEventEntry> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await EventBus.TriggerAsync(domainEvent.EventData.GetType(), domainEvent.EventData);
            }
        }

        protected virtual async Task TriggerEventWithEntity(Type genericEventType, object entity, bool triggerInCurrentUnitOfWork)
        {
            var entityType = ProxyHelper.UnProxy(entity).GetType();
            var eventType = genericEventType.MakeGenericType(entityType);

            if (triggerInCurrentUnitOfWork || _unitOfWorkManager.Current == null)
            {
                await EventBus.TriggerAsync(eventType, Activator.CreateInstance(eventType, entity));
                return;
            }

            _unitOfWorkManager.Current.OnCompleted(() => EventBus.TriggerAsync(eventType, Activator.CreateInstance(eventType, entity)));
        }
    }
=======
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public class EntityChangeEventHelper : IEntityChangeEventHelper, ITransientDependency
    {
        public ILocalEventBus LocalEventBus { get; set; }
        public IDistributedEventBus DistributedEventBus { get; set; }

        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IEntityToEtoMapper EntityToEtoMapper { get; }

        public EntityChangeEventHelper(
            IUnitOfWorkManager unitOfWorkManager,
            IEntityToEtoMapper entityToEtoMapper)
        {
            UnitOfWorkManager = unitOfWorkManager;
            EntityToEtoMapper = entityToEtoMapper;

            LocalEventBus = NullLocalEventBus.Instance;
            DistributedEventBus = NullDistributedEventBus.Instance;
        }

        public async Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            await TriggerEventsInternalAsync(changeReport);

            if (changeReport.IsEmpty() || UnitOfWorkManager.Current == null)
            {
                return;
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public virtual async Task TriggerEntityCreatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityCreatingEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityCreatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityCreatedEventData<>), entity, false);
            await TriggerEventWithEntity(DistributedEventBus, typeof(EntityCreatedEto<>), EntityToEtoMapper.Map(entity), false);
        }

        public virtual async Task TriggerEntityUpdatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityUpdatingEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityUpdatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityUpdatedEventData<>), entity, false);
            await TriggerEventWithEntity(DistributedEventBus, typeof(EntityUpdatedEto<>), EntityToEtoMapper.Map(entity), false);
        }

        public virtual async Task TriggerEntityDeletingEventAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityDeletingEventData<>), entity, true);
        }

        public virtual async Task TriggerEntityDeletedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(LocalEventBus, typeof(EntityDeletedEventData<>), entity, false);
            await TriggerEventWithEntity(DistributedEventBus, typeof(EntityDeletedEto<>), EntityToEtoMapper.Map(entity), false);
        }

        protected virtual async Task TriggerEventsInternalAsync(EntityChangeReport changeReport)
        {
            await TriggerEntityChangeEvents(changeReport.ChangedEntities);
            await TriggerLocalEvents(changeReport.DomainEvents);
            await TriggerDistributedEvents(changeReport.DistributedEvents);
        }

        protected virtual async Task TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (var changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        await TriggerEntityCreatingEventAsync(changedEntity.Entity);
                        await TriggerEntityCreatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        await TriggerEntityUpdatingEventAsync(changedEntity.Entity);
                        await TriggerEntityUpdatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        await TriggerEntityDeletingEventAsync(changedEntity.Entity);
                        await TriggerEntityDeletedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    default:
                        throw new AbpException("Unknown EntityChangeType: " + changedEntity.ChangeType);
                }
            }
        }

        protected virtual async Task TriggerLocalEvents(List<DomainEventEntry> localEvents)
        {
            foreach (var localEvent in localEvents)
            {
                await LocalEventBus.PublishAsync(localEvent.EventData.GetType(), localEvent.EventData);
            }
        }

        protected virtual async Task TriggerDistributedEvents(List<DomainEventEntry> distributedEvents)
        {
            foreach (var distributedEvent in distributedEvents)
            {
                await DistributedEventBus.PublishAsync(distributedEvent.EventData.GetType(), distributedEvent.EventData);
            }
        }

        protected virtual async Task TriggerEventWithEntity(IEventBus eventPublisher, Type genericEventType, object entity, bool triggerInCurrentUnitOfWork)
        {
            var entityType = ProxyHelper.UnProxy(entity).GetType();
            var eventType = genericEventType.MakeGenericType(entityType);

            if (triggerInCurrentUnitOfWork || UnitOfWorkManager.Current == null)
            {
                await eventPublisher.PublishAsync(eventType, Activator.CreateInstance(eventType, entity));
                return;
            }

            UnitOfWorkManager.Current.OnCompleted(() => eventPublisher.PublishAsync(eventType, Activator.CreateInstance(eventType, entity)));
        }
    }
>>>>>>> upstream/master
}