using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure
{
    public class Queue<V> where V : IComparable
    {
        QueueNode Root;
        public Queue(QueueNode root)
        {
            Root = root;
        }
        public Queue() : this(null)
        {
        }

        public bool IsEmpty() => Root == null;

        public class QueueNode 
        {
            private QueueNode _next;
            private V _value;

            public V Value { get => _value; set => _value = value; }
            public QueueNode Next { get => _next; set => _next = value; }
            public int CompareTo(V val) => Value.CompareTo(val);
        }
    }
}
