<<<<<<< HEAD
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolver
    {
        /// <summary>
        /// Tries to resolve current tenant using registered <see cref="ITenantResolveContributer"/> implementations.
        /// </summary>
        /// <returns>
        /// Tenant id, unique name or null (if could not resolve).
        /// </returns>
        [CanBeNull]
        string ResolveTenantIdOrName();
    }
=======
using JetBrains.Annotations;

namespace Volo.Abp.MultiTenancy
{
    public interface ITenantResolver
    {
        /// <summary>
        /// Tries to resolve current tenant using registered <see cref="ITenantResolveContributor"/> implementations.
        /// </summary>
        /// <returns>
        /// Tenant id, unique name or null (if could not resolve).
        /// </returns>
        [CanBeNull]
        string ResolveTenantIdOrName();
    }
>>>>>>> upstream/master
}