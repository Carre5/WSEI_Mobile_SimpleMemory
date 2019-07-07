using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MemoryFinal
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Game : ContentPage
	{
        public Game()
        {
            InitializeComponent();
            ShuffleArray();
            buildBoard();
        }
        Stopwatch sw = new Stopwatch();

        string[] boardValues = new string[16]
        {
            "jeden","dwa","trzy","cztery","pięć","sześć","siedem","osiem",
            "jeden","dwa","trzy","cztery","pięć","sześć","siedem","osiem"
        };

        List<Button> Buttons = new List<Button>();

        bool wasAnotherFlipped = false;
        int flippedCardIndex = 0;

        int gamePoints = 0;

        private void buildBoard()
        {
            var grid = new Grid();
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            grid.VerticalOptions = LayoutOptions.FillAndExpand;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            int rowID = 0;
            int colID = 0;

            for (int i = 0; i < boardValues.Length; i++)
            {
                if (i % 4 == 0)
                {
                    rowID++;
                    colID = 0;
                }

                Button btn = new Button();
                btn.Text = "?";
                btn.VerticalOptions = LayoutOptions.FillAndExpand;
                btn.HorizontalOptions = LayoutOptions.FillAndExpand;
                btn.Clicked += new EventHandler(flipCard);

                Buttons.Add(btn);

                grid.Children.Add(btn, rowID, colID);
                colID++;
            }

            Board.Children.Add(grid);
            sw.Start();
        }

        private void ShuffleArray()
        {
            Random gen = new Random();
            for (int i = 0; i < 10; i++)
            {
                int firstVal = gen.Next(0, 15);
                int secondVal = gen.Next(0, 15);

                string pom = boardValues[firstVal];
                boardValues[firstVal] = boardValues[secondVal];
                boardValues[secondVal] = pom;
            }
        }

        private void flipCard(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int btnIndex = findClickedButtonIndex(btn);
            btn.Text = boardValues[btnIndex];

            if (wasAnotherFlipped && Buttons[flippedCardIndex] != btn)
            {
                if (gamePoints != 7)
                {
                    if (boardValues[flippedCardIndex] == boardValues[btnIndex])
                    {
                        Thread.Sleep(200);
                        Buttons[flippedCardIndex].IsVisible = false;
                        btn.IsVisible = false;
                        gamePoints++;
                    }
                    else
                    {
                        Thread.Sleep(200);
                        Buttons[flippedCardIndex].Text = "?";
                        btn.Text = "?";
                    }
                    flippedCardIndex = 0;
                }
                else
                {
                    Buttons[flippedCardIndex].IsVisible = false;
                    btn.IsVisible = false;
                    sw.Stop();
                    winGame();
                }

                wasAnotherFlipped = false;
            }
            else
            {
                btn.Text = boardValues[btnIndex];
                flippedCardIndex = btnIndex;
                wasAnotherFlipped = true;
            }
        }

        private int findClickedButtonIndex(Button clicked)
        {
            int i = 0;
            while (clicked != Buttons[i]) { i++; }
            return i;
        }

        private async void winGame()
        {
            saveData();
            DisplayAlert("Gratulacje!", String.Format("Wygrałeś! Czas w sekundach: {0}", (sw.ElapsedMilliseconds / 1000)), "Super");
            await Navigation.PushAsync(new MainPage());
        }

        private void saveData()
        {
            if(DataProperties.AppProperties["winTimes"].ToString().Length == 0)
            {
                DataProperties.AppProperties["winTimes"] = (sw.ElapsedMilliseconds / 1000).ToString();
            }
            else
            {
                DataProperties.AppProperties["winTimes"] += ';' + (sw.ElapsedMilliseconds / 1000).ToString();
            }
        }
    }
}