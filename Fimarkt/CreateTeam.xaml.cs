using Fimarkt.Services;
using Fimarkt.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Fimarkt;

public partial class CreateTeam : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<Team> teams;
    public ICommand TakimSilCommand { get; set; }
    public CreateTeam()
	{
        
        InitializeComponent();
        teams = new ObservableCollection<Team>();
        lstTeam.ItemsSource= teams;
        fimarktService = new FimarktService();

        TakimSilCommand = new Command<Team>(async (Team seciliTakim) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu takýmý silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.TakimSil(seciliTakim.teamId);
                await GetTeams();
            }

        });
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

    private async Task UlkeId(string countryName)
    {
        var id = await fimarktService.GetUlkeId(countryName);
        lblCountry.Text = id.ToString();
    }

    private void sliderStadiumCapacity_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int capacity = Convert.ToInt32(e.NewValue);
        capacity = capacity * 1000;
        lblStadiumCapacity.Text = capacity.ToString();
    }

    private async Task GetTeams()
    {
        var sonuc = await fimarktService.GetTeam();
        teams.Clear();
        foreach (var item in sonuc)
        {
           teams.Add(item);
        }
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

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var team = new Team()
        {
            teamName = teamName.Text,
            teamStadiumName = stadiumName.Text,
            teamStadiumCapacity = Convert.ToInt32(lblStadiumCapacity.Text),
            teamImageUrl = imageSource.Text,
            countryId=Convert.ToInt32(lblCountry.Text)
        };

        await fimarktService.TakimEkle(team);
        await GetTeams();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetTeams();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliTakim = (Team)lstTeam.SelectedItem;
        if (seciliTakim != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu takýmý silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.TakimSil(seciliTakim.teamId);
                await GetTeams();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliTakim = (Team)lstTeam.SelectedItem;
        if(seciliTakim != null)
        {
            var team = new Team()
            {
                teamId=seciliTakim.teamId,
                teamName = teamName.Text,
                teamStadiumName = stadiumName.Text,
                teamStadiumCapacity = Convert.ToInt32(lblStadiumCapacity.Text),
                teamImageUrl = imageSource.Text,
                countryId = Convert.ToInt32(lblCountry.Text)
            };

            await fimarktService.TakimGuncelle(team);
            await GetTeams();
        }

    }

    private  void lstTeam_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliTakim = (Team)lstTeam.SelectedItem;
        if (seciliTakim != null)
        {
            lstTeam.BackgroundColor = Colors.Orange;
            teamName.Text = seciliTakim.teamName;
            stadiumName.Text = seciliTakim.teamStadiumName;
            lblStadiumCapacity.Text = seciliTakim.teamStadiumCapacity.ToString();
            imageSource.Text = seciliTakim.teamImageUrl;
        }
    }

    private void originPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ulkeAd = originPicker.SelectedItem;
        UlkeId(ulkeAd.ToString());
    }
}