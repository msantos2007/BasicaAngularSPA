using webapibasica.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace webapibasica.Data.Configuration
{
    public class AlunoImagemConfiguration : IEntityTypeConfiguration<AlunoImagem>
    {
        public void Configure(EntityTypeBuilder<AlunoImagem> builder)
        {
            builder.ToTable("aluno_imagem_tb");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.AlunoId).HasColumnName("id_aluno").IsRequired();
            builder.Property(x => x.Ativo).HasColumnName("ativo").IsRequired().HasDefaultValue(true);
            builder.Property(x => x.ImagemId).HasColumnName("id_imagem").HasMaxLength(24);

            builder.HasOne(an => an.Aluno);
        }
    }
}