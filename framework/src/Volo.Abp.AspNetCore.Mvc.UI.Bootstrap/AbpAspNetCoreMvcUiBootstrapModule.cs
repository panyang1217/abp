<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiModule))]
    public class AbpAspNetCoreMvcUiBootstrapModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiBootstrapModule>("Volo.Abp.AspNetCore.Mvc.UI.Bootstrap");
            });
        }
    }
}
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiModule))]
    public class AbpAspNetCoreMvcUiBootstrapModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcUiBootstrapModule>("Volo.Abp.AspNetCore.Mvc.UI.Bootstrap");
            });
        }
    }
}
>>>>>>> upstream/master
