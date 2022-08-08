﻿using DataStructure;
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
        private MyQueue<Box> DatesQueue;
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
            //DatesQueue.Sort();
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
        internal int Add(double width, double height, int quantety, DateTime date)
        {
            var box = new Box(width, height, quantety, date);
            return Add(box);
        }
        public int Add(double width, double height, int quantety)
        {
            return Add(width, height, quantety, DateTime.Now);
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

            DatesQueue.Remove(new Box(width,height,quantity));
            if (Ynode.Value.Count > quantity)
            {
                Ynode.Value.Count -= quantity;
                Ynode.Value.Date = DateTime.Now;
                DatesQueue.Add(Ynode.Value); //Update the queue
            }
            else if (Ynode.Value.Count == quantity)
                Xnode.Value.Remove(Ynode);
            else
            {
                var count = Ynode.Value.Count;
                Xnode.Value.Remove(Ynode);
                //throw new NotImplementedException(); //Alert if toke all boxes quantity
                return count;
            }
            if (Ynode != null && Ynode.Value.Count < MinBoxesPerSize) { }
            //throw new NotImplementedException(); //Alert if minimal quantity
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
        //Do action for all fitting boxes in range of LimitPercentage
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

        public IEnumerable<Box> GetBestInRange(double width, double height, int quantity)
        //Do action for all fitting boxes in range of LimitPercentage
        {
            if (width > 0 && height > 0 && quantity > 0)
            {
                BST<double, BST<double, Box>> KeyTree = MainTree.GetTreeByRange(width, width * (1 + LimitPercentage));
                if (KeyTree != null)
                {
                    foreach (var val in KeyTree.GetEnumerator(Order.InOrderV))
                    {
                        if (val is BST<double, Box> ValTreee)
                        {
                            ValTreee = ValTreee.GetTreeByRange(height, height * (1 + LimitPercentage));
                            foreach (Box box in ValTreee.GetEnumerator(Order.InOrderV))
                            {
                                for (int i = box.Count; quantity > 0 && i > 0; i--)
                                {
                                    yield return box;
                                    quantity--;
                                }
                            }
                        }
                        if (quantity <= 0)
                            break;
                    }
                }
            }
        }

        public IEnumerable<Box> GetBest(double width, double height, int quantity)
        //Do action for all fitting boxes in range of LimitPercentage
        {
            int temp;
            if (width > 0 && height > 0 && quantity > 0)
            {
                foreach (BST<double, Box> Ytree in MainTree.GetNextTreeByRange(width, width * (1 + LimitPercentage)))
                {
                    foreach (Box b in Ytree.GetNextTreeByRange(height, height * (1 + LimitPercentage)))
                    {
                        temp = quantity < b.Count ? quantity : b.Count;
                        quantity -= temp;
                        yield return new Box(b.Width, b.Height, temp);
                        if (quantity < 1)
                            break;
                    }
                    if (quantity < 1)
                        break;
                }
            }
        }

        public IEnumerable GetAll()
        {
            foreach (var val in MainTree.GetEnumerator(Order.InOrderV))
                if (val is BST<double, Box> YTree)
                    foreach (var b in YTree.GetEnumerator(Order.InOrderV))
                        if (b is Box box)
                            yield return box;
        }
        public IEnumerable GetQueue()
        {
            return DatesQueue.GetQueue();
        }
    }
}
