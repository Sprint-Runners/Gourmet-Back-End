
using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IFoodService
    {
        Task<IEnumerable<Food>> GetAllFoodWithONeMT(Guid category);
        Task<IEnumerable<Food>> GetAllFoodWithOneN(Guid category);
        Task<IEnumerable<Food>> GetAllFoodWithOneFT(Guid category);
        Task<IEnumerable<Food>> GetAllFoodWithOneCM(Guid category);
        Task<IEnumerable<Food>> GetAllFoodWithOnePSOI(Guid category);
    }
}
