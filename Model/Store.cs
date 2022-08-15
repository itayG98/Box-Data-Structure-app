using DataStructure;
using System;
using System.Collections.Generic;
using MoockData;
using System.Collections;

namespace Model
{
    /// <summary>
    /// A class represent a box store
    /// </summary>
    public class Store : IEnumerable
    {
        /// <summary>
        ///<param name="MainTree"> A double dimention BST representing box data</param>
        ///<param name="DatesQueue">A queue of last to first purchased or inserted boxes </param>
        /// <param name="_data">Singeltone Datamock loading init data to the program </param>
        /// </summary>
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
            if (box == null || box.Count <= 0) return box.Count;
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
                    if (box.Node != null)           //Remove form queue
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
            if (quantity <= 0 || width <= 0 || height <= 0) //Check if box data is valid
                return quantity;

            if (quantity > MAX_BOXES_PER_SIZE) //lower to max amount
            {
                returnedBoxes += quantity - MAX_BOXES_PER_SIZE;
                quantity = MAX_BOXES_PER_SIZE;
            }
            var Xnode = MainTree.FindNode(width);
            if (Xnode != null)  //Found x dim
            {
                var Ynode = Xnode.Value.FindNode(height);
                if (Ynode != null)  //Found y dim   => Ynode.Value==box to update
                {
                    box = Ynode.Value;
                    box.Date = date;
                    DatesQueue.Remove(box.Node);
                    if (Ynode.Value.Count >= MAX_BOXES_PER_SIZE) //If already too much boxes
                    {
                        returnedBoxes += quantity;
                    }
                    else if (box.Count + quantity >= MAX_BOXES_PER_SIZE) //If sum of current and added boxes greater than maximum
                    {
                        int prevCount = Ynode.Value.Count;
                        box.Count = MAX_BOXES_PER_SIZE;
                        returnedBoxes += prevCount + quantity - MAX_BOXES_PER_SIZE;
                    }
                    else //Adding the boxes regulary
                    {
                        box.Count += quantity;
                    }
                }
                else //Creating new inner tree
                {
                    box = new Box(width, height, quantity, date);
                    Xnode.Value.AddNode(height, box);
                }
            }
            else //Creating new node in mainTree and inner tree
            {
                box = new Box(width, height, quantity, date);
                BST<double, Box> YTree = new BST<double, Box>(height, box);
                MainTree.AddNode(width, YTree);
            }
            box.Node = DatesQueue.Add(box);
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
            if (box.Width <= 0 || box.Height <= 0 || box.Count < 1 || box == null)
                return null;
            var Xnode = MainTree.FindNode(box.Width);
            if (Xnode == null)
                return null;
            var Ynode = Xnode.Value.FindNode(box.Height);
            if (Ynode == null)
                return null;

            if (box.Node != null)           //Remove form queue
                DatesQueue.Remove(box.Node);
            else
                DatesQueue.Remove(box);
            if (box.Count > quantity)
            {
                box.Count -= quantity;
                box.Date = DateTime.Now;
                box.Node = DatesQueue.Add(Ynode.Value); //Update the queue if removed only part of the boxes
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
            Box box;
            if (width <= 0 || height <= 0 || quantity < 1)
                return 0;
            var Xnode = MainTree.FindNode(width);
            if (Xnode == null)
                return 0;
            var Ynode = Xnode.Value.FindNode(height);
            if (Ynode == null)
                return 0;

            box = Ynode.Value;
            if (box.Node != null)           //Remove form queue
                DatesQueue.Remove(box.Node);
            else
                DatesQueue.Remove(box);
            if (Ynode.Value.Count > quantity)
            {
                box.Count -= quantity;
                box.Date = DateTime.Now;
                box.Node = DatesQueue.Add(Ynode.Value); //Update the queue
            }
            else if (box.Count == quantity)
                Xnode.Value.Remove(Ynode);
            else
            {
                var count = box.Count;
                Xnode.Value.Remove(Ynode);
                quantity = count;
            }
            if (Xnode.Value.IsEmpty())
                MainTree.Remove(Xnode);
            return quantity;
        }
        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Do action for all fitting boxes in range of LimitPercentage
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        /// 
        public void ActionOnBoxes(Action<Box> act, Order ord)
        {
            if (MainTree.IsEmpty())
                return;
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(ord))
                foreach (Box box in YTree.GetEnumerator(ord))
                    act(box);
        }


        //--------------------------------------------------------------------------------------
        /// <summary>
        /// Enumerate over the Main BST and return fitting boxes from closer to far
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Remove and return the old boxes
        /// </summary>
        /// <returns>IEnumerable of the poped boxes </returns>
        public IEnumerable GetandPopOldBoxes()
        {
            foreach (Box box in GetQueueOldFirst())
            {
                if (box != null && box.LastPurchased >= MAX_DAYS)
                    yield return RemoveBoxes(box, box.Count);
            }
        }
        /// <summary>
        /// Get all the Boxes in size order width first
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetAll()
        {
            foreach (BST<double, Box> YTree in MainTree.GetEnumerator(Order.InOrderV))
                foreach (Box box in YTree.GetEnumerator(Order.InOrderV))
                    yield return box;
        }

        /// <summary>
        /// Get oldest boxes first
        /// </summary>
        /// <returns> IEnumerable of boxes descending order </returns>
        public IEnumerable GetQueueOldFirst() => DatesQueue.GetQueueRootFirstByValue();

        public IEnumerable GetQueueNewFirst() => DatesQueue.GetQueueTalisFirstValue();

        public IEnumerator GetEnumerator()
        {
            yield return GetAll();
        }
    }
}
