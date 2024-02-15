using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Fimarkt;

public partial class CreateBaller : ContentPage
{
    // Service k�sm�n� kullanmak i�in bir nesne olu�turduk.
    IFimarktService fimarktService;
    //Futbolcular� listelemek i�in bir observablecollection olu�turduk.
    ObservableCollection<Baller> players;
    

    //Oyuncular� silmek i�in bir command olu�turduk
    public ICommand OyuncuSilCommand { get; set; }
    public CreateBaller()
    {
        
        InitializeComponent();
        fimarktService = new FimarktService();


        players = new ObservableCollection<Baller>();

        //collection viewin veri kayna��n� olu�turdu�umuz observablecollectiona
        //atad�k
        lstBaller.ItemsSource = players;
        
        
        //Oyuncu silme command�n� tan�mlad�k
        OyuncuSilCommand = new Command<Baller>(async (Baller seciliOyuncu) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu oyuncuyu silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (cevap)
            {
                await fimarktService.Sil(seciliOyuncu.ballerId);
                await GetPlayers();
            }

        });
        //Futbolcular�n mevkilerini tutmak i�in bir liste olu�turduk.
        List<string> mevkiler = new List<string>()
{
           "Kaleci",
          "Sa� Bek",
          "Sol Bek",
          "Stoper",
          "Defansif Orta Saha",
          "Merkez Orta Saha",
          "Ofansif Orta Saha",
          "Sa� Kanat",
          "Sol Kanat",
          "Santrafor",
          "Sa� Kanat Beki",
          "Sol Kanat Beki"
};
        //Mevkileri ilgili picker nesnesine ekledik
        foreach (var mevki in mevkiler.OrderBy(x => x))
        {
            positionPicker.Items.Add(mevki);
        }
        //Futbolcular�n hangi aya��n� kulland�klar�n� tutmak i�in bir liste olu�turduk.
        List<string> foot = new List<string>()
{
           "Sa� Ayak",
          "Sol Ayak",
          "�oklu",
};
        //Ayak bilgilerini ilgili picker nesnesine ekledik
        foreach (var prefer in foot.OrderBy(x => x))
        {
            footPicker.Items.Add(prefer);
        }

        
    }
    //Futbolcular� collection view de  listelemek i�in bir task olu�turduk
    private async Task GetPlayers()
    {
        var sonuc = await fimarktService.GetBaller();
        
        players.Clear();
        foreach (var item in sonuc)
        {
            players.Add(item);
        }
    }
    //�lke adlar�n� getirmek  i�in bir task olu�turduk
    private async Task GetCountryNames()
    {
        var sonuc = await fimarktService.GetCountryName();
        
        originPicker.ItemsSource = sonuc;
    }
    //�lke adlar� ba�lang��ta gelsin
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetCountryNames();
        
        
        
    }
    //�lkelerini id lerini getirdik
    private async Task UlkeId(string countryName)
    {
        var id = await  fimarktService.GetUlkeId(countryName);
        lblCountry.Text = id.ToString();
    }
    





    private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
		lblSslider.Text = slider.Value.ToString();
    }

    private void sliderMarketValue_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int marketValue =Convert.ToInt32( e.NewValue);
        marketValue = marketValue * 1000000;
        lblMarketValue.Text = marketValue.ToString();
    }
    //Resim se�me i�lemi
    private async void pickFile_Clicked(object sender, EventArgs e)
    {
        PickOptions options= new PickOptions();
        try
        {
            var result= await FilePicker.Default.PickAsync(options);
            if(result != null)
            {
                if(result.FileName.EndsWith("jpg",StringComparison.OrdinalIgnoreCase) ||
                   result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream=await result.OpenReadAsync();
                    var image=ImageSource.FromStream(() => stream);
                    string fullpathName=Path.Combine(result.FullPath,result.FileName);
                    imageSource.Text=fullpathName;
                }

            }
        }
        catch (Exception ex)
        {

        }
    }
    //Kaydetme i�lemi
    private async void Save_Clicked(object sender, EventArgs e)
    {
        
        
        var baller = new Baller()
        {
            ballerName = playerName.Text,
            ballerSurname = ballerSurname.Text,
            ballerbirthTime = Convert.ToDateTime(lblbirthDate.Text),
            ballerbirthPlace = ballerBithPlace.Text,
            ballerHeight = Convert.ToDecimal(lblSslider.Text),
            ballerPosition = positionPicker.SelectedItem.ToString(),
            ballerValue = Convert.ToDecimal(lblMarketValue.Text),
            ballerFoot = footPicker.SelectedItem.ToString(),
            ballerImageUrl = imageSource.Text,
            countryId=Convert.ToInt32(lblCountry.Text)
        };

        await fimarktService.Ekle(baller);
        await GetPlayers();
    

    }

    private void ballerBirthDate_DateSelected(object sender, DateChangedEventArgs e)
    {
        lblbirthDate.Text=ballerBirthDate.Date.ToShortDateString();
    }

    private  void lstBaller_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var seciliOyuncu = (Baller)lstBaller.SelectedItem;
        if(seciliOyuncu != null)
        {
            lstBaller.BackgroundColor = Colors.Blue;
            playerName.Text = seciliOyuncu.ballerName;
            ballerSurname.Text = seciliOyuncu.ballerSurname.ToString();
            lblbirthDate.Text = seciliOyuncu.ballerbirthTime.ToString();
            ballerBithPlace.Text = seciliOyuncu.ballerbirthPlace.ToString();
            lblSslider.Text = seciliOyuncu.ballerHeight.ToString();
            positionPicker.SelectedItem = seciliOyuncu.ballerPosition.ToString();
            lblMarketValue.Text = seciliOyuncu.ballerValue.ToString();
            footPicker.SelectedItem = seciliOyuncu.ballerFoot.ToString();
            imageSource.Text = seciliOyuncu.ballerImageUrl.ToString();
        }

        
    }
    //Listeleme i�lemi
    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetPlayers();
    }
    //Silme ��lemi
    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliOyuncu = (Baller)lstBaller.SelectedItem;
        if (seciliOyuncu != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu oyuncuyu silmek istedi�inizden emin misiniz?", "Evet", "Hay�r");
            if (cevap)
            {
                await fimarktService.Sil(seciliOyuncu.ballerId);
                await GetPlayers();
            }

        }
    }
    //G�ncelleme i�lemi
    private async void Update_Clicked(object sender, EventArgs e)
    {
        var seciliOyuncu = (Baller)lstBaller.SelectedItem;
        if(seciliOyuncu != null)
        {
            var baller = new Baller()
            {
                ballerId = seciliOyuncu.ballerId,
                ballerName = playerName.Text,
                ballerSurname = ballerSurname.Text,
                ballerbirthTime = Convert.ToDateTime(lblbirthDate.Text),
                ballerbirthPlace = ballerBithPlace.Text,
                ballerHeight = Convert.ToDecimal(lblSslider.Text),
                ballerPosition = positionPicker.SelectedItem.ToString(),
                ballerValue = Convert.ToDecimal(lblMarketValue.Text),
                ballerFoot = footPicker.SelectedItem.ToString(),
                ballerImageUrl = imageSource.Text,
                countryId = Convert.ToInt32(lblCountry.Text)
            };
            await fimarktService.Guncelle(baller);
            await GetPlayers();
        }

    }

    private  void originPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ulkeAd = originPicker.SelectedItem;
        UlkeId(ulkeAd.ToString());
        //lblCountry.Text= fimarktService.GetUlkeId(ulkeAd).ToString();
        
    }
}