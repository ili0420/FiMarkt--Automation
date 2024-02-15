using Fimarkt.Models;
using Fimarkt.Services;
namespace Fimarkt;

public partial class ChangePassword : ContentPage
{
    IFimarktService fimarktService;
    public ChangePassword()
	{
		InitializeComponent();
		fimarktService = new FimarktService();
	}


    private async Task UyeId(string userMail)
    {
        lblMail.Text = mailtxt.Text;
        userMail = lblMail.Text;
        var id = await fimarktService.GetUyeId(userMail);
        lblUyeId.Text = id.ToString();
    }

    private  void change_Clicked(object sender, EventArgs e)
    {
         
    }
}