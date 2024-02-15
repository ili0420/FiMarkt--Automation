using Fimarkt.Models;
using Fimarkt.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Fimarkt;

public partial class CreateTeamLeague : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<TeamLeague> teamLeagues;
    ObservableCollection<TeamLeagueViewModel> teamLeagueViewModels;
    public ICommand LigTakimSilCommand { get; set; }
    public CreateTeamLeague()
	{
		InitializeComponent();
        teamLeagues= new ObservableCollection<TeamLeague>();
        teamLeagueViewModels= new ObservableCollection<TeamLeagueViewModel>();
        lstTeamLeague.ItemsSource = teamLeagueViewModels;
        fimarktService = new FimarktService();

        LigTakimSilCommand = new Command<TeamLeague>(async (TeamLeague seciliLigTakimId) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigTakimSil(seciliLigTakimId.teamLeagueId);
                await GetLeagueTeamModels();
            }

        });
    }
    private async Task GetTeamNames()
    {
        var sonuc = await fimarktService.GetTeamName();

        teamPicker.ItemsSource = sonuc;
    }

    private async Task LeagueNames()
    {
        var sonuc = await fimarktService.GetLeagueName();

        leaguePicker.ItemsSource = sonuc;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetTeamNames();
        LeagueNames();



    }

    private async Task TakimId(string countryName)
    {
        var id = await fimarktService.GetTakimId(countryName);
        lblTeam.Text = id.ToString();
    }



    private async Task LigId(string countryName)
    {
        var id = await fimarktService.GetLeagueId(countryName);
        lblLeague.Text = id.ToString();
    }
    private void teamPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var takimAd = teamPicker.SelectedItem;
        TakimId(takimAd.ToString());
    }

    private void leaguePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ligAd = leaguePicker.SelectedItem;
        LigId(ligAd.ToString());
    }

    private void startSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1; 
        double newValue = Math.Round(e.NewValue / step) * step;
        startSlider.Value = newValue;
        lblStartYear.Text = e.NewValue.ToString();
    }

    private void endSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1; 
        double newValue = Math.Round(e.NewValue / step) * step;
        endSlider.Value = newValue;
        lblEndYear.Text = e.NewValue.ToString();
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var leagueTeam = new TeamLeague()
        {
            teamId = Convert.ToInt32(lblTeam.Text),
            leagueId = Convert.ToInt32(lblLeague.Text),
            startYear = Convert.ToInt32(lblStartYear.Text),
            endYear = Convert.ToInt32(lblEndYear.Text)
        };

        await fimarktService.LigTakimEkle(leagueTeam);
        await GetLeagueTeamModels();
    }


    private async Task GetLeagueTeamModels()
    {
        var sonuc = await fimarktService.GetLeagueTeamModel();
        teamLeagueViewModels.Clear();
        foreach (var item in sonuc)
        {
            teamLeagueViewModels.Add(item);
        }
    }
    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetLeagueTeamModels();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliTakimLig = (TeamLeagueViewModel)lstTeamLeague.SelectedItem;
        if (seciliTakimLig != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigTakimSil(seciliTakimLig.TeamLeagueId);
                await GetLeagueTeamModels();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliTakimLig = (TeamLeagueViewModel)lstTeamLeague.SelectedItem;
        if (seciliTakimLig != null)
        {
            var leagueTeam = new TeamLeague()
            {
                teamLeagueId = seciliTakimLig.TeamLeagueId,
                teamId = Convert.ToInt32(lblTeam.Text),
                leagueId= Convert.ToInt32(lblLeague.Text),
                startYear=Convert.ToInt32(lblStartYear.Text),
                endYear= Convert.ToInt32(lblEndYear.Text),
            };

            await fimarktService.LigTakimGuncelle(leagueTeam);
            await GetLeagueTeamModels();
        }
    }

    private void lstTeamLeague_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliTakimLig = (TeamLeagueViewModel)lstTeamLeague.SelectedItem;
        if (seciliTakimLig != null)
        {
            lstTeamLeague.BackgroundColor = Colors.Bisque;
            teamPicker.SelectedItem = seciliTakimLig.TeamName.ToString();
            leaguePicker.SelectedItem = seciliTakimLig.LeagueName.ToString();
            lblStartYear.Text=seciliTakimLig.StartYear.ToString();
            lblEndYear.Text = seciliTakimLig.EndYear.ToString();
        }
    }
}