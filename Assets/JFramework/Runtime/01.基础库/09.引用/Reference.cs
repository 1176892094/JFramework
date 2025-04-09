using System;

namespace JFramework
{
    internal struct Reference
    {
        public Type assetType { get; }
        public string assetPath { get; }
        public int acquire { get; }
        public int release { get; }
        public int dequeue { get; }
        public int enqueue { get; }

        public Reference(Type assetType, string assetPath, int acquire, int release, int dequeue, int enqueue)
        {
            this.assetType = assetType;
            this.assetPath = assetPath;
            this.acquire = acquire;
            this.release = release;
            this.dequeue = dequeue;
            this.enqueue = enqueue;
        }

        public override string ToString()
        {
            return Service.Text.Format("{0}\t\t{1}\t\t{2}\t\t{3}", release, acquire, dequeue, enqueue);
        }
    }
}