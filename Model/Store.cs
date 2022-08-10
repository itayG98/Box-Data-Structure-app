using DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using MoockData;
using System.Collections;

namespace Model
{
    public class Store
    {
        private readonly BST<double, BST<double, Box>> MainTree;
        private readonly MyQueue<Box> DatesQueue;
        private readonly DB _data;
        public readonly double LIMIT_PERCENTAGE;
        public readonly int MAX_BOXES_PER_SIZE;
        public readonly int MIN_BOXES_PER_SIZE;
        public readonly int MAX_DAYS;

        public DB Data => _data;
        public Store()
        {
            MainTree = new BST<double, BST<double, Box>>();
            DatesQueue = new MyQueue<Box>();
            _data = DB.Instance;
            LIMIT_PERCENTAGE = Data.LimitPercebtage;
            MAX_BOXES_PER_SIZE = Data.MaxBoxesPerSize;
            MIN_BOXES_PER_SIZE = Data.MinBoxesPerSize;
            MAX_DAYS = Data.MaxDays;
            LoadFromDB();
        }

        private void LoadFromDB()
        {
            foreach (var elem in DB.Boxes)
            {
                if (elem is Box box)
                    Add(box);
            }
            //DatesQueue.Sort();
        }
        private int Add(Box box)
        //Return the boxes count which were not added
        {
            int returnedBoxes = 0;
            //Check if box data is valid
            if (box == null || box.Count <= 0) return -1;
            //lower to max amount
            if (box.Count >= MAX_BOXES_PER_SIZE)
            {
                returnedBoxes += box.Count - MAX_BOXES_PER_SIZE;
                box.Count = MAX_BOXES_PER_SIZE;
            }

            var Xnode = MainTree.FindNode(box.Width);
            if (Xnode != null)//Found x dim
            {
                var Ynode = Xnode.Value.FindNode(box.Height);
                if (Ynode != null) //Found y dim
                {
                    DatesQueue.Remove(Ynode.Value.Node);
                    if (Ynode.Value.Count + box.Count >= MAX_BOXES_PER_SIZE)
                    {
                        returnedBoxes += Ynode.Value.Count + box.Count - MAX_BOXES_PER_SIZE;
                        Ynode.Value.Count = MAX_BOXES_PER_SIZE;
                    }
                    else
                        Ynode.Value.Count += box.Count;
                    Ynode.Value.Date = box.Date;
                }
                else
                    Xnode.Value.AddNode(box.Height, box);
            }
            else
            {
                BST<double, Box> YTree = new BST<double, Box>(box.Height, box);
                MainTree.AddNode(box.Width, YTree);
            }
            box.Node = DatesQueue.Add(box);
            return returnedBoxes;
        }
        internal int Add(double width, double height, int quantety, DateTime date)
        {
            var box = new Box(width, height, quantety, date);
            return Add(box);
        }
        public int Add(double width, double height, int quantety)
        {
            return Add(width, height, quantety, DateTime.Now);
        }
        public int RemoveBoxes(Box box,int quantity)
        //Return how many boxes removed
        {
            //Search the box in both trees
            if (box.Width <= 0 || box.Height <= 0 || box.Count < 1)
                return 0;
            var Xnode = MainTree.FindNode(box.Width);
            if (Xnode == null)
                return 0;
            var Ynode = Xnode.Value.FindNode(box.Height);
            if (Ynode == null)
                return 0;
            
            DatesQueue.Remove(box);
            if (box.Count > quantity)
            {
                box.Count -= quantity;
                box.Date = DateTime.Now;
                box.Node = DatesQueue.Add(Ynode.Value); //Update the queue
            }
            else if (box.Count == quantity) { 
                Xnode.Value.Remove(Ynode);
            }
            else
            {
                Xnode.Value.Remove(Ynode);
                quantity -= box.Count; 
            }
            if (Xnode.Value.IsEmpty())
                MainTree.Remove(Xnode);
            return quantity;
        }
        public int RemoveBoxes(double width, double height, int quantity)
        //Return how many boxes removed
        {
            if (width <= 0 || height <= 0 || quantity < 1)
                return 0;
            var Xnode = MainTree.FindNode(width);
            if (Xnode == null)
                return 0;
            var Ynode = Xnode.Value.FindNode(height);
            if (Ynode == null)
                return 0;

            DatesQueue.Remove(Ynode.Value.Node);
            if (Ynode.Value.Count > quantity)
            {
                Ynode.Value.Count -= quantity;
                Ynode.Value.Date = DateTime.Now;
                Ynode.Value.Node = DatesQueue.Add(Ynode.Value); //Update the queue
            }
            else if (Ynode.Value.Count == quantity)
            {
                Xnode.Value.Remove(Ynode);
            }
            else
            {
                var count = Ynode.Value.Count;
                Xnode.Value.Remove(Ynode);
                quantity = count;
            }
            if (Xnode.Value.IsEmpty())
                MainTree.Remove(Xnode);
            return quantity;
        }
        public void ActionOnBoxes(Action<Box> act, Order ord)
        {
            if (MainTree.IsEmpty())
                return;
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(ord))
                foreach (Box box in YTree.GetEnumerator(ord))
                    act(box);
        }
        public IEnumerable<Box> GetBestOffer(double width, double height, int quantity)
        //Do action for all fitting boxes in range of LimitPercentage
        {
            int temp;
            if (width > 0 && height > 0 && quantity > 0)
            {
                foreach (BST<double, Box> Ytree in MainTree.GetNextTreeByRange(width, width * (1 + LIMIT_PERCENTAGE)))
                {
                    foreach (Box b in Ytree.GetNextTreeByRange(height, height * (1 + LIMIT_PERCENTAGE)))
                    {
                        temp = quantity < b.Count ? quantity : b.Count;
                        quantity -= temp;
                        yield return (b);
                        if (quantity < 1)
                            break;
                    }
                    if (quantity < 1)
                        break;
                }
            }
        }

        public void PopOldBoxes()
        {
            foreach (Box box in GetQueue())
            {
                if (box != null && box.LastPurchased >= MAX_DAYS)
                {
                    RemoveBoxes(box.Width, box.Height, box.Count);
                    DatesQueue.Pop();
                }
                else
                    return;
            }
        }
        public IEnumerable GetandPopOldBoxes()
        {
            foreach (Box box in GetQueue())
            {
                if (box != null && box.LastPurchased >= MAX_DAYS)
                {
                    RemoveBoxes(box.Width, box.Height, box.Count);
                    yield return DatesQueue.Pop();
                }
            }
        }
        public IEnumerable GetAll()
        {
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(Order.InOrderV))
                foreach (Box box in YTree.GetEnumerator(Order.InOrderV))
                    yield return box;
        }
        public IEnumerable GetQueue()
        {
            return DatesQueue.GetQueue();
        }
    }
}
