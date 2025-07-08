
using System.Threading.Tasks;

namespace MAUI_MobileApp;

public class NewPage2 : ContentPage
{
	public NewPage2()
	{

		var button = new Button { Text = "Go Back Home" };
		button.Clicked += Button_Clicked;
		Content = new VerticalStackLayout
		{
			Children = {
				button,
				new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to .NET MAUI! from code"
				}
			}
		};
	}

    private async void Button_Clicked(object? sender, EventArgs e)
    {
#if ANDROID
await DisplayAlert("Androidziasz", "Heh", "OK");
#elif WINDOWS
await DisplayAlert("Pececiarz", "Lol", "OK");
#endif
        await Shell.Current.GoToAsync("..");
	}
}