<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [DependsOn(typeof(AbpAuditLoggingDomainModule))]
    [DependsOn(typeof(AbpMongoDbModule))]
    public class AbpAuditLoggingMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AbpAuditLoggingBsonClassMap.Configure();

            context.Services.AddMongoDbContext<AuditLoggingMongoDbContext>(options =>
            {
                options.AddRepository<AuditLog, MongoAuditLogRepository>();
            });
        }
    }
}
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Volo.Abp.AuditLogging.MongoDB
{
    [DependsOn(typeof(AbpAuditLoggingDomainModule))]
    [DependsOn(typeof(AbpMongoDbModule))]
    public class AbpAuditLoggingMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<AuditLoggingMongoDbContext>(options =>
            {
                options.AddRepository<AuditLog, MongoAuditLogRepository>();
            });
        }
    }
}
>>>>>>> upstream/master
