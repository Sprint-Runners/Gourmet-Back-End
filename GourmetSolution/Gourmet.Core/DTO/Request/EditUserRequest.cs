using System.ComponentModel.DataAnnotations;

namespace Gourmet.Core.DTO.Request
{
    public class EditUserRequest
    {
        [Required(ErrorMessage = "UserName is required")]
        public string oldusername { get; set; }
        public string newusername { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string aboutme { get; set; }=string.Empty;

    }
}
