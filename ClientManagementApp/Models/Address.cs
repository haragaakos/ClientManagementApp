namespace ClientManagementApp.Models
{
    public class Address
    {
        public int Id { get; set; }
        public AddressType Type { get; set; }
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty; //Irányítószám: lehetett volna egy négy számjegyű integer, viszont nem mindenhol a világon csak szám karakterből áll, ezért lett string
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty; //Házszám: lehetett volna integer is, de mivel lehetnek még külön megjelölések (pl. emelet, ajtó), ezért stringként hoztam létre
        [ForeignKey("Client")]
        public int ClientId {  get; set; }
        [JsonIgnore]
        public Client? Client { get; set; } = null!;

    }
    public enum AddressType
    {
        Szállítási,
        Számlázási,
        Egyéb
    }
}
