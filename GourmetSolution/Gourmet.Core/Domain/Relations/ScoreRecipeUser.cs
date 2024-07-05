using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Relations
{
    public class ScoreRecipeUser
    {
        [Required]
        public Guid RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe recipe { get; set; }
        [Required]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }
        public int Rate {  get; set; }
    }
}
