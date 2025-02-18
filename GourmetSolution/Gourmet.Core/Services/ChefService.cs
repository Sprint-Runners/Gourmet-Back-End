﻿using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Services
{
    public class ChefService : IChefService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<Chef> _userManager;
        public ChefService(AppDbContext db, UserManager<Chef> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Recipe>> GetRecipesByChefId(string chefId)
        {
            var Recipes = _db.Recipes.Where(r => r.ChefId == chefId).ToList();
            return Recipes;
        }
        public async Task<IEnumerable<Recipe>> GetAcceptedRecipesByChefId(string chefId)
        {
            var Recipes = _db.Recipes.Where(r => r.ChefId == chefId && r.IsAccepted==true && r.IsReject==false).ToList();
            return Recipes;
        }
        public async Task<IEnumerable<Recipe>> GetNotAcceptedRecipesByChefId(string chefId)
        {
            var Recipes = _db.Recipes.Where(r => r.ChefId == chefId && r.IsAccepted == false && r.IsReject == false).ToList();
            return Recipes;
        }
        public async Task<IEnumerable<Recipe>> GetNotRejectedRecipesByChefId(string chefId)
        {
            var Recipes = _db.Recipes.Where(r => r.ChefId == chefId && r.IsAccepted == false && r.IsReject == true).ToList();
            return Recipes;
        }
        public async Task<double> GetChefScore(string chefId)
        {
            var foods = await GetAcceptedRecipesByChefId(chefId);
            double score = 0;
            if (foods.Count()> 0)
            {
                score = foods.Sum(r => r.Score);
                score=score/foods.Count();
            }
           
            var chef= await _userManager.FindByIdAsync(chefId);
            Console.WriteLine(chef.Id + "))))))000000000000000000000000000((((((" + chefId);
            chef.Score = score;
            _db.Users.Update(chef);
            _db.SaveChanges();
            Console.WriteLine("nnfjjjjjjjjjjjj0000000000000000000000000000000000jjjjj%%%%%%%%%%%%%" + chef.Score);
            return score;
        }
        public async Task<UserResponse> MakeChefAsync(UpdatePermissionRequest updatePermission)
        {
            var new_user = await _userManager.FindByNameAsync(updatePermission.UserName);

            if (new_user is null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!",
                    user = null
                };

            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.CHEF);
            var chef = await _userManager.FindByNameAsync(updatePermission.UserName);
            chef.Score = 0;
            return new UserResponse()
            {
                IsSucceed = true,
                user = (Chef)new_user,
                Message = "User is now an CHEF"
            };

        }
    }
}
