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
        private IEnumerable boxesOffer;
        private int _remained;
        public Store store;
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable BoxesOffer { get {return boxesOffer; } }

        public int Remained { get => _remained; private set => _remained = value; }

        public Logic()
        {
            store = new Store();
        }

        public void GetOffer(int x , int y, int quantity) => boxesOffer = store.GetBestInRange(x, y, quantity);
    }
}
