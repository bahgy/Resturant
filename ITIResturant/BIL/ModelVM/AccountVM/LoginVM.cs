
namespace Restaurant.BLL.ModelVMAccountVM
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email Address Required")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
