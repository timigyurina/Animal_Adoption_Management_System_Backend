namespace Animal_Adoption_Management_System_Backend.Models.DTOs.ImageDTOs
{
    public class ImageDTO : BaseImageDTO
    {
        public int Id { get; set; }
        public DateTime DateTaken { get; set; }
        public string ImagePath { get; set; }
    }
}
