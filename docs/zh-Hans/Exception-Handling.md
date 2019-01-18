<<<<<<< HEAD
## Exception Handling

ABP provides a built-in infrastructure and offers a standard model for handling exceptions in a web application.

* Automatically **handles all exceptions** and sends a standard **formatted error message** to the client for an API/AJAX request.
* Automatically hides **internal infrastructure errors** and returns a standard error message.
* Provides a configurable way to **localize** exception messages.
* Automatically maps standard exceptions to **HTTP status codes** and provides a configurable option to map these to custom exceptions.

### Automatic Exception Handling

`AbpExceptionFilter` handles an exception if **any of the following conditions** are meet:

* Exception is thrown by a **controller action** which returns an **object result** (not a view result).
* The request is an AJAX request (`X-Requested-With` HTTP header value is `XMLHttpRequest`).
* Client explicitly accepts the `application/json` content type (via `accept` HTTP header).

If the exception is handled it's automatically **logged** and a formatted **JSON message** is returned to the client.

#### Error Message Format

Error Message is an instance of the `RemoteServiceErrorResponse` class. The simplest error JSON has a **message** property as shown below:

````json
{
  "error": {
    "message": "This topic is locked and can not add a new message"
  }
}
````

There are **optional fields** those can be filled based upon the exception that has occured.

##### Error Code

Error **code** is an optional and unique string value for the exception. Thrown `Exception` should implement the `IHasErrorCode` interface to fill this field. Example JSON value:

````json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message"
  }
}
````

Error code can also be used to localize the exception and customize the HTTP status code (see the related sections below).

##### Error Details

Error **details** in an optional field of the JSON error message. Thrown `Exception` should implement the `IHasErrorDetails` interface to fill this field. Example JSON value:

```json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message",
    "details": "A more detailed info about the error..."
  }
}
```

##### Validation Errors

**validationErrors** is a standard field that is filled if the thrown exception implements the `IHasValidationErrors` interface.

````json
{
  "error": {
    "code": "App:010046",
    "message": "Your request is not valid, please correct and try again!",
    "validationErrors": [{
      "message": "Username should be minimum lenght of 3.",
      "members": ["userName"]
    },
    {
      "message": "Password is required",
      "members": ["password"]
    }]
  }
}
````

`AbpValidationException` implements the `IHasValidationErrors` interface and it is automatically thrown by the framework when a request input is not valid. So, usually you don't need to deal with validation errors unless you have higly customised validation logic.

#### Logging

Caught exceptions are automatically logged. 

##### Log Level

Exceptions are logged with the `Error` level by default. The Log level can be determined by the exception if it implements the `IHasLogLevel` interface. Example:

````C#
public class MyException : Exception, IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    //...
}
````

##### Self Logging Exceptions

Some exception types may need to write additional logs. They can implement the `IExceptionWithSelfLogging` if needed. Example:

````C#
public class MyException : Exception, IExceptionWithSelfLogging
{
    public void Log(ILogger logger)
    {
        //...log additional info
    }
}
````

> `ILogger.LogException` extension methods is used to write exception logs. You can use the same extension method when needed.

### Business Exceptions

Most of your own exceptions will be business exceptions. The `IBusinessException` interface is used to mark an exception as a business exception.

`BusinessException` implements the `IBusinessException` interface in addition to the `IHasErrorCode`, `IHasErrorDetails` and `IHasLogLevel` interfaces. The default log level is `Warning`.

Usually you have an error code related to a particular business exception. For example:

````C#
throw new BusinessException(QaErrorCodes.CanNotVoteYourOwnAnswer);
````

`QaErrorCodes.CanNotVoteYourOwnAnswer` is just a `const string`. The following error code format is recommended:

````
<code-namespace>:<error-code>
````

**code-namespace** is a **unique value** specific to your module/application. Example:

````
Volo.Qa:010002
````

`Volo.Qa` is the code-namespace here. code-namespace is then will be used while **localizing** exception messages.

* You can **directly throw** a `BusinessException` or **derive** your own exception types from it when needed.
* All properties are optional for the `BusinessException` class. But you generally set either `ErrorCode` or  `Message` property.

### Exception Localization

One problem with throwing exceptions is how to localize error messages while sending it to the client. ABP offers two models and their variants.

#### User Friendly Exception

If an exception implements the `IUserFriendlyException` interface, then ABP does not change it's `Message` and `Details` properties and directly send it to the client.

`UserFriendlyException` class is the built-in implementation of the `IUserFriendlyException` interface. Example usage:

````C#
throw new UserFriendlyException(
    "Username should be unique!"
);
````

In this way, there is **no need for localization** at all. If you want to localize the message, you can inject and use the standard **string localizer** (see the [localization document](Localization.md)). Example:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage"]);
````

Then define it in the **localization resource** for each language. Example:

````json
{
  "culture": "en",
  "texts": {
    "UserNameShouldBeUniqueMessage": "Username should be unique!"
  }
}
````

String localizer already supports **parameterized messages**. For example:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage", "john"]);
````

Then the localization text can be:

````json
"UserNameShouldBeUniqueMessage": "Username should be unique! '{0}' is already taken!"
````

* The `IUserFriendlyException` interface is derived from the `IBusinessException` and the `UserFriendlyException` class is derived from the `BusinessException` class.

#### Using Error Codes

`UserFriendlyException` is fine, but it has a few problems in advanced usages:

* It requires you to **inject the string localizer** everywhere and always use it while throwing exceptions.
* However, in some of the cases, it may **not be possible** to inject the string localizer (in a static context or in an entity method).

Instead of localizing the message while throwing the exception, you can separate the process using **error codes**.

First, define the **code-namespace** to **localization resource** mapping in the module configuration:

````C#
services.Configure<ExceptionLocalizationOptions>(options =>
{
    options.MapCodeNamespace("Volo.Qa", typeof(QaResource));
});
````

Then any of the exceptions with `Volo.Qa` namespace will be localized using their given localization resource. The localization resource should always have an entry with the error code key. Example:

````json
{
  "culture": "en",
  "texts": {
    "Volo.Qa:010002": "You can not vote your own answer!"
  }
}
````

Then a business exception can be thrown with the error code:

````C#
throw new BusinessException(QaDomainErrorCodes.CanNotVoteYourOwnAnswer);
````

* Throwing any exception implementing the `IHasErrorCode` interface behaves the same. So, the error code localization approach is not unique to the `BusinessException` class.
* Defining localized string is not required for an error message. If it's not defined, ABP sends the default error message to the client. It does not use the `Message` property of the exception! if you want that, use the `UserFriendlyException` (or use an exception type that implements the `IUserFriendlyException` interface).

##### Using Message Parameters

If you have a parameterized error message, then you can set it with the exception's `Data` property. For example:

````C#
throw new BusinessException("App:010046")
{
    Data =
    {
        {"UserName", "john"}
    }
};

````

Fortunately there is a shortcut way to code this:

````C#
throw new BusinessException("App:010046")
    .WithData("UserName", "john");
````

Then the localized text can contain the `UserName` parameter:

````json
{
  "culture": "en",
  "texts": {
    "App:010046": "Username should be unique. '{UserName}' is already taken!"
  }
}
````

* `WithData` can be chained with more than one parameter (like `.WithData(...).WithData(...)`).

### HTTP Status Code Mapping

ABP tries to automatically determine the most suitable HTTP status code for common exception types by following these rules:

* For the `AbpAuthorizationException`:
  * Returns `401` (unauthorized) if user has not logged in.
  * Returns `403` (forbidden) if user has logged in.
* Returns `400` (bad request) for the `AbpValidationException`.
* Returns `404` (not found) for the `EntityNotFoundException`.
* Returns `403` (forbidden) for the `IBusinessException` (and `IUserFriendlyException` since it extends the `IBusinessException`).
* Returns `501` (not implemented) for the `NotImplementedException`.
* Returns `500` (internal server error) for other exceptions (those are assumed as infrastructure exceptions).

The `IHttpExceptionStatusCodeFinder` is used to automatically determine the HTTP status code. The default implementation is the `DefaultHttpExceptionStatusCodeFinder` class. It can be replaced or extended as needed.

#### Custom Mappings

Automatic HTTP status code determination can be overrided by custom mappings. For example:

````C#
services.Configure<ExceptionHttpStatusCodeOptions>(options =>
{
    options.Map("Volo.Qa:010002", HttpStatusCode.Conflict);
});
````

### Built-In Exceptions

Some exception types are automatically thrown by the framework:

- `AbpAuthorizationException` is thrown if the current user has no permission to perform the requested operation. See authorization document (TODO: link) for more.
- `AbpValidationException` is thrown if the input of the current request is not valid. See validation document (TODO: link) for more.
- `EntityNotFoundException` is thrown if the requested entity is not available. This is mostly thrown by [repositories](Repositories.md).

You can also throw these type of exceptions in your code (although it's rarely needed).
=======
## 异常处理

ABP提供了用于处理Web应用程序异常的标准模型.

* 自动 **处理所有异常** .如果是API/AJAX请求,会向客户端返回一个**标准格式化后的错误消息** .
* 自动隐藏 **内部详细错误** 并返回标准错误消息.
* 为异常消息的 **本地化** 提供一种可配置的方式.
* 自动为标准异常设置 **HTTP状态代码** ,并提供可配置选项,以映射自定义异常.

### 自动处理异常

当满足下面**任意一个条件**时,`AbpExceptionFilter` 会处理此异常:

* 当**controller action**方法返回类型是**object**(不是view)并有异常抛出时.
* 当是一个请求为AJAX(Http请求头中`X-Requested-With`为`XMLHttpRequest`)时.
* 当客户端接受的返回类型为`application/json`(Http请求头中`accept` 为`application/json`)时.

如果异常被处理过,则会自动**记录日志**并将格式化的**JSON消息**返回给客户端.

#### 异常消息格式

每个异常消息都是`RemoteServiceErrorResponse` 类的实例.下面是一个只有 **Message** 属性的错误JSON:

````json
{
  "error": {
    "message": "This topic is locked and can not add a new message"
  }
}
````

当异常发生时,会自动填充到这些**可选字段**.

##### 错误代码

错误的 **Code** 是字符串类型,并要求唯一的可选属性.如果抛出的异常包含  **Code**  属性,那么应该实现`IHasErrorCode` 接口,来填充这个字段.示例JSON如下:

````json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message"
  }
}
````

错误 **Code** 同样可用于异常的本地化及自定义HTTP状态代码(请参阅下面的相关部分).

##### 错误详细信息

错误的 **Details** 是可选属性.抛出的异常应该实现`IHasErrorDetails` 接口来填充这个字段.示例JSON如下:

```json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message",
    "details": "A more detailed info about the error..."
  }
}
```

##### 验证错误

当抛出的异常继承至`IHasValidationErrors` 接口时,返回错误对象会包含一个可选属性**validationErrors** .示例JSON如下:

````json
{
  "error": {
    "code": "App:010046",
    "message": "Your request is not valid, please correct and try again!",
    "validationErrors": [{
      "message": "Username should be minimum lenght of 3.",
      "members": ["userName"]
    },
    {
      "message": "Password is required",
      "members": ["password"]
    }]
  }
}
````

`AbpValidationException`已经实现了`IHasValidationErrors`接口,当请求输入无效时,框架会自动抛出此错误. 因此,除非你有自定义的验证逻辑,否则不需要处理验证错误.

#### 日志

自动记录捕获异常的日志.

##### 日志级别

默认情况下,记录异常级别为`Error` .可以通过实现`IHasLogLevel` 接口来指定日志的级别,例如:

````C#
public class MyException : Exception, IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    //...
}
````

##### 异常自定义日志 

某些异常类型可能需要记录额外日志信息.可以通过实现`IExceptionWithSelfLogging` 来记录指定日志,例如:

````C#
public class MyException : Exception, IExceptionWithSelfLogging
{
    public void Log(ILogger logger)
    {
        //...log additional info
    }
}
````

> 扩展方法`ILogger.LogException` 用来记录日志. 在需要时可以使用相同的扩展方法.

### 业务异常

大多数异常都是业务异常.可以通过使用`IBusinessException` 接口来标记异常为业务异常.

`BusinessException` 除了实现`IHasErrorCode`,`IHasErrorDetails` ,`IHasLogLevel` 接口外,还实现了`IBusinessException` 接口.其默认日志级别为`Warning`.

通常你会将一个错误代码关联至特定的业务异常.例如:

````C#
throw new BusinessException(QaErrorCodes.CanNotVoteYourOwnAnswer);
````

`QaErrorCodes.CanNotVoteYourOwnAnswer` 是一个字符串常量. 建议使用下面的错误代码格式:

````
<code-namespace>:<error-code>
````

**code-namespace**,应在指定的模块/应用层中保证其唯一.例如:

````
Volo.Qa:010002
````

`Volo.Qa`在这是作为`code-namespace`. `code-namespace` 同样可以在异常 **本地化** 中使用.

* 你可以直接抛出一个 `BusinessException` 异常或者自定义的异常.
* 对于`BusinessException` 类型,其所有属性都是可选的.但是通常会设置`ErrorCode`或`Message`属性.

### 异常本地化

这里有个问题,就是如何在发送错误消息到客户端时,对错误消息进行本地化.ABP提供了2个模型.

#### 用户友好异常

如果异常实现了 `IUserFriendlyException` 接口,那么ABP不会修改 `Message`和`Details`属性,而直接将它发送给客户端.

`UserFriendlyException` 类默认实现了 `IUserFriendlyException` 接口,示例如下:

````C#
throw new UserFriendlyException(
    "Username should be unique!"
);
````

采用这种方式是不需要本地化的.如果需要本地化消息,则可以注入**string localizer**( 请参阅[本地化文档](Localization.md) )来实现. 例:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage"]);
````

再在本地化资源的语言中添加对应的定义.例如:

````json
{
  "culture": "en",
  "texts": {
    "UserNameShouldBeUniqueMessage": "Username should be unique!"
  }
}
````

**string localizer** 是支持格式化参数.例如

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage", "john"]);
````

其本地化文本如下:

````json
"UserNameShouldBeUniqueMessage": "Username should be unique! '{0}' is already taken!"
````

* `IUserFriendlyException`接口派生自`IBusinessException`,而 `UserFriendlyException `类派生自`BusinessException`类.

#### 错误代码

`UserFriendlyException`很好用,但是在一些高级用法里面,它存在以下问题:

* 在抛出异常的地方必须注入**string localizer** 来实现本地化 .
* 但是,在某些情况下,**可能注入不了string localizer**(比如,静态方法或实体中)

那么这时就可以通过使用 **错误代码** 的方式来处理本地化,而不是在抛出异常的时候.

首先,在模块配置代码中将 **code-namespace**  映射至 **本地化资源**:

````C#
services.Configure<ExceptionLocalizationOptions>(options =>
{
    options.MapCodeNamespace("Volo.Qa", typeof(QaResource));
});
````

再使用本地化资源,来本地化`Volo.Qa`命名空间下的所有异常. 本地化资源中应包含对应错误代码的文本. 例如:

````json
{
  "culture": "en",
  "texts": {
    "Volo.Qa:010002": "You can not vote your own answer!"
  }
}
````

最后就可以抛出一个包含错误代码的业务异常了:

````C#
throw new BusinessException(QaDomainErrorCodes.CanNotVoteYourOwnAnswer);
````

* 所有实现`IHasErrorCode` 接口的异常都具有相同的行为.因此,对错误代码的本地化,并不是`BusinessException`类所特有的.
* 错误消息的本地化文本的并不是必须. 如果未定义,ABP会将默认的错误消息发送给客户端. 而并不是发送异常的`Message`属性. 如果你想要发送异常的`Message`,使用`UserFriendlyException`(或使用实现`IUserFriendlyException`接口的异常类型)

##### 使用消息的格式化参数

如果错误消息包含格式化参数时,则可以使用异常的`Data`属性进行设置.例如:

````C#
throw new BusinessException("App:010046")
{
    Data =
    {
        {"UserName", "john"}
    }
};

````

另外有一种更为快捷的方式:

````C#
throw new BusinessException("App:010046")
    .WithData("UserName", "john");
````

下面就是一个包含`UserName` 参数的错误消息:

````json
{
  "culture": "en",
  "texts": {
    "App:010046": "Username should be unique. '{UserName}' is already taken!"
  }
}
````

* `WithData` 支持链式调用 (如`.WithData(...).WithData(...)`).

### HTTP状态代码 映射

ABP尝试按照以下规则,自动映射常见的异常类型的HTTP状态代码:

* 对于 `AbpAuthorizationException`:
  * 用户没有登录,返回 `401` (未认证).
  * 用户已登录,但是当前访问未授权,返回 `403` (未授权).
* 对于 `AbpValidationException` 返回 `400` (错误的请求) .
* 对于  `EntityNotFoundException`返回 `404` (未找到).
* 对于 `IBusinessException` 和  `IUserFriendlyException` (它是`IBusinessException`的扩展) 返回`403` (未授权) .
* 对于 `NotImplementedException` 返回 `501` (未实现) .
* 对于其他异常 (基础架构中未定义的) 返回 `500` (服务器内部错误) .

`IHttpExceptionStatusCodeFinder` 是用来自动判断HTTP状态代码.默认的实现是`DefaultHttpExceptionStatusCodeFinder`.可以根据需要对其进行更换或扩展.

#### 自定义映射

可以重写HTTP状态代码的自动映射,示例如下:

````C#
services.Configure<ExceptionHttpStatusCodeOptions>(options =>
{
    options.Map("Volo.Qa:010002", HttpStatusCode.Conflict);
});
````

### 内置的异常

框架会自动抛出以下异常类型:

- 当用户没有权限执行操作时,会抛出 `AbpAuthorizationException` 异常. 有关更多信息,请参阅授权文档(TODO:link).
- 如果当前请求的输入无效,则抛出`AbpValidationException 异常`. 有关更多信息,请参阅授权文档(TODO:link).
- 如果请求的实体不存在,则抛出`EntityNotFoundException` 异常. 此异常由 [repositories](Repositories.md) 抛出.

你同样可以在代码中抛出这些类型的异常(虽然只在很少时候)
>>>>>>> upstream/master
