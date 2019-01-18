<<<<<<< HEAD
﻿namespace Volo.Abp.Modularity
{
    public abstract class ModuleLifecycleContributerBase : IModuleLifecycleContributer
    {
        public virtual void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IAbpModule module)
        {
        }
    }
=======
﻿namespace Volo.Abp.Modularity
{
    public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
    {
        public virtual void Initialize(ApplicationInitializationContext context, IAbpModule module)
        {
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IAbpModule module)
        {
        }
    }
>>>>>>> upstream/master
}