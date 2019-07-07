using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MemoryFinal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            fillRecords();
        }
        private void fillRecords()
        {
            string[] wins = DataProperties.AppProperties["winTimes"].ToString().Split(';');

            foreach (var item in wins)
            {
                Label label = new Label()
                {
                    Text = item+" sekund",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                winBoard.Children.Add(label);
            }
        }

        private async Task Button_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Game());
        }
    }
}
