
namespace Restaurant.BLL.ModelVMAccountVM
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email Address Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
