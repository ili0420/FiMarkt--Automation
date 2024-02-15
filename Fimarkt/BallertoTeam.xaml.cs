using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Fimarkt;

public partial class BallertoTeam : ContentPage
{
    IFimarktService fimarktService;
    ObservableCollection<BallerTeam> ballerTeam;
    ObservableCollection<TeamBallerViewModel> teamBallerViewModels;
    public ICommand OyuncuTakimSilCommand { get; set; }
    public BallertoTeam()
	{
		InitializeComponent();
        ballerTeam = new ObservableCollection<BallerTeam>();
        teamBallerViewModels = new ObservableCollection<TeamBallerViewModel>();
        lstTeamBaller.ItemsSource = teamBallerViewModels;
        fimarktService = new FimarktService();

        OyuncuTakimSilCommand = new Command<BallerTeam>(async (BallerTeam secilOyuncuTakimId) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.FutbolcuTakimSil(secilOyuncuTakimId.ballerTeamId);
                await GetBallerTeamModels();
            }

        });
    }
    private async Task GetBallerSurnames()
    {
        var sonuc = await fimarktService.GetBallerSurname();

        ballerPicker.ItemsSource = sonuc;
    }
    private async Task GetTeamNames()
    {
        var sonuc = await fimarktService.GetTeamName();

        teamPicker.ItemsSource = sonuc;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetBallerSurnames();
        GetTeamNames();


    }
    private async Task GetBallerTeamModels()
    {
        var sonuc = await fimarktService.GetTeamBallerModel();
        teamBallerViewModels.Clear();
        foreach (var item in sonuc)
        {
            teamBallerViewModels.Add(item);
        }
    }
    private async Task TakimId(string teamName)
    {
        var id = await fimarktService.GetTakimId(teamName);
        lblTeam.Text = id.ToString();
    }
    private async Task FutbolcuId(string ballerSurname)
    {
        var id = await fimarktService.GetFutbolcuId(ballerSurname);
        lblBaller.Text = id.ToString();
    }
    private void teamPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var takimAd = teamPicker.SelectedItem;
        TakimId(takimAd.ToString());
    }

    private void ballerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var futbolcuAd = ballerPicker.SelectedItem;
        FutbolcuId(futbolcuAd.ToString());
    }

    private void contractStartTime_DateSelected(object sender, DateChangedEventArgs e)
    {
        lblstarttime.Text = contractStartTime.Date.ToShortDateString();
    }

    private void contractEndTime_DateSelected(object sender, DateChangedEventArgs e)
    {
        lblendtime.Text = contractEndTime.Date.ToShortDateString();
    }

    private void sliderSalary_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int salary = Convert.ToInt32(e.NewValue);
        salary = salary * 1000000;
        lblSalary.Text = salary.ToString();
    }

    private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lblShirtNumber.Text = shirtNumber.Value.ToString();
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var ballerTeam = new BallerTeam()
        {
            teamId = Convert.ToInt32(lblTeam.Text),
            ballerId = Convert.ToInt32(lblBaller.Text),
            contractStartTime = Convert.ToDateTime(lblstarttime.Text),
            contractEndTime = Convert.ToDateTime(lblendtime.Text),
            salary= Convert.ToDecimal(lblSalary.Text),
            shirtNumber=Convert.ToInt32(lblShirtNumber.Text)
        };

        await fimarktService.TakimFutbolcuEkle(ballerTeam);
        await GetBallerTeamModels();
    }

    private async void List_Clicked(object sender, EventArgs e)
    {
       await GetBallerTeamModels();
    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliFutbolcuTakým = (TeamBallerViewModel)lstTeamBaller.SelectedItem;
        if (seciliFutbolcuTakým != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu veriyi silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.FutbolcuTakimSil(seciliFutbolcuTakým.BallerTeamId);
                await GetBallerTeamModels();
            }

        }
    }

    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliFutbolcuTakým = (TeamBallerViewModel)lstTeamBaller.SelectedItem;
        if (seciliFutbolcuTakým != null)
        {
            var ballerTeam = new BallerTeam()
            {
                ballerTeamId=seciliFutbolcuTakým.BallerTeamId,
                teamId = Convert.ToInt32(lblTeam.Text),
                ballerId = Convert.ToInt32(lblBaller.Text),
                contractStartTime = Convert.ToDateTime(lblstarttime.Text),
                contractEndTime = Convert.ToDateTime(lblendtime.Text),
                salary = Convert.ToDecimal(lblSalary.Text),
                shirtNumber = Convert.ToInt32(lblShirtNumber.Text)
            };

            await fimarktService.FutbolcuTakimGuncelle(ballerTeam);
            await GetBallerTeamModels();
        }
    }

    private void lstTeamBaller_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliFutbolcuTakým = (TeamBallerViewModel)lstTeamBaller.SelectedItem;
        if (seciliFutbolcuTakým != null)
        {
            lstTeamBaller.BackgroundColor = Colors.Cyan;
            teamPicker.SelectedItem = seciliFutbolcuTakým.TeamName.ToString();
            ballerPicker.SelectedItem = seciliFutbolcuTakým.BallerSurname.ToString();
            lblstarttime.Text = seciliFutbolcuTakým.ContractStartTime.ToString();
            lblendtime.Text = seciliFutbolcuTakým.ContractEndTime.ToString();
            lblSalary.Text=seciliFutbolcuTakým.Salary.ToString();
            lblShirtNumber.Text = seciliFutbolcuTakým.ShirtNumber.ToString();
        }
    }
}