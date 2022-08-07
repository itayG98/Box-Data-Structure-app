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
        public int Remained { get => _remained; private set => _remained = value; }

        public Logic()
        {
            store = new Store();
            _boxesOffer = new MyQueue<Box>();
        }

        public void GetOffer(double x, double y, int quantity) 
        {
            foreach (var b in store.GetBestInRange(x, y, quantity))
                _boxesOffer.Add(b);
            Remained = quantity- _boxesOffer.Length;
        } 

        public void Remove(double x, double y, int quantity)=> Remained = store.RemoveBoxes(x, y, quantity);

        public void Add(double x, double y, int quantity) => Remained = store.Add(x, y, quantity);

    }
}
