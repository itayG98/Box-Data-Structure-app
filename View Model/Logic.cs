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
        private IEnumerable<Box> boxesOffer;
        public Store store;
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable<Box> BoxesOffer { get {return boxesOffer; } }

        public Logic()
        {
            store = new Store();
        }
    }
}
