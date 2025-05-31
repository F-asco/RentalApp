using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentalApp.Models;
using System;
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
            var existingRoles = roleManager.Roles.ToList();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
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

            if (!context.Equipment.Any())
            {
                context.Equipment.AddRange(new List<Equipment>
                {
                    new Equipment { Name = "Kamera Sony A7", Description = "Lustrzanka z funkcją nagrywania w 4K" },
                    new Equipment { Name = "Laptop Dell XPS 13", Description = "Ultrabook do pracy biurowej" },
                    new Equipment { Name = "Mikrofon Rode NT1", Description = "Mikrofon pojemnościowy do nagrań studyjnych" }
                });

                await context.SaveChangesAsync();
            }
            if (!context.Rentals.Any())
            {
                var kamera = context.Equipment.FirstOrDefault(e => e.Name.Contains("Sony"));
                var laptop = context.Equipment.FirstOrDefault(e => e.Name.Contains("Dell"));
                var userKasia = await userManager.FindByEmailAsync("kasia@klient.com");
                var userJan = await userManager.FindByEmailAsync("jan@firma.com");

                if (kamera != null && userKasia != null)
                {
                    context.Rentals.Add(new Rental
                    {
                        EquipmentId = kamera.Id,
                        UserId = userKasia.Id,
                        RentedAt = DateTime.UtcNow.AddDays(-2)
                    });
                }

                if (laptop != null && userJan != null)
                {
                    context.Rentals.Add(new Rental
                    {
                        EquipmentId = laptop.Id,
                        UserId = userJan.Id,
                        RentedAt = DateTime.UtcNow.AddDays(-1)
                    });
                }

                await context.SaveChangesAsync();
            }
        }

    }
}