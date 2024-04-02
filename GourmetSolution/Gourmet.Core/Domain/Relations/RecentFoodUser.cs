using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Relations
{
    public class RecentFoodUser
    {
        [Required]
        public DateTime VisitTime {  get; set; }
        [Required]
        public Guid FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food food { get; set; }
        [Required]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }
    }
}
