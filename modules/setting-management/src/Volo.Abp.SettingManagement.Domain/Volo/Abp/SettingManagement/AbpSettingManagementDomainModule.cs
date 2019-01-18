<<<<<<< HEAD
﻿using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(typeof(AbpSettingsModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpSettingManagementDomainSharedModule))]
    public class AbpSettingManagementDomainModule : AbpModule
    {
        
    }
}
=======
﻿using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    [DependsOn(
        typeof(AbpSettingsModule),
        typeof(AbpDddDomainModule),
        typeof(AbpSettingManagementDomainSharedModule), 
        typeof(AbpCachingModule)
        )]
    public class AbpSettingManagementDomainModule : AbpModule
    {
        
    }
}
>>>>>>> upstream/master
