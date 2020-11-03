# NetCoreXZMHui

#### 介绍
`NetCoreXZMHui`是一个简单的NetCore项目脚手架，里面包含了最基本的跨域、jwt、通用异常处理、统一返回格式和基本的数据库应用。

因为`EF Core`本身如果要执行`sql`语句的话需要再`DbContext`中声明实体，所以不是很方便，本身用到自定义查询的地方基本都是业务实体，所以没必要再`DbContext`中声明，所以里面内置了一个抽象的业务仓库，用来执行sql语句。

主要适用于NetCore初学者，模板结构比较简单易懂。


#### 安装教程


```
dotnet new -i XingFeng.XZMHui.NetCore::*
```


#### 使用说明

```
dotnet new TestProject -n Company.Group -o .
```

- `TestProject`为你的项目名称
- `Company.Group`为你要创建的项目前缀

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request

