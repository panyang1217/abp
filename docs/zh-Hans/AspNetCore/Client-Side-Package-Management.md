<<<<<<< HEAD

## ASP.NET Core MVC Client Side Package Management

ABP framework can work with any type of client side package management systems. You can even decide to use no package management system and manage your dependencies manually.

However, ABP framework works best with **NPM/Yarn**. By default, built-in modules are configured to work with NPM/Yarn.

Finally, we suggest the [**Yarn**](https://yarnpkg.com/) over the NPM since it's faster, stable and also compatible with the NPM.

### @ABP NPM Packages

ABP is a modular platform. Every developer can create modules and the modules should work together in a **compatible** and **stable** state.

One challenge is the **versions of the dependant NPM packages**. What if two different modules use the same JavaScript library but its different (and potentially incompatible) versions.

To solve the versioning problem, we created a **standard set of packages** those depends on some common third-party libraries. Some example packages are [@abp/jquery](https://www.npmjs.com/package/@abp/jquery), [@abp/bootstrap](https://www.npmjs.com/package/@abp/bootstrap) and [@abp/font-awesome](https://www.npmjs.com/package/@abp/font-awesome). You can see the **list of packages** from the [Github repository](https://github.com/volosoft/abp/tree/master/npm/packs).

The benefit of a **standard package** is:

* It depends on a **standard version** of a package. Depending on this package is **safe** because all modules depend on the same version.
* It contains the gulp task to copy library resources (js, css, img... files) from the **node_modules** folder to **wwwroot/libs** folder. See the *Mapping The Library Resources* section for more.

Depending on a standard package is easy. Just add it to your **package.json** file like you normally do. Example:


    {
      ...
      "dependencies": {
        "@abp/bootstrap": "^1.0.0"
      }
    }


It's suggested to depend on a standard package instead of directly depending on a third-party package.

#### Package Installation

After depending on a NPM package, all you should do is to run the **yarn** command from the command line to install all the packages and their dependencies:

````
yarn
````

Alternatively, you can use `npm install` but [Yarn](https://yarnpkg.com/) is suggested as mentioned before.

#### Package Contribution

If you need a third-party NPM package that is not in the standard set of packages, you can create a Pull Request on the Github [repository](https://github.com/volosoft/abp). A pull request that follows these rules is accepted:

* Package name should be named as `@abp/package-name` for a `package-name` on NPM (example: `@abp/bootstrap` for the `bootstrap` package).
* It should be the **latest stable** version of the package.
* It should only depend a **single** third-party package. It can depend on multiple `@abp/*` packages.
* The package should include a `abp.resourcemapping.js` file formatted as defined in the *Mapping The Library Resources* section. This file should only map resources for the depended package.
* You also need to create [bundle contributor(s)](Bundling-Minification.md) for the package you have created.

See current standard packages for examples.

### Mapping The Library Resources

Using NPM packages and NPM/Yarn tool is the de facto standard for client side libraries. NPM/Yarn tool creates a **node_modules** folder in the root folder of your web project. 

Next challenge is copying needed resources (js, css, img... files) from the `node_modules` into a folder inside the **wwwroot** folder to make it accessible to the clients/browsers.

ABP defines a [Gulp](https://gulpjs.com/) based task to **copy resources** from **node_modules** to **wwwroot/libs** folder. Each **standard package** (see the *@ABP NPM Packages* section) defines the mapping for its own files. So, most of the time, you only configure dependencies.

The **startup templates** are already configured to work all these out of the box. This section will explain the configuration options.

#### Resource Mapping Definition File

A module should define a JavaScript file named `abp.resourcemapping.js` which is formatted as in the example below:

````js
module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        
    }
}
````

* **aliases** section defines standard aliases (placeholders) that can be used in the mapping paths. **@node_modules** and **@libs** are required (by the standard packages), you can define your own aliases to reduce duplication.
* **clean** section is a list of folders to clean before copying the files.
* **mappings** section is a list of mappings of files/folders to copy. This example does not copy any resource itself, but depends on a standard package.

An example mapping configuration is shown below:

````js
mappings: {
    "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
    "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/"
}
````

#### Using The Gulp

Once you properly configure the `abp.resourcemapping.js` file, you can run the gulp command from the command line:

````
gulp
````

When you run the `gulp`, all packages will copy their own resources into the **wwwroot/libs** folder. Running `yarn & gulp` is only necessary if you make a change in your dependencies in the **package.json** file.

> When you run the Gulp command, dependencies of the application are resolved using the package.json file. The Gulp task automatically discovers and maps all resources from all dependencies (recursively).

#### See Also

* [Bundling & Minification](Bundling-Minification.md)
* [Theming](Theming.md)
=======

## ASP.NET Core MVC 客户端包管理

ABP框架可以与任何类型的客户端包管理系统一起使用. 甚至你可以决定不使用包管理系统并手动管理依赖项.

但是, ABP框架最适用于**NPM/Yarn**. 默认情况下,内置模块配置为与NPM/Yarn一起使用.

最后, 我们建议[**Yarn**](https://yarnpkg.com/)而不是NPM,因为它更快,更稳定并且与NPM兼容.

### @ABP NPM Packages

ABP是一个模块化平台. 每个开发人员都可以创建模块, 模块应该在**兼容**和**稳定**状态下协同工作.

一个挑战是依赖NPM包的**版本**. 如果两个不同的模块使用相同的JavaScript库但其不同(并且可能不兼容)的版本会怎样.

为了解决版本问题, 我们创建了一套**标准包**, 这取决于一些常见的第三方库. 一些示例包是[@abp/jquery](https://www.npmjs.com/package/@abp/jquery), [@ abp/bootstrap](https://www.npmjs.com/package/@abp/bootstrap)和[@abp/font-awesome](https://www.npmjs.com/package/@abp/font-awesome). 你可以从[Github存储库](https://github.com/volosoft/abp/tree/master/npm/packs)中查看**列表**.

**标准包**的好处是:

* 它取决于包装的**标准版本**. 取决于此包是**安全**,因为所有模块都依赖于相同的版本.
* 它包含将库资源(js,css,img...文件)从**node_modules**文件夹复制到**wwwroot/libs**文件夹的gulp任务. 有关更多信息, 请参阅 *映射库资源* 部分.

依赖标准包装很容易. 只需像往常一样将它添加到**package.json**文件中. 例如:

````
    {
      ...
      "dependencies": {
        "@abp/bootstrap": "^1.0.0"
      }
    }
````

建议依赖于标准软件包, 而不是直接依赖于第三方软件包.

#### 安装包

依赖于NPM包后, 你应该做的就是从命令行运行**yarn**命令来安装所有包及其依赖项:

````
yarn
````

虽然你可以使用`npm install`,但如前所述,建议使用[Yarn](https://yarnpkg.com/).

#### 贡献包

如果你需要不在标准软件包中的第三方NPM软件包,你可以在Github[repository](https://github.com/volosoft/abp)上创建Pull请求. 接受遵循这些规则的拉取请求:

* 对于NPM上的`package-name`, 包名称应该命名为`@abp/package-name`(例如:`bootstrap`包的`@abp/bootstrap`).
* 它应该是**最新的稳定**版本的包.
* 它应该只依赖于**单个**第三方包. 它可以依赖于多个`@abp/*`包.
* 包应包含一个`abp.resourcemapping.js`文件格式,如*映射库资源*部分中所定义. 此文件应仅映射所依赖包的资源.
* 你还需要为你创建的包创建[bundle贡献者](Bundling-Minification.md).

有关示例, 请参阅当前标准包.

### 映射库资源

使用NPM包和NPM/Yarn工具是客户端库的事实标准.  NPM/Yarn工具在Web项目的根文件夹中创建一个**node_modules**文件夹.

下一个挑战是将所需的资源(js,css,img ...文件)从`node_modules`复制到**wwwroot**文件夹内的文件夹中,以使其可供客户端/浏览器访问.

ABP将基于[Gulp](https://gulpjs.com/)的任务定义为**将资源**从**node_modules**复制到**wwwroot/libs**文件夹. 每个**标准包**(参见*@ABP NPM Packages*部分)定义了自己文件的映射. 因此, 大多数情况你只配置依赖项.

**启动模板**已经配置为开箱即用的所有这些. 本节将介绍配置选项.

#### 资源映射定义文件

模块应该定义一个名为`abp.resourcemapping.js`的JavaScript文件,其格式如下例所示:

````js
module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@libs": "./wwwroot/libs"
    },
    clean: [
        "@libs"
    ],
    mappings: {
        
    }
}
````

* **aliases**部分定义了可在映射路径中使用的标准别名(占位符). **@node_modules**和 **@libs**是必需的(通过标准包), 你可以定义自己的别名以减少重复.
* **clean**部分是在复制文件之前要清理的文件夹列表.
* **mappings**部分是要复制的文件/文件夹的映射列表.此示例不会复制任何资源本身,但取决于标准包.

示例映射配置如下所示:

````js
mappings: {
    "@node_modules/bootstrap/dist/css/bootstrap.css": "@libs/bootstrap/css/",
    "@node_modules/bootstrap/dist/js/bootstrap.bundle.js": "@libs/bootstrap/js/"
}
````

#### 使用 Gulp

正确配置`abp.resourcemapping.js`文件后, 可以从命令行运行gulp命令:

````
gulp
````

当你运行`gulp`时,所有包都会将自己的资源复制到**wwwroot/libs**文件夹中. 只有在**package.json**文件中对依赖项进行更改时, 才需要运行`yarn＆gulp`.

> 运行Gulp命令时, 使用package.json文件解析应用程序的依赖关系. Gulp任务自动发现并映射来自所有依赖项的所有资源(递归).

#### 参见

* [捆绑 & 压缩](Bundling-Minification.md)
* [主题](Theming.md)
>>>>>>> upstream/master
