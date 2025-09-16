
namespace Restaurant.BLL.ModelVM.AccountVM
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email Address Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
