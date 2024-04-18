using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Other_Object
{
    public class RecipeResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Recipe recipe { get; set; }
    }
    public class InCompleteRecipeResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public InCompleteRecipe recipe { get; set; }
    }
}