using Fimarkt.Models;
using Fimarkt.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace Fimarkt;

public partial class CreateBaller : ContentPage
{
    // Service kýsmýný kullanmak için bir nesne oluþturduk.
    IFimarktService fimarktService;
    //Futbolcularý listelemek için bir observablecollection oluþturduk.
    ObservableCollection<Baller> players;
    

    //Oyuncularý silmek için bir command oluþturduk
    public ICommand OyuncuSilCommand { get; set; }
    public CreateBaller()
    {
        
        InitializeComponent();
        fimarktService = new FimarktService();


        players = new ObservableCollection<Baller>();

        //collection viewin veri kaynaðýný oluþturduðumuz observablecollectiona
        //atadýk
        lstBaller.ItemsSource = players;
        
        
        //Oyuncu silme commandýný tanýmladýk
        OyuncuSilCommand = new Command<Baller>(async (Baller seciliOyuncu) => {
            bool cevap = await DisplayAlert("Emin misiniz", "Bu oyuncuyu silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.Sil(seciliOyuncu.ballerId);
                await GetPlayers();
            }

        });
        //Futbolcularýn mevkilerini tutmak için bir liste oluþturduk.
        List<string> mevkiler = new List<string>()
{
           "Kaleci",
          "Sað Bek",
          "Sol Bek",
          "Stoper",
          "Defansif Orta Saha",
          "Merkez Orta Saha",
          "Ofansif Orta Saha",
          "Sað Kanat",
          "Sol Kanat",
          "Santrafor",
          "Sað Kanat Beki",
          "Sol Kanat Beki"
};
        //Mevkileri ilgili picker nesnesine ekledik
        foreach (var mevki in mevkiler.OrderBy(x => x))
        {
            positionPicker.Items.Add(mevki);
        }
        //Futbolcularýn hangi ayaðýný kullandýklarýný tutmak için bir liste oluþturduk.
        List<string> foot = new List<string>()
{
           "Sað Ayak",
          "Sol Ayak",
          "Çoklu",
};
        //Ayak bilgilerini ilgili picker nesnesine ekledik
        foreach (var prefer in foot.OrderBy(x => x))
        {
            footPicker.Items.Add(prefer);
        }

        
    }
    //Futbolcularý collection view de  listelemek için bir task oluþturduk
    private async Task GetPlayers()
    {
        var sonuc = await fimarktService.GetBaller();
        
        players.Clear();
        foreach (var item in sonuc)
        {
            players.Add(item);
        }
    }
    //Ülke adlarýný getirmek  için bir task oluþturduk
    private async Task GetCountryNames()
    {
        var sonuc = await fimarktService.GetCountryName();
        
        originPicker.ItemsSource = sonuc;
    }
    //Ülke adlarý baþlangýçta gelsin
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        GetCountryNames();
        
        
        
    }
    //Ülkelerini id lerini getirdik
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
    //Resim seçme iþlemi
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
    //Kaydetme iþlemi
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
    //Listeleme iþlemi
    private async void List_Clicked(object sender, EventArgs e)
    {
        await GetPlayers();
    }
    //Silme Ýþlemi
    private async void Delete_Clicked(object sender, EventArgs e)
    {
        var seciliOyuncu = (Baller)lstBaller.SelectedItem;
        if (seciliOyuncu != null)
        {

            bool cevap = await DisplayAlert("Emin misiniz", "Bu oyuncuyu silmek istediðinizden emin misiniz?", "Evet", "Hayýr");
            if (cevap)
            {
                await fimarktService.Sil(seciliOyuncu.ballerId);
                await GetPlayers();
            }

        }
    }
    //Güncelleme iþlemi
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