using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class TopChefResponse
    {
        public string Name {  get; set; }
        public string ImagePath {  get; set; }
        public double Score {  get; set; }
        public string AboutMe { get; set; }
        public List<SummaryRecipeInfoResponse> Top_Chef_Recipes { get; set; }
    }
}
