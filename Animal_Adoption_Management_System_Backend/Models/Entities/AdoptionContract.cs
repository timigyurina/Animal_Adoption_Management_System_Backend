namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class AdoptionContract
    {
        public int Id { get; set; }
        public DateTime ContractDate { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public Animal Animal { get; set; }
        public User Applier { get; set; }
        public Address ApplierAddress { get; set; }
    }
}
