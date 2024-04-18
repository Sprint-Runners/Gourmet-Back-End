using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IIngredientService
    {
        Task<IngredientResponse> Create(AddIngredientRequest request);
    }
}
