using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Fimarkt;

public partial class CreateCountry : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<Country> countries;
    public ICommand UlkeSilCommand { get; set; }
    public CreateCountry()
	{
        fimarktService = new FimarktService();
        InitializeComponent();
        countries = new ObservableCollection<Country>();
        lstCountry.ItemsSource = countries;
        UlkeSilCommand = new Command<Country>(async (Country seciliUlke) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu ülkeyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.UlkeSil(seciliUlke.countryId);
                await GetCountries();
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

    private async Task GetCountries()
    {
        var sonuc = await fimarktService.GetCountry();
        countries.Clear();
        foreach (var item in sonuc)
        {
            countries.Add(item);
        }
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var country = new Country()
        {
            countryName = countryName.Text,
            countryImageUrl = imageSource.Text
        };

        await fimarktService.UlkeEkle(country);
        await GetCountries();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetCountries();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliUlke = (Country)lstCountry.SelectedItem;
        if (seciliUlke != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu ülkeyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.UlkeSil(seciliUlke.countryId);
                await GetCountries();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliUlke = (Country)lstCountry.SelectedItem;
        if (seciliUlke != null)
        {
            var country= new Country()
            {
                countryId = seciliUlke.countryId,
                countryName = countryName.Text,
                countryImageUrl = imageSource.Text
            };
            await fimarktService.UlkeGuncelle(country);
            await GetCountries();
        }
    }

    private void lstCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliUlke = (Country)lstCountry.SelectedItem;
        if (seciliUlke != null)
        {
            lstCountry.BackgroundColor = Colors.Blue;
            countryName.Text = seciliUlke.countryName;
            imageSource.Text = seciliUlke.countryImageUrl.ToString();
        }
    }


}