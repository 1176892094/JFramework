# 欢迎来到JFramework介绍

JFramework是基于Unity的游戏框架，封装了一些常用的Unity功能。

1.功能介绍

(1) AssetManager：基于AssetBundle的加载管理器。优先加载AssetBundle资源，找不到资源再寻找Resources中的资源。

(2) AudioManager：游戏音效管理，基于AssetManager加载音效资源，可控制背景音乐和游戏音效，会在场景中生成音效池。

(3) BundleManger：用于下载服务器上的AssetBundle资源，可通过委托获取AssetBundle的大小和数量。

(4) DataManager：运行时读取Excel转化的ScriptableObject文件，所有的Excel数据都通过该管理器调用。

(5) EntityManager：用于生成实体的ScriptableObject组件，减少MonoBehaviour的性能开销。

(6) EventManager：事件中心，用于广播全局事件，建议使用结构体作为事件参数，并传入结构体类型。

(7) GlobalManager：全局管理器，用于注册和卸载所有的管理器，包括在游戏生命周期中添加事件。

(8) InputManager：基于EventManager的来广播玩家输入事件，使用Unity旧版的Input，可自定义改键。

(9) JsonManager：基于Unity自带的JsonUtility，负责游戏存储相关，可对游戏数据进行AES加密。

(10) PoolManager：基于AssetManager的对象池，可对Unity类以及普通类对象进行存入和取出，以减少性能开销。

(11) SceneManager：基于AssetManager的场景加载，获取AssetBundle的场景并进行加载。

(12) SettingManager：存储一些游戏用到的路径，资源加载模式和资源构建路径等。

(13) TimerManager：计时器管理器，用于延迟调用或者循环调用方法，使用计时器池，避免使用协程产生性能开销。

(14) UIManager：用户界面管理，可以加载或者隐藏界面，推荐使用 [Inject] 特性来注册界面和按钮。

(15) SecretInt,SecretFloat,SecretString：用于反作弊，防止玩家直接修改内存来修改玩家数据。

(16) State<T>,StateMachine<T>：状态机组件，用于玩家或者怪物的AI，状态机所有者需要继承 IEntity 接口

(17) AttributeComponent<T>：属性组件，推荐使用枚举定义玩家的属性并传入组件中，使用SecretFloat，防止内存作弊。

(18) UIScroll<TItem,TGrid>：无限滚动条，用于制作排行榜，背包滚动，商店滚动，使用对象池进行优化。

7.贡献者

* [龙傲天](https://github.com/Molth)
