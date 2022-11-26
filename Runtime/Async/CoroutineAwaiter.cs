using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace JFramework.Async
{ 
    public class CoroutineAwaiter : INotifyCompletion
    {
        private bool isCompleted;
        private Action continuation;
        private Exception exception;
        public bool IsCompleted => isCompleted;

        public void GetResult()
        {
            AwaitExtension.Assert(isCompleted);
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }

        public void Complete(Exception exception)
        {
            AwaitExtension.Assert(!isCompleted);
            isCompleted = true;
            this.exception = exception;
            if (continuation != null)
            {
                AwaitExtension.RunOnUnityScheduler(continuation);
            }
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            AwaitExtension.Assert(this.continuation == null);
            AwaitExtension.Assert(!isCompleted);
            this.continuation = continuation;
        }
    }
    
    public class CoroutineAwaiter<T> : INotifyCompletion
    {
        private T result;
        private bool isCompleted;
        private Action continuation;
        private Exception exception;
        public bool IsCompleted => isCompleted;

        public T GetResult()
        {
            AwaitExtension.Assert(isCompleted);
            if (exception != null)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return result;
        }

        public void Complete(T result, Exception exception)
        {
            AwaitExtension.Assert(!isCompleted);
            isCompleted = true;
            this.result = result;
            this.exception = exception;
            if (continuation != null)
            {
                AwaitExtension.RunOnUnityScheduler(continuation);
            }
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            AwaitExtension.Assert(this.continuation == null);
            AwaitExtension.Assert(!isCompleted);
            this.continuation = continuation;
        }
    }
}