using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{
    public class FullSearchRequest
    {
        public string Text {  get; set; }
        public int Time {  get; set; }
        public string StartDate {  get; set; }
        public string EndDate { get; set; }
        public List<string> PSOI { get; set; }
        public List<string> CM {  get; set; }
        public List<string> DL {  get; set; }
        public List<string> FT {  get; set; }
        public List<string> MT {  get; set; }
        public List<string> N {  get; set; }
        public List<string> ingredients {  get; set; }
    }
}
