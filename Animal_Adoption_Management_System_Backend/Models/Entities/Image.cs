namespace Animal_Adoption_Management_System_Backend.Models.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateTaken { get; set; }

        public User Uploader { get; set; }
        public Animal Animal { get; set; }
    }
}
