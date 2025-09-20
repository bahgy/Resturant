public class UserVM
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }
    public string? UserName { get; set; }

    [Required, DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    public string? PhoneNumber { get; set; }
    public string? UserType { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedDate { get; set; }
}
