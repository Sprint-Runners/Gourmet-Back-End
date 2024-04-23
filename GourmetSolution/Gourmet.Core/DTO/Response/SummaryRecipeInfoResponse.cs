using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SummaryRecipeInfoResponse
    {
        public string Name {  get; set; }
        public string ChefName { get; set; }
        public double Score {  get; set; }
        public string ImagePath { get; set; }
    }
}
