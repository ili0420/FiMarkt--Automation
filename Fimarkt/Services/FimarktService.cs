using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Fimarkt.Models;




namespace Fimarkt.Services
{
    internal interface IFimarktService
    {
        //Futbolcu Ekleme Task kısmı
        Task Ekle(Baller baller);
        Task<List<Baller>> GetBaller();

        Task Sil(int playerid);

        Task Guncelle(Baller player);

        //Takım Ekleme Task kısmı

        Task TakimEkle(Team team);

        Task<List<Team>> GetTeam();

        Task TakimSil(int teamid);

        Task TakimGuncelle(Team team);

        //Ülke Ekleme Task Kısmı

        Task UlkeEkle(Country country);

        Task<List<Country>> GetCountry();

        Task UlkeSil(int countryid);
        Task UlkeGuncelle(Country country);

        Task<List<String>> GetCountryName();

        Task<int> GetUlkeId(string cName);

        //Lig İşlemleri

        Task LigEkle(League league);

        Task<List<League>> GetLeague();

        Task LigSil(int leagueid);

        Task LigGuncelle(League league);

        //Lige Takım Ekleme İşlemleri (Sezon sezon)

        Task<List<String>> GetTeamName();

        Task<int> GetTakimId(string cName);

        Task<List<String>> GetLeagueName();

        Task<int> GetLeagueId(string cName);

        Task LigTakimEkle(TeamLeague teamLeague);

        Task<List<TeamLeague>> GetTeamLeague();

        Task LigTakimSil(int leagueteamid);

        Task LigTakimGuncelle(TeamLeague teamLeague);

        Task<List<TeamLeagueViewModel>> GetLeagueTeamModel();

        //Ligi Kazanan Takımnları Ekleme İşlemleri (Sezon sezon)

        Task LigTakimKazananEkle(TeamLeagueWinner teamLeagueWinner);

        Task<List<TeamLeagueWinnerViewModel>> GetLeagueWinnerTeamModel();

        Task LigKazananTakimSil(int leagueteamwinnerid);

        Task LigKazananTakimGuncelle(TeamLeagueWinner teamLeagueWinner);

        //Futbolcu Takım Transfer İşlemleri 

        Task<List<String>> GetBallerSurname();

        Task<int> GetFutbolcuId(string cSurname);

        Task TakimFutbolcuEkle(BallerTeam ballerTeam);

        Task<List<TeamBallerViewModel>> GetTeamBallerModel();

        Task FutbolcuTakimSil(int ballerteamid);

        Task FutbolcuTakimGuncelle(BallerTeam teamBaller);

        //Futbolcu Transfer İşlemleri

        Task FutbolcuPerformansEkle(BallerPerformance ballerPerformance);

        Task<List<BallerPerformanceViewModel>> GetBallerPerformance();

        Task FutbolcuPerformansSil(int ballerperformanceid);

        Task FutbolcuPerformansGuncelle(BallerPerformance ballerPerformance);

        //Üye kayıt olma
        Task Added(Member member);

        //Kullanıcı Arama Ekranı
        Task<List<Baller>> SearchBaller(String filterText);

        Task<List<TeamSearchViewModel>> SearchLig(String filterText);
        Task<List<Team>> SearchTeam(String filterText);
        Task<List<Country>> SearchCountry(String filterText);

        Task<int> GetMember(String memberMail,String memberPassword);

        //Üye şifre değiştirme için id getirme
        Task<int> GetUyeId(string mail);


    }
    public class UrlHelper
    {
        public static string BaseAdress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:7108" : "https://localhost:7108";
        public static string url = $"{BaseAdress}/players";
        public static string oyuncuGuncelleUrl = $"{url}/guncelle";
        public static string takimUrl = $"{BaseAdress}/teams";
        public static string takimGuncelleUrl = $"{takimUrl}/guncelle";
        public static string ulkeUrl = $"{BaseAdress}/countrys";
        public static string ulkeGuncelleUrl = $"{ulkeUrl}/guncelle";
        public static string ulkeAd = $"{BaseAdress}/countrys/countryname";

        public static string ligUrl = $"{BaseAdress}/leagues";
        public static string ligGuncelleUrl = $"{ligUrl}/guncelle";

        public static string takimAd = $"{BaseAdress}/teams/teamname";

        public static string ligAd = $"{BaseAdress}/leagues/leaguename";

        public static string takimLigUrl = $"{BaseAdress}/teamLeague";
        public static string takimLigGuncelleUrl = $"{takimLigUrl}/guncelle";

        public static string takimLigKazananUrl = $"{BaseAdress}/teamLeagueWinners";
        public static string takimLigKazananGuncelleUrl = $"{takimLigKazananUrl}/guncelle";

        public static string oyuncuSoyad = $"{BaseAdress}/players/playersurname"; 
        public static string takimFutbolcuUrl = $"{BaseAdress}/ballerteam";
        public static string takimFutbolcuGuncelleUrl = $"{takimFutbolcuUrl}/guncelle";

        public static string futbolcuPerformansUrl = $"{BaseAdress}/ballerperformance";
        public static string futbolcuPerformansGuncelleUrl = $"{futbolcuPerformansUrl}/guncelle";

    }

    public class FimarktService : IFimarktService
    {
        HttpClient httpClient;
        JsonSerializerOptions jsonSerializerOptions;

       

        public FimarktService()
        {
#if (DEBUG && ANDROID)
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            httpClient = new HttpClient(handler.GetPlatformMessageHandler());
#else
            httpClient = new HttpClient();
#endif

            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public async Task Ekle(Baller baller)
        {

            var json = JsonSerializer.Serialize(baller);
            JsonContent jsonContent = JsonContent.Create(baller);
            var response = await httpClient.PostAsync(UrlHelper.url, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

       




        public async Task Sil(int playerid)
        {
            var url = UrlHelper.url + $"/{playerid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task<List<Baller>> GetBaller()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<Baller>>(UrlHelper.url);
            return sonuc;
        }



        public async Task Guncelle(Baller player)
        {
            var json = JsonSerializer.Serialize(player);
            JsonContent jsonContent = JsonContent.Create(player);
            var response = await httpClient.PostAsync(UrlHelper.oyuncuGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task TakimEkle(Team team)
        {
            var json = JsonSerializer.Serialize(team);
            JsonContent jsonContent = JsonContent.Create(team);
            var response = await httpClient.PostAsync(UrlHelper.takimUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<Team>> GetTeam()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<Team>>(UrlHelper.takimUrl);
            return sonuc;
        }

        public async Task TakimSil(int teamid)
        {
            var url = UrlHelper.takimUrl + $"/{teamid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task TakimGuncelle(Team team)
        {
            var json = JsonSerializer.Serialize(team);
            JsonContent jsonContent = JsonContent.Create(team);
            var response = await httpClient.PostAsync(UrlHelper.takimGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task UlkeEkle(Country country)
        {
            var json = JsonSerializer.Serialize(country);
            JsonContent jsonContent = JsonContent.Create(country);
            var response = await httpClient.PostAsync(UrlHelper.ulkeUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<Country>> GetCountry()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<Country>>(UrlHelper.ulkeUrl);
            return sonuc;
        }

        public async Task UlkeSil(int countryid)
        {
            var url = UrlHelper.ulkeUrl + $"/{countryid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task UlkeGuncelle(Country country)
        {
            var json = JsonSerializer.Serialize(country);
            JsonContent jsonContent = JsonContent.Create(country);
            var response = await httpClient.PostAsync(UrlHelper.ulkeGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<String>> GetCountryName()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<String>>(UrlHelper.ulkeAd);
            

            return sonuc;
        }

        public async Task<int> GetUlkeId(string cName)
        {
            var sonuc = await httpClient.GetFromJsonAsync<int>($"{UrlHelper.ulkeAd}/{cName}");


            return sonuc;
        }

        public async Task LigEkle(League league)
        {

            var json = JsonSerializer.Serialize(league);
            JsonContent jsonContent = JsonContent.Create(league);
            var response = await httpClient.PostAsync(UrlHelper.ligUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task LigSil(int leagueid)
        {
            var url = UrlHelper.ligUrl + $"/{leagueid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task LigGuncelle(League league)
        {
            var json = JsonSerializer.Serialize(league);
            JsonContent jsonContent = JsonContent.Create(league);
            var response = await httpClient.PostAsync(UrlHelper.ligGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<League>> GetLeague()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<League>>(UrlHelper.ligUrl);
            return sonuc;
        }

       

        public async Task<List<string>> GetTeamName()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<String>>(UrlHelper.takimAd);


            return sonuc;
        }

        public async Task<int> GetTakimId(string cName)
        {
            var sonuc = await httpClient.GetFromJsonAsync<int>($"{UrlHelper.takimAd}/{cName}");


            return sonuc;
        }

        public async Task<List<string>> GetLeagueName()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<String>>(UrlHelper.ligAd);


            return sonuc;
        }

        public async Task<int> GetLeagueId(string cName)
        {
            var sonuc = await httpClient.GetFromJsonAsync<int>($"{UrlHelper.ligAd}/{cName}");


            return sonuc;
        }

        public async Task LigTakimEkle(TeamLeague teamLeague)
        {
            var json = JsonSerializer.Serialize(teamLeague);
            JsonContent jsonContent = JsonContent.Create(teamLeague);
            var response = await httpClient.PostAsync(UrlHelper.takimLigUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<TeamLeague>> GetTeamLeague()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<TeamLeague>>(UrlHelper.takimLigUrl);
            return sonuc;
        }

        public async Task LigTakimSil(int leagueteamid)
        {
            var url = UrlHelper.takimLigUrl + $"/{leagueteamid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task LigTakimGuncelle(TeamLeague teamLeague)
        {
            var json = JsonSerializer.Serialize(teamLeague);
            JsonContent jsonContent = JsonContent.Create(teamLeague);
            var response = await httpClient.PostAsync(UrlHelper.takimLigGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<TeamLeagueViewModel>> GetLeagueTeamModel()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<TeamLeagueViewModel>>(UrlHelper.takimLigUrl);
            return sonuc;
        }

        public async Task LigTakimKazananEkle(TeamLeagueWinner teamLeagueWinner)
        {
            var json = JsonSerializer.Serialize(teamLeagueWinner);
            JsonContent jsonContent = JsonContent.Create(teamLeagueWinner);
            var response = await httpClient.PostAsync(UrlHelper.takimLigKazananUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<TeamLeagueWinnerViewModel>> GetLeagueWinnerTeamModel()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<TeamLeagueWinnerViewModel>>(UrlHelper.takimLigKazananUrl);
            return sonuc;
        }

        public async Task LigKazananTakimSil(int leagueteamwinnerid)
        {
            var url = UrlHelper.takimLigKazananUrl + $"/{leagueteamwinnerid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task LigKazananTakimGuncelle(TeamLeagueWinner teamLeagueWinner)
        {
            var json = JsonSerializer.Serialize(teamLeagueWinner);
            JsonContent jsonContent = JsonContent.Create(teamLeagueWinner);
            var response = await httpClient.PostAsync(UrlHelper.takimLigKazananGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        //Futbolcu Takım Transfer İşlemleri

        public async Task<List<string>> GetBallerSurname()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<String>>(UrlHelper.oyuncuSoyad);


            return sonuc;
        }

        public async Task<int> GetFutbolcuId(string cSurname)
        {
            var sonuc = await httpClient.GetFromJsonAsync<int>($"{UrlHelper.oyuncuSoyad}/{cSurname}");


            return sonuc;
        }

        public async Task TakimFutbolcuEkle(BallerTeam ballerTeam)
        {
            var json = JsonSerializer.Serialize(ballerTeam);
            JsonContent jsonContent = JsonContent.Create(ballerTeam);
            var response = await httpClient.PostAsync(UrlHelper.takimFutbolcuUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<TeamBallerViewModel>> GetTeamBallerModel()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<TeamBallerViewModel>>(UrlHelper.takimFutbolcuUrl);
            return sonuc;
        }

        public async Task FutbolcuTakimSil(int ballerteamid)
        {
            var url = UrlHelper.takimFutbolcuUrl + $"/{ballerteamid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task FutbolcuTakimGuncelle(BallerTeam teamBaller)
        {
            var json = JsonSerializer.Serialize(teamBaller);
            JsonContent jsonContent = JsonContent.Create(teamBaller);
            var response = await httpClient.PostAsync(UrlHelper.takimFutbolcuGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }
        //Futbolcu Performans Bilgileri
        public async Task FutbolcuPerformansEkle(BallerPerformance ballerPerformance)
        {
            var json = JsonSerializer.Serialize(ballerPerformance);
            JsonContent jsonContent = JsonContent.Create(ballerPerformance);
            var response = await httpClient.PostAsync(UrlHelper.futbolcuPerformansUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task<List<BallerPerformanceViewModel>> GetBallerPerformance()
        {
            var sonuc = await httpClient.GetFromJsonAsync<List<BallerPerformanceViewModel>>(UrlHelper.futbolcuPerformansUrl);
            return sonuc;
        }

        public async Task FutbolcuPerformansSil(int ballerperformanceid)
        {
            var url = UrlHelper.futbolcuPerformansUrl + $"/{ballerperformanceid}";
            await httpClient.DeleteAsync(url);
        }

        public async Task FutbolcuPerformansGuncelle(BallerPerformance ballerPerformance)
        {
            var json = JsonSerializer.Serialize(ballerPerformance);
            JsonContent jsonContent = JsonContent.Create(ballerPerformance);
            var response = await httpClient.PostAsync(UrlHelper.futbolcuPerformansGuncelleUrl, jsonContent);
            if (response.IsSuccessStatusCode)
            {

            }
        }

        public async Task Added(Member member)
        {
            var json = JsonSerializer.Serialize(member);
            JsonContent jsonContent = JsonContent.Create(member);
            var response = await httpClient.PostAsync("https://localhost:7108/member", jsonContent);
            if (response.IsSuccessStatusCode)
            {



            }
        }

        //Kullanıcı veri aratma

        public async Task<List<TeamSearchViewModel>> SearchLig(string filterText)
        {
            var result = await httpClient.GetFromJsonAsync<List<TeamSearchViewModel>>($"https://localhost:7108/league/{filterText}");
            return result;
        }

        public async Task<List<Team>> SearchTeam(string filterText)
        {
            var result = await httpClient.GetFromJsonAsync<List<Team>>($"https://localhost:7108/teams/{filterText}");
            return result;
        }

        public async Task<List<Country>> SearchCountry(string filterText)
        {
            var result = await httpClient.GetFromJsonAsync<List<Country>>($"https://localhost:7108/country/{filterText}");
            return result;
        }

        public async Task<List<Baller>> SearchBaller(string filterText)
        {
            var result = await httpClient.GetFromJsonAsync<List<Baller>>($"https://localhost:7108/players/{filterText}");
            return result;
        }
        //Kullanıcı girişi
        

        public async Task<int> GetMember(string memberMail, string memberPassword)
        {
            var result = await httpClient.GetFromJsonAsync<int>($"https://localhost:7108/member/{memberMail}/{memberPassword}");

            return result;
        }

        public async Task<int> GetUyeId(string mail)
        {
            var sonuc = await httpClient.GetFromJsonAsync<int>($"https://localhost:7108/member/{mail}");


            return sonuc;
        }
    }

    
    public class HttpsClientHandlerService
    {

        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if ANDROID
            var handler = new Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert != null && cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
#elif IOS
            var handler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl = IsHttpsLocalhost
            };
            return handler;
#else
     throw new PlatformNotSupportedException("Only Android and iOS supported.");
#endif
        }

#if IOS
        public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
        {
            if (url.StartsWith("https://localhost"))
                return true;
            return false;
        }
#endif
    }
}
