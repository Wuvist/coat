# Coat

Coat is a very tiny wrapper of dapper ORM using t4 code-gen approach.

# Design Goal

* Provide ActiveRecord pattern ORM API
* Flexibility to add transparent cache layer
* Flexibility to auto integrate with elastic-search

# Usages

	coat.exe default.yaml

`default.yaml` could be config values to be loaded, it requires 4 parameters, like:

	conn: data source=.\SQLEXPRESS;Initial Catalog=d2d;user id=sa;password=;
	tables: ["*"]
	output: C:\Users\wuvis_000\Desktop\65dg\d2d\web\DeliverySystem\GenModels
	namespace: d2d

* conn: string, the sql server connection string
* tables: string array, the list of tables names to be processed.
  * _"*"_ is a special value, meaning all tables.
* output: string, output path for generated files.
  * generated file name is like `tablename.generated.cs`
* namespace: string, the namespace of generated files
