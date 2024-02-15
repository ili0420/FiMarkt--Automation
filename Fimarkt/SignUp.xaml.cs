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
            "�zmir",
            "�stanbul",
            "Giresun",
            "Malatya",
            "Bilecik",
            "Ankara",
            "Batman",
            "�anakkale",
            "Edirne",
            "Hatay",
            "Mersin",
            "Antalya",
            "Zonguldak",
            "Kastamonu",
            "Mu�la",
            "A�r�",
            "Ayd�n",
            "Bart�n",
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
                await DisplayAlert("Kay�t ba�ar�l�. L�tfen bilgilerinizi unutmay�n�z.", "Kay�t Ba�ar�l�", "Tamam");
                Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("�ifre bilgilerini kontrol ediniz.", "�ifrelerin ayn� oldu�una dikkat ediniz.", "Tamam");
                
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
        seciliGender = "kad�n";
    }
    private async void ButtonVazgec_Clicked(object sender, EventArgs e)
    {
        bool cevap = await DisplayAlert("Emin misiniz?", "�yeli�inizi �ptal Etmek istiyor musunuz?", "evet", "hay�r");
        if (cevap)
        {

            await Navigation.PushAsync(new MainPage());

        }
    }
}