<<<<<<< HEAD
## Multi-Tenancy

ABP Multi-tenancy module provides base functionality to create multi tenant applications. 

Wikipedia [defines](https://en.wikipedia.org/wiki/Multitenancy) multi-tenancy as like that:

> Software **Multi-tenancy** refers to a software **architecture** in which a **single instance** of a software runs on a server and serves **multiple tenants**. A tenant is a group of users who share a common access with specific privileges to the software instance. With a multitenant architecture, a software application is designed to provide every tenant a **dedicated share of the instance including its data**, configuration, user management, tenant individual functionality and non-functional properties. Multi-tenancy contrasts with multi-instance architectures, where separate software instances operate on behalf of different tenants.

### Volo.Abp.MultiTenancy.Abstractions Package

Volo.Abp.MultiTenancy.Abstractions package defines fundamental interfaces to make your code "multi-tenancy ready". So, install it to your project using the package manager console (PMC):

````
Install-Package Volo.Abp.MultiTenancy.Abstractions
````

> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

Then you can add **AbpMultiTenancyAbstractionsModule** dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> With the "Multi-tenancy ready" concept, we intent to develop our code to be compatible with multi-tenancy approach. Then it can be used in a multi-tenant application or not, depending on the requirements of the final application.

#### Define Entities

You can implement **IMultiTenant** interface for your entities to make them multi-tenancy ready. Example:

````C#
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class Product : AggregateRoot, IMultiTenant
    {
        public Guid? TenantId { get; set; } //IMultiTenant defines TenantId property

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

IMultiTenant requires to define a **TenantId** property in the implementing entity (See entity documentation (TODO: link) for more about entities).

#### Obtain Current Tenant's Id

Your code may require to get current tenant's id (regardless of how it's retrieved actually). You can [inject](Dependency-Injection.md) and use **ICurrentTenant** interface for such cases. Example:

````C#
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;

        public MyService(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public void DoIt()
        {
            var tenantId = _currentTenant.Id;
            //use tenantId in your code...
        }
    }
}
````

#### Change Current Tenant

TODO: ...

### Volo.Abp.MultiTenancy Package

Volo.Abp.MultiTenancy is the actual package that makes your application multi-tenant. Install it into your project using PMC:

````
Install-Package Volo.Abp.MultiTenancy
````

Then you can add **AbpMultiTenancyAbstractionsModule** dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> If you add AbpMultiTenancyModule dependency to your module, then you don't need to add AbpMultiTenancyAbstractionsModule dependency separately since AbpMultiTenancyModule already depends on it.

#### Determining Current Tenant

The first thing for a multi-tenant application is to determine the current tenant on the runtime. Volo.Abp.MultiTenancy package only provides abstractions (named as tenant resolver) for determining the current tenant, however it does not have any implementation out of the box.

**Volo.Abp.AspNetCore.MultiTenancy** package has implementation to determine the current tenant from current web request (from subdomain, header, cookie, route... etc.). See Volo.Abp.AspNetCore.MultiTenancy Package section later in this document.

##### Custom Tenant Resolvers

You can add your custom tenant resolver to **TenantResolveOptions** in your module's ConfigureServices method as like below:

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Add(new MyCustomTenantResolver());
            });

            //...
        }
    }
}
````

MyCustomTenantResolver must implement **ITenantResolver** as shown below:

````C#
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyCustomTenantResolver : ITenantResolver
    {
        public void Resolve(ITenantResolveContext context)
        {
            context.TenantIdOrName = ... //find tenant id or tenant name from somewhere...
        }
    }
}
````

A tenant resolver can set **TenantIdOrName** if it can determine it. If not, just leave it as is to allow next resolver to determine it.

#### Tenant Store

Volo.Abp.MultiTenancy package defines **ITenantStore** to abstract data source from the framework. You can implement ITenantStore to work with any data source (like a relational database) that stores information of your tenants.

...

##### Configuration Data Store

There is a built in (and default) tenant store, named ConfigurationTenantStore, that can be used to store tenants using standard [configuration system](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) (with [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) package). Thus, you can define tenants as hard coded or get from your appsettings.json file.

###### Example: Define tenants as hard-coded

````C#
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInformation(
                        Guid.Parse("446a5211-3d72-4339-9adc-845151f8ada0"), //Id
                        "tenant1" //Name
                    ),
                    new TenantInformation(
                        Guid.Parse("25388015-ef1c-4355-9c18-f6b6ddbaf89d"), //Id
                        "tenant2" //Name
                    )
                    {
                        //tenant2 has a seperated database
                        ConnectionStrings =
                        {
                            {ConnectionStrings.DefaultConnectionStringName, "..."}
                        }
                    }
                };
            });
        }
    }
}
````

###### Example: Define tenants in appsettings.json

First create your configuration from your appsettings.json file as you always do.

````C#
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = BuildConfiguration();

            context.Services.Configure<ConfigurationTenantStoreOptions>(configuration);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
````

Then add a "**Tenants**" section to your appsettings.json:

````json
"Tenants": [
    {
      "Id": "446a5211-3d72-4339-9adc-845151f8ada0",
      "Name": "tenant1"
    },
    {
      "Id": "25388015-ef1c-4355-9c18-f6b6ddbaf89d",
      "Name": "tenant2",
      "ConnectionStrings": {
        "Default": "...write tenant2's db connection string here..."
      }
    }
  ]
````

##### Volo.Abp... Package (TODO)

TODO: This package implements ITenantStore using a real database...

#### Tenant Information

ITenantStore works with **TenantInformation** class that has several properties for a tenant:

* **Id**: Unique Id of the tenant.
* **Name**: Unique name of the tenant.
* **ConnectionStrings**: If this tenant has dedicated database(s) to store it's data, then connection strings can provide database connection strings (it may have a default connection string and connection strings per modules - TODO: Add link to Abp.Data package document).

A multi-tenant application may require additional tenant properties, but these are the minimal requirements for the framework to work with multiple tenants.

#### Change Tenant By Code

TODO...

### Volo.Abp.AspNetCore.MultiTenancy Package

Volo.Abp.AspNetCore.MultiTenancy package integrate multi-tenancy to ASP.NET Core applications. To install it to your project, run the following command on PMC:

````
Install-Package Volo.Abp.AspNetCore.MultiTenancy
````

Then you can add **AbpAspNetCoreMultiTenancyModule** dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

#### Multi-Tenancy Middleware

Volo.Abp.AspNetCore.MultiTenancy package includes the multi-tenancy middleware...

````C#
app.UseMultiTenancy();
````

TODO:...

#### Determining Current Tenant From Web Request

Volo.Abp.AspNetCore.MultiTenancy package adds following tenant resolvers to determine current tenant from current web request (ordered by priority). These resolvers are added and work out of the box:

* **QueryStringTenantResolver**: Tries to find current tenant id from query string parameter. Parameter name is "__tenant" by default.
* **RouteTenantResolver**: Tries to find current tenant id from route (URL path). Variable name is "__tenant" by default. So, if you defined a route with this variable, then it can determine the current tenant from the route.
* **HeaderTenantResolver**: Tries to find current tenant id from HTTP header. Header name is "__tenant" by default.
* **CookieTenantResolver**: Tries to find current tenant id from cookie values. Cookie name is "__tenant" by default.

"__tenant" parameter name can be changed using AspNetCoreMultiTenancyOptions. Example:

````C#
services.Configure<AspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
````

##### Domain Tenant Resolver

In a real application, most of times you will want to determine current tenant either by subdomain (like mytenant1.mydomain.com) or by the whole domain (like mytenant.com). If so, you can configure TenantResolveOptions to add a domain tenant resolver.

###### Example: Add a subdomain resolver

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<TenantResolveOptions>(options =>
            {
                //Subdomain format: {0}.mydomain.com (adding as the highest priority resolver)
                options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));
            });

            //...
        }
    }
}
````

{0} is the the placeholder to determine current tenant's unique name.

Instead of ``options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));`` you can use this shortcut:

````C#
options.AddDomainTenantResolver("{0}.mydomain.com");
````

###### Example: Add a domain resolver

````C#
options.AddDomainTenantResolver("{0}.com");
````

=======
## 多租户

ABP的多租户模块提供了创建多租户应用程序的基本功能.

维基百科中是这样[定义](https://en.wikipedia.org/wiki/Multitenancy)多租户的:

> 软件多租户技术指的是一种软件架构,这种架构可以使用软件的单实例运行并为多个租户提供服务.租户是通过软件实例的特定权限共享通用访问的一组用户.使用多租户架构,软件应用为每个租户提供实例的专用共享,包括实例的数据、配置、用户管理、租户的私有功能和非功能属性.多租户与多实例架构形成对比,将软件实例的行为根据不同的租户分割开来.

### Volo.Abp.MultiTenancy.Abstractions

Volo.Abp.MultiTenancy.Abstractions定义了一些基础接口让你的代码"multi-tenancy ready",使用包管理器控制台(PMC)将它安装到你的项目中:

````
Install-Package Volo.Abp.MultiTenancy.Abstractions
````

> 这个包默认安装在了快速启动模板中.所以,大多数情况下,你不需要手动安装它.

然后你可以添加 **AbpMultiTenancyAbstractionsModule** 依赖到你的模块:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> 随着"Multi-tenancy ready"的概念,我们打算开发我们的代码和多租户方法兼容.然后它可以被用于多租户和非多租户的程序中,这取决于最终程序的需求.

#### 定义实体

你可以在你的实体中实现 **IMultiTenant** 接口来实现多租户,例如:

````C#
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class Product : AggregateRoot, IMultiTenant
    {
        public Guid? TenantId { get; set; } //IMultiTenant 定义了 TenantId 属性

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

实现IMultiTenant接口,需要在实体中定义一个 **TenantId** 的属性(查看更多有关[实体](Entities.md)的文档)

#### 获取当前租户的Id

你的代码中可能需要获取当前租户的Id(先不管它具体是怎么取得的).对于这种情况你可以[注入](Dependency-Injection.md)并使用 **ICurrentTenant** 接口.例如:

````C#
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;

        public MyService(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public void DoIt()
        {
            var tenantId = _currentTenant.Id;
            //在你的代码中使用tenantId
        }
    }
}
````

#### 改变当前租户

TODO: ...

### Volo.Abp.MultiTenancy

Volo.Abp.MultiTenancy 才是让你的程序实现多租户的真正的包.使用PMC将它安装到你的项目中:

````
Install-Package Volo.Abp.MultiTenancy
````

然后添加 **AbpMultiTenancyAbstractionsModule** 依赖到你的模块中:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> 如果你添加了AbpMultiTenancyModule依赖,就不需要再另外添加AbpMultiTenancyAbstractionsModule依赖了,因为AbpMultiTenancyModule已经依赖它了.

#### 确定当前租户

多租户的应用程序运行的时候首先要做的就是确定当前租户.
Volo.Abp.MultiTenancy只提供了用于确定当前租户的抽象(称为租户解析器),但是并没有现成的实现.

**Volo.Abp.AspNetCore.MultiTenancy**已经实现了从当前Web请求(从子域名,请求头,cookie,路由...等)中确定当前租户.本文后面会介绍Volo.Abp.AspNetCore.MultiTenancy.

##### 自定义租户解析器

你可以像下面这样,在你模块的ConfigureServices方法中将自定义解析器并添加到 **TenantResolveOptions**中:

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Add(new MyCustomTenantResolver());
            });

            //...
        }
    }
}
````

MyCustomTenantResolver必须像下面这样实现**ITenantResolver**接口:

````C#
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyCustomTenantResolver : ITenantResolver
    {
        public void Resolve(ITenantResolveContext context)
        {
            context.TenantIdOrName = ... //从其他地方获取租户id或租户名字...
        }
    }
}
````

如果能确定租户id或租户名字可以在租户解析器中设置 **TenantIdOrName**.如果不能确定,那就空着让下一个解析器来确定它.

#### 租户存储

Volo.Abp.MultiTenancy中定义了 **ITenantStore** 从框架中抽象数据源.你可以实现ITenantStore,让它跟任何存储你租户的数据源(例如关系型数据库)一起工作.


##### 配置数据存储

有一个内置的(默认的)租户存储,叫ConfigurationTenantStore.它可以被用于存储租户,通过标准的[配置系统](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)(使用[Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration)).因此,你可以通过硬编码或者在appsettings.json文件中定义租户.

###### 例子:硬编码定义租户

````C#
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInformation(
                        Guid.Parse("446a5211-3d72-4339-9adc-845151f8ada0"), //Id
                        "tenant1" //Name
                    ),
                    new TenantInformation(
                        Guid.Parse("25388015-ef1c-4355-9c18-f6b6ddbaf89d"), //Id
                        "tenant2" //Name
                    )
                    {
                        //tenant2 有单独的数据库连接字符串
                        ConnectionStrings =
                        {
                            {ConnectionStrings.DefaultConnectionStringName, "..."}
                        }
                    }
                };
            });
        }
    }
}
````

###### 例子:appsettings.json定义租户

首先从appsetting.json文件中创建你的配置.

````C#
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = BuildConfiguration();

            Configure<ConfigurationTenantStoreOptions>(configuration);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
````

然后在appsettings.json中添加 "**Tenants**" 节点:

````json
"Tenants": [
    {
      "Id": "446a5211-3d72-4339-9adc-845151f8ada0",
      "Name": "tenant1"
    },
    {
      "Id": "25388015-ef1c-4355-9c18-f6b6ddbaf89d",
      "Name": "tenant2",
      "ConnectionStrings": {
        "Default": "...write tenant2's db connection string here..."
      }
    }
  ]
````

##### Volo.Abp... Package (TODO)

TODO: This package implements ITenantStore using a real database...

#### 租户信息

ITenantStore跟 **TenantInformation**类一起工作,并且包含了几个租户属性:

* **Id**:租户的唯一Id.
* **Name**: 租户的唯一名称.
* **ConnectionStrings**:如果这个租户有专门的数据库来存储数据.它可以提供数据库的字符串(它可以具有默认的连接字符串和每个模块的连接字符串).


多租户应用程序可能需要其他租户属性,但这些属性是框架与多个租户一起使用的最低要求.

#### 代码中改变租户

TODO...

### Volo.Abp.AspNetCore.MultiTenancy

Volo.Abp.AspNetCore.MultiTenancy将多租户整合到了ASP.NET Core的程序中.在PMC中使用下面的代码将它安装到项目中.

````
Install-Package Volo.Abp.AspNetCore.MultiTenancy
````

然后添加 **AbpAspNetCoreMultiTenancyModule** 依赖到你的模块:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

#### 多租户中间件

Volo.Abp.AspNetCore.MultiTenancy包含了多租户中间件...

````C#
app.UseMultiTenancy();
````

TODO:...

#### 从Web请求中确定当前租户

Volo.Abp.AspNetCore.MultiTenancy 添加了下面这些租户解析器,从当前Web请求(按优先级排序)中确定当前租户.

* **QueryStringTenantResolver**: 尝试从query string参数中获取当前租户,默认参数名为"__tenant".
* **RouteTenantResolver**:尝试从当前路由中获取(URL路径),默认是变量名是"__tenant".所以,如果你的路由中定义了这个变量,就可以从路由中确定当前租户.
* **HeaderTenantResolver**: 尝试从HTTP header中获取当前租户,默认的header名称是"__tenant".
* **CookieTenantResolver**: 尝试从当前cookie中获取当前租户.默认的Cookie名称是"__tenant".

可以使用AspNetCoreMultiTenancyOptions修改默认的参数名"__tenant".例如:

````C#
services.Configure<AspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
````

##### 域名租户解析器

实际项目中,大多数情况下你想通过子域名(如mytenant1.mydomain.com)或全域名(如mytenant.com)中确定当前租户.如果是这样,你可以配置TenantResolveOptions添加一个域名租户解析器.

###### 例子:添加子域名解析器

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<TenantResolveOptions>(options =>
            {
                //子域名格式: {0}.mydomain.com (作为最高优先级解析器添加)
                options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));
            });

            //...
        }
    }
}
````

{0}是用来确定当前租户唯一名称的占位符.

你可以使用下面的方法,代替``options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));``:

````C#
options.AddDomainTenantResolver("{0}.mydomain.com");
````

###### 例子:添加全域名解析器

````C#
options.AddDomainTenantResolver("{0}.com");
````

>>>>>>> upstream/master
