namespace SeithmanSoftware.Login.Database.Models
{
    public class NewPasswordData
    {
        public int Id { get; set; }
        public byte[] PwSalt { get; set; }
        public byte[] PwHash { get; set; }
    }
}
