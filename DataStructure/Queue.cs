using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
    public class MyQueue<V> where V : IComparable
    {
        QueueNode Root;
        public MyQueue(QueueNode root) => Root = root;

        public void Add(V val)
        {
            if (IsEmpty())
                Root.Value = val;
            else
            {
                QueueNode node = Root;
                while (node.Next != null)
                    node = node.Next;
                node.Next = new QueueNode(val);
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
                return val;
            }
        }

        public bool Contains(V val)
        {
            if (IsEmpty())
                return default;
            QueueNode node = Root;
            while (node.Next != null)
            {
                if (node.CompareTo(val) == 0)
                    return true;
                node = node.Next;
            }
            return node.CompareTo(val)==0;
        }

        public MyQueue() : this(null) { }


        public bool IsEmpty() => Root == null;

        public class QueueNode
        {
            private QueueNode _next;
            private V _value;
            public QueueNode(V val) => Value = val;
            public QueueNode() : this(default) { }


            public V Value { get => _value; set => _value = value; }
            public QueueNode Next { get => _next; set => _next = value; }
            public int CompareTo(V val) => Value.CompareTo(val);
        }
    }
}
