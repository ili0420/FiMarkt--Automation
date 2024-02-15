
using Fimarkt.Services;
using Fimarkt.Models;
using System.Collections.ObjectModel;

namespace Fimarkt;

public partial class TeamSearch : ContentPage
{
	FimarktService userService;
    ObservableCollection<Team> team;
    public TeamSearch()
	{
		InitializeComponent();
        team = new ObservableCollection<Team>();
        lstteam.ItemsSource = team;
        userService = new FimarktService();
    }

    

    private async void SearchofTeam_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (String.IsNullOrEmpty(e.NewTextValue))
        {
            lstteam.ItemsSource = null;
        }
        else
        {
            var gelenLigler = await userService.SearchTeam(e.NewTextValue);
            lstteam.ItemsSource = gelenLigler;
        }
    }
}