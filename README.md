# 欢迎来到JFramework介绍

1.导入

打开Unity的Package Manager左上角“+”号使用URL方式导入：https://github.com/1176892094/JFramework.git

2.开始

导入后找到Unity上方工具栏Tools，在Framework下点击Initialization完成初始配置。

其中Resources/Setttings/JsonData用于配置JsonManager，Scenes/StartScene为基础场景。

备注：Json序列化依赖于NewtonsoftJson包

3.使用

(1)EventManager（事件管理类）

```csharp
public class Test1 : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Instance.Listen(EventName.EventTrigger, EventTrigger); //监听事件
    }

    private void Update()
    {
        EventManager.Instance.Send(EventName.EventTrigger); //发送事件
    }

    private void EventTrigger() //触发事件调用该方法
    {
        Debug.Log("触发事件!");
    }

    private void OnDestroy()
    {
        EventManager.Instance.Remove(EventName.EventTrigger, EventTrigger); //移除事件
    }
}

public struct EventName
{
    public const string EventTrigger = "EventTrigger"; //建议定一个事件的常量
}
```
(2)ResourcesManager（资源加载管理类）
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
(3)JsonManager（找到Unity上方Tools/Framework/PersistentPath可查看存档数据）
```csharp
public class Test3 : MonoBehaviour
{
    private void SaveAndLoad1()
    {
        ScriptableObject playerData = ResourceManager.Load<ScriptableObject>(ResPath.PlayerData);
        JsonManager.Save(playerData, "玩家数据"); //保存SO文件,名称为"玩家数据"
        JsonManager.Load(playerData); //读取该SO文件
    }

    private void SaveAndLoad2()
    {
        ScriptableObject playerData = ResourceManager.Load<ScriptableObject>(ResPath.PlayerData);
        JsonManager.Save(playerData, "玩家数据", true); //储存数据并加密
        JsonManager.Load(playerData, true); //解析加密数据并读取
    }

    private void SaveAndLoad3()
    {
        List<string> playerNameList = new List<string>();
        JsonManager.Save(playerNameList, "strList"); //储存playerNameList
        playerNameList = JsonManager.Load<List<string>>("strList"); //读取playerNameList
    }
}
```
(4)PoolManager(对象池工具)
```csharp
public class Test4: MonoBehaviour
{
    private GameObject bullet;
    private async void Start()
    {
        PoolManager.Instance.Pop(PoolPath.Bullet, obj =>
        {
            bullet = obj;//从对象池中取出Bullet
            obj.transform.position = transform.position;//设置生成的子弹位置在自身位置
        });

        await new WaitForSeconds(5);//等待5秒
        PoolManager.Instance.Push(bullet.name, bullet);//将物体放入对象池
    }
}

public struct PoolPath
{
    public const string Bullet = "Bullet";//Bullet的真实路径是：Assets/Resources/Bullet
}
```
(5)AudioManager（游戏声音管理）
```csharp
public class Test5 : MonoBehaviour
{
    private AudioSource audioSource;

    private void BGMusic()
    {
        AudioManager.Instance.PlaySound(AudioPath.BGMusic); //播放背景音乐
        AudioManager.Instance.StopSound(); //停止背景音乐
        AudioManager.Instance.ChangeSound(0); //改变背景音乐大小为0
    }

    private void GameAudio()
    {
        AudioManager.Instance.PlayAudio(AudioPath.BTClick); //播放该音效
        AudioManager.Instance.PlayAudio(AudioPath.BTClick, audio =>
        {
            audioSource = audio; //播放并获取该音效
        });
        AudioManager.Instance.StopAudio(audioSource); //停止该音效
        AudioManager.Instance.ChangeAudio(0); //改变游戏音效大小为0
    }
}

public struct AudioPath
{
    public const string BGMusic = "Audio/BGMusic"; //BGMusic的真实路径是：Assets/Resources/Audio/BGMusic
    public const string BTClick = "Audio/BTClick"; //BTClick的真实路径是：Assets/Resources/Audio/BTClick
}
```
(6)UIManager（UI管理类）
```csharp
public class Test7: MonoBehaviour
{
    private void ShowPanel()
    {
        UIManager.Instance.ShowPanel<LoginPanel>(UIPanelPath.LoginPanel); //加载LoginPanel(可以重复加载，但只有一个实例)
        UIManager.Instance.ShowPanel<LoginPanel>(UIPanelPath.LoginPanel,UILayerType.Middle);//设置层级
        UIManager.Instance.ShowPanel<LoginPanel>(UIPanelPath.LoginPanel,UILayerType.Middle, panel =>
        {
            panel.SetUseruame("JINYIJIE");//设置属性
            panel.SetPassword("123456");//设置属性
        });
    }
    
    private void HidePanel()
    {
        UIManager.Instance.HidePanel(UIPanelPath.LoginPanel); //隐藏LoginPane
    }

    private void GetPanel()
    {
        LoginPanel panel = UIManager.Instance.GetPanel<LoginPanel>(UIPanelPath.LoginPanel);//得到面板
        panel.SetUseruame("JINYIJIE");//设置属性
        panel.SetPassword("123456");//设置属性
    }

    private void GetLayer()
    {
        UIManager.Instance.GetLayer(UILayerType.Bottom);//得到层级
        Transform common = UIManager.Instance.GetLayer(UILayerType.Height);
    }

    private void Clear()
    {
        UIManager.Instance.Clear();//清除并销毁所有面板
    }
}

public struct UIPanelPath
{
    public const string LoginPanel = "UI/LoginPanel";//LoginPanel的真实路径是：Assets/Resources/UI/LoginPanel
}

public class LoginPanel : BasePanel //需要管理的UI都要继承BasePanel
{
    private string username;
    private string password;
    public void SetUseruame(string username) => this.username = username;
    public void SetPassword(string password) => this.password = password;
}
```
(7)LoadManager(场景加载管理)
```csharp
public class Test8: MonoBehaviour
{
    private void LoadScene()
    {
        LoadManager.Instance.LoadScene("SceneName");
    }

    private void LoadSceneAsync()
    {
        LoadManager.Instance.LoadSceneAsync("SceneName", () =>
        {
            //加载完成后事件
        });
    }
}
```
(8)TimerManager(自定义计时器工具)
```csharp
public class Test9 : MonoBehaviour
{
    private Timer timer;
    private void Start()
    {
        timer = TimerManager.Instance.GetTimer(); //新建计时器
        timer.Open(0.3f, 10, FinishAction); //开启计时器，0.3秒计时一次，循环10次
        timer.Open(5f, FinishAction);//新建一个5秒的计时器
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) timer.Stop(); //暂停计时器
        if (Input.GetKeyDown(KeyCode.E)) timer.Play(); //启动计时器
    }

    private void FinishAction() => Debug.Log("Timer Update"); //计时器更新
}
```
(9)AwaitExtensions
```csharp
  private async void Start()
    {
        await new WaitForSeconds(1);//等待一秒
        await new WaitForSecondsRealtime(1);//等待1秒，不受timeScale影响
        await new WaitForUpdate();//在Update最后一帧执行
        await new WaitForFixedUpdate();//在FixedUpdate最后一帧执行
        await new WaitForEndOfFrame();//等待这一帧结束
        await new WaitWhile(WaitTime);//等待WaitTime结果，不会挂起异步
        await new WaitUntil(WaitTime);//等待WaitTime结果，false不会执行后面语句
        await SceneManager.LoadSceneAsync("SceneName");
        await Resources.LoadAsync("ResourcesName");
        AsyncOperation asyncOperation = new AsyncOperation();
        await asyncOperation;//等待异步操作
        ResourceRequest request = new ResourceRequest();
        await request;//等待资源请求
        AssetBundleRequest bundleRequest = new AssetBundleRequest();
        await bundleRequest;//等待AB包请求
        AssetBundleCreateRequest bundleCreateRequest = new AssetBundleCreateRequest();
        await bundleCreateRequest;//等待AB包创建请求
    }
    
    private bool WaitTime()
    {
        return true;
    }
```
