using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.OtherObject
{
    public class ImageResponse
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
    }
}
