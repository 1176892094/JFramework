# 欢迎来到JFramework介绍

JFramework是基于Unity的游戏框架，封装了一些常用的Unity功能。

1.导入

(1)克隆下来拖进项目进行导入(需要添加AddressableAsset和NewtonsoftJson包)

(2)打开Unity的Package Manager左上角“+”号使用URL方式导入

URL：https://github.com/1176892094/JFramework.git

2.开始

GlobalManager使用：

(1)GlobalManager负责所有管理的初始化

(2)预制体在框架中的Resources文件夹中

(3)将预制体拖入场景即可

ExcelToAsset使用：

(1)在上方工具栏找到Tools/JFramework/ExcelToAsset

(2)或者在JFrameworkInspector中找到ExcelToAsset

(3)找到存放Excel的文件夹，点击确定即可转化成ScriptableObject

(4)配合AddressableAsset工具使用

Addressable使用：

(1)第一次请在上方工具栏找到Window/AssetManagement/Addressable/Groups创建组

(2)在JFrameworkInspector中找到JFrameworkEditor并点击

(3)或者按F1呼出快捷菜单，点击Addressable资源生成

(4)在Addressables Groups面板中可查看资源生成的情况

3.注意

(1)所有的Entity都会被加入到GlobalManager的生命周期中

(2)为了更好的管理，请使用Entity来代替MonoBehaviour

(2)请使用OnUpdate来代替Update

4.使用

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
            EventManager.Send(EventName.OnPlayerDeath); //发送事件
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

    private void LoadAsync() //异步加载
    {
        AssetManager.LoadAsync<GameObject>(AssetPath.Player, obj =>
        {
            Player player = obj.GetComponent<Player>(); //obj为加载出来的GameObject预制体，可以从obj中获取玩家的组件
        });
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
    private void SaveAndLoad1()
    {
        AssetManager.LoadAsync<ScriptableObject>(AssetPath.Data, data =>
        {
            JsonManager.Save(data, data.name); //保存SO文件,名称为"玩家数据"
            JsonManager.Load(data); //读取该SO文件
        });
    }

    private void SaveAndLoad2()
    {
        AssetManager.LoadAsync<ScriptableObject>(AssetPath.Data, data =>
        {
            JsonManager.Save(data, "玩家数据", true); //储存数据并加密
            JsonManager.Load(data, true); //解析加密数据并读取
        });
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PoolManager.Pop(AssetPath.Bullet, obj =>
            {
                obj.transform.position = transform.position; //设置生成的子弹位置在自身位置
                TimerManager.Pop(3, () => //创建一个3秒的计时器
                {
                    PoolManager.Push(obj); //将物体放回对象池
                });
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
        AudioManager.PlayAudio(AudioPath.BTClick, audio =>
        {
            audioSource = audio; //播放并获取该音效
        });
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
(6)UIManager（面板数据请在Resources文件夹中找到名称为UIPanelData的Json文件）
```csharp
public class Test7: MonoBehaviour
{
    private void ShowPanel()
    {
        UIManager.ShowPanel<LoginPanel>(); //加载LoginPanel(可以重复加载，但只有一个实例)
        UIManager.ShowPanel<LoginPanel>();//设置层级
        UIManager.ShowPanel<LoginPanel>(panel =>
        {
            panel.SetUsername("JINYIJIE");//设置属性
            panel.SetPassword("123456");//设置属性
        });
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
        UIManager.GetLayer(UILayerType.Layer1);//得到层级
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
        EventManager.Listen(EventName.LoadProgress, LoadProgress); //侦听场景异步加载进度
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("SceneName");
    }

    private void LoadSceneAsync()
    {
        SceneManager.LoadSceneAsync("SceneName", () =>
        {
            //异步加载完成后执行的方法
        });
    }

    private void LoadProgress(params object[] value) //获取异步加载进度
    {
        var progress = (float)value[0]; //获取当前加载进度
        Debug.Log(progress);
    }

    private void OnDestroy()
    {
        EventManager.Remove(EventName.LoadProgress, LoadProgress); //移除场景异步加载进度
    }

    public struct EventName
    {
        public const int LoadProgress = 999; //固定为999号事件
    }
}
```
(8)TimerManager(自定义计时器工具)
```csharp
public class Test9 : MonoBehaviour
{
    private ITimer timer;

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
            Debug.Log("循环5次/间隔1秒的计时器完成");
        }).Unscale().SetLoop(5, () =>
        {
            count++;
            Debug.Log("第" + count + "次循环完成");
        });
        
        TimerManager.Pop(10, () =>
        {
            Debug.Log("设置计时器随物体销毁而停止");
        }).SetTarget(gameObject);
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
    public class Enemy : Entity //敌人继承状态机
    {
        public EnemyMachine machine; //敌人状态机
        public Animator an; //敌人动画组件

        private void Start()
        {
            machine.Enalbe(); //启动敌人状态机
        }
    }
   
   
    public class EnemyMachine : Controller<Enemy> //属于敌人的控制器
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
        protected override void OnInit(Enemy owner)
        {
            base.OnInit(owner); //父类初始化状态
            //自定义初始化
        }

        protected override void OnEnter()
        {
            owner.an.SetBool("Idle",true); //播放Idle动画
            owner.machine.ChangeState("Walk",3); //3秒切换到Walk动画
        }

        protected override void OnUpdate() //状态更新
        {
        }

        protected override void OnExit()
        {
            owner.an.SetBool("Idle",false); //停止Idle动画
        }
    }

    public class EnemyWalk : State<Enemy> //设置状态的所有者
    {
        protected override void OnEnter()
        {
            owner.an.SetBool("Walk",true); //播放Walk动画
            owner.machine.ChangeState("Idle",3); //3秒切换到Idle动画
        }

        protected override void OnUpdate()
        {
           
        }

        protected override void OnExit()
        {
            owner.an.SetBool("Walk",false); //停止Walk动画
        }
    }
```
(10)Entity/Controller(EC实体控制器分离)
```csharp
    public class Player : Entity //玩家继承实体
    {
        public BuffController buffCtrl; //效果控制器
        public SkillController skillCtrl; //技能控制器
        public AttributeController attrCtrl; //属性控制器

        protected override void Awake()
        {
            base.Awake();
            skillCtrl = ScriptableObject.CreateInstance<SkillController>();
            buffCtrl = ScriptableObject.CreateInstance<BuffController>();
            attrCtrl = ScriptableObject.CreateInstance<AttributeController>();
            skillCtrl.Get<IController>().OnInit(this);
            attrCtrl.Get<IController>().OnInit(this);
            buffCtrl.Get<IController>().OnInit(this);
        }
    }

    public class SkillController : Controller<Player> //技能控制器(ScriptableObject)
    {
        private AttributeController attrCtrl => owner.attrCtrl;
    }

    public class AttributeController : Controller<Player> //属性控制器(ScriptableObject)
    {
        private SkillController skillCtrl => owner.skillCtrl;
    }

    public class BuffController : Controller<Player> //效果控制器(ScriptableObject)
    {
        private AttributeController attrCtrl => owner.attrCtrl;
        private SkillController skillCtrl => owner.skillCtrl;
    }
```

(11)AwaitExtensions(异步拓展)
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
