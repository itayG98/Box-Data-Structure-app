using DataStructure;
using System;
using System.Collections.Generic;
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
            DatesQueue.QuickSort();
        }
        /// <summary>
        /// Return the boxes count which were not added
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>

        //--------------------------------------------------------------------------------------

        /// <summary>
        ///  Adding boxes from the DB in the initialiation
        ///</summary>
        private int Add(Box box)
        {
            int returnedBoxes = 0;
            //Check if box data is valid
            if (box == null || box.Count <= 0) return 0;
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
                    if (box.Node == null) //If the box doesnt have node field
                        DatesQueue.Remove(box.Node);
                    else
                        DatesQueue.Remove(box);
                    if (Ynode.Value.Count >= MAX_BOXES_PER_SIZE) //If already too much boxes
                        returnedBoxes = box.Count;
                    if (Ynode.Value.Count + box.Count >= MAX_BOXES_PER_SIZE) //If sum of current and added boxes greater than maximum
                    {
                        returnedBoxes += Ynode.Value.Count + box.Count - MAX_BOXES_PER_SIZE;
                        Ynode.Value.Count = MAX_BOXES_PER_SIZE;
                    }
                    else //Adding the boxes count regulary
                        Ynode.Value.Count += box.Count;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <param name="date"></param>
        /// <returns>Amount of boxes which returned</returns>
        internal int Add(double width, double height, int quantity, DateTime date)
        {
            Box box;
            int returnedBoxes = 0;
            //Check if box data is valid
            if (quantity <= 0 || height <= 0 || width <= 0)
                return 0;
            //lower to max amount
            if (quantity > MAX_BOXES_PER_SIZE)
            {
                returnedBoxes += quantity - MAX_BOXES_PER_SIZE;
                quantity = MAX_BOXES_PER_SIZE;
            }
            var Xnode = MainTree.FindNode(width);
            if (Xnode != null)//Found x dim
            {
                var Ynode = Xnode.Value.FindNode(height);
                if (Ynode != null) //Found y dim   => Ynode.Value==box to update
                {
                    box = Ynode.Value;
                    if (Ynode.Value.Node != null) //If the box doesnt have node field
                        DatesQueue.Remove(Ynode.Value);
                    else
                        DatesQueue.Remove(Ynode.Value.Node);
                    if (Ynode.Value.Count >= MAX_BOXES_PER_SIZE) //If already too much boxes
                    {
                        Ynode.Value.Date = DateTime.Now;
                        returnedBoxes += quantity;
                    }
                    if (Ynode.Value.Count + quantity >= MAX_BOXES_PER_SIZE) //If sum of current and added boxes greater than maximum
                    {
                        int prevCount = Ynode.Value.Count;
                        Ynode.Value.Count = MAX_BOXES_PER_SIZE;
                        returnedBoxes= prevCount + quantity - MAX_BOXES_PER_SIZE + returnedBoxes;
                    }
                    else //Adding the boxes regulary
                    {
                        Ynode.Value.Count += quantity;
                        Ynode.Value.Date = date;
                        Ynode.Value.Node = DatesQueue.Add(Ynode.Value);
                        returnedBoxes= 0;
                    }
                }
                else //Creating new inner tree
                {
                    box = new Box(width, height, quantity);
                    DatesQueue.Remove(box);
                    Xnode.Value.AddNode(width, box);
                }
            }
            else //Creating new node in mainTree and inner tree
            {
                box = new Box(width, height, quantity);
                BST<double, Box> YTree = new BST<double, Box>(height, box);
                MainTree.AddNode(width, YTree);
            }
            box.Node =DatesQueue.Add(box);
            return returnedBoxes;
        }

        /// <summary>
        /// Adding new boxes from the requested type
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <param name="date initialise to current date time"></param>
        /// <returns>Amount of boxes which returned</returns>
        public int Add(double width, double height, int quantety) => Add(width, height, quantety, DateTime.Now);


        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Return the box instance or null if couldnt find it
        /// </summary>
        /// <param name="box"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Box RemoveBoxes(Box box, int quantity)
        {
            //Search the box in both trees
            if (box.Width <= 0 || box.Height <= 0 || box.Count < 1)
                return null;
            var Xnode = MainTree.FindNode(box.Width);
            if (Xnode == null)
                return null;
            var Ynode = Xnode.Value.FindNode(box.Height);
            if (Ynode == null)
                return null;

            DatesQueue.Remove(box);
            if (box.Count > quantity)
            {
                box.Count -= quantity;
                box.Date = DateTime.Now;
                box.Node = DatesQueue.Add(Ynode.Value); //Update the queue
            }
            else if (box.Count <= quantity)
            {
                Xnode.Value.Remove(Ynode);
                box.Count = 0;
            }
            if (Xnode.Value.IsEmpty())
                MainTree.Remove(Xnode);
            return box;
        }
        /// <summary>
        /// Return how many boxes removed
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public int RemoveBoxes(double width, double height, int quantity)
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
                Xnode.Value.Remove(Ynode);
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
        //--------------------------------------------------------------------------------------
        public void ActionOnBoxes(Action<Box> act, Order ord)
        {
            if (MainTree.IsEmpty())
                return;
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(ord))
                foreach (Box box in YTree.GetEnumerator(ord))
                    act(box);
        }
        /// <summary>
        /// Do action for all fitting boxes in range of LimitPercentage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        /// 
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


        //--------------------------------------------------------------------------------------
        public IEnumerable<Box> GetBestOffer(double width, double height, int quantity)
        {
            int temp;
            if (width > 0 && height > 0 && quantity > 0)
            {
                foreach (BST<double, Box> Ytree in MainTree.GetNextTreeByRange(width, width * (1 + LIMIT_PERCENTAGE / 100)))
                {
                    foreach (Box b in Ytree.GetNextTreeByRange(height, height * (1 + LIMIT_PERCENTAGE / 100)))
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
        public IEnumerable GetandPopOldBoxes()
        {
            foreach (Box box in GetQueue())
            {
                if (box != null && box.LastPurchased >= MAX_DAYS)
                    yield return RemoveBoxes(box, box.Count);
            }
        }
        public IEnumerable GetAll()
        {
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(Order.InOrderV))
                foreach (Box box in YTree.GetEnumerator(Order.InOrderV))
                    yield return box;
        }

        /// <summary>
        /// Get oldest boxes first
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetQueue() => DatesQueue.GetQueueRootFirstByValue();
    }
}
