using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SearchResponse
    {
        public String SearchName {  get; set; }
        public int PartialRatioScore {  get; set; }
        public int RatioScore { get; set; }
    }
    public class SearchRecipeResponse
    {
        public bool Success { get; set; }
        public string Message {  get; set; }
    }
}
