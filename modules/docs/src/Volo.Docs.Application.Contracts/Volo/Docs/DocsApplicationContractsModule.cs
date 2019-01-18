<<<<<<< HEAD
﻿using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(typeof(DocsDomainSharedModule))]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
=======
﻿using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(
        typeof(DocsDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class DocsApplicationContractsModule : AbpModule
    {
        
    }
}
>>>>>>> upstream/master
