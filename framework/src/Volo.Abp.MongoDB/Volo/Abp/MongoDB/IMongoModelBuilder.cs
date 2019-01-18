<<<<<<< HEAD
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MongoDB
{
    public interface IMongoModelBuilder
    {
        void Entity<TEntity>([NotNull] Action<MongoEntityModelBuilder> buildAction);

        void Entity([NotNull] Type entityType, [NotNull] Action<MongoEntityModelBuilder> buildAction);

        IReadOnlyList<MongoEntityModelBuilder> GetEntities();
    }
=======
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MongoDB
{
    public interface IMongoModelBuilder
    {
        void Entity<TEntity>(Action<IMongoEntityModelBuilder<TEntity>> buildAction = null);

        void Entity([NotNull] Type entityType, Action<IMongoEntityModelBuilder> buildAction = null);

        IReadOnlyList<IMongoEntityModel> GetEntities();
    }
>>>>>>> upstream/master
}