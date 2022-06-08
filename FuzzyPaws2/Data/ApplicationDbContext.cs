
using FuzzyPaws2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuzzyPaws2.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetType> PetType { get; set; }
        public DbSet<PetBreed> PetBreed { get; set; }
        public DbSet<IdentityUser> AspNetUsers { get; set; }
        public DbSet<IdentityRole> AspNetRoles { get; set; }
        public DbSet<MyPet> MyPets { get; set; }

    }
}