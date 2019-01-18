<<<<<<< HEAD
﻿using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule))]
    public class DocsHttpApiModule : AbpModule
    {
        
    }
}
=======
﻿using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
        )]
    public class DocsHttpApiModule : AbpModule
    {
        
    }
}
>>>>>>> upstream/master
