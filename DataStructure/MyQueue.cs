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

        public QueueNode<V> Root { get => _root; private set => _root = value; }
        public QueueNode<V> Tail { get => _tail; private set => _tail = value; }
        public int Length { get => _length; private set => _length = value < 0 ? 0 : 1; }

        public MyQueue(V val)
        {
            Length = 0;
            Add(val);
        }
        public MyQueue() => Root = Tail = null;
        public void Add(V val)
        {
            if (IsEmpty())
            {
                Root = new QueueNode<V>(val);
                Tail = null;
                Root.Next = Tail;
                Length = 1;
                return;
            }
            else if (Root != null && Tail == null)
            {
                Tail = new QueueNode<V>(val);
                Root.Next = Tail;
                Tail.Prev = Root;
                Length++;
                return;
            }
            QueueNode<V> newRoot = new QueueNode<V>(val);
            newRoot.Prev = Tail;
            Tail.Next = newRoot;
            Tail = newRoot;
            Length++;
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public void Empty()
        {
            Root = Tail = null;
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
            else if (Tail.Value.Equals(val))
            {
                Tail = Tail.Prev;
                Length--;
                return;
            }
            QueueNode<V> left = Root.Next;
            QueueNode<V> right = Tail.Prev;
            while (right.Next.Equals(left))
            {
                if (right == null || left == null)
                    break;
                if (left.Value.Equals(val))
                {
                    left.Prev.Next = left.Next;
                    left.Next.Prev = left.Prev;
                    Length--;
                    return;
                }
                else if (right.Value.Equals(val))
                {
                    right.Prev.Next = right.Next;
                    right.Next.Prev = right.Prev;
                    Length--;
                    return;
                }
                left = left.Next;
                right = right.Prev;
            }
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

        public V Value { get => _value; internal set => _value = value; }
        public QueueNode<V> Next { get => _next; internal set => _next = value; }
        public QueueNode<V> Prev { get => _prev; internal set => _prev = value; }

        public int CompareTo(V val) => Value.CompareTo(val);
        public override bool Equals(object obj)
        {
            if (obj is QueueNode<V> other)
                return Value.Equals(other.Value);
            return false;
        }
    }
}
