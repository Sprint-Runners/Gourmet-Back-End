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
    public class FavouritFoodUser
    {
        [Required]
        public Guid FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
        [Required]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }
        [Required]
        public DateTime TimeToLike { get; set; }

    }
}
