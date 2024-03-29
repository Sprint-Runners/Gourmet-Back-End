using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{
    public class ImageUserRequest
    {
        [Required(ErrorMessage = "file is required")]
        public IFormFile file {  get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string username {  get; set; }
    }
}
