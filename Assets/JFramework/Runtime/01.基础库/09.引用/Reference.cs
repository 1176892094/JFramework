using System;

namespace JFramework
{
    internal struct Reference
    {
        public Type type;
        public string path;
        public int acquire;
        public int release;
        public int dequeue;
        public int enqueue;

        public Reference(Type type, string path, int acquire, int release, int dequeue, int enqueue)
        {
            this.type = type;
            this.path = path;
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