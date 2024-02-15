using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fimarkt.Models
{



    public class Baller
    {
        public int ballerId { get; set; }
        public string ballerName { get; set; }
        public string ballerSurname { get; set; }
        public DateTime ballerbirthTime { get; set; }

        public string ballerbirthPlace { get; set; }

        public decimal ballerHeight { get; set; }
        public string ballerPosition { get; set; }

        public decimal ballerValue { get; set; }
        public string ballerFoot { get; set; }
        public string ballerImageUrl { get; set; }

        public int countryId { get; set; } 

        public Country country { get; set; }


    }

    public class Team
    {
        public int teamId { get; set; }
        public string teamName { get; set; }

        public string teamStadiumName { get; set; }
        public int teamStadiumCapacity { get; set; }
        public string teamImageUrl { get; set; }


        public int countryId { get; set; }
        public Country country { get; set; }



    }

    public class League
    {
        public int leagueId { get; set; }
        public string leagueName { get; set; }
        public string leagueImageUrl { get; set; }

        public string cupImageUrl { get; set; }

        public int countryId { get; set; }

        public Country country { get; set; }


    }


    public class Country
    {
        public int countryId { get; set; }
        public string countryName { get; set; }

        public string countryImageUrl { get; set; }

    }

    public class Users
    {
        public int usersId { get; set; }
        public string usersName { get; set; }
        public string usersPassword { get; set; }

    }




    public class BallerTeam
    {
        public int ballerTeamId { get; set; }
        public int ballerId { get; set; }
        public Baller baller { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }
        public DateTime contractStartTime { get; set; }
        public DateTime contractEndTime { get; set; }
        public decimal salary { get; set; }
        public int shirtNumber { get; set; }
    }


    public class BallerPerformance
    {
        public int ballerPerformanceId { get; set; }
        public int ballerId { get; set; }
        public Baller baller { get; set; }
        public int LeagueId { get; set; }
        public League league { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
        public int goal { get; set; }
        public int assist { get; set; }
        public int redCard { get; set; }
        public int yellowCard { get; set; }
        public int goalConceded { get; set; }
        public int totalMatchNumber { get; set; }
        public int time { get; set; }
        public int cleanSheet { get; set; }
    }
    public class TeamLeague
    {

        public int teamLeagueId { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }
        public int leagueId { get; set; }

        public League league { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }

    }

    public class TeamLeagueViewModel
    {
        public int TeamLeagueId { get; set; }
        public string TeamName { get; set; }
        public string LeagueName { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }

    public class TeamLeagueWinnerViewModel
    {
        public int TeamLeagueWinnerId { get; set; }
        public string TeamName { get; set; }
        public string LeagueName { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
    public class TeamBallerViewModel
    {
        public int BallerTeamId { get; set; }
        public string BallerName { get; set; }
        public string BallerSurname { get; set; }
        public string TeamName { get; set; }
        public DateTime ContractStartTime { get; set; }
        public DateTime ContractEndTime { get; set; }

        public decimal Salary { get; set; }
        public int ShirtNumber { get; set; }
    }

    public class BallerPerformanceViewModel
    {
        public int ballerPerformanceId { get; set; }
        public string BallerName { get; set; }
        public string BallerSurname { get; set; }
        public string LeagueName { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public int Goal { get; set; }

        public int Assist { get; set; }

        public int RedCard { get; set; }

        public int YellowCard { get; set; }

        public int TotalMatch { get; set; }

        public int TotalTime { get; set; }

        public int GoalConceded { get; set; }
        public int CleanSheet { get; set; }
    }
    public class TeamLeagueWinner
    {
        public int teamLeagueWinnerId { get; set; }

        public int teamId { get; set; }
        public Team team { get; set; }
        public int leagueId { get; set; }
        public League league { get; set; }
        public int startYear { get; set; }
        public int endYear { get; set; }
    }

    public class Member1
    {

        public Guid MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }

        public DateTime MemberStartYear { get; set; }
        public string MemberGender { get; set; }
        public string MemberMail { get; set; }
        public string MemberCity { get; set; }
        public string MemberPassword { get; set; }
        public string MemberagainPassword { get; set; }
    }

    public class Member
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberSurname { get; set; }

        public DateTime MemberStartYear { get; set; }
        public string MemberGender { get; set; }
        public string MemberMail { get; set; }
        public string MemberCity { get; set; }
        public string MemberPassword { get; set; }
        public string MemberagainPassword { get; set; }
    }


    public class TeamSearchViewModel
    {
        
        public string TeamName { get; set; }
        public string LeagueName { get; set; }
        public int StartYear { get; set; }
    }














}


