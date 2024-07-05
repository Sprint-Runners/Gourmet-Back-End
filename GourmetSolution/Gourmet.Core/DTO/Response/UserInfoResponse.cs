using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class UserInfoResponse
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Aboutme { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageURL { get; set; }
        public bool isChef {  get; set; }
        public bool isPremium {  get; set; }
        public bool requestChef {  get; set; }
        public bool isBan {  get; set; }
        public DateTime premium {  get; set; }
    }
}
