using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{/// <summary>
/// Represent a Queue where the boxes with more transaction are first
/// </summary>
/// <typeparam name="V"></typeparam>
    public class MyQueue<V> where V : IComparable
    {
        private QueueNode<V> _root;
        private QueueNode<V> _tail;
        private int _length;

        public QueueNode<V> Root { get => _root; set => _root = value; }
        public QueueNode<V> Tail { get => _tail; set => _tail = value; }
        public int Length { get => _length; private set => _length = value > -1 ? value : 0; }

        public MyQueue(V val)
        {
            Length = 0;
            Add(val);
        }
        public MyQueue() => Root = null;
        public void Add(V val)
        {
            if (IsEmpty())
                Root = new QueueNode<V>(val);
            else
            {
                QueueNode<V> newRoot = new QueueNode<V>(val);
                newRoot.Next = Root;
                Root = newRoot;
            }
            Length++;
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public void Empty()
        {
            Root = null;
            Length = 0;
        }

        public void Remove(V val)
        {
            if (IsEmpty())
                return;
            else if (Root.Value.Equals(val))
            {
                Root = Root.Next;
                Length--;
                return;
            }
            QueueNode<V> prev = Root;
            QueueNode<V> current = Root.Next;
            while (current != null)
            {
                if (current.Value.Equals(val))
                {
                    prev.Next = current.Next;
                    return;
                }
                prev = current;
                current = current.Next;
            }
            Length--;
        }

        public V Pop()
        {
            if (IsEmpty())
                return default;
            else
            {
                V val = Root.Value;
                Root = Root.Next;
                Length--;
                return val;
            }
        }

        public bool Contains(V val)
        {
            if (IsEmpty())
                return default;
            QueueNode<V> node = Root;
            while (node.Next != null)
            {
                if (node.CompareTo(val) == 0)
                    return true;
                node = node.Next;
            }
            return node.CompareTo(val) == 0;
        }
        public bool IsEmpty() => Root == null;
        public IEnumerable<V> GetQueue()
        {
            if (!IsEmpty())
            {
                QueueNode<V> node = Root;
                yield return node.Value;
                while (node.Next != null)
                {
                    yield return node.Next.Value;
                    node = node.Next;
                }
            }
        }

    }
    public class QueueNode<V> where V : IComparable
    {
        private QueueNode<V> _next;
        private QueueNode<V> _prev;

        private V _value;
        public QueueNode(V val) => Value = val;
        public QueueNode() : this(default) { }


        public V Value { get => _value; set => _value = value; }
        public QueueNode<V> Next { get => _next; set => _next = value; }
        public int CompareTo(V val) => Value.CompareTo(val);
        public override bool Equals(object obj)
        {
            if (obj is QueueNode<V> other)
                return Value.Equals(other.Value);
            return false;
        }
    }
}
