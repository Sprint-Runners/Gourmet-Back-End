using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SummaryCategoryRecipeResponse
    {
        public string Ingredients {  get; set; }
        public string Description {  get; set; }
        public string Name {  get; set; }
        public string Category {  get; set; }
    }
}
