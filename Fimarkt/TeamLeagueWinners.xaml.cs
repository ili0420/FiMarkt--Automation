using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Fimarkt;

public partial class TeamLeagueWinners : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<TeamLeagueWinner> teamLeagueWinners;
    ObservableCollection<TeamLeagueWinnerViewModel> teamLeagueWinnerViewModels;
    public ICommand LigTakimKazananSilCommand { get; set; }
    public TeamLeagueWinners()
	{
		InitializeComponent();
        teamLeagueWinners = new ObservableCollection<TeamLeagueWinner>();
        teamLeagueWinnerViewModels = new ObservableCollection<TeamLeagueWinnerViewModel>();
        lstTeamLeagueWinner.ItemsSource = teamLeagueWinnerViewModels;
        fimarktService = new FimarktService();

        LigTakimKazananSilCommand = new Command<TeamLeagueWinner>(async (TeamLeagueWinner seciliLigTakimKazananId) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigKazananTakimSil(seciliLigTakimKazananId.teamLeagueWinnerId);
                await GetLeagueWinnersTeamModels();
            }

        });
    }
    private async Task GetLeagueWinnersTeamModels()
    {
        var sonuc = await fimarktService.GetLeagueWinnerTeamModel();
        teamLeagueWinnerViewModels.Clear();
        foreach (var item in sonuc)
        {
            teamLeagueWinnerViewModels.Add(item);
        }
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

    private async Task TakimId(string teamName)
    {
        var id = await fimarktService.GetTakimId(teamName);
        lblTeam.Text = id.ToString();
    }



    private async Task LigId(string leagueName)
    {
        var id = await fimarktService.GetLeagueId(leagueName);
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
        var leagueTeamWinner = new TeamLeagueWinner()
        {
            teamId = Convert.ToInt32(lblTeam.Text),
            leagueId = Convert.ToInt32(lblLeague.Text),
            startYear = Convert.ToInt32(lblStartYear.Text),
            endYear = Convert.ToInt32(lblEndYear.Text)
        };

        await fimarktService.LigTakimKazananEkle(leagueTeamWinner);
        await GetLeagueWinnersTeamModels();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetLeagueWinnersTeamModels();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliTakimLigKazanan = (TeamLeagueWinnerViewModel)lstTeamLeagueWinner.SelectedItem;
        if (seciliTakimLigKazanan != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.LigKazananTakimSil(seciliTakimLigKazanan.TeamLeagueWinnerId);
                await GetLeagueWinnersTeamModels();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliTakimLigKazanan = (TeamLeagueWinnerViewModel)lstTeamLeagueWinner.SelectedItem;
        if (seciliTakimLigKazanan != null)
        {
            var teamLeagueWinner = new TeamLeagueWinner()
            {
                teamLeagueWinnerId = seciliTakimLigKazanan.TeamLeagueWinnerId,
                teamId = Convert.ToInt32(lblTeam.Text),
                leagueId = Convert.ToInt32(lblLeague.Text),
                startYear = Convert.ToInt32(lblStartYear.Text),
                endYear = Convert.ToInt32(lblEndYear.Text),
            };

            await fimarktService.LigKazananTakimGuncelle(teamLeagueWinner);
            await GetLeagueWinnersTeamModels();
        }
    }

    private void lstTeamLeagueWinner_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliTakimLigKazanan = (TeamLeagueWinnerViewModel)lstTeamLeagueWinner.SelectedItem;
        if (seciliTakimLigKazanan != null)
        {
            lstTeamLeagueWinner.BackgroundColor = Colors.Bisque;
            teamPicker.SelectedItem = seciliTakimLigKazanan.TeamName.ToString();
            leaguePicker.SelectedItem = seciliTakimLigKazanan.LeagueName.ToString();
            lblStartYear.Text = seciliTakimLigKazanan.StartYear.ToString();
            lblEndYear.Text = seciliTakimLigKazanan.EndYear.ToString();
        }
    }
}