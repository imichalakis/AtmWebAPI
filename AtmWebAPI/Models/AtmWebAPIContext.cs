using Microsoft.EntityFrameworkCore;
using AtmWebAPI.Models;



    public class AtmWebAPIDbContext : DbContext
    {
        public AtmWebAPIDbContext(DbContextOptions<AtmWebAPIDbContext> options)
            : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


}

