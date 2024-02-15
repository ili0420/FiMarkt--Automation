using ApiService.EF;
using ApiService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



app.MapGet("/", () => "Hello World!");

//Futbolcu Ekleme-Silme-Güncelleme
app.MapGet("/players", () =>
{
    Context context = new Context();

    return context.Baller.ToList();
});

app.MapPost("/players", (Baller baller) =>
{
    Context context = new Context();
    context.Baller.Add(baller);
    context.SaveChanges();

});

app.MapDelete("/players/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.Baller.Where(x => x.ballerId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.Baller.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/players/guncelle", (Baller player) =>
{
    Context context = new Context();
    context.Baller.Update(player);
    context.SaveChanges();

});

//Takým Ekleme-Silme-Güncelleme
app.MapGet("/teams", () =>
{
    Context context = new Context();

    return context.Team.ToList();
});

app.MapPost("/teams", (Team team) =>
{
    Context context = new Context();
    context.Team.Add(team);
    context.SaveChanges();

});

app.MapDelete("/teams/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.Team.Where(x => x.teamId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.Team.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/teams/guncelle", (Team team) =>
{
    Context context = new Context();
    context.Team.Update(team);
    context.SaveChanges();

});
//Takým Adý Getirme (Bunu yapma amacý takýma futbolcu ya da lige takým ekleme kýsmýnda
//takýmlarýn adlarýný pickerda listeleme)

app.MapGet("/teams/teamname", () =>
{
    Context context = new Context();

    return context.Team.Select(x => x.teamName).ToList();
});
//Takým adýna göre id getir (Bunu yapma amacý takýma futbolcu ya da lige takým ekleme kýsmýnda
//ekle sil güncelle iþlemlerini yapma)

app.MapGet("/teams/teamname/{name}", (String name) =>
{
    Context context = new Context();

    var sonuc = context.Team.Where(x => x.teamName == name).Select(x => x.teamId).ToList();
    var id = Convert.ToInt32(sonuc[0]);
    return id;
});

//Ülke Ekleme-Silme-Güncellem

app.MapGet("/countrys", () =>
{
    Context context = new Context();

    return context.Country.ToList();
});

app.MapPost("/countrys", (Country country) =>
{
    Context context = new Context();
    context.Country.Add(country);
    context.SaveChanges();

});

app.MapDelete("/countrys/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.Country.Where(x => x.countryId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.Country.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/countrys/guncelle", (Country country) =>
{
    Context context = new Context();
    context.Country.Update(country);
    context.SaveChanges();

});

//Ülke Adý Getirme (Bunu yapma amacý futbolcuya uyruk eklerken
//ülkelerin adlarýný pickerda listeleme)

app.MapGet("/countrys/countryname", () =>
{
    Context context = new Context();

    return context.Country.Select(x=> x.countryName).ToList();
});
//Ülke Adýna göre id Getirme (Bunu yapma amacý futbolcuya uyruk eklerken
//ekleme iþlemi yapma)
app.MapGet("/countrys/countryname/{name}", (String name) =>
{
    Context context = new Context();

    var sonuc = context.Country.Where(x => x.countryName == name).Select(x => x.countryId).ToList();
    var id = Convert.ToInt32(sonuc[0]);
    return id;
});


//Lig EKLE-SÝL-GÜNCELLE


app.MapGet("/leagues", () =>
{
    Context context = new Context();

    return context.League.ToList();
});

app.MapPost("/leagues", (League league) =>
{
    Context context = new Context();
    context.League.Add(league);
    context.SaveChanges();

});

app.MapDelete("/leagues/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.League.Where(x => x.leagueId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.League.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/leagues/guncelle", (League league) =>
{
    Context context = new Context();
    context.League.Update(league);
    context.SaveChanges();

});

//Lig Adý getir
app.MapGet("/leagues/leaguename", () =>
{
    Context context = new Context();

    return context.League.Select(x => x.leagueName).ToList();
});
//Lig adýna göre id getir

app.MapGet("/leagues/leaguename/{name}", (String name) =>
{
    Context context = new Context();

    var sonuc = context.League.Where(x => x.leagueName == name).Select(x => x.leagueId).ToList();
    var id = Convert.ToInt32(sonuc[0]);
    return id;
});

//Lige Takým Ekleme Ýþlemleri (Sezon sezon)

app.MapGet("/teamLeague", () =>
{
    Context context = new Context();

    

    var result = (from team in context.Team
                  join teamLeague
in context.TeamLeague on team.teamId equals teamLeague.teamId
                  join league in context.League
                  on teamLeague.leagueId equals league.leagueId
                  select new
                  {
                      teamLeagueId= teamLeague.teamLeagueId,
                      teamName = team.teamName,
                      leagueName = league.leagueName,
                      startYear = teamLeague.startYear,
                      endYear = teamLeague.endYear,
                  }).ToList();
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
    return result;



});

app.MapPost("/teamLeague", (TeamLeague teamLeague) =>
{
    Context context = new Context();
    context.TeamLeague.Add(teamLeague);
    context.SaveChanges();

});

app.MapDelete("/teamLeague/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.TeamLeague.Where(x => x.teamLeagueId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.TeamLeague.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/teamLeague/guncelle", (TeamLeague teamLeague) =>
{
    Context context = new Context();
    context.TeamLeague.Update(teamLeague);
    context.SaveChanges();

});



//Ligi Kazanan Takýmlarý  Ekleme Ýþlemleri (Sezon sezon)

app.MapGet("/teamLeagueWinners", () =>
{
    Context context = new Context();

    var result = (from team in context.Team
                  join teamLeagueWinners
in context.teamLeagueWinners on team.teamId equals teamLeagueWinners.teamId
                  join league in context.League
                  on teamLeagueWinners.leagueId equals league.leagueId
                  select new
                  {
                      TeamLeagueWinnerId = teamLeagueWinners.teamLeagueWinnerId,
                      teamName = team.teamName,
                      leagueName = league.leagueName,
                      startYear = teamLeagueWinners.startYear,
                      endYear = teamLeagueWinners.endYear
                  }).ToList();
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
    return result;



});

app.MapPost("/teamLeagueWinners", (TeamLeagueWinner teamLeagueWinner) =>
{
    Context context = new Context();
    context.teamLeagueWinners.Add(teamLeagueWinner);
    context.SaveChanges();

});

app.MapDelete("/teamLeagueWinners/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.teamLeagueWinners.Where(x => x.teamLeagueWinnerId == id).FirstOrDefault();
    if (silinecek != null)
    {
        context.teamLeagueWinners.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/teamLeagueWinners/guncelle", (TeamLeagueWinner teamLeagueWinner) =>
{
    Context context = new Context();
    context.teamLeagueWinners.Update(teamLeagueWinner);
    context.SaveChanges();

});






//Futbolcu Takým Transfer Ýþlemleri 

app.MapGet("/ballerteam", () =>
{
    Context context = new Context();

    var result = (from team in context.Team
                  join ballerTeam
in context.BallerTeam on team.teamId equals ballerTeam.teamId
                  join baller in context.Baller
                  on ballerTeam.ballerId equals baller.ballerId
                  select new
                  {
                      BallerTeamId = ballerTeam.ballerTeamId,
                      teamName =team.teamName,
                      ballerName = baller.ballerName,
                      ballerSurname = baller.ballerSurname,
                      contractStartTime = ballerTeam.contractStartTime,
                      contractEndTime = ballerTeam.contractEndTime,
                      salary=ballerTeam.salary,
                      shirtNumber=ballerTeam.shirtNumber,
                  }).ToList();
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
    return result;



});

app.MapPost("/ballerteam", (BallerTeam ballerTeam) =>
{
    Context context = new Context();
    context.BallerTeam.Add(ballerTeam);
    context.SaveChanges();

});

app.MapDelete("/ballerteam/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.BallerTeam.Where(x => x.ballerTeamId== id).FirstOrDefault();
    if (silinecek != null)
    {
        context.BallerTeam.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/ballerteam/guncelle", (BallerTeam ballerTeam) =>
{
    Context context = new Context();
    context.BallerTeam.Update(ballerTeam);
    context.SaveChanges();

});

//Futbolcu SoyAdý getir
app.MapGet("/players/playersurname", () =>
{
    Context context = new Context();

    return context.Baller.Select(x => x.ballerSurname).ToList();
});
//Futbolcu SoyAdýna göre id getir

app.MapGet("/players/playersurname/{surname}", (String surname) =>
{
    Context context = new Context();

    var sonuc = context.Baller.Where(x => x.ballerSurname == surname).Select(x => x.ballerId).ToList();
    var id = Convert.ToInt32(sonuc[0]);
    return id;
});


//Futbolcu Performans Bilgileri

app.MapGet("/ballerperformance", () =>
{
    Context context = new Context();

    var result = (from baller in context.Baller
                  join ballerPerformance
in context.BallerPerformance on baller.ballerId equals ballerPerformance.ballerId
                  join league in context.League
                  on ballerPerformance.LeagueId equals league.leagueId
                  select new
                  {
                      ballerPerformanceId = ballerPerformance.ballerPerformanceId,
                      ballerName = baller.ballerName,
                      ballerSurname = baller.ballerSurname,
                      leagueName=league.leagueName,
                      startYear = ballerPerformance.startYear,
                      endYear = ballerPerformance.endYear,
                      goal = ballerPerformance.goal,
                      assist = ballerPerformance.assist,
                      redCard=ballerPerformance.redCard,
                      yellowCard=ballerPerformance.yellowCard,
                      totalMatch=ballerPerformance.totalMatchNumber,
                      totalTime=ballerPerformance.time,
                      goalConceded=ballerPerformance.goalConceded,
                      cleanSheet=ballerPerformance.cleanSheet
                  }).ToList();
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
    return result;



});

app.MapPost("/ballerperformance", (BallerPerformance ballerPerformance) =>
{
    Context context = new Context();
    context.BallerPerformance.Add(ballerPerformance);
    context.SaveChanges();

});

app.MapDelete("/ballerperformance/{Id}", (int id) =>
{
    Context context = new Context();
    var silinecek = context.BallerPerformance.Where(x => x.ballerPerformanceId== id).FirstOrDefault();
    if (silinecek != null)
    {
        context.BallerPerformance.Remove(silinecek);
        context.SaveChanges();
    }


});

app.MapPost("/ballerperformance/guncelle", (BallerPerformance ballerPerformance) =>
{
    Context context = new Context();
    context.BallerPerformance.Update(ballerPerformance);
    context.SaveChanges();

});

//Üye Kayýt Olma
app.MapPost("/member", (Member member) =>
{

    Context context = new Context();
    context.Member.Add(member);
    context.SaveChanges();
});

//Kullanýcý Futbolcu Aratma

app.MapGet("/players/{filterText}", (String filterText) =>
{
    Context context = new Context();
    var gelenballer = context.Baller.Where(x => x.ballerName.StartsWith(filterText)).ToList();
    return gelenballer;
});

//Kullanýcý Takým Aratma

app.MapGet("/teams/{filterText}", (String filterText) =>
{

    Context context = new Context();

    var gelentakim = context.Team.Where(x => x.teamName.StartsWith(filterText)).ToList();
    return gelentakim;
});

//Ülke Aratma

app.MapGet("/country/{filterText}", (String filterText) =>
{

    Context context = new Context();

    var gelencountry = context.Country.Where(x => x.countryName.StartsWith(filterText)).ToList();
    return gelencountry;
});

// Lig Aratma

app.MapGet("/league/{filterText}", (String filterText) =>
{

    Context context = new Context();

    var result = (from team in context.Team
                  join teamLeague
in context.TeamLeague on team.teamId equals teamLeague.teamId
                  join league in context.League
                  on teamLeague.leagueId equals league.leagueId
                  select new
                  {
                      teamName= team.teamName,
                      startYear=teamLeague.startYear,
                      leagueName=league.leagueName
                  }).Where(x=>x.startYear==2022).Where(y=>y.leagueName==filterText).ToList();
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
    return result;

    
    
});

//Üye giriþi 

app.MapGet("/member/{memberMail}/{memberPassword}", (String memberMail,String memberPassword) =>
{

    Context context = new Context();
    var memberLog=context.Member.Where(x => x.MemberMail == memberMail && x.MemberPassword == memberPassword).Select(x=>x.MemberId).ToList();
    int id = Convert.ToInt32(memberLog[0]);
    
    return id;
    
    
});

//Üyenin mailine göre id getir

app.MapGet("/member/{memberMail}", (String mail) =>
{
    Context context = new Context();

    var sonuc = context.Member.Where(x => x.MemberMail == mail).Select(x => x.MemberId).ToList();
    var id = Convert.ToInt32(sonuc[0]);
    return id;
});


app.Run();
