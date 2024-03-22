using Microsoft.EntityFrameworkCore;
using TaskAPI.Entities;

namespace TaskAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}