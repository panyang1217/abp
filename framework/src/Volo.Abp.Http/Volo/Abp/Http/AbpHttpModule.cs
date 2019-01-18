<<<<<<< HEAD
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpHttpAbstractionsModule))]
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpHttpModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpApiProxyScriptingOptions>(options =>
            {
                options.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);
            });
        }
    }
}
=======
﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.ProxyScripting.Configuration;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpHttpAbstractionsModule))]
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpHttpModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpApiProxyScriptingOptions>(options =>
            {
                options.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);
            });
        }
    }
}
>>>>>>> upstream/master
