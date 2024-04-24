using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.OtherObject
{
    public class SearchRecipesFood
    {
        public bool IsSucceed { get; set; }
        public string Message {  get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
