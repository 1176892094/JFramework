# JYJFramework
欢迎来到JYJFramework介绍
1.导入
打开Unity的Package Manager左上角“+”号使用URL方式导入：https://github.com/1176892094/JYJFramework.git

2.使用

(1)EventManager

```csharp
public class Test: MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddEventListener(EventName.EventTrigger,EventTrigger);
    }

    private void Update()
    {
        EventManager.OnEventTrigger(EventName.EventTrigger);
    }

    private void EventTrigger()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.RemoveEventListener(EventName.EventTrigger,EventTrigger);
    }
}

public struct EventName
{
    public const string EventTrigger="EventTrigger";
}
