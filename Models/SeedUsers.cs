using LibApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Models
{
    public static class SeedUsers
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Customers.Any())
                {
                    Console.WriteLine("Users already seeded");
                    return;
                }
                context.Customers.AddRange(
                    new Customer
                    {
                        Name = "Osoba1 Osobiasta",
                        HasNewsletterSubscribed = false,
                        MembershipType = context.MembershipTypes.Where(c => c.Id == 1).FirstOrDefault(),
                        MembershipTypeId = 1,
                        Birthdate = new DateTime(1995, 10, 15)

                    },
                    new Customer
                    {
                        Name = "Osoba2 Osobiasta",
                        HasNewsletterSubscribed = false,
                        MembershipType = context.MembershipTypes.Where(c => c.Id == 1).FirstOrDefault(),
                        MembershipTypeId = 1,
                        Birthdate = new DateTime(1995, 12, 5)

                    },
                    new Customer
                    {
                        Name = "Osoba3 Osobiasta",
                        HasNewsletterSubscribed = false,
                        MembershipType = context.MembershipTypes.Where(c => c.Id == 1).FirstOrDefault(),
                        MembershipTypeId = 1,
                        Birthdate = new DateTime(1995, 6, 6)

                    },
                    new Customer
                    {
                        Name = "Osoba1 Osobiasta",
                        HasNewsletterSubscribed = false,
                        MembershipType = context.MembershipTypes.Where(c => c.Id == 1).FirstOrDefault(),
                        MembershipTypeId = 1,
                        Birthdate = new DateTime(1995, 10, 15)

                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
