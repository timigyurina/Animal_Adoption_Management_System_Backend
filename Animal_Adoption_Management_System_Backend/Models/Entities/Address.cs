namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
    }
}
