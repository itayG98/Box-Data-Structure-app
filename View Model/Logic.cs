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
        private MyQueue<Box> _boxesOffer;
        private int _remained;
        public Store store;
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable BoxesOffer { get { return _boxesOffer.GetQueue(); } }
        public int Remained { get => _remained; private set => _remained = value>0? value :0 ; }

        public Logic()
        {
            store = new Store();
            _boxesOffer = new MyQueue<Box>();
            Remained = 0;
        }

        public void GetOfferEfficintely(double x, double y, int quantity) 
        {
            _boxesOffer.Empty();
            foreach (Box b in store.GetBest(x, y, quantity))
                _boxesOffer.Add(b);
            Remained = quantity - _boxesOffer.Length;
        }
        public void GetOffer(double x, double y, int quantity)
        {
            _boxesOffer.Empty();
            foreach (var b in store.GetBestInRange(x, y, quantity))
                _boxesOffer.Add(b);
            Remained = quantity - _boxesOffer.Length;
        }
        public void TakeOffer(IEnumerable boxes)
        {
            foreach (Box b in boxes)
            {
                _boxesOffer.Remove(b);
                store.RemoveBoxes(b.Width,b.Height,b.Count);
                Remained--;
            }
        }
        public void Remove(double x, double y, int quantity) => Remained = store.RemoveBoxes(x, y, quantity);

        public void Add(double x, double y, int quantity) => Remained = store.Add(x, y, quantity);

    }
}
