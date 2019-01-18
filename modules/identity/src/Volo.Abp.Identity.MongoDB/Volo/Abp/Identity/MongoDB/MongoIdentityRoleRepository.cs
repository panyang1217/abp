<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Guids;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityRoleRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        private readonly IGuidGenerator _guidGenerator;

        public MongoIdentityRoleRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider, IGuidGenerator guidGenerator) 
            : base(dbContextProvider)
        {
            _guidGenerator = guidGenerator;
        }

        public async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityRole>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task UpdateClaimsAsync(Guid id, List<IdentityRoleClaim> claims)
        {
            var role = await GetAsync(id);

            role.Claims.Clear();

            foreach (var claim in claims)
            {
                role.Claims.Add(new IdentityRoleClaim(_guidGenerator.Create(), id, claim.ClaimType, claim.ClaimValue, CurrentTenant.Id));
            }
        }

        public async Task<List<IdentityRoleClaim>> GetClaimsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var role = await GetAsync(id, cancellationToken: GetCancellationToken(cancellationToken));
            return role.Claims.ToList();
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Guids;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityRoleRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        private readonly IGuidGenerator _guidGenerator;

        public MongoIdentityRoleRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider, IGuidGenerator guidGenerator) 
            : base(dbContextProvider)
        {
            _guidGenerator = guidGenerator;
        }

        public async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityRole>> GetListAsync(
            string sorting = null, 
            int maxResultCount = int.MaxValue, 
            int skipCount = 0, 
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
>>>>>>> upstream/master
}