using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SummaryChefInformation
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email {  get; set; }
        public string ImageURL { get; set; }
        public double Score { get; set; }
        public string AboutMe { get; set; }
        public string Experience {  get; set; }
    }
}
