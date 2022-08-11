using DataStructure;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View_Model
{
    public class Logic
    {
        private string msg;
        private int _amountRequested;
        private int _remain;
        private MyQueue<Box> _boxesOffer;
        public Store store;


        public int AmountRequested { get { return _amountRequested; } set { _amountRequested = value >= 0 ? value : 0; } }
        public int Remain { get => _remain; private set { _remain = value >= 0 ? value : 0; } }
        public string Msg { get => msg; private set => msg = value; }
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable BoxesOffer { get { return _boxesOffer.GetQueueRootFirstByValue(); } }


        public Logic()
        {
            store = new Store();
            _boxesOffer = new MyQueue<Box>();
        }

        public int GetOfferEfficintely(double x, double y)
            //Return remaining boxes
        {
            Remain = AmountRequested;
            if (x < 0 && y < 0)
                return Remain;
            _boxesOffer.Empty();
            foreach (Box b in store.GetBestOffer(x, y, AmountRequested))
            {
                if (Remain == 0)
                    return Remain;
                _boxesOffer.Add(b);
                Remain -= b.Count;
            }
            return Remain;
        }
        public int TakeOffer(IEnumerable offer)
        //Return number of boxes wich taken
        {
            StringBuilder sb = new StringBuilder();
            Remain = AmountRequested;
            foreach (Box b in offer)
            {
                if (Remain <= 0)
                {
                    _boxesOffer.Empty();
                    sb.Append($"Total :{AmountRequested}");
                    Msg = sb.ToString(); 
                    return Remain;
                }
                 if (Remain >= b.Count)
                {
                    _boxesOffer.Remove(b.Node);
                    Remain -= store.RemoveBoxes(b, b.Count);
                    sb.AppendLine($"{b.Count} Boxes of width {b.Width} and height {b.Height} .");
                }
                else if (Remain < b.Count)
                {
                    _boxesOffer.Remove(b.Node);
                    Remain -= store.RemoveBoxes(b, AmountRequested);
                }
            }
            _boxesOffer.Empty();
            if (Remain>0)
                sb.Append($"Counldnt fulfill :{Remain} boxes");
            Msg = sb.ToString();
            return Remain;
        }
        public void Remove(double x, double y, int quantity)
        {
            _boxesOffer.Empty();
            store.RemoveBoxes(x, y,quantity);
        }
        public void Add(double x, double y, int quantity)
        {
            AmountRequested = store.Add(x, y, quantity);
            _boxesOffer.Empty();
        }
    }
}
