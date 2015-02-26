# Coat

Coat is a very tiny wrapper of dapper ORM using t4 code-gen approach.

# Design Goal

* Provide ActiveRecord pattern ORM API
* Support MS SQL + MYSQL
* Flexibility to add transparent cache layer
* Flexibility to auto integrate with elastic-search / solr

# Usages

	coat.exe default.yaml
	coat.exe d1.yaml d2.yaml

Coat currently supports R->O mapping, i.e. it reads existing schema from database and then generate related entity object.

## Config File

`default.yaml` could be config values to be loaded, it requires 4 parameters, like:

	conn: data source=.\SQLEXPRESS;Initial Catalog=d2d;user id=sa;password=;
	tables: ["*", -UserTemplate]
	output: C:\Users\wuvis_000\Desktop\65dg\d2d\web\DeliverySystem\GenModels
	namespace: d2d
	connection: db1Connection

* conn: string, the sql server connection string
* tables: string array, the list of tables names to be processed.
  * _"*"_ is a special value, meaning all tables.
  * if tablename starts with "-"， means that it won't be included
* output: string, output path for generated files.
  * generated file name is like `tablename.generated.cs`
* namespace: string, the namespace of generated files

The config file should be as simple as possible. It may be extended to contains model/table schema info to fascilitate R->O mapping, or support O->R mapping.

### O->R mapping

UserAccount:
  fields:
    - UserID: string
      attrs : [unique, required]
    - UserName: string
      label: 用户名
      attrs : [unique, required]

UserWechat:
  extend: WechatAccount
  fields:
    - UserID: string
      attrs : [unique, required]

WechatAccount:
  fields:
    - OpenId: string
      label: 微信OpenID
      attrs: [unique]
      remark: 普通用户的标识，对当前开发者帐号唯一
    - UnionId: string
      label: 微信UnionID
      attrs: [unique]
      remark: 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。

# Static APIs

db.Execute(...)

## GetById

var u = User.GetById(1);
if (u != null) {
	...
}

## GetByIds

var users = User.GetByIds(new List<int>(){1, 2});
foreach (var u in users) {
	...
}

## GetAll

var users = User.GetAll();
foreach (var u in users) {
	...
}

## Count

var count = User.Count(string where  sql, object params);
foreach (var u in users) {
	...
}

## Find

var users = User.Find(string where  sql, object params, int limit, offset limit);
foreach (var u in users) {
	...
}

## FindOne

var user = User.FindOne(string sql, object params);

# Object APIs

## Insert()

var u = new User();
u.Insert();
Console.Writeline(u.ID);

## Update()

var job = Job.GetById(1);
job.User.XXX
job.SetUser(u)
job.Update()

## Delete()

obj.Delete();

Class.Delete("where sql", params);

## Foreign Key

var job = Job.GetById(1);
job.User.XXX
job.SetUser(u)
job.Update()

## Transaction

using (var transactionScope = new TransactionScope()) {
    var obj = AdminInfo.Get("14B63059-DF00-4945-AD5D-60AF0EAB6E96");
    var s = Snapshotter.Start<AdminInfo>(obj);
    obj.Message = "bingo";
    Console.WriteLine(obj.Message);
    transactionScope.Complete();
}

## Object Diff

var obj = d2d.AdminInfo.FindOne("username=@username", new { username=""});
var sn = Dapper.Snapshotter.Start<d2d.AdminInfo>(obj);
obj.IsAdmin = true;
sn.Diff();
obj.Update();

# Todo

* Foreign Key support
* mysql support
* Integrate where predicate
* Add db obj
* Add diff Update support
