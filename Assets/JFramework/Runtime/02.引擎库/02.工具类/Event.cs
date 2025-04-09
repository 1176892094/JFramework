// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 22:04:35
// // # Recently: 2025-04-09 22:04:35
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

namespace JFramework.Common
{
    public struct PackAwake : IEvent
    {
        public int[] sizes { get; private set; }

        public PackAwake(int[] sizes)
        {
            this.sizes = sizes;
        }
    }

    public struct PackUpdate : IEvent
    {
        public string name { get; private set; }
        public float progress { get; private set; }

        public PackUpdate(string name, float progress)
        {
            this.name = name;
            this.progress = progress;
        }
    }

    public struct PackComplete : IEvent
    {
        public int status { get; private set; }
        public string message { get; private set; }

        public PackComplete(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }

    public struct AssetAwake : IEvent
    {
        public string[] names { get; private set; }

        public AssetAwake(string[] names)
        {
            this.names = names;
        }
    }

    public struct AssetUpdate : IEvent
    {
        public string name { get; private set; }

        public AssetUpdate(string name)
        {
            this.name = name;
        }
    }

    public struct AssetComplete : IEvent
    {
    }

    public struct SceneAwake : IEvent
    {
        public string name { get; private set; }

        public SceneAwake(string name)
        {
            this.name = name;
        }
    }

    public struct SceneUpdate : IEvent
    {
        public float progress { get; private set; }

        public SceneUpdate(float progress)
        {
            this.progress = progress;
        }
    }

    public struct SceneComplete : IEvent
    {
    }

    public struct DataAwake : IEvent
    {
        public string[] names { get; private set; }

        public DataAwake(string[] names)
        {
            this.names = names;
        }
    }

    public struct DataUpdate : IEvent
    {
        public string name { get; private set; }

        public DataUpdate(string name)
        {
            this.name = name;
        }
    }

    public struct DataComplete : IEvent
    {
    }
}