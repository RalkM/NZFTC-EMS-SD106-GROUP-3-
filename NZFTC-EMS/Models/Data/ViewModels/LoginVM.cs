using System.ComponentModel.DataAnnotations;

namespace NZFTC_EMS.ViewModels
{
    public class LoginVm
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
