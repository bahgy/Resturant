
namespace Restaurant.DAL.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }  

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }   

        [MaxLength(1000)]
        public string Comment { get; set; }   

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
