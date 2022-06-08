using webapibasica.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace webapibasica.Data.Configuration
{
    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("aluno_tb");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            builder.Property(x => x.Nome).HasColumnName("nome").IsRequired();
            builder.Property(x => x.DtNascimento).HasColumnName("dt_nascimento").IsRequired();
            builder.Property(x => x.DtInclusao).HasColumnName("dt_inclusao");
            builder.Property(x => x.DtModificacao).HasColumnName("dt_alteracao").HasDefaultValue(new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc));

            builder.HasMany(al => al.AlunoNotas)
                   .WithOne(al => al.Aluno);

            builder.HasMany(al => al.AlunoImagens)
                   .WithOne(al => al.Aluno);
        }
    }
}