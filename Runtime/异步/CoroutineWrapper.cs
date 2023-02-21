using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JFramework
{
    internal class CoroutineWrapper<T>
    {
        private readonly CoroutineAwaiter<T> awaiter;
        private readonly Stack<IEnumerator> processStack;

        public CoroutineWrapper(IEnumerator coroutine, CoroutineAwaiter<T> awaiter)
        {
            processStack = new Stack<IEnumerator>();
            processStack.Push(coroutine);
            this.awaiter = awaiter;
        }

        public IEnumerator Run()
        {
            while (true)
            {
                var worker = processStack.Peek();
                bool isDone;
                try
                {
                    isDone = !worker.MoveNext();
                }
                catch (Exception e)
                {
                    var objectTrace = TraceObject(processStack);
                    awaiter.Complete(default, objectTrace.Any() ? new Exception(TraceMessage(objectTrace), e) : e);
                    yield break;
                }

                if (isDone)
                {
                    processStack.Pop();

                    if (processStack.Count == 0)
                    {
                        awaiter.Complete((T)worker.Current, null);
                        yield break;
                    }
                }

                if (worker.Current is IEnumerator current)
                {
                    processStack.Push(current);
                }
                else
                {
                    yield return worker.Current;
                }
            }
        }

        private string TraceMessage(List<Type> objTrace)
        {
            var result = new StringBuilder();

            foreach (var trace in objTrace)
            {
                if (result.Length != 0)
                {
                    result.Append(" -> ");
                }

                result.Append(trace);
            }

            result.AppendLine();
            return "异步错误追踪" + result;
        }

        private static List<Type> TraceObject(IEnumerable<IEnumerator> enumerators)
        {
            var objTrace = new List<Type>();

            foreach (var enumerator in enumerators)
            {
                const BindingFlags attr = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                var field = enumerator.GetType().GetField("$this", attr);
                if (field == null)
                {
                    continue;
                }

                var obj = field.GetValue(enumerator);
                if (obj == null)
                {
                    continue;
                }

                var objType = obj.GetType();
                if (!objTrace.Any() || objType != objTrace.Last())
                {
                    objTrace.Add(objType);
                }
            }

            objTrace.Reverse();
            return objTrace;
        }
    }
}