<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.DynamicProxying;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpAspNetCoreMvcTestModule), typeof(AbpHttpClientModule))]
    public class AbpHttpTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(TestAppModule).Assembly);
            context.Services.AddHttpClientProxy<IRegularTestController>();

            context.Services.Configure<RemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });
        }
    }
}
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.DynamicProxying;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpAspNetCoreMvcTestModule), typeof(AbpHttpClientModule))]
    public class AbpHttpTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(typeof(TestAppModule).Assembly);
            context.Services.AddHttpClientProxy<IRegularTestController>();

            Configure<RemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });
        }
    }
}
>>>>>>> upstream/master
