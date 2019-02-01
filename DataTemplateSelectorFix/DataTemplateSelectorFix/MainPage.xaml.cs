using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataTemplateSelectorFix
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = 0;
            Start();
        }

        async void Start()
        {
            while (true)
            {
                BindingContext = ((int)BindingContext) + 1;
                await Task.Delay(2000);
            }
        }
    }
}
