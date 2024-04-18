using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.OtherObject
{
    public class PSOIResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Primary_Source_of_Ingredient PSOI { get; set; }

    }
    public class CMResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Cooking_Method CM { get; set; }

    }
    public class FTResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Food_type FT { get; set; }

    }
    public class NResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Nationality N {  get; set; }

    }
    public class MTResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Meal_Type MT {  get; set; }

    }
}
