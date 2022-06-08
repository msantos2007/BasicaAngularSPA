using Microsoft.EntityFrameworkCore;
using webapibasica.Entities;
using webapibasica.Data.Configuration;

namespace webapibasica.Data
{
    public class BasicaContext : DbContext
    {
        public DbSet<Aluno> AlunoDbSet { get; set; }
        public DbSet<AlunoNota> AlunoNotaDbSet { get; set; }
        public DbSet<AlunoImagem> AlunoImagemDbSet { get; set; }

        public BasicaContext(DbContextOptions<BasicaContext> options) : base(options)
        {
            AlunoDbSet = Set<Aluno>();
            AlunoNotaDbSet = Set<AlunoNota>();
            AlunoImagemDbSet = Set<AlunoImagem>();
            //PostgreSQL Connection String
            //> "Default": "User Id=postgres; Password=abc123; Host=localhost; Port=5432; Database=dbAluno"            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AlunoConfiguration());
            modelBuilder.ApplyConfiguration(new AlunoNotaConfiguration());
            modelBuilder.ApplyConfiguration(new AlunoImagemConfiguration());
        }
    }
}