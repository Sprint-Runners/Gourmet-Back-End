using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class ShowChefInformationResponse
    {
        public string UserName {  get; set; }
        public string FullName { get; set; }
        public string Aboutme {  get; set; }
        public double Score {  get; set; }
        public string PhoneNumber {  get; set; }
        public string ImageURL {  get; set; }
        public List<SummaryRecipeInfoResponse>LastRecipes { get; set; }
        public List<SummaryRecipeInfoResponse> TopRecipes { get; set; }

    }
}
