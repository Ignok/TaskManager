﻿namespace MAUI_MobileApp.ViewModel
{
    [QueryProperty("Monkey", "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        IMap map;
        public MonkeyDetailsViewModel(IMap map)
        {
            this.map = map;
        }

        [ObservableProperty]
        Monkey monkey;

        [RelayCommand]
        async Task OpenMapAsync()
        {
            try
            {
                await map.OpenAsync(Monkey.Latitude, Monkey.Longitude,
                    new MapLaunchOptions
                    {
                        Name = Monkey.Name,
                        NavigationMode = NavigationMode.None
                    });
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);

                //this later will be in interface
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to open map: {ex.Message}", "OK");
            }
        }
    }
}
