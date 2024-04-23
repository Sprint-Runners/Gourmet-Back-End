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
    public interface ICategoriesService
    {
        Task<IEnumerable<Meal_Type>> GetAllMTCategory();
        Task<IEnumerable<Nationality>> GetAllNCategory();
        Task<IEnumerable<Food_type>> GetAllFTCategory();
        Task<IEnumerable<Cooking_Method>> GetAllCMCategory();
        Task<IEnumerable<Primary_Source_of_Ingredient>> GetAllPSOICategory();
        Task<PSOIResponse> CreatePSOICategory(AddCategoryRequest request);
        Task<CMResponse> CreateCMCategory(AddCategoryRequest request);
        Task<FTResponse> CreateFTCategory(AddCategoryRequest request);
        Task<NResponse> CreateNCategory(AddCategoryRequest request);
        Task<MTResponse> CreateMTCategory(AddCategoryRequest request);

    }
}
