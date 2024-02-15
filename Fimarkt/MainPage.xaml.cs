using Fimarkt.Models;
using Fimarkt.Services;

namespace Fimarkt;

public partial class MainPage : ContentPage
{

    FimarktService userService;
    public MainPage()
	{
		InitializeComponent();
        userService = new FimarktService();
    }

    private async void Giris_Clicked(object sender, EventArgs e)
    {
        if(mailtxt.Text=="admin" & passtxt.Text == "1234"  )
        {
            Navigation.PushAsync(new AdminPage());
        }
        else
        {
            try
            {
                string memberMail = mailtxt.Text;
                string memberPassword = passtxt.Text;
                var member = await userService.GetMember(memberMail, memberPassword);
                if (member != null)
                {
                    Navigation.PushAsync(new UserPage());
                }
                if(member == null)
                {
                    await DisplayAlert("Kullanıcı adı ya da şifre yanlış", "Verilerinizi doğru giriniz", "Tamam");
                }

            }

           catch (Exception ex)
            {
                throw;
            }
        }
    }

    private void uye_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SignUp());
    }
}

