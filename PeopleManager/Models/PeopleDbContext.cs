using System.Data.Entity;

namespace PeopleManager.Models
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext() : base("PeopleDb")
        {
        }

        public DbSet<Person> People { get; set; }
    }
}