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
    public class RecipeStep
    {
        [Required]
        public Guid RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe recipe { get; set; }
        [Required]
        public int Number {  get; set; }
        [Required]
        [MaxLength(15000)]
        public string explenation {  get; set; }
    }
}
