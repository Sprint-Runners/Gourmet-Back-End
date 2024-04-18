using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.OtherObject
{
    public class IngredientResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Ingredient ingredient { get; set; }
    }
}
