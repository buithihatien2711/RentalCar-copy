using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentalCar.Model.Models;

namespace RentalCar.Data.Seed
{
    public class Seed
    {
        public static void SeedRole(DataContext context)
        {
            if(context.Roles.Any()) return;

            context.Roles.Add(new Role(1, "admin"));
            context.Roles.Add(new Role(2, "renter"));
            context.Roles.Add(new Role(3, "lease"));

            context.SaveChanges();
            
        }
    }
}