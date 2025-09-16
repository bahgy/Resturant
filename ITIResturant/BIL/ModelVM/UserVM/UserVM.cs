    namespace Restaurant.BLL.ModelVM.UserVM
    {
        public class UserVM
        {
            public int Id { get; set; }

            [Required]
            [MaxLength(50)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(50)]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Phone]
            public string PhoneNumber { get; set; }

            public DateTime CreatedDate { get; set; }

            [Required]
            public string UserType { get; set; }
            
            public string? Address { get; set; }
        }
    }
