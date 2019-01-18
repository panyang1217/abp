<<<<<<< HEAD
﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientRepository : EfCoreRepository<IIdentityServerDbContext, Client, Guid>, IClientRepository
    {
        public ClientRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public virtual async Task<Client> FindByCliendIdAsync(
            string clientId, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public override IQueryable<Client> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.IdentityServer.Clients
{
    public class ClientRepository : EfCoreRepository<IIdentityServerDbContext, Client, Guid>, IClientRepository
    {
        public ClientRepository(IDbContextProvider<IIdentityServerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public virtual async Task<Client> FindByCliendIdAsync(
            string clientId, 
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(x => x.ClientId == clientId, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Client>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails).OrderBy(sorting ?? nameof(Client.ClientName) + " desc")
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetTotalCount()
        {
            return await DbSet.CountAsync();
        }

        public override IQueryable<Client> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}
>>>>>>> upstream/master
