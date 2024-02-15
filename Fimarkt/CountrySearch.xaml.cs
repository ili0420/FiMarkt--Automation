using Fimarkt.Services;
using Fimarkt.Models;
using System.Collections.ObjectModel;
namespace Fimarkt;

public partial class CountrySearch : ContentPage
{
    FimarktService userService;
    ObservableCollection<Country> country;
    public CountrySearch()
	{
		InitializeComponent();
        country = new ObservableCollection<Country>();
        lstCountry.ItemsSource = country;
        userService = new FimarktService();
    }
    private async void CountrySearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (String.IsNullOrEmpty(e.NewTextValue))
        {
            lstCountry.ItemsSource = null;
        }
        else
        {
            var gelenLigler = await userService.SearchCountry(e.NewTextValue);
            lstCountry.ItemsSource = gelenLigler;
        }
    }
}