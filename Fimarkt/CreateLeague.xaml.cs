using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace Fimarkt;

public partial class CreateLeague : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<League> leagues;
    public ICommand LigSilCommand { get; set; }
    public CreateLeague()
	{
        InitializeComponent();
        leagues = new ObservableCollection<League>();
        lstLeague.ItemsSource = leagues;
        fimarktService = new FimarktService();

        LigSilCommand = new Command<League>(async (League seciliLig) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu ligi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigSil(seciliLig.leagueId);
                await GetLeagues();
            }

        });
    }

    private async void pickFile_Clicked(object sender, EventArgs e)
    {
        PickOptions options = new PickOptions();
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                   result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);
                    string fullpathName = Path.Combine(result.FullPath, result.FileName);
                    imageSource.Text = fullpathName;
                }

            }
        }
        catch (Exception ex)
        {

        }
    }
    private async Task UlkeId(string countryName)
    {
        var id = await fimarktService.GetUlkeId(countryName);
        lblCountry.Text = id.ToString();
    }

    private async Task GetLeagues()
    {
        var sonuc = await fimarktService.GetLeague();
        leagues.Clear();
        foreach (var item in sonuc)
        {
            leagues.Add(item);
        }
    }
    private async void pickCupFile_Clicked(object sender, EventArgs e)
    {
        PickOptions options = new PickOptions();
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                   result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);
                    string fullpathName = Path.Combine(result.FullPath, result.FileName);
                    cupImageSource.Text = fullpathName;
                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    private async Task GetCountryNames()
    {
        var sonuc = await fimarktService.GetCountryName();

        originPicker.ItemsSource = sonuc;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetCountryNames();



    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var league = new League()
        {
            leagueName = leagueName.Text,
            leagueImageUrl = imageSource.Text,
            cupImageUrl = cupImageSource.Text,
            countryId = Convert.ToInt32(lblCountry.Text)
        };

        await fimarktService.LigEkle(league);
        await GetLeagues();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetLeagues();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliLig = (League)lstLeague.SelectedItem;
        if (seciliLig != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu ligi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigSil(seciliLig.leagueId);
                await GetLeagues();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliLig = (League)lstLeague.SelectedItem;
        if (seciliLig != null)
        {
            var league = new League()
            {
                leagueId = seciliLig.leagueId,
                leagueName= leagueName.Text,
                leagueImageUrl = imageSource.Text,
                cupImageUrl = cupImageSource.Text,
                countryId = Convert.ToInt32(lblCountry.Text)
            };

            await fimarktService.LigGuncelle(league);
            await GetLeagues();
        }
    }

    private void originPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ulkeAd = originPicker.SelectedItem;
        UlkeId(ulkeAd.ToString());
    }

    private void lstLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliLig = (League)lstLeague.SelectedItem;
        if (seciliLig != null)
        {
            lstLeague.BackgroundColor = Colors.Bisque;
            leagueName.Text = seciliLig.leagueName;
            imageSource.Text = seciliLig.leagueImageUrl;
            cupImageSource.Text = seciliLig.cupImageUrl;
        }
    }
}