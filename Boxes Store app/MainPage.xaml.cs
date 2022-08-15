using View_Model;
using Windows.UI.Xaml.Controls;
using DataStructure;
using System.Linq;
using Model;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.Globalization;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Boxes_Store_app
{
    /// <summary>
    /// An The main page of the boxes store 
    /// Handle the store using logic.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Logic logic;

        public MainPage()
        {
            this.InitializeComponent();
            logic = new Logic(Update);
            GetOffer.Click += GetOffer_Click;
            Add.Click += Add_Click;
            Remove.Click += Remove_Click;
            TakeOffer.Click += TakeOffer_Click;
            Avilable_Item.IsItemClickEnabled = true;
            Avilable_Item.ItemClick += Avilable_Item_ItemClick;
            Queue.IsItemClickEnabled = true;
            Queue.ItemClick += Queue_ItemClick;
        }


        public void Update()
        {
            Avilable_Item.ItemsSource = logic.Boxes;
            Queue.ItemsSource = logic.DatesQueue;
            Offer.ItemsSource = logic.BoxesOffer;
        }

        /// <summary>
        /// Get offer of the boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Avilable_Item_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Box box)
            {
                X.Text = box.Width.ToString();
                Y.Text = box.Height.ToString();
                Quantity.Text = box.Count.ToString();
                GetOffer_Click(GetOffer, e);
            }
        }
        /// <summary>
        /// Get offer of the boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queue_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Box box)
            {
                X.Text = box.Width.ToString();
                Y.Text = box.Height.ToString();
                Quantity.Text = box.Count.ToString();
                GetOffer_Click(GetOffer, e);
            }
        }
        /// <summary>
        /// Get offer of the boxes with the parameters that submited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOffer_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(X.Text, out double x) || x < 0)
                return;
            if (!double.TryParse(Y.Text, out double y) || y < 0)
                return;
            if (!int.TryParse(Quantity.Text, out int q) || q <= 0)
                return;
            logic.AmountRequested = q;
            logic.GetOfferEfficintely(x, y);
            Offer.ItemsSource = logic.BoxesOffer;
            Offer.SelectAll();
        }


        private void TakeOffer_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog WarMmsgDial = new MessageDialog(String.Empty, "Warning");
            logic.TakeOffer(Offer.SelectedItems, WarMmsgDial);
            ShowMessegeDialog(logic.Msg, "Recipt");
            if (WarMmsgDial.Content.Length > 0)
                WarMmsgDial.ShowAsync();
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog Removal = new MessageDialog(String.Empty, "Remove");
            if (!double.TryParse(X.Text, out double x) || x < 0)
                return;
            if (!double.TryParse(Y.Text, out double y) || y < 0)
                return;
            if (!int.TryParse(Quantity.Text, out int q) || q <= 0)
                return;
            logic.Remove(x, y, q, Removal);
            if (Removal.Content.Length > 0)
                Removal.ShowAsync();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog TooManyAlert = new MessageDialog(String.Empty, "Too many boxes");
            if (!double.TryParse(X.Text, out double x) || x < 0)
                return;
            if (!double.TryParse(Y.Text, out double y) || y < 0)
                return;
            if (!int.TryParse(Quantity.Text, out int q) || q <= 0)
                return;
            logic.Add(x, y, q, TooManyAlert);
            if (TooManyAlert.Content.Length > 0)
                TooManyAlert.ShowAsync();
        }

        /// <summary>
        /// Can only assign digits and floating point
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');
        }
        /// <summary>
        /// Can noly assign digits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TextBox_OnBeforeTextChangingInt(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        public async void ShowMessegeDialog(string msg, string header)
        {
            MessageDialog msgDialgo = new MessageDialog(msg, header);
            await msgDialgo.ShowAsync();
        }
    }

}
