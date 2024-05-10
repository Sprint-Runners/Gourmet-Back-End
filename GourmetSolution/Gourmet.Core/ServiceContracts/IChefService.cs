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
    public interface IChefService
    {
        Task<double> GetChefScore(string chefId);
        Task<IEnumerable<Recipe>> GetRecipesByChefId(string chefId);
        Task<IEnumerable<Recipe>> GetAcceptedRecipesByChefId(string chefId);
        Task<IEnumerable<Recipe>> GetNotAcceptedRecipesByChefId(string chefId);
        Task<UserResponse> MakeChefAsync(UpdatePermissionRequest updatePermission);
    }
}
