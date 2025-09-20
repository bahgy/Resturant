
namespace Restaurant.BLL.Helper
{
    public static class UserNameHelper
    {
        public static string GetUserNameFromEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return string.Empty;
            var atIndex = email.IndexOf('@');
            return atIndex > 0 ? email.Substring(0, atIndex) : email;
        }
    }
}
