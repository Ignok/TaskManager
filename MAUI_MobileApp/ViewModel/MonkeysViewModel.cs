using MAUI_MobileApp.Services;
using Microsoft.Maui.ApplicationModel;

namespace MAUI_MobileApp.ViewModel
{
    public partial class MonkeysViewModel : BaseViewModel
    {
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        MonkeyService monkeyService;

        IConnectivity connectivity;
        IGeolocation geolocation;

        public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
        {
            Title = "Monkey Finder";
            this.monkeyService = monkeyService;
            this.connectivity = connectivity;
            this.geolocation = geolocation;
        }

        [RelayCommand]
        async Task GetClosestMonkeyAsync()
        {
            if (IsBusy || Monkeys.Count == 0)
                return;

            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("No Geolocation Permission", "Application needs permission for geolocation", "OK");
                    return;
                }

                var location = await geolocation.GetLastKnownLocationAsync();
                if(location is null)
                {
                    location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30),

                        });
                }
                if (location is null)
                    return;

                var first = Monkeys.OrderBy(m =>
                location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers)
                ).FirstOrDefault();

                if (first is null)
                {
                    return;
                }

                await Shell.Current.DisplayAlert("Closest Monkey",
                        $"{first.Name} in {first.Location}", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                //this later will be in interface
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to get closest Monkey: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToDetailsAsync(Monkey monkey)
        {
            if (monkey is null)
                return;

            // There should be an interface later on so that we won't use Shell directly from the view model
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true,
                new Dictionary<string, object>
                {
                    {"Monkey", monkey}
                });

        }


        [RelayCommand]
        async Task GetMonkeysAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if(connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No Internet",
                        "Please check your internet connection and try again.", "OK");
                    return;
                }
                IsBusy = true;
                var monkeys = await monkeyService.GetMonkeys();

                if (Monkeys.Count != 0)
                    Monkeys.Clear();

                foreach (var monkey in monkeys)
                    Monkeys.Add(monkey);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

//this later will be in interface
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to get monkeys: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
