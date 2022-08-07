using View_Model;
using Windows.UI.Xaml.Controls;
using DataStructure;
using System.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Boxes_Store_app
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Logic logic;

        public MainPage()
        {
            this.InitializeComponent();
            logic = new Logic();
            Submit.Click += Submit_Click;
            Add.Click += Add_Click;
            Remove.Click += Remove_Click;
        }

        private void Remove_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Add_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Submit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!double.TryParse(X.Text, out double x))
                return;
            if (!double.TryParse(X.Text, out double y))
                return;
            if (!int.TryParse(Quantity.Text, out int q))
                return ;
            logic.GetOffer(x,y,q);
        }
        private void TextBox_OnBeforeTextChanging(TextBox sender,TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
