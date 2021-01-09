namespace SeithmanSoftware.Login.Database.Models
{
    public class NewUserData
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public byte[] PwSalt { get; set; }

        public byte[] PwHash { get; set; }
    }
}
