<<<<<<< HEAD
﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolveContributer> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributer>
            {
                new CurrentClaimsPrincipalTenantResolveContributer()
            };
        }
    }
=======
﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public class TenantResolveOptions
    {
        [NotNull]
        public List<ITenantResolveContributor> TenantResolvers { get; }

        public TenantResolveOptions()
        {
            TenantResolvers = new List<ITenantResolveContributor>
            {
                new CurrentClaimsPrincipalTenantResolveContributor()
            };
        }
    }
>>>>>>> upstream/master
}