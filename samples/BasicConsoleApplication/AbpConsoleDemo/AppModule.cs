<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace AbpConsoleDemo
{
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AppModule>();
        }
    }
}
=======
﻿using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpConsoleDemo
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class AppModule : AbpModule
    {

    }
}
>>>>>>> upstream/master
