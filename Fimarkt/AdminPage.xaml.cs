namespace Fimarkt;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
	}

    private void btnAddBaller_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new CreateBaller());
    }

    private void btnAddTeam_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateTeam());
    }

    private void btnAddCountry_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateCountry());
    }

    private void btnAddLeague_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateLeague());
    }

    private void btnAddLeagueTeam_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateTeamLeague());
    }

    private void btnLeagueWinners_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new TeamLeagueWinners());
    }

    private void btnAddBallerTeam_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new BallertoTeam());
    }

    private void btnAddBallerPerformance_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Performance());
    }

    private void BallerToTeam_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new BallertoTeam());
    }
}