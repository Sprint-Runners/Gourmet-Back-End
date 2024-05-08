
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IFoodService
    {
        //Task<IEnumerable<Food>> GetAllFoodWithONeMT(Guid category);
        //Task<IEnumerable<Food>> GetAllFoodWithOneN(Guid category);
        //Task<IEnumerable<Food>> GetAllFoodWithOneFT(Guid category);
        //Task<IEnumerable<Food>> GetAllFoodWithOneCM(Guid category);
        //Task<IEnumerable<Food>> GetAllFoodWithOnePSOI(Guid category);
        Task<FoodResponse> Create(AddFoodRequest request);
        Task<SearchRecipesFood> GetAllRecipe(string request);
        Task<List<Food>> Get_All();
        //Task<List<Special_Foods>> Get_Special();
    }
}
