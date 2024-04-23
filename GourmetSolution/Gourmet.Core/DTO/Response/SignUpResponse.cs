//using System;
using Gourmet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SignUpResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
    }

}
