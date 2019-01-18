<<<<<<< HEAD
## Repository Best Practices & Conventions

### Repository Interfaces

* **Do** define repository interfaces in the **domain layer**.
* **Do** define a repository interface (like `IIdentityUserRepository`) and create its corresponding implementations for **each aggregate root**.
  * **Do** always use the created repository interface from the application code.
  * **Do not** use generic repository interfaces (like `IRepository<IdentityUser, Guid>`) from the application code.
  * **Do not** use `IQueryable<TEntity>` features in the application code (domain, application... layers).

For the example aggregate root:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

Define the repository interface as below:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **Do not** inherit the repository interface from the `IRepository<TEntity, TKey>` interface. Because it inherits the `IQueryable` and the repository should not expose `IQueryable` to the application.
* **Do** inherit the repository interface from `IBasicRepository<TEntity, TKey>` (as normally) or a lower-featured interface, like `IReadOnlyRepository<TEntity, TKey>` (if it's needed).
* **Do not** define repositories for entities those are **not aggregate roots**.

### Repository Methods

* **Do** define all repository methods as **asynchronous**.
* **Do** add an **optional** `cancellationToken` parameter to every method of the repository. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    CancellationToken cancellationToken = default
);
````

* **Do** create a **synchronous extension** method for each asynchronous repository method. Example:

````C#
public static class IdentityUserRepositoryExtensions
{
    public static IdentityUser FindByNormalizedUserName(
        this IIdentityUserRepository repository,
        [NotNull] string normalizedUserName)
    {
        return AsyncHelper.RunSync(
            () => repository.FindByNormalizedUserNameAsync(normalizedUserName)
        );
    }
}
````

This will allow synchronous code to use the repository methods easier.

* **Do** add an optional `bool includeDetails = true` parameter (default value is `true`) for every repository method which returns a **single entity**. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    bool includeDetails = true,
    CancellationToken cancellationToken = default
);
````

This parameter will be implemented for ORMs to eager load sub collections of the entity.

* **Do** add an optional `bool includeDetails = false` parameter (default value is `false`) for every repository method which returns a **list of entities**. Example:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````

* **Do not** create composite classes to combine entities to get from repository with a single method call. Examples: *UserWithRoles*, *UserWithTokens*, *UserWithRolesAndTokens*. Instead, properly use `includeDetails` option to add all details of the entity when needed.
* **Avoid** to create projection classes for entities to get less property of an entity from the repository. Example: Avoid to create BasicUserView class to select a few properties needed for the use case needs. Instead, directly use the aggregate root class. However, there may be some exceptions for this rule, where:
  * Performance is so critical for the use case and getting the whole aggregate root highly impacts the performance.

### See Also

* [Entity Framework Core Integration](Entity-Framework-Core-Integration.md)
* [MongoDB Integration](MongoDB-Integration.md)
=======
## 仓储最佳实践 & 约定

### 仓储接口

* **推荐** 在**领域层**中定义仓储接口.
* **推荐** 为**每个聚合根**定义仓储接口(如 `IIdentityUserRepository`)并创建相应的实现.
  * **推荐** 在应用代码中使用仓储时应该注入仓储接口.
  * **不推荐** 在应用代码中使用泛型仓储接口(如 `IRepository<IdentityUser, Guid>`).
  * **不推荐** 在应用代码(领域, 应用... 层)中使用 `IQueryable<TEntity>` 特性.

聚合根的示例:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

定义仓储接口, 如下所示:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **不推荐** 仓储接口继承 `IRepository<TEntity, TKey>` 接口. 因为它继承了 `IQueryable` 而仓储不应该将`IQueryable`暴漏给应用.
* **推荐** 通常仓储接口继承自 `IBasicRepository<TEntity, TKey>` 或更低级别的接口, 如 `IReadOnlyRepository<TEntity, TKey>` (在需要的时候).
* **不推荐** 为实体定义仓储接口,因为它们**不是聚合根**.

### 仓储方法

* **推荐** 所有的仓储方法定义为 **异步**.
* **推荐** 为仓储的每个方法添加 **可选参数** `cancellationToken` . 例:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    CancellationToken cancellationToken = default
);
````

* **推荐** 为仓储的每个异步方法创建一个 **同步扩展** 方法. 示例:

````C#
public static class IdentityUserRepositoryExtensions
{
    public static IdentityUser FindByNormalizedUserName(
        this IIdentityUserRepository repository,
        [NotNull] string normalizedUserName)
    {
        return AsyncHelper.RunSync(
            () => repository.FindByNormalizedUserNameAsync(normalizedUserName)
        );
    }
}
````

对于同步方法而言, 这会让它们更方便的调用仓储方法.

* **推荐** 为仓储中返回**单个实体**的方法添加一个可选参数 `bool includeDetails = true`  (默认值为`true`). 示例:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    bool includeDetails = true,
    CancellationToken cancellationToken = default
);
````

该参数由ORM实现, 用来加载实体子集合.

* **推荐** 为仓储中返回**实体列表**的方法添加一个可选参数 `bool includeDetails = false` (默认值为`false`). 示例:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````

* **不推荐** 创建复合类通过调用仓储单个方法返回组合实体. 比如: *UserWithRoles*, *UserWithTokens*, *UserWithRolesAndTokens*. 相反, 正确的使用 `includeDetails` 选项, 在需要时加载实体所有的详细信息.
* **避免** 为了从仓储中获取实体的部分属性而为实体创建投影类. 比如: 避免通过创建BasicUserView来选择所需的一些属性. 相反可以直接使用聚合根类. 不过这条规则有例外情况:
  * 性能对于用例来说非常重要,而且使用整个聚合根对性能的影响非常大.

### 另外请参阅

* [Entity Framework Core 集成](Entity-Framework-Core-Integration.md)
* [MongoDB 集成](MongoDB-Integration.md)
>>>>>>> upstream/master
