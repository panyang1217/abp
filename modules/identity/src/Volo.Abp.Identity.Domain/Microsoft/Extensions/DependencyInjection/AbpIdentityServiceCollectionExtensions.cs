<<<<<<< HEAD
﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddAbpIdentity(this IServiceCollection services)
        {
            return services.AddAbpIdentity(setupAction: null);
        }

        public static IdentityBuilder AddAbpIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            //AbpRoleManager
            services.TryAddScoped<IdentityRoleManager>();
            services.TryAddScoped(typeof(RoleManager<IdentityRole>), provider => provider.GetService(typeof(IdentityRoleManager)));

            //AbpUserManager
            services.TryAddScoped<IdentityUserManager>();
            services.TryAddScoped(typeof(UserManager<IdentityUser>), provider => provider.GetService(typeof(IdentityUserManager)));

            //AbpSecurityStampValidator
            services.TryAddScoped<AbpSecurityStampValidator>();
            services.TryAddScoped(typeof(SecurityStampValidator<IdentityUser>), provider => provider.GetService(typeof(AbpSecurityStampValidator)));
            services.TryAddScoped(typeof(ISecurityStampValidator), provider => provider.GetService(typeof(AbpSecurityStampValidator)));

            //AbpUserStore
            services.TryAddScoped<IdentityUserStore>();
            services.TryAddScoped(typeof(IUserStore<IdentityUser>), provider => provider.GetService(typeof(IdentityUserStore)));

            //AbpRoleStore
            services.TryAddScoped<IdentityRoleStore>();
            services.TryAddScoped(typeof(IRoleStore<IdentityRole>), provider => provider.GetService(typeof(IdentityRoleStore)));
            
            return services.AddIdentity<IdentityUser, IdentityRole>(setupAction)
                .AddDefaultTokenProviders()
                .AddClaimsPrincipalFactory<AbpUserClaimsPrincipalFactory>();
            //return services.AddIdentityCore<IdentityUser>(setupAction);
        }
    }
}
=======
﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddAbpIdentity(this IServiceCollection services)
        {
            return services.AddAbpIdentity(setupAction: null);
        }

        public static IdentityBuilder AddAbpIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            //AbpRoleManager
            services.TryAddScoped<IdentityRoleManager>();
            services.TryAddScoped(typeof(RoleManager<IdentityRole>), provider => provider.GetService(typeof(IdentityRoleManager)));

            //AbpUserManager
            services.TryAddScoped<IdentityUserManager>();
            services.TryAddScoped(typeof(UserManager<IdentityUser>), provider => provider.GetService(typeof(IdentityUserManager)));

            //AbpUserStore
            services.TryAddScoped<IdentityUserStore>();
            services.TryAddScoped(typeof(IUserStore<IdentityUser>), provider => provider.GetService(typeof(IdentityUserStore)));

            //AbpRoleStore
            services.TryAddScoped<IdentityRoleStore>();
            services.TryAddScoped(typeof(IRoleStore<IdentityRole>), provider => provider.GetService(typeof(IdentityRoleStore)));
            
            return services
                .AddIdentityCore<IdentityUser>(setupAction)
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<AbpUserClaimsPrincipalFactory>();
        }
    }
}
>>>>>>> upstream/master
