<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityClaimTypeRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityClaimType, Guid>, IIdentityClaimTypeRepository
    {
        public MongoIdentityClaimTypeRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<bool> DoesNameExist(string name, Guid? claimTypeId = null)
        {
            return GetMongoQueryable().WhereIf(claimTypeId != null, ct => ct.Id != claimTypeId).Count(ct => ct.Name == name) > 0;
        }

        public async Task<List<IdentityClaimType>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            var identityClaimTypes = GetMongoQueryable().OrderBy(sorting ?? "name desc")
                .PageBy(skipCount, maxResultCount)
                .ToList();

            return identityClaimTypes;
        }

        public async Task<int> GetTotalCount()
        {
            return await GetMongoQueryable().CountAsync();
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityClaimTypeRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityClaimType, Guid>, IIdentityClaimTypeRepository
    {
        public MongoIdentityClaimTypeRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<bool> AnyAsync(string name, Guid? ignoredId = null)
        {
            if (ignoredId == null)
            {
                return await GetMongoQueryable()
                    .Where(ct => ct.Name == name)
                    .AnyAsync();
            }
            else
            {
                return await GetMongoQueryable()
                    .Where(ct => ct.Id != ignoredId && ct.Name == name)
                    .AnyAsync();
            }
        }

        public async Task<List<IdentityClaimType>> GetListAsync(string sorting, int maxResultCount, int skipCount)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(IdentityClaimType.Name))
                .As<IMongoQueryable<IdentityClaimType>>()
                .PageBy<IdentityClaimType, IMongoQueryable<IdentityClaimType>>(skipCount, maxResultCount)
                .ToListAsync();
        }
    }
}
>>>>>>> upstream/master
