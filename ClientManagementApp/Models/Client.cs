namespace ClientManagementApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public bool IsJsonGenerated { get; set; } = false;
    }
}
