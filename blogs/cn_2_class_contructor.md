ORM框架在基类中定义各增删改查方法，子类实现保存尽可能简洁，除了各属性表示表结构（也就是POCO Plain Old Csharp Object）至少还得有表名等信息。

考虑一下代码片段：

```
public abstract class RecordBase<T> where T : RecordBase<T>, new()
{
    protected static string TableName;
	public static List<T> GetByID(int id)
    {
        using (var conn = OpenConnection())
        {
            var sql = "select * from " + TableName + " where ID = @id";
            return conn.Query<T>(sql, new {id = id}).First();
        }
    }
}

public class User : RecordBase<User>
{
    public int Id { get; set; }
    public string Name { get; set; }

    static User ()
    {
        TableName = "Users";
    }
}
```

调用代码则类似：
```
var u = User.GetByID(1);
```

User子类中的静态构造函数中对TableName进行赋值；那么父类中的`GetByID`方法执行时，便可以获得正确的表名。

类型的静态构造函数一般用于类型的静态数据初识化，它会并且仅会执行一次。

（RecordBase<T>若有多个子类，引用的TableName属性相互独立。）

看上去这个场景正式静态构造函数的典型应用。

遗憾的是，上面代码是有坑的。

MSDN中对静态构造函数static constructor定义如下：

	A static constructor is used to initialize any static data, or to perform a particular action that needs to be performed once only. It is called automatically before the first instance is created or any static members are referenced.

`GetByID`方法是在父类`RecordBase<T>`中定义的；即便调用时是写`User.GetByID()`，实际上是被引用的还是父类的静态成员，而不是子类；如果程序之前没有创建过任何子类实例，那子类的静态构造函数到这里仍可能未被执行。

也就是说如果程序的其它地方都没有引用到User子类，那么`User.GetByID()`会抛异常：TableName是空的。

必须先在调用GetByID之前，先创建一个User实例（或者调用User而不是父类的静态成员），才能正常工作，比方说：

```
var tmp = new User();
var u = User.GetByID(1);
```

如果ORM这么实现，这就是一个大坑；如果强制程序初始化的时候，写显式代码去调用一下各个子类，也是相当麻烦。

偶找到的解决方法是：添加父类的静态构造函数，在其中触发子类的静态构造函数。

```
    public abstract class RecordBase<T> where T : RecordBase<T>, new()
    {
        protected static string TableName;

        static RecordBase()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        }
    }
```

`GetByID`是父类的静态成员，它被调用的话，父类的静态构造函数便会先被运行时调用，而函数内通过范型有子类的类型，便可通过CompilerServices调用子类的静态构造函数。

（RunClassConstructor函数即便被多次调用，类型的静态构造函数也只会被运行一次。）

c#中的静态构造函数虽然可以做到精准的控制各类型的初始化，需要的时候才调用，不重复调用。

但若是控制得不精准，或者说实现代码时不充分注意，便可能在程序中留下坑。若缺乏相关经验知识，那踩中坑时也会困惑不知如何处理。

实际上，我是当年曾经搞过相关的开发，所以这次才特意去测试了不实例化子类，直接调用`GetByID`以确定这块是否存在问题。如果我是新手并且对文档理解不够细，那采坑是妥妥的。

c#这门语言在很多方面都相当优秀，但具体到类型、模块初始化这块，做为Go程序员，我会说Go做得更好。

Go语言不允许无效引用，更不允许模块循环引用，确保了所有的Go语言模块/类型有清晰的依赖关系。

而所有模块被引用前，便自动运行模块`init`方法。

Go程序员需要考虑的场景简单，自然也就不容易采坑。

有些语言或者说工具的“强大”是体现在功能繁多上，功能越多，组合场景就越多，程序员便需要去精心琢磨各个具体场景需要如何处理，这实际上是相当大的心智负担，并且程序往往也只是做到“没有明显bug”。

而像Go这样的语言，它的“强大”之处是体现于提供尽可能相互不重叠的少数功能，程序员可以轻易的考量所有潜在场景，写“明显没有bug”的程序。

这事用术语来说是Orthogonality，偶当年第一次看到这英文词是不懂，查中文翻译：正交性。字都认识，但组成词，依旧是不懂。

:)