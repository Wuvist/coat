ORM实现有反射、范型、代码生成等几种常见方式，或者单用，或者混合。

c#的范型非常强大，应用于ORM时，可能有些特性显得更重要。

一开始实现`coat`时，我尝试写一下代码做为ORM基类

```
namespace Coat
{
    public class ORMBase<T> where T : class
    {
    	...
        public bool Update()
        {
            using (var conn = OpenConnection())
            {
                //Beblow compile error, because conn.Update<T> expect parameter to be T
                //i.e. the sub-class, but "this" is parent class.
                return conn.Update<T>(this);
            }
        }
    }
}

// 子类生成的代码类似：
	public class User: ORMBase<User> {
	...
	}
```

意图是在基类中实现ActiveRecord对象增删改查等通用方法，相比起在具体子类中使用代码生成实现相应的代码会更简洁些。并且，编辑一个实际类型，总比编辑模板方便。

做为一个玩了两年没有范型的语言（GO）的人，我会觉得 c# `class User: ORMBase<User> {` 这样的类型声明很强大。

User类型继承于ORMBase<T>，而类型ORMBase<T>正是使用User类型做为范型参数。这没有循环依赖？

这样ORMBase中，便可以利用范型T做各种编程。

上面代码是卡在了`conn.Update<T>(this);`这句调用。

因为dapper的Update方法签名类似`Update<T>(T entityToUpdate)`，我在ORMBase<T>中写的`this`是父类，也就是ORMBase<T>；而传进去给Update的类型参数T，则是子类，比方说User。

编译器直接就报错了。

ORMBase<T>跟T是两个不同的类型，无法直接转换，写`conn.Update<T>((T)this);`编译器也是报错。

有同事建议修改ORMBase的Update签名，变成`public bool Update(T obj)`，然后把传obj而不是this给dapper。

这样虽然可以解决编译问题，但会让应用调用时变麻烦；还不如直接把Update方法搬去子类里面生成出来，但还是不漂亮。

研究了一番范型约束，结果找到更漂亮的方式。

ORMBase<T>跟T无法相互转换是因为编译器不知道他们之间的继承关系，把他们的继承关系写到范型约束中便可以转换了。

	public class RecordBase<T> where T : RecordBase<T>

这样声明约束T必须是`RecordBase<T>`的子类；Update方法改为：

	return conn.Update<T>((T)this);

便可以顺利编译了。

虽然可以编译，但这里是把父类转换为子类，何以可以顺利编译，我其实还木有搞明白细节。

有朋友知道，还望告知。

谢谢。