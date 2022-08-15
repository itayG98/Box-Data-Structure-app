using DataStructure;
using Model;
using System;
using System.Collections;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace View_Model
{
    /// <summary>
    /// View model class having one single store object
    /// </summary>
    public class Logic
    {
        private string recipt;
        private int _amountRequested;
        private int _remain;
        private MyQueue<Box> _boxesOffer;
        public Store store;
        DispatcherTimer dayTimer;
        private readonly Action _upDateAct;
        private readonly TimeSpan _interval ;


        public int AmountRequested { get { return _amountRequested; } set { _amountRequested = value >= 0 ? value : 0; } }
        public int Remain { get => _remain; private set { _remain = value >= 0 ? value : 0; } }
        public string Msg { get => recipt; private set => recipt = value; }
        public IEnumerable Boxes { get { return store.GetAll(); } }
        public IEnumerable DatesQueue { get { return store.GetQueue(); } }
        public IEnumerable BoxesOffer { get { return _boxesOffer.GetQueueRootFirstByValue(); } }
        public DispatcherTimer DayTimer { get => dayTimer; private set => dayTimer = value; }
        public Action UpDateAct => _upDateAct;




        public Logic(Action update)
        {
            store = new Store();
            _boxesOffer = new MyQueue<Box>();
            _upDateAct = update;
            _interval = new TimeSpan(12, 0, 0, 0);
            StartClock();
        }



        private void StartClock()
        {
            DayTimer = new DispatcherTimer() { Interval = _interval };
            DayTimer.Tick += DayTimer_Tick;
            DayTimer.Start();
        }

        /// <summary>
        /// Get the offer according to dimentions and quantity
        /// <Return> remaining boxes </Return> 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetOfferEfficintely(double x, double y)

        {
            Remain = AmountRequested;
            if (x < 0 || y < 0)
                return Remain;
            _boxesOffer.Empty();
            foreach (Box b in store.GetBestOffer(x, y, AmountRequested))
            {
                if (Remain == 0)
                    return Remain;
                _boxesOffer.Add(b);
                Remain -= b.Count;
            }
            UpDateAct.Invoke();
            return Remain;
        }

        public void RemoveOld(MessageDialog msgDial)
        {
            foreach (Box b in store.GetandPopOldBoxes())
                msgDial.Content += $"Out of {b:dim} boxes\n";
        }

        /// <summary>
        /// This method get the boxes which selected
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="msgDial"></param>
        /// <returns Return number of boxes which taken /returns>
        public int TakeOffer(IEnumerable offer, MessageDialog msgDial)
        {
            StringBuilder sb = new StringBuilder();
            Remain = AmountRequested;
            foreach (Box b in offer)
            {
                if (Remain <= 0)
                {
                    _boxesOffer.Empty();
                    sb.Append($"Total :{AmountRequested}");
                    break;
                }
                else
                {
                    int temp = b.Count;
                    _boxesOffer.Remove(b);
                    store.RemoveBoxes(b, Remain < b.Count ? Remain : b.Count); //Only subtruct needed amount
                    Remain -= temp - b.Count;
                    sb.AppendLine($"{temp - b.Count} Boxes of {b:dim}");
                }
                if (b.Count > 0 && b.Count < store.MIN_BOXES_PER_SIZE) //Append apropriate msg to the dialog of warning
                    msgDial.Content += $"Boxes of {b:dim} is below the limit! " + $"Only-{b.Count} left";
                else if (b.Count <= 0)
                    msgDial.Content += $"Out of {b:dim}" + $"{b.Count} boxes\n";
            }
            _boxesOffer.Empty();
            if (Remain > 0) //If could'nt get the amount of boxes in the request
                sb.Append($"Could not fulfill :{Remain} boxes");
            Msg = sb.ToString();
            UpDateAct.Invoke();
            return Remain;
        }

        public void Remove(double x, double y, int quantity)
        {
            _boxesOffer.Empty();
            store.RemoveBoxes(x, y, quantity);
            UpDateAct.Invoke();

        }

        public void Add(double x, double y, int quantity, MessageDialog msgDial)
        {
            Remain = store.Add(x, y, quantity);
            if (Remain > 0)
                msgDial.Content += $"Couldn't add {Remain} boxes of Width - {x} height - {y}";
            _boxesOffer.Empty();
            UpDateAct.Invoke();
        }

        /// <summary>
        /// This method remove the old boxes  each tick 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DayTimer_Tick(object sender, object e)
        {
            MessageDialog RemoveDutToTime = new MessageDialog(String.Empty, "Expired boxes");
            RemoveOld(RemoveDutToTime);
            if (RemoveDutToTime.Content.Length > 0)
                RemoveDutToTime.ShowAsync();
            UpDateAct.Invoke();
        }
    }
}
