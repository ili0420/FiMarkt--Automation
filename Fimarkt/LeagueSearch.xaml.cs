using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;

namespace Fimarkt;

public partial class LeagueSearch : ContentPage
{
    FimarktService userService;
    ObservableCollection<TeamSearchViewModel> league;
    public LeagueSearch()
	{
		InitializeComponent();
        league = new ObservableCollection<TeamSearchViewModel>();
        lstLeagues.ItemsSource = league;
        userService = new FimarktService();
    }
    private async void LeaguesSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (String.IsNullOrEmpty(e.NewTextValue))
        {
            lstLeagues.ItemsSource = null;
        }
        else
        {
            var gelenLigler = await userService.SearchLig(e.NewTextValue);
            lstLeagues.ItemsSource = gelenLigler;
        }
    }
}