using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
namespace Fimarkt;

public partial class Performance : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<BallerPerformance> ballerPerformances;
    ObservableCollection<BallerPerformanceViewModel> performances;
    public ICommand OyuncuPerformansSilCommand { get; set; }
    public Performance()
	{
		InitializeComponent();
        ballerPerformances = new ObservableCollection<BallerPerformance>();
        performances = new ObservableCollection<BallerPerformanceViewModel>();
        lstPerformance.ItemsSource = performances;
        fimarktService = new FimarktService();

        OyuncuPerformansSilCommand = new Command<BallerPerformance>(async (BallerPerformance secilOyuncuPerformansId) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.FutbolcuPerformansSil(secilOyuncuPerformansId.ballerPerformanceId);
                await GetPerformances();
            }

        });
    }
    private async Task GetBallerSurnames()
    {
        var sonuc = await fimarktService.GetBallerSurname();

        ballerPicker.ItemsSource = sonuc;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetBallerSurnames();
        LeagueNames();

    }
    private async Task LeagueNames()
    {
        var sonuc = await fimarktService.GetLeagueName();

        leaguePicker.ItemsSource = sonuc;
    }
    private async Task LigId(string leagueName)
    {
        var id = await fimarktService.GetLeagueId(leagueName);
        lblLeague.Text = id.ToString();
    }
    private async Task FutbolcuId(string ballerSurname)
    {
        var id = await fimarktService.GetFutbolcuId(ballerSurname);
        lblBaller.Text = id.ToString();
    }

    private async Task GetPerformances()
    {
        var sonuc = await fimarktService.GetBallerPerformance();
        performances.Clear();
        foreach (var item in sonuc)
        {
            performances.Add(item);
        }
        
    }

    private void ballerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var futbolcuSoyad = ballerPicker.SelectedItem;
        FutbolcuId(futbolcuSoyad.ToString());
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

    private void goalSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        goalSlider.Value = newValue;
        lblGoal.Text = e.NewValue.ToString();
    }

    private void asistSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        asistSlider.Value = newValue;
        lblAsist.Text = e.NewValue.ToString();
    }

    private void redCard_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lblRedCard.Text = redCard.Value.ToString();
    }

    private void yellowCard_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lblYellowCard.Text = yellowCard.Value.ToString();
    }

    private void totalMatchSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        totalMatchSlider.Value = newValue;
        lblTotalMatch.Text = e.NewValue.ToString();
    }

    private void totalTimeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        totalTimeSlider.Value = newValue;
        lblTotalTime.Text = e.NewValue.ToString();
    }

    private void totalGoalConcededSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        totalGoalConcededSlider.Value = newValue;
        lblGoalConceded.Text = e.NewValue.ToString();
    }

    private void cleanSheetSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double step = 1;
        double newValue = Math.Round(e.NewValue / step) * step;
        cleanSheetSlider.Value = newValue;
        lbCleanSheet.Text = e.NewValue.ToString();
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var ballerPerformance = new BallerPerformance()
        {
            ballerId = Convert.ToInt32(lblBaller.Text),
            LeagueId = Convert.ToInt32(lblLeague.Text),
            startYear = Convert.ToInt32(lblStartYear.Text),
            endYear = Convert.ToInt32(lblEndYear.Text),
            goal = Convert.ToInt32(lblGoal.Text),
            assist = Convert.ToInt32(lblAsist.Text),
            redCard = Convert.ToInt32(lblRedCard.Text),
            yellowCard = Convert.ToInt32(lblYellowCard.Text),
            goalConceded = Convert.ToInt32(lblGoalConceded.Text),
            totalMatchNumber = Convert.ToInt32(lblTotalMatch.Text),
            time = Convert.ToInt32(lblTotalTime.Text),
            cleanSheet = Convert.ToInt32(lbCleanSheet.Text)
        };

        await fimarktService.FutbolcuPerformansEkle(ballerPerformance);
        await GetPerformances();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetPerformances();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliFutbolcuPerformans = (BallerPerformanceViewModel)lstPerformance.SelectedItem;
        if (seciliFutbolcuPerformans != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.FutbolcuPerformansSil(seciliFutbolcuPerformans.ballerPerformanceId);
                await GetPerformances();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliFutbolcuPerformans = (BallerPerformanceViewModel)lstPerformance.SelectedItem;
        if (seciliFutbolcuPerformans != null)
        {
            var ballerPerformance = new BallerPerformance()
            {
                ballerPerformanceId= seciliFutbolcuPerformans.ballerPerformanceId,
                ballerId = Convert.ToInt32(lblBaller.Text),
                LeagueId = Convert.ToInt32(lblLeague.Text),
                startYear = Convert.ToInt32(lblStartYear.Text),
                endYear = Convert.ToInt32(lblEndYear.Text),
                goal = Convert.ToInt32(lblGoal.Text),
                assist = Convert.ToInt32(lblAsist.Text),
                redCard = Convert.ToInt32(lblRedCard.Text),
                yellowCard = Convert.ToInt32(lblYellowCard.Text),
                goalConceded = Convert.ToInt32(lblGoalConceded.Text),
                totalMatchNumber = Convert.ToInt32(lblTotalMatch.Text),
                time = Convert.ToInt32(lblTotalTime.Text),
                cleanSheet = Convert.ToInt32(lbCleanSheet.Text)
            };

            await fimarktService.FutbolcuPerformansGuncelle(ballerPerformance);
            await GetPerformances();
        }
    }

    private void lstPerformance_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliFutbolcuPerformans = (BallerPerformanceViewModel)lstPerformance.SelectedItem;
        if (seciliFutbolcuPerformans != null)
        {
            lstPerformance.BackgroundColor = Colors.Cyan;
            leaguePicker.SelectedItem = seciliFutbolcuPerformans.LeagueName.ToString();
            ballerPicker.SelectedItem = seciliFutbolcuPerformans.BallerSurname.ToString();
            lblStartYear.Text = seciliFutbolcuPerformans.StartYear.ToString();
            lblEndYear.Text = seciliFutbolcuPerformans.EndYear.ToString();
            lblGoal.Text = seciliFutbolcuPerformans.Goal.ToString();
            lblAsist.Text = seciliFutbolcuPerformans.Assist.ToString();
            lblYellowCard.Text = seciliFutbolcuPerformans.YellowCard.ToString();
            lblRedCard.Text = seciliFutbolcuPerformans.RedCard.ToString();
            lblGoalConceded.Text= seciliFutbolcuPerformans.GoalConceded.ToString();
            lblTotalMatch.Text = seciliFutbolcuPerformans.TotalMatch.ToString();
            lblTotalTime.Text = seciliFutbolcuPerformans.TotalTime.ToString();
            lbCleanSheet.Text= seciliFutbolcuPerformans.CleanSheet.ToString();
        }
    }
}