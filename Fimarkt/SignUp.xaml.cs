using Fimarkt.Models;
using Fimarkt.Services;

namespace Fimarkt;

public partial class SignUp : ContentPage
{
    FimarktService userService;
    string seciliGender;

    public SignUp()
	{

		InitializeComponent();
        List<string> iller = new List<string>
        {
            "Denizli",
            "Isparta",
            "Adana",
            "Sivas",
            "Sakarya",
            "Ýzmir",
            "Ýstanbul",
            "Giresun",
            "Malatya",
            "Bilecik",
            "Ankara",
            "Batman",
            "Çanakkale",
            "Edirne",
            "Hatay",
            "Mersin",
            "Antalya",
            "Zonguldak",
            "Kastamonu",
            "Muðla",
            "Aðrý",
            "Aydýn",
            "Bartýn",
            "Kocaeli"

        };
        foreach (var il in iller.OrderBy(x => x))
        {
            pickerCity.Items.Add(il);
        }
        userService = new FimarktService();

    }
    private async void buttonSave_Clicked(object sender, EventArgs e)
    {
        string password = memberPassword.Text;
        string passwordAgain= memberPasswordAgain.Text;

        try
        {
            if (password==passwordAgain)
            {
                var user = new Member()
                {
                    MemberName = memberName.Text,
                    MemberSurname = memberSurname.Text,
                    MemberStartYear = Convert.ToDateTime(lblBirthday.Text),
                    MemberGender = seciliGender,
                    MemberMail = memberMail.Text,
                    MemberCity = pickerCity.SelectedItem.ToString(),
                    MemberPassword = memberPassword.Text,
                    MemberagainPassword = memberPasswordAgain.Text,


                };
                await userService.Added(user);
                await DisplayAlert("Kayýt baþarýlý. Lütfen bilgilerinizi unutmayýnýz.", "Kayýt Baþarýlý", "Tamam");
                Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Þifre bilgilerini kontrol ediniz.", "Þifrelerin ayný olduðuna dikkat ediniz.", "Tamam");
                
            }
        }
        catch(Exception ex)
        {
            throw;
        }

        

    }

    private void dtPicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        lblBirthday.Text = dtPicker.Date.ToShortDateString();
    }

    private void man_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        seciliGender = "erkek";
    }

    private void woman_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        seciliGender = "kadýn";
    }
    private async void ButtonVazgec_Clicked(object sender, EventArgs e)
    {
        bool cevap = await DisplayAlert("Emin misiniz?", "Üyeliðinizi Ýptal Etmek istiyor musunuz?", "evet", "hayýr");
        if (cevap)
        {

            await Navigation.PushAsync(new MainPage());

        }
    }
}