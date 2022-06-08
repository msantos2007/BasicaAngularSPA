using webapibasica.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace webapibasica.Data.Configuration
{
    public class AlunoNotaConfiguration : IEntityTypeConfiguration<AlunoNota>
    {
        public void Configure(EntityTypeBuilder<AlunoNota> builder)
        {
            builder.ToTable("aluno_nota_tb");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.AlunoId).HasColumnName("id_aluno").IsRequired();
            builder.Property(x => x.Nota).HasColumnName("nota");
            builder.Property(x => x.DtInclusao).HasColumnName("dt_inclusao");
            builder.Property(x => x.DtModificacao).HasColumnName("dt_modificacao").HasDefaultValue(new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc));

            builder.HasOne(an => an.Aluno);
        }
    }
}