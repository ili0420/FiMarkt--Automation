using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;

namespace Fimarkt;

public partial class BallerSearch : ContentPage
{
	FimarktService userService;
	ObservableCollection<Baller> baller;
	public BallerSearch()
	{

		InitializeComponent();
        baller = new ObservableCollection<Baller>();
        lstBaller.ItemsSource = baller;
        userService = new FimarktService();
    }
    private async void BallerSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (String.IsNullOrEmpty(e.NewTextValue))
        {
            lstBaller.ItemsSource = null;
        }
        else
        {
            var gelenLigler = await userService.SearchBaller(e.NewTextValue);
            lstBaller.ItemsSource = gelenLigler;
        }
    }
}