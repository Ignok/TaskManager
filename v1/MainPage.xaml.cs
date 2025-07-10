namespace MAUI_MobileApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NewPage2");
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var hasInternet =
                Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

            DisplayAlert("Has Internet?", $"{hasInternet}", "OK");
        }
    }
}
