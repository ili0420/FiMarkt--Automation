namespace ApiService.Models
{

    //Üye giriş çıkışı için bir tablo oluşturduk
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
}
