# 欢迎来到JFramework介绍

JFramework是基于Unity的游戏框架，封装了一些常用的Unity功能。

1.持久化

```c#
    public class Example
    {
        private Variable<string> playerName; // 对内存进行绑定的变量 防止内存修改
        private Variable<int> playerType;
        private Variable<int> playerCoin;

        public void Save()
        {
            Service.Json.Save(this, "Example"); // 存储玩家数据
        }

        public void Load()
        {
            Service.Json.Save(this, "Example"); // 加载玩家数据
        }
    }
```
2.压缩加密
```c#
    public class Example
    {
        public void Test(string result, byte[] buffer)
        {
            result = Service.Zip.Compress(result);    // 字符串压缩
            buffer = Encoding.UTF8.GetBytes(result);  // 转化为字节
            buffer = Service.Xor.Encrypt(buffer);     // 字节异或加密
            
            buffer = Service.Xor.Decrypt(buffer);     // 字节异或解密
            result = Encoding.UTF8.GetString(buffer); // 转化为字符串
            result = Service.Zip.Decompress(result);  // 字符串解压
        }
    }
```
3.引用池
```c#
    public class Example
    {
        public void Test()
        {
            for (int i = 0; i < 1000; i++)
            {
                var builder = Service.Heap.Dequeue<StringBuilder>(); // 从引用池取出
                builder.AppendLine("Example"); // 添加字符串
                Service.Heap.Enqueue(builder); // 放入引用池
                builder.Length = 0; // 重置对象
            }
        }
    }
```
4.事件池
```c#
    public class Example : MonoBehaviour, IEvent<PackCompleteEvent>
    {
        private void Awake()
        {
            Service.Pack.LoadAssetData(); // 从服务器更新并下载 AssetBundle
        }

        private void OnEnable()
        {
            Service.Event.Listen(this); // 添加下载完成事件
        }

        private void OnDisable()
        {
            Service.Event.Remove(this);// 移除下载完成事件
        }

        public void Execute(PackCompleteEvent message)
        {
            Service.Asset.LoadAssetData(); // 当 AssetBundle 更新下载完成后 加载 AssetBundle 到内存中
        }
    }
```
5.资源加载
```c#
    public class Example : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // AssetBundle: prefabs
                // Asset: Monster
                Service.Asset.Load<GameObject>("Prefabs/Monster"); // 从 prefabs 中 加载 Monster
            }

            if (Input.GetMouseButtonDown(1))
            {
                // AssetBundle: scenes
                // Asset: StartScene
                Service.Asset.LoadScene("StartScene"); // 从 scenes 中 加载 StartScene
            }
        }
    }
```
6.对象池
```c#
    public class Example : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Service.Pool.Show<Monster>("Prefabs/Monster", monster => // 从对象池中 取出 或 生成
                {
                    monster.Watch(5).Invoke(() =>
                    {
                        Service.Pool.Hide(monster); // 等待5秒后 放入对象池
                    });
                });
            }
        }
    }

    public class Monster : MonoBehaviour, IEntity
    {
    }
```
7.贡献者

* [龙傲天](https://github.com/Molth)
