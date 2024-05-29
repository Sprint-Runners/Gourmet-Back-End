using System;
using Gourmet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gourmet.Core.DTO.Request
{
    public class BanUserRequest
    {
        public string UserName { get; set; }
    }
}
