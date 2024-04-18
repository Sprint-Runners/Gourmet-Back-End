using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _db;
        public CategoriesService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<PSOIResponse> CreatePSOICategory(AddCategoryRequest request)
        {
            var isExistPSOI = _db.PSOIs.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistPSOI != null)
            {
                return new PSOIResponse
                {
                    IsSucceed = false,
                    Message = "This PSOI Already Exists",
                    PSOI=null
                };
            }
            Primary_Source_of_Ingredient PSOI = new Primary_Source_of_Ingredient
            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.PSOIs.Add(PSOI);
            _db.SaveChanges();
            return new PSOIResponse
            {
                IsSucceed = true,
                Message = "PSOI Added Successfully",
                PSOI=PSOI
            };
        }
        public async Task<CMResponse> CreateCMCategory(AddCategoryRequest request)
        {
            var isExistCM = _db.CMs.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistCM != null)
            {
                return new CMResponse
                {
                    IsSucceed = false,
                    Message = "This CM Already Exists",
                    CM=null
                };
            }
            Cooking_Method CM = new Cooking_Method
            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.CMs.Add(CM);
            _db.SaveChanges();
            return new CMResponse
            {
                IsSucceed = true,
                Message = "CM Added Successfully",
                CM=CM
            };
        }
        public async Task<FTResponse> CreateFTCategory(AddCategoryRequest request)
        {
            var isExistFT = _db.FTs.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistFT != null)
            {
                return new FTResponse
                {
                    IsSucceed = false,
                    Message = "This FT Already Exists",
                    FT=null
                };
            }
            Food_type FT = new Food_type            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.FTs.Add(FT);
            _db.SaveChanges();
            return new FTResponse
            {
                IsSucceed = true,
                Message = "FT Added Successfully",
                FT=FT
            };
        }
        public async Task<NResponse> CreateNCategory(AddCategoryRequest request)
        {
            var isExistN = _db.Ns.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistN != null)
            {
                return new NResponse
                {
                    IsSucceed = false,
                    Message = "This N Already Exists",
                    N=null
                };
            }
            Nationality N = new Nationality
            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.Ns.Add(N);
            _db.SaveChanges();
            return new NResponse
            {
                IsSucceed = true,
                Message = "N Added Successfully",
                N=N
            };
        }
        public async Task<MTResponse> CreateMTCategory(AddCategoryRequest request)
        {
            var isExistMT = _db.MTs.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistMT != null)
            {
                return new MTResponse
                {
                    IsSucceed = false,
                    Message = "This MT Already Exists",
                    MT=null
                };
            }
            Meal_Type MT = new Meal_Type
            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.MTs.Add(MT);
            _db.SaveChanges();
            return new MTResponse
            {
                IsSucceed = true,
                Message = "MT Added Successfully",
                MT=MT
            };
        }
        public async Task<IEnumerable<Primary_Source_of_Ingredient>> GetAllPSOICategory()
        {
            var PSOIs =  _db.PSOIs.ToList();
            return PSOIs;
        }
        public async Task<IEnumerable<Cooking_Method>> GetAllCMCategory()
        {
            var CMs =  _db.CMs.ToList();
            return CMs;
        }
        public async Task<IEnumerable<Food_type>> GetAllFTCategory()
        {
            var FTs =  _db.FTs.ToList();
            return FTs;
        }
        public async Task<IEnumerable<Nationality>> GetAllNCategory()
        {
            var Ns = _db.Ns.ToList();
            return Ns;
        }
        public async Task<IEnumerable<Meal_Type>> GetAllMTCategory()
        {
            var MTs = _db.MTs.ToList();
            return MTs;
        }
    }
}
