using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Services
{
    public class IngredientService:IIngredientService
    {
        private readonly AppDbContext _db;
        public IngredientService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IngredientResponse> Create(AddIngredientRequest request)
        {
            var isExistIngredient= _db.Ingredients.Where(r=>r.Name.ToLower()==request.Name.ToLower()).FirstOrDefault();
            if (isExistIngredient != null)
            {
                return new IngredientResponse
                {
                    IsSucceed = false,
                    Message = "This Ingredient Already Exists",
                    ingredient=null
                };
            }
            Ingredient ingredient = new Ingredient
            {
                Id = new Guid(),
                Name = request.Name.ToLower()

            };
            _db.Ingredients.Add(ingredient);
            _db.SaveChanges();
            return new IngredientResponse
            {
                IsSucceed = true,
                Message = "Ingredient Added Successfully",
                ingredient=ingredient
            };
        }
    }
}
