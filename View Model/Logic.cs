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
        private int _amount;
        private MyQueue<Box> _boxesOffer;
        public Store store;

        public int Amount { get { return _amount; } set { _amount = value >= 0 ? value : 0; } }
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable BoxesOffer { get { return _boxesOffer.GetQueue(); } }

        public Logic()
        {
            store = new Store();
            _boxesOffer = new MyQueue<Box>();
        }

        public void GetOfferEfficintely(double x, double y)
        {
            _boxesOffer.Empty();
            foreach (Box b in store.GetBestOffer(x, y, Amount))
                _boxesOffer.Add(b);
        }
        public void TakeOffer(IEnumerable boxes)
            //Test the Method
        {
            foreach (Box b in boxes)
            {
                if (Amount <= 0)
                {
                    _boxesOffer.Empty();
                    return;
                }
                if (Amount >= b.Count)
                {
                    _boxesOffer.Remove(b.Node);
                    Amount -= store.RemoveBoxes(b,b.Count);
                }
                else if (Amount < b.Count)
                {
                    _boxesOffer.Remove(b.Node);
                    Amount -= store.RemoveBoxes(b, Amount);
                }
            }
            _boxesOffer.Empty();
        }
        public void Remove(double x, double y, int quantity)
        {
            _boxesOffer.Empty();
        }
        public void Add(double x, double y, int quantity) => Amount = store.Add(x, y, quantity);

    }
}
