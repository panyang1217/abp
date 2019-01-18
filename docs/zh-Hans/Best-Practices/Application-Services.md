<<<<<<< HEAD
﻿## Application Services Best Practices & Conventions

* **Do** create an application service for each **aggregate root**.

### Application Service Interface

* **Do** define an `interface` for each application service in the **application contracts** package.
* **Do** inherit from the `IApplicationService` interface.
* **Do** use the `AppService` postfix for the interface name (ex: `IProductAppService`).
* **Do** create DTOs (Data Transfer Objects) for inputs and outputs of the service.
* **Do not** get/return entities for the service methods.
* **Do** define DTOs based on the [DTO best practices](Data-Transfer-Objects.md).

#### Outputs

* **Avoid** to define too many output DTOs for same or related entities. Instead, define a **basic** and a **detailed** DTO for an entity.

##### Basic DTO

**Do** define a **basic** DTO for an entity.

- Include all the **primitive properties** directly on the entity.
  - Exception: Can **exclude** properties for **security** reasons (like User.Password).
- Include all the **sub collections** of the entity where every item in the collection is a simple **relation DTO**.

Example:

```c#
public class IssueDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Guid? MilestoneId { get; set; }
    public Collection<IssueLabelDto> Labels { get; set; }
}

public class IssueLabelDto
{
    public Guid IssueId { get; set; }
    public Guid LabelId { get; set; }
}
```

##### Detailed DTO

**Do** define a **detailed** DTO for an entity if it has reference(s) to other aggregate roots.

* Include all the **primitive properties** directly on the entity.
  - Exception-1: Can **exclude** properties for **security** reasons (like `User.Password`).
  - Exception-2: **Do** exclude reference properties (like `MilestoneId` in the example above). Will already add details for the reference properties.
* Include a **basic DTO** property for every reference property.
* Include all the **sub collections** of the entity where every item in the collection is the **basic DTO** of the related entity.

Example:

````C#
public class IssueWithDetailsDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public MilestoneDto Milestone { get; set; }
    public Collection<LabelDto> Labels { get; set; }
}

public class MilestoneDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public bool IsClosed { get; set; }
}

public class LabelDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Color { get; set; }
}
````

#### Inputs

* **Do not** define any property in an input DTO that is not used in the service class.
* **Do not** share input DTOs between application service methods.
* **Do not** inherit an input DTO class from another one.
  * **May** inherit from an abstract base DTO class and share some properties between different DTOs in that way. However, should be very careful in that case because manipulating the base DTO would effect all related DTOs and service methods. Avoid from that as a good practice.

#### Methods

* **Do** define service methods as asynchronous with **Async** postfix.
* **Do not** repeat the entity name in the method names.
  * Example: Define `GetAsync(...)` instead of `GetProductAsync(...)` in the `IProductAppService`.

##### Getting A Single Entity

* **Do** use the `GetAsync` **method name**.
* **Do** get Id with a **primitive** method parameter.
* Return the **detailed DTO**. Example:

````C#
Task<QuestionWithDetailsDto> GetAsync(Guid id);
````

##### Getting A List Of Entities

* **Do** use the `GetListAsync` **method name**.
* **Do** get a single DTO argument for **filtering**, **sorting** and **paging** if necessary.
  * **Do** implement filters optional where possible.
  * **Do** implement sorting & paging properties as optional and provide default values.
  * **Do** limit maximum page size (for performance reasons).
* **Do** return a list of **detailed DTO**s. Example:

````C#
Task<List<QuestionWithDetailsDto>> GetListAsync(QuestionListQueryDto queryDto);
````

##### Creating A New Entity

* **Do** use the `CreateAsync` **method name**.
* **Do** get a **specialized input** DTO to create the entity.
* **Do** use **data annotations** for input validation.
  * Share constants between domain wherever possible (via constants defined in the **domain shared** package).
* **Do** return **the detailed** DTO for new created entity.
* **Do** only require the **minimum** info to create the entity but provide possibility to set others as optional properties.

Example **method**:

````C#
Task<QuestionWithDetailsDto> CreateAsync(CreateQuestionDto questionDto);
````

The related **DTO**:

````C#
public class CreateQuestionDto
{
    [Required]
    [StringLength(QuestionConsts.MaxTitleLength, MinimumLength = QuestionConsts.MinTitleLength)]
    public string Title { get; set; }
    
    [StringLength(QuestionConsts.MaxTextLength)]
    public string Text { get; set; } //Optional
    
    public Guid? CategoryId { get; set; } //Optional
}
````

##### Updating An Existing Entity

- **Do** use the `UpdateAsync` **method name**.
- **Do** get a **specialized input** DTO to update the entity.
- **Do** get the Id of the entity as a separated primitive parameter. Do not include to the update DTO.
- **Do** use **data annotations** for input validation.
  - Share constants between domain wherever possible (via constants defined in the **domain shared** package).
- **Do** return **the detailed** DTO for the updated entity.

Example:

````C#
Task<QuestionWithDetailsDto> UpdateAsync(Guid id, UpdateQuestionDto updateQuestionDto);
````

##### Deleting An Existing Entity

- **Do** use the `DeleteAsync` **method name**.
- **Do** get Id with a **primitive** method parameter. Example:

````C#
Task DeleteAsync(Guid id);
````

##### Other Methods

* **Can** define additional methods to perform operations on the entity. Example:

````C#
Task<int> VoteAsync(Guid id, VoteType type);
````

This method votes a question and returns the current score of the question.

### Application Service Implementation

* **Do** develop the application layer **completely independent from the web layer**.
* **Do** implement application service interfaces in the **application layer**.
  * **Do** use the naming convention. Ex: Create `ProductAppService` class for the `IProductAppService` interface.
  * **Do** inherit from the `ApplicationService` base class.

#### Using Repositories

* **Do** use the specifically designed repositories (like `IProductRepository`).
* **Do not** use generic repositories (like `IRepository<Product>`).

#### Querying Data

* **Do not** use LINQ/SQL for querying data from database inside the application service methods. It's repository's responsibility to perform LINQ/SQL queries from the data source.

#### Manipulating / Deleting Entities

* **Do** always get all the related entities from repositories to perform the operations on them.

#### Using Other Application Services

* **Do not** use other application services of the same module/application. Instead;
  * Use domain layer to perform the required task.
  * Extract a new class and share between the application services to accomplish the code reuse when necessary.
* **Can** use application services of others only if;
  * They are parts of another module / microservice.
  * The current module has only reference to the application contracts of the used module.



=======
﻿## 应用服务最佳实践 & 约定

* **推荐** 为每个 **聚合根** 创建一个应用服务.

### 应用服务接口

* **推荐** 在 **application.contracts**层中为每一个应用服务定义一个`接口`.
* **推荐** 继承 `IApplicationService` 接口 .
* **推荐** 接口名称使用`AppService` 后缀 (如: `IProductAppService`).
* **推荐** 为服务创建输入输出DTO(数据传输对象).
* **不推荐** 服务中含有返回实体的方法.
* **推荐** 根据[DTO 最佳实践](Data-Transfer-Objects.md)定义DTO.

#### 输出

* **避免** 为相同或相关实体定义过多的输出DTO. 为实体定义 **基础** 和 **详细** DTO.

##### 基础DTO

**推荐** 为实体定义一个**基础**DTO.

- 直接包含实体中所有的**原始属性**.
  - 例外: 出于**安全**原因,可以**排除**某些属性(像 `User.Password`).
- 包含实体中所有**子集合**, 每个集合项都是一个简单的**关系DTO**.

示例:

```c#
[Serializable]
public class IssueDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public Guid? MilestoneId { get; set; }
    public Collection<IssueLabelDto> Labels { get; set; }
}

[Serializable]
public class IssueLabelDto
{
    public Guid IssueId { get; set; }
    public Guid LabelId { get; set; }
}
```

##### 详细DTO

**Do** 如果实体持有对其他聚合根的引用,那么应该为其定义**详细**DTO.

* 直接包含实体中所有的 **原始属性**.
  - 例外-1: 出于**安全**原因,可以**排除**某些属性(像 `User.Password`).
  - 例外-2: **推荐** 排除引用属性(如上例中的 `MilestoneId`). 为其添加引用属性的详细信息.
* 为每个引用属性添加其**基本DTO** .
* 包含实体的**所有子集合**, 集合中的每项都是相关实体的基本DTO.

示例:

````C#
[Serializable]
public class IssueWithDetailsDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public MilestoneDto Milestone { get; set; }
    public Collection<LabelDto> Labels { get; set; }
}

[Serializable]
public class MilestoneDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public bool IsClosed { get; set; }
}

[Serializable]
public class LabelDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Color { get; set; }
}
````

#### 输入

* **不推荐** 在输入DTO中定义未在服务类中使用的属性.
* **不推荐** 在应用服务方法之间共享输入DTO.
* **不推荐** 继承另一个输入DTO类.
  * **可以** 继承自抽象基础DTO类, 并以这种方式在不同的DTO之间共享一些属性. 但是在这种情况下需要非常小心, 因为更新基础DTO会影响所有相关的DTO和服务方法. 所以避免这样做是一种好习惯.

#### 方法

* **推荐** 为异步方法使用 **Async** 后缀.
* **不推荐** 在方法名中重复实体的名称.
  * 例如: 在 `IProductAppService` 中定义`GetAsync(...)` 而不是 `GetProductAsync(...)` .

##### 获取单一实体

* **推荐** 使用 `GetAsync` 做为**方法名**.
* **推荐** 使用id做为方法参数.
* 返回 **详细DTO**. 示例:

````C#
Task<QuestionWithDetailsDto> GetAsync(Guid id);
````

##### 获取实体集合

* **推荐** 使用 `GetListAsync` 做为**方法名**.
* **推荐** 如果需要获取单个DTO可以使用参数进行 **过滤**, **排序** 和 **分页**.
  * **推荐** 尽可能让过滤参数可选.
  * **推荐** 将排序与分页属性设置为可选, 并且提供默认值.
  * **推荐** 限制最大页数大小 (基于性能考虑).
* **推荐** 返回 **详细DTO**集合. 示例:

````C#
Task<List<QuestionWithDetailsDto>> GetListAsync(QuestionListQueryDto queryDto);
````

##### 创建一个新实体

* **推荐** 使用 `CreateAsync` 做为**方法名**.
* **推荐** 使用**专门的输入DTO**来创建实体.
* **推荐** 使用 **data annotations** 进行输入验证.
  * 尽可能在**领域**之间共享常量(通过**domain shared** package定义的常量).
* **推荐** 只需要创建实体的**最少**信息, 但是提供了其他可选属性.

示例**方法**:

````C#
Task<QuestionWithDetailsDto> CreateAsync(CreateQuestionDto questionDto);
````

输入**DTO**:

````C#
[Serializable]
public class CreateQuestionDto
{
    [Required]
    [StringLength(QuestionConsts.MaxTitleLength, MinimumLength = QuestionConsts.MinTitleLength)]
    public string Title { get; set; }
    
    [StringLength(QuestionConsts.MaxTextLength)]
    public string Text { get; set; } //Optional
    
    public Guid? CategoryId { get; set; } //Optional
}
````

##### 更新已存在的实体

- **推荐** 使用 `UpdateAsync` 做为**方法名**.
- **推荐** 使用**专门的输入DTO**来更新实体.
- **推荐** 获取实体的id做为分离的原始参数. 不要包含更新DTO.
- **推荐** 使用 **data annotations** 进行输入验证.
  - 尽可能在**领域**之间共享常量(通过**domain shared** package定义的常量).
- **推荐** 返回更新实体的**详细**DTO.

示例:

````C#
Task<QuestionWithDetailsDto> UpdateAsync(Guid id, UpdateQuestionDto updateQuestionDto);
````

##### 删除已存在的实体

- **推荐** 使用 `DeleteAsync` 做为**方法名**.
- **推荐** 使用原始参数 id. 示例:

````C#
Task DeleteAsync(Guid id);
````

##### 其他方法

* **可以** 定义其他方法以对实体执行操作. 示例:

````C#
Task<int> VoteAsync(Guid id, VoteType type);
````

此方法为试题投票并返回试题的当前分数.

### 应用服务实现

* **推荐** 开发**完全独立于web层**的应用层.
* **推荐** 在**应用层**实现应用服务接口.
  * **推荐** 使用命名约定. 如: 为 `IProductAppService` 接口创建 `ProductAppService` 类.
  * **推荐** 继承自 `ApplicationService` 基类.
* **推荐** 将所有的公开方法定义为 **virtual**, 以便开发人员继承和覆盖它们.
* **不推荐** 定义 **private** 方法. 应该定义为 **protected virtual**, 这样开发人员可以继承和覆盖它们.

#### 使用仓储

* **推荐** 使用专门设计的仓储 (如 `IProductRepository`).
* **不推荐** 使用泛型仓储 (如 `IRepository<Product>`).z`

#### 查询数据

* **不推荐** 在应用程序服务方法中使用linq/sql查询来自数据库的数据. 让仓储负责从数据源执行linq/sql查询.

#### 操作/删除 实体

* **推荐** 总是从数据库中获取所有的相关实体以对他们执行操作.

#### 使用其他应用服务

* **不推荐** 使用相同 **模块/应用程序** 的其他应用服务. 相反;
  * 使用领域层执行所需的任务.
  * 提取新类并在应用程序服务之间共享, 在必要时代码重用.
* **可以** 在以下情况下使用其他应用服务;
  * 它们是另一个模块/微服务的一部分.
  * 当前模块仅引用已使用模块的application contracts.
>>>>>>> upstream/master
