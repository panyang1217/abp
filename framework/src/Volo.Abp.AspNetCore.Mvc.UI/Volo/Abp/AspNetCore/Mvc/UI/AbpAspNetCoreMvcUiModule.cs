﻿using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    [DependsOn(typeof(AbpUiNavigationModule))]
    public class AbpAspNetCoreMvcUiModule : AbpModule
    {

    }
}
