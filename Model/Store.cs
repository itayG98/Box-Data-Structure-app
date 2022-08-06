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
        BST<double, BST<double, Box>> MainTree;
        public MyQueue<Box> DatesQueue;
        public const double LimitPercentage = 0.33;
        public const int MaxBoxesPerSize = 50;
        public const int MinBoxesPerSize = 10;

        public Store()
        {
            MainTree = new BST<double, BST<double, Box>>();
            DatesQueue = new MyQueue<Box>();
            LoadFromDB();
        }

        private void LoadFromDB()
        {
            _ = DB.Instance;
            foreach (var elem in DB.Boxes)
            {
                if (elem != null)
                    Add(elem);
            }
        }
        private int Add(Box box)
        //Return the boxes count which were not added
        {
            int returnedBoxes = 0;
            //Check if box data is valid
            if (box == null || box.Count <= 0) return -1;
            if (box.Count >= MaxBoxesPerSize)
            {
                returnedBoxes += box.Count - MaxBoxesPerSize;
                box.Count = MaxBoxesPerSize;
            }

            var Xnode = MainTree.FindNode(box.Width);
            if (Xnode != null)//Found x dim
            {
                var Ynode = Xnode.Value.FindNode(box.Height);
                if (Ynode != null) //Found y dim
                {
                    DatesQueue.Remove(box);
                    if (Ynode.Value.Count + box.Count >= MaxBoxesPerSize)
                    {

                        returnedBoxes += Ynode.Value.Count + box.Count - MaxBoxesPerSize;
                        Ynode.Value.Count = MaxBoxesPerSize;
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
            DatesQueue.Add(box);
            return returnedBoxes;
        }
        public int Add(double width, double height, int quantety, DateTime date)
        {
            var box = new Box(width, height, quantety, date);
            return Add(box);
        }
        public int Add(double width, double height, int quantety)
        {
            return Add(width, height, quantety, DateTime.Now);
        }
        public int RemoveBoxes(double width, double height, int quantity) //Contin here!!!
        //Return how many boxes removed
        {
            if (width <= 0 || height <= 0 || quantity <= 0)
                return -1;
            var Xnode = MainTree.FindNode(width);
            if (Xnode == null)
                return -1;
            var Ynode = Xnode.Value.FindNode(height);
            if (Ynode == null)
                return -1;
            DatesQueue.Remove(Ynode.Value);
            if (Ynode.Value.Count > quantity)
                Ynode.Value.Count -= quantity;
            else if (Ynode.Value.Count == quantity)
                Xnode.Value.Remove(Ynode);
            else
            {
                var count = Ynode.Value.Count;
                Xnode.Value.Remove(Ynode);
                return count;
            }
            if (Ynode != null && Ynode.Value.Count < MinBoxesPerSize)
                throw new NotImplementedException();
            return quantity;
        }
        public void ActionOnBoxes(Action<Box> act, Order ord)
        {
            if (MainTree.IsEmpty())
                return;
            foreach (var val in MainTree.GetEnumerator(ord))
                if (val is BST<double, Box> YTree)
                    foreach (Box box in YTree.GetEnumerator(ord))
                        act(box);
        }

        public int GetOffer(Action<Box> act, double width, double height, int quantity)
        //Get all fitting boxes
        {
            if (width > 0 && height > 0 && quantity > 0)
            {
                BST<double, BST<double, Box>> KeyTree = MainTree.GetTreeByMinKey(width);
                foreach (var val in KeyTree.GetEnumerator(Order.InOrderV))
                {
                    if (val is BST<double, Box> ValTreee)
                    {
                        ValTreee = ValTreee.GetTreeByMinKey(height);
                        foreach (Box box in ValTreee.GetEnumerator(Order.InOrderV))
                        {
                            for (int i = box.Count; quantity > 0 && i > 0; i--)
                            {
                                act(box);
                                quantity--;
                            }
                        }
                    }
                    if (quantity <= 0)
                        break;
                }
                return quantity;
            }
            return -1;
        }
        public int GetBestInRange(Action<Box> act, double width, double height, int quantity)
        //Get all fitting boxes in range of LimitPercentage
        {
            if (width > 0 && height > 0 && quantity > 0)
            {
                BST<double, BST<double, Box>> KeyTree = MainTree.GetTreeByRange(width, width * (1 + LimitPercentage));
                if (KeyTree == null)
                    return quantity;
                foreach (var val in KeyTree.GetEnumerator(Order.InOrderV))
                {
                    if (val is BST<double, Box> ValTreee)
                    {
                        ValTreee = ValTreee.GetTreeByRange(height, height * (1 + LimitPercentage));
                        foreach (Box box in ValTreee.GetEnumerator(Order.InOrderV))
                        {
                            for (int i = box.Count; quantity > 0 && i > 0; i--)
                            {
                                act(box);
                                quantity--;
                            }
                        }
                    }
                    if (quantity <= 0)
                        break;
                }
                return quantity;
            }
            return -1;
        }

        public IEnumerable GetAll()
        {
            foreach (var val in MainTree.GetEnumerator(Order.InOrderV))
                if (val is BST<double, Box> YTree)
                    foreach (Box box in YTree.GetEnumerator(Order.InOrderV))
                        yield return box;
        }
    }
}
