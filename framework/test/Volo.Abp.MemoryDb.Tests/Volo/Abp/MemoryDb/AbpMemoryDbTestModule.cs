<<<<<<< HEAD
﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Data;
using Volo.Abp.Autofac;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(
        typeof(TestAppModule), 
        typeof(AbpMemoryDbModule), 
        typeof(AbpAutofacModule))]
    public class AbpMemoryDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connStr = Guid.NewGuid().ToString();

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });

            context.Services.AddMemoryDbContext<TestAppMemoryDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.AddRepository<City, CityRepository>();
            });
        }
    }
}
=======
﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.MemoryDb;
using Volo.Abp.Data;
using Volo.Abp.Autofac;
using Volo.Abp.TestApp;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(
        typeof(TestAppModule), 
        typeof(AbpMemoryDbModule), 
        typeof(AbpAutofacModule))]
    public class AbpMemoryDbTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var connStr = Guid.NewGuid().ToString();

            Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connStr;
            });

            context.Services.AddMemoryDbContext<TestAppMemoryDbContext>(options =>
            {
                options.AddDefaultRepositories();
                options.AddRepository<City, CityRepository>();
            });
        }
    }
}
>>>>>>> upstream/master
