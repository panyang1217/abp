## Entity Framework Core Integration

This document explains how to integrate EF Core as an ORM provider to ABP based applications and how to configure it.

### Installation

`Volo.Abp.EntityFrameworkCore` is the main nuget package for the EF Core integration. Install it to your project (for a layered application, to your data/infrastructure layer):

````
Install-Package Volo.Abp.EntityFrameworkCore
````

Then add `AbpEntityFrameworkCoreModule` module dependency to your [module](Module-Development-Basics.md):

````C#
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

### Creating DbContext

You can create your DbContext as you normally do. It should be derived from `AbpDbContext<T>` as shown below:

````C#
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompany.MyProject
{
    public class MyDbContext : AbpDbContext<MyDbContext>
    {
        //...your DbSet properties

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
````

### Registering DbContext To Dependency Injection

Use `AddAbpDbContext` method in your module to register your DbContext class for [dependency injection](Dependency-Injection.md) system.

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyDbContext>();

            //...
        }
    }
}
````

#### Add Default Repositories

ABP can automatically create repositories (TODO: link) for the entities in your DbContext. Just use `AddDefaultRepositories()` option on registration:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

This will create a repository for each aggreate root entity (classes derived from AggregateRoot) by default. If you want to create repositories for other entities too, then set `includeAllEntities` to `true`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
````

Then you can inject and use `IRepository<TEntity>` or `IQueryableRepository<TEntity>` in your services.

#### Add Custom Repositories

TODO...

#### Set Base DbContext Class or Interface for Default Repositories

...

#### Replace Other Repository

...