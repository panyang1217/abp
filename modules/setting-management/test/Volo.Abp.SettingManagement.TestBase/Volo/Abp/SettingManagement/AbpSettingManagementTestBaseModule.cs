<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpSettingManagementDomainModule))]
    public class AbpSettingManagementTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<TestSettingDefinitionProvider>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<SettingTestDataBuilder>()
                    .Build();
            }
        }
    }
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpTestBaseModule),
        typeof(AbpSettingManagementDomainModule))]
    public class AbpSettingManagementTestBaseModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<SettingOptions>(options =>
            {
                options.DefinitionProviders.Add<TestSettingDefinitionProvider>();
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            SeedTestData(context);
        }

        private static void SeedTestData(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<SettingTestDataBuilder>()
                    .Build();
            }
        }
    }
>>>>>>> upstream/master
}