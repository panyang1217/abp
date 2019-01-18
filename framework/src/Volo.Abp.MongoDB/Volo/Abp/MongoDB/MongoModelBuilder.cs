<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace Volo.Abp.MongoDB
{
    public class MongoModelBuilder : IMongoModelBuilder
    {
        private readonly Dictionary<Type, MongoEntityModelBuilder> _entityModelBuilders;

        public MongoModelBuilder()
        {
            _entityModelBuilders = new Dictionary<Type, MongoEntityModelBuilder>();
        }

        public MongoDbContextModel Build()
        {
            var entityModels = _entityModelBuilders
                .Select(x => x.Value)
                .ToImmutableDictionary(x => x.EntityType, x => (IMongoEntityModel) x);

            return new MongoDbContextModel(entityModels);
        }

        public virtual void Entity<TEntity>([NotNull] Action<MongoEntityModelBuilder> buildAction)
        {
            Entity(typeof(TEntity), buildAction);
        }

        public virtual void Entity([NotNull] Type entityType, [NotNull] Action<MongoEntityModelBuilder> buildAction)
        {
            Check.NotNull(entityType, nameof(entityType));
            Check.NotNull(buildAction, nameof(buildAction));

            var model = _entityModelBuilders.GetOrAdd(entityType, () => new MongoEntityModelBuilder(entityType));
            buildAction(model);
        }

        public virtual IReadOnlyList<MongoEntityModelBuilder> GetEntities()
        {
            return _entityModelBuilders.Values.ToImmutableList();
        }
    }
=======
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Volo.Abp.MongoDB
{
    public class MongoModelBuilder : IMongoModelBuilder
    {
        private readonly Dictionary<Type, object> _entityModelBuilders;

        private static readonly object SyncObj = new object();

        public MongoModelBuilder()
        {
            _entityModelBuilders = new Dictionary<Type, object>();
        }

        public MongoDbContextModel Build()
        {
            var entityModels = _entityModelBuilders
                .Select(x => x.Value)
                .Cast<IMongoEntityModel>()
                .ToImmutableDictionary(x => x.EntityType, x => x);

            foreach (var entityModel in entityModels.Values)
            {
                var map = entityModel.As<IHasBsonClassMap>().GetMap();
                lock (SyncObj)
                {
                    if (!BsonClassMap.IsClassMapRegistered(map.ClassType))
                    {
                        BsonClassMap.RegisterClassMap(map);
                    }
                }
            }

            return new MongoDbContextModel(entityModels);
        }

        public virtual void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null)
        {
            var model = (IMongoEntityModelBuilder<TEntity>)_entityModelBuilders.GetOrAdd(
                typeof(TEntity),
                () => new MongoEntityModelBuilder<TEntity>()
            );

            buildAction?.Invoke(model);
        }

        public virtual void Entity(Type entityType, Action<IMongoEntityModelBuilder> buildAction = null)
        {
            Check.NotNull(entityType, nameof(entityType));

            var model = (IMongoEntityModelBuilder)_entityModelBuilders.GetOrAdd(
                entityType,
                () => (IMongoEntityModelBuilder)Activator.CreateInstance(
                    typeof(MongoEntityModelBuilder<>).MakeGenericType(entityType)
                )
            );

            buildAction?.Invoke(model);
        }

        public virtual IReadOnlyList<IMongoEntityModel> GetEntities()
        {
            return _entityModelBuilders.Values.Cast<IMongoEntityModel>().ToImmutableList();
        }
    }
>>>>>>> upstream/master
}