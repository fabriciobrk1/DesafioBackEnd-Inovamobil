using ContaBancaria.Models;
using Microsoft.EntityFrameworkCore;

namespace ContaBancaria.Context
{ 
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
        
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        
    }
}
