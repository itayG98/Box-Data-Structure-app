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
        public QueueNode<V> Add(V val)
        {
            if (IsEmpty())
            {
                Root = new QueueNode<V>(val);
                Tail = null;
                Root.Next = Tail;
                Length = 1;
                return Root;
            }
            else if (Tail == null)
            {
                Tail = new QueueNode<V>(val);
                Root.Next = Tail;
                Tail.Prev = Root;
                Length = 2;
                return Tail;
            }
            QueueNode<V> newRoot = new QueueNode<V>(val);
            newRoot.Prev = Tail;
            Tail.Next = newRoot;
            Tail = newRoot;
            Length++;
            return newRoot;
        }
        public void Empty()
        {
            Root = null;
            Tail = null;
            Length = 0;
        }
        public bool Remove(V val)
        {
            if (IsEmpty() || val != null)
                return false;
            QueueNode<V> temp = Root;
            while (temp != null)
            {
                if (temp.CompareTo(val) == 0)
                {
                    if (temp == Root)
                    {
                        temp.Next = Root.Next;
                        Root = temp;
                        Length--;
                    }
                    else if (temp == Tail)
                    {
                        temp.Prev = Tail.Prev;
                        Tail = temp;
                    }
                    else
                    {
                        temp.Prev.Next = temp.Next;
                        temp.Next.Prev = temp.Prev;
                        Length--;
                    }
                    Length--;
                    return true;
                }
                temp = temp.Next;
            }
            return false;
        }
        //Fix removing tail and head
        public bool Remove(QueueNode<V> toRemove)
        //Fix removing tail and head
        {
            if (toRemove == null)
                return false;
            if (toRemove.Prev != null && toRemove.Next != null)
            {
                toRemove.Prev.Next = toRemove.Next;
                toRemove.Next.Prev = toRemove.Prev;
                Length--;
                return true;
            }
            else if (toRemove.Prev == null) //Root
            {
                Root = toRemove.Next;
                Length--;
                return true;
            }
            else if (toRemove.Next == null) //Tail
            {
                Tail = toRemove.Prev;
                Length--;
                return true;
            }
            return false;
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
        public bool IsEmpty() => Root == null && Tail == null;

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
