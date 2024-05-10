namespace TaskAPI.Models
{
    public class UserForgotPasswordRequestModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
        public string VerificationCode { get; set; }
    }
}
