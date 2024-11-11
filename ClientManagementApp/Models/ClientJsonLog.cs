namespace ClientManagementApp.Models
{
    public class ClientJsonLog
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string JsonData { get; set; } = string.Empty; 
        public DateTime Timestamp { get; set; } 
    }
}
