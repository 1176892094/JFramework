# JYJFramework
欢迎来到JYJFramework介绍
1.导入
打开Unity的Package Manager左上角“+”号使用URL方式导入：https://github.com/1176892094/JYJFramework.git

2.使用

(1)EventManager

```csharp
public class Test1 : MonoBehaviour
{
    private void Awake()
    {
        EventManager.AddEventListener(EventName.EventTrigger, EventTrigger); //添加事件
    }

    private void Update()
    {
        EventManager.OnEventTrigger(EventName.EventTrigger); //触发事件
    }

    private void EventTrigger() //触发事件调用该方法
    {
        Debug.Log("触发事件!");
    }

    private void OnDestroy()
    {
        EventManager.RemoveEventListener(EventName.EventTrigger, EventTrigger); //移除事件
    }
}

public struct EventName
{
    public const string EventTrigger = "EventTrigger"; //建议定一个事件的常量
}
```
(2)ResourcesManager
```csharp
public class Test2 : MonoBehaviour
{
    private void LoadAsset() //同步加载
    {
        GameObject obj = ResourceManager.Load<GameObject>(ResPath.Player);
        Player player = obj.GetComponent<Player>();
    }

    private void LoadAssetAsync() //异步加载
    {
        ResourceManager.LoadAsync<GameObject>(ResPath.Player, obj =>
        {
            Player player = obj.GetComponent<Player>();
        });
    }
}

public struct ResPath
{
    public const string Player = "Prefabs/Player"; //Player预制的体真实路径是：Assets/Resources/Prefabs/Player
}

public class Player: MonoBehaviour
{
}
```
(3)JsonManager
```csharp
public class Test3 : MonoBehaviour
{
    private void SaveAndLoad1()
    {
        ScriptableObject playerData = ResourceManager.Load<ScriptableObject>(ResPath.PlayerData);
        JsonManager.SaveJson(playerData, "玩家数据"); //保存SO文件,名称为"玩家数据"
        JsonManager.LoadJson(playerData); //读取该SO文件
    }

    private void SaveAndLoad2()
    {
        ScriptableObject playerData = ResourceManager.Load<ScriptableObject>(ResPath.PlayerData);
        JsonManager.SaveJson(playerData, "玩家数据", true); //储存数据并加密
        JsonManager.LoadJson(playerData, true); //解析加密数据并读取
    }

    private void SaveAndLoad3()
    {
        List<string> playerNameList = new List<string>();
        JsonManager.SaveJson(playerNameList, "strList"); //储存playerNameList
        playerNameList = JsonManager.LoadJson<List<string>>("strList"); //读取playerNameList
    }
}
```
(4)PoolManager
```csharp
public class Test4: MonoBehaviour
{
    private GameObject bullet;
    private async void Start()
    {
        PoolManager.PopObject(PoolPath.Bullet, obj =>
        {
            bullet = obj;//从对象池中取出Bullet
            obj.transform.position = transform.position;//设置生成的子弹位置在自身位置
        });

        await new WaitForSeconds(5);//等待5秒
        PoolManager.PushObject(bullet.name, bullet);//将物体放入对象池
    }
}

public struct PoolPath
{
    public const string Bullet = "Bullet";//Bullet的真实路径是：Assets/Resources/Bullet
}
```

