using System.ComponentModel.DataAnnotations;

namespace Gourmet.Core.DTO.Request
{
    public class EditUserRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Gen { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string FullName {  get; set; }
        public string Aboutme { get; set; }=string.Empty;

    }
}
