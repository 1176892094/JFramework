# JYJFramework
欢迎来到JYJFramework介绍
1.导入
打开Unity的Package Manager左上角“+”号使用URL方式导入：https://github.com/1176892094/JYJFramework.git
2.使用
EventManager

```csharp
public class Foo
{
    ISomeService _service;

    public Foo()
    {
        _service = new SomeService();
    }

    public void DoSomething()
    {
        _service.PerformTask();
       … 
    }
}
