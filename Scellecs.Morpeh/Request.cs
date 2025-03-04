#if UNITY_EDITOR
#define MORPEH_DEBUG
#endif

using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Scellecs.Morpeh.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Scellecs.Morpeh
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class Request<TData> : RequestBase where TData : struct, IRequestData
    {
        internal readonly FastList<TData> changes = new FastList<TData>();

        internal int lastConsumedIndex;

        [PublicAPI]
        public void Publish(in TData request, bool allowNextFrame = false)
        {
            changes.Add(request);
        }

        [PublicAPI]
        public void Clear()
        {
            changes.Clear();
            lastConsumedIndex = 0;
        }

        [PublicAPI]
        public Consumer Consume()
        {
            Consumer consumer;
            consumer.request = this;
            return consumer;
        }
        

        public struct Consumer
        {
            public Request<TData> request;

            public Enumerator GetEnumerator()
            {
                Enumerator e;
                e.request = request;
                e.current = default;
                return e;
            }
        }

        public struct Enumerator
        {
            public Request<TData> request;
            public TData current;

            public bool MoveNext()
            {
                if (request.lastConsumedIndex >= request.changes.length)
                {
                    request.Clear();
                    return false;
                }

                current = request.changes.data[request.lastConsumedIndex++];
                return true;
            }

            public TData Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => this.current;
            }
        }
    }

    public abstract class RequestBase
    {
        internal RequestRegistry registry;
    }
}