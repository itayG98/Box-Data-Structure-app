using DataStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Box :IComparable
    {
        private int _count = 0;
        private readonly double _height;
        private readonly double _width;
        private DateTime _date;
        QueueNode<Box> _node;



        public double Height { get => _height; }
        public double Width { get => _width; }
        public int Count { get => _count; set => _count = value > 0 ? value : 0; }
        public DateTime Date { get => _date; set => _date = value; }
        public int LastPurchased => (DateTime.Now - Date).Days;

        public QueueNode<Box> Node { get => _node; set => _node = value; }
        public Box(double width, double height, int count, DateTime date)
        {
            if (width<=0)
                width = 1;
            if(height<=0)
                height = 1;
            _width = width;
            _height = height;
            Count = count;
            Date = date;
        }
        public Box(double width, double height, int count) : this(width, height, count,DateTime.Now)
        {
        }

        public override string ToString()
        {
            return $"Width : {Width:f2} Height : {Height:f2} Count :{Count}";
        }
        public string ToLongString()
        {
            return $"Width {Width:f2} Height {Height:f2} Count = {Count} Last purhesed {Date}";
        }

        public override bool Equals(object obj)
        {
            if(obj is Box other) 
                return Height==other.Height && Width==other.Width;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj is Box other)
                return Date.CompareTo(other.Date);
            return default;
        }
    }
}
