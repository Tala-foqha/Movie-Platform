using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Utils
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task DataSeed()
        {
            string[] roles = { "User", "Admin", "SuperAdmin" };


            foreach (var role in roles)

            {
                //هل الرول موجودة من قبل في قاعدة البيانات اول ما نشغل قاعدة اابنات بنتأكد عشان نربط الادوار فيها 
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
    }

