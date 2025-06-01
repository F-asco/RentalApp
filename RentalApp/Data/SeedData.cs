using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
                                            string[] roles,
                                            string adminEmail,
                                            string adminPassword)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            
            var existingRoles = roleManager.Roles.ToList();
            foreach (var role in existingRoles)
            {
                if (!roles.Contains(role.Name))
                {
                    await roleManager.DeleteAsync(role);
                }
            }

            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail };
                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            var emp = await userManager.FindByEmailAsync("jan@firma.com");
            if (emp == null)
            {
                emp = new ApplicationUser { UserName = "jan@firma.com", Email = "jan@firma.com" };
                await userManager.CreateAsync(emp, "Haslo123!");
                await userManager.AddToRoleAsync(emp, "Pracownik");
            }

            var client = await userManager.FindByEmailAsync("kasia@klient.com");
            if (client == null)
            {
                client = new ApplicationUser { UserName = "kasia@klient.com", Email = "kasia@klient.com" };
                await userManager.CreateAsync(client, "Haslo123!");
                await userManager.AddToRoleAsync(client, "Użytkownik");
            }

            
            var categories = new[] { "Narty", "Piłki", "Łyżwy" };
            foreach (var name in categories)
            {
                if (!context.EquipmentCategories.Any(c => c.Name == name))
                {
                    context.EquipmentCategories.Add(new EquipmentCategory { Name = name });
                }
            }
            await context.SaveChangesAsync();

            
            var narty = context.EquipmentCategories.FirstOrDefault(c => c.Name == "Narty");
            var pilki = context.EquipmentCategories.FirstOrDefault(c => c.Name == "Piłki");
            var obuwie = context.EquipmentCategories.FirstOrDefault(c => c.Name == "Łyżwy");

            
            if (!context.Equipment.Any())
            {
                context.Equipment.AddRange(new List<Equipment>
                {
                    new Equipment { Name = "Narty Fischer", Description = "Profesjonalne narty zjazdowe", QuantityAvailable = 5, CategoryId = narty.Id },
                    new Equipment { Name = "Piłka nożna Adidas", Description = "Piłka meczowa", QuantityAvailable = 10, CategoryId = pilki.Id },
                    new Equipment { Name = "Łyżwy Oxelo 500", Description = "Łyżwy ", QuantityAvailable = 7, CategoryId = obuwie.Id }
                });

                await context.SaveChangesAsync();
            }

            
            if (!context.Rentals.Any())
            {
                var eq1 = context.Equipment.FirstOrDefault(e => e.Name.Contains("Narty"));
                var eq2 = context.Equipment.FirstOrDefault(e => e.Name.Contains("Piłka"));
                var userKasia = await userManager.FindByEmailAsync("kasia@klient.com");
                var userJan = await userManager.FindByEmailAsync("jan@firma.com");

                if (eq1 != null && userKasia != null)
                {
                    context.Rentals.Add(new Rental
                    {
                        EquipmentId = eq1.Id,
                        UserId = userKasia.Id,
                        RentDate = DateTime.UtcNow.AddDays(-5),
                        DueDate = DateTime.UtcNow.AddDays(2),
                        IsReturned = false
                    });
                }

                if (eq2 != null && userJan != null)
                {
                    context.Rentals.Add(new Rental
                    {
                        EquipmentId = eq2.Id,
                        UserId = userJan.Id,
                        RentDate = DateTime.UtcNow.AddDays(-10),
                        DueDate = DateTime.UtcNow.AddDays(-2),
                        IsReturned = true,
                        ReturnedAt = DateTime.UtcNow.AddDays(-1)
                    });
                }

                await context.SaveChangesAsync();
            }
        }
    }
}