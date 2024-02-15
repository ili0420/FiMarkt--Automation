using Fimarkt.Models;
using Fimarkt.Services;
namespace Fimarkt;

public partial class UserPage : ContentPage
{
    IFimarktService fimarktService;
    public UserPage()
	{
		InitializeComponent();
        fimarktService = new FimarktService();
    }
    private async void btnLigSearch_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LeagueSearch());
    }

    private async void btnTeamSearch_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TeamSearch());
    }

    private async void btnCountrySearch_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CountrySearch());
    }

    private async void btnBallerSearch_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BallerSearch());
    }

    private async void btnLogOut_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }

    private async void changePassword_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePassword());
    }
}