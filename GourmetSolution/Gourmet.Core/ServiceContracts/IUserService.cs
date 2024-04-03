using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<Response> Edit(EditUserRequest request);
        Task<Response> Read(ReadUserRequest request);
        Task<IEnumerable<Food>> FavouritFoodByUser(string userId);
        Task<IEnumerable<Food>> RecentFoodByUser(string userId);
    }
}
