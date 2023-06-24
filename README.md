# 欢迎来到JFramework介绍

JFramework是基于Unity的游戏框架，封装了一些常用的Unity功能。

1.使用教程

B站：https://www.bilibili.com/video/BV13X4y1Z75g/?spm_id_from=333.999.list.card_archive.click&vd_source=64e481400752df328433068b87764dc5

3.导表工具和配表

B站：https://www.bilibili.com/video/BV1f84y1g73U/?spm_id_from=333.999.0.0&vd_source=64e481400752df328433068b87764dc5

4.热更新

B站：https://www.bilibili.com/video/BV1Ct4y1P79J/?spm_id_from=333.999.0.0&vd_source=64e481400752df328433068b87764dc5

5.其他拓展

ChatGPT接口：https://github.com/1176892094/ChatGPT-To-Unity

基于JobSystem的寻路：https://github.com/1176892094/JFramework-AStar

Async/Await拓展：https://github.com/1176892094/JFramework-Async

网络同步框架JFramework.Net：还在测试阶段，暂未开放

6.常用管理器

(1)EventManager（事件管理类）

```csharp
public class Test1 : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Listen(EventName.OnPlayerDeath, OnPlayerDeath); //侦听事件
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.Invoke(EventName.OnPlayerDeath); //发送事件
        }
    }

    private void OnPlayerDeath(params object[] value)
    {
        Debug.Log("玩家死亡"); //触发事件
    }

    private void OnDestroy()
    {
        EventManager.Remove(EventName.OnPlayerDeath, OnPlayerDeath); //移除事件
    }
}

public struct EventName
{
    public const int OnPlayerDeath = 1001; //建议给事件定一个常量
}
```
(2)AssetManager（资源加载管理类）
```csharp
public class Test2 : MonoBehaviour
{
    private void Load() //同步加载
    {
        GameObject obj = AssetManager.Load<GameObject>(AssetPath.Player);
        Player player = obj.GetComponent<Player>();
    }

    private async void LoadAsync() //异步加载
    {
        var obj = await AssetManager.LoadAsync<GameObject>(AssetPath.Player);
        Player player = obj.GetComponent<Player>(); //obj为加载出来的GameObject预制体，可以从obj中获取玩家的组件
    }

    public struct AssetPath
    {
        public const string Player = "Prefabs/Player"; //Player预制的体真实路径是：Assets/AddressableResources/Prefabs/Player
    }
}

public class Player : MonoBehaviour
{
}
```
(3)JsonManager（找到Unity上方Tools/JFramework/PersistentPath可查看存档数据）
```csharp
public class Test3 : MonoBehaviour
{
    private async void SaveAndLoad1()
    {
        var data = await AssetManager.LoadAsync<ScriptableObject>(AssetPath.Data);
        JsonManager.Save(data, data.name); //保存SO文件,名称为"玩家数据"
        JsonManager.Load(data); //读取该SO文件
    }

    private async void SaveAndLoad2()
    {
        var data=await AssetManager.LoadAsync<ScriptableObject>(AssetPath.Data;
        JsonManager.Encrypt(data, "玩家数据"); //储存数据并加密
        JsonManager.Decrypt(data); //解析加密数据并读取
    }

    private void SaveAndLoad3()
    {
        List<string> bagData = new List<string>();
        JsonManager.Save(bagData, "BagData"); //储存BagData
        bagData = JsonManager.Load<List<string>>("BagData"); //读取BagData
    }
}

public struct AssetPath
{
    public const string Data = "Settings/PlayerData"; //PlayerData真实路径是：Assets/AddressableResources/Settings/Data
}

public class PlayerData : ScriptableObject
{
    public string Name;
}
```
(4)PoolManager(对象池工具)
```csharp
public class Test4 : MonoBehaviour
{
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var obj = await PoolManager.Pop<GameObject>(AssetPath.Bullet);
            obj.transform.position = transform.position; //设置生成的子弹位置在自身位置
            TimerManager.Pop(3, () => //创建一个3秒的计时器
            {
                PoolManager.Push(obj); //将物体放回对象池
            });
        }
    }
}

public struct AssetPath
{
    public const string Bullet = "Prefabs/Bullet"; //Bullet的真实路径是：Assets/AddressableResources/Prefabs/Bullet
}
```
(5)AudioManager（游戏声音管理）
```csharp
public class Test5 : MonoBehaviour
{
     private AudioSource audioSource;
    
    private void Sound()
    {
        AudioManager.PlaySound(AudioPath.BGMusic); //播放背景音乐
        AudioManager.StopSound(); //停止背景音乐
        AudioManager.SetSound(0); //改变背景音乐大小为0
    }

    private void Audio()
    {
        AudioManager.PlayAudio(AudioPath.BTClick); //播放该音效
        AudioManager.StopAudio(audioSource); //停止该音效
        AudioManager.SetAudio(0); //改变游戏音效大小为0
    }
}

public struct AudioPath
{
    public const string BGMusic = "Audio/BGMusic"; //BGMusic的真实路径是：Assets/Resources/Audio/BGMusic
    public const string BTClick = "Audio/BTClick"; //BTClick的真实路径是：Assets/Resources/Audio/BTClick
}
```
(6)UIManager（UI面板请放在AddressableResources/UI/...目录下）
```csharp
public class Test7: MonoBehaviour
{
    private async void ShowPanel()
    {
        UIManager.ShowPanel<LoginPanel>(); //加载LoginPanel(可以重复加载，但只有一个实例)
        var panel = await UIManager.ShowPanel<LoginPanel>();
        panel.SetUsername("JINYIJIE");//设置属性
        panel.SetPassword("123456");//设置属性
    }
    
    private void HidePanel()
    {
        UIManager.HidePanel<LoginPanel>(); //隐藏LoginPane
    }

    private void GetPanel()
    {
        LoginPanel panel = UIManager.GetPanel<LoginPanel>();//得到面板
        panel.SetUsername("JINYIJIE");//设置属性
        panel.SetPassword("123456");//设置属性
    }

    private void GetLayer()
    {
        UIManager.GetLayer(UILayerType.Layer1);//获取层级
        Transform common = UIManager.GetLayer(UILayerType.Layer2);
    }

    private void Clear()
    {
        UIManager.Clear();//清除并销毁所有面板
    }
}

public struct UIPanelPath
{
    public const string LoginPanel = "UI/LoginPanel";//LoginPanel的真实路径是：Assets/AddressableResources/UI/LoginPanel
}

public class LoginPanel : UIPanel //需要管理的UI都要继承BasePanel
{
    private string username;
    private string password;
    public void SetUsername(string username) => this.username = username;
    public void SetPassword(string password) => this.password = password;
}
```
(7)SceneManager(场景加载管理)
```csharp
public class Test8 : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.OnLoadScene += LoadProgress;
    }

    private async void LoadSceneAsync()
    {
        await SceneManager.LoadSceneAsync("SceneName");
    }

    private void LoadProgress(float progress) //获取异步加载进度
    {
        Debug.Log(progress); //获取当前加载进度
    }

    private void OnDestroy()
    {
        SceneManager.OnLoadScene -= LoadProgress;
    }
}
```
(8)TimerManager(自定义计时器工具)
```csharp
public class Test9 : MonoBehaviour
{
    private ITimer timer;
    private bool isFinish;

    private void Start()
    {
        timer = TimerManager.Pop(5, () =>
        {
            Debug.Log("不循环/间隔5秒的计时器完成");
        });

        TimerManager.Pop(5, () =>
        {
            Debug.Log("不循环/间隔5秒的不受TimeScale影响的计时器完成");
        }).Unscale();

        int count = 0;
        TimerManager.Pop(1, () =>
        {
            Debug.Log($"计时器循环第{++count}次");
        }).Loop(5);

        
        TimerManager.Pop(1, timer =>
        {
            if (isFinish)//根据条件回收计时器
            {
                timer.Push();//等价于 TimerManager.Push(timer);
            }
        }).Loop(-1);//无限循环
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) timer.Stop();  //启动计时器，从剩余时间开始
        if (Input.GetKeyDown(KeyCode.E)) timer.Play();  //暂停计时器，从当前时间停止
    }
}
```
(9)StateMachine(有限状态机)
```csharp
public class Enemy : Entity //敌人继承实体
{
    public EnemyMachine machine; //敌人状态机
    public Animator animator; //敌人动画组件

    private void Start()
    {
        machine.Enable(); //启动敌人状态机
    }

    protected override void OnUpdate()
    {
        machine.OnUpdate(); //更新敌人状态机
    }
}

public class EnemyMachine : StateMachine<Enemy> //设置状态机的所有者
{
    protected override void Start()
    {
        AddState<EnemyIdle>(); //状态机增加Idle状态
        AddState<EnemyWalk>(); //状态机增加Walk状态
    }

    public void Enable()
    {
        ChangeState<EnemyIdle>(); //启动后切换到Idle状态
    }
}

public class EnemyIdle : State<Enemy> //设置状态的所有者
{
    protected override void OnAwake()
    {
        //创建状态时调用
    }

    protected override void OnEnter()
    {
        owner.animator.SetBool("Idle", true); //播放Idle动画
        owner.machine.ChangeState<EnemyWalk>(3); //3秒后切换到Walk动画
    }

    protected override void OnUpdate() //状态更新
    {
    }

    protected override void OnExit()
    {
        owner.animator.SetBool("Idle", false); //停止Idle动画
    }
}

public class EnemyWalk : State<Enemy> //设置状态的所有者
{
    protected override void OnEnter()
    {
        owner.animator.SetBool("Walk", true); //播放Walk动画
        owner.machine.ChangeState<EnemyIdle>(3); //3秒后切换到Idle动画
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnExit()
    {
        owner.animator.SetBool("Walk", false); //停止Walk动画
    }
}
```
(10)Entity/Controller(EC实体控制器分离)
```csharp
 public class Player : EntitySpecial //玩家继承实体
{
    public BuffController buffCtrl => GetOrAddCtrl<BuffController>(); //效果控制器
    public SkillController skillCtrl => GetOrAddCtrl<SkillController>(); //技能控制器
    public AttributeController attrCtrl => GetOrAddCtrl<AttributeController>(); //属性控制器
}

public class SkillController : Controller<Player> //设置控制器的所有者
{
    private AttributeController attrCtrl => owner.attrCtrl;

    protected override void Start() //初始化方法
    {
    }
}

public class AttributeController : Controller<Player> //设置控制器的所有者
{
    private SkillController skillCtrl => owner.skillCtrl;
    private BuffController buffCtrl => owner.buffCtrl;

    protected override void Start() //初始化方法
    {
    }
}

public class BuffController : Controller<Player> //设置控制器的所有者
{
    private AttributeController attrCtrl => owner.attrCtrl;
    private SkillController skillCtrl => owner.skillCtrl;

    protected override void Start() //初始化方法
    {
    }
}
```