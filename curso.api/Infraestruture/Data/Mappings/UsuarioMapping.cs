using curso.api.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace curso.api.Infraestruture.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("TB_USUARIO");
            builder.HasKey(P => P.Codigo);
            builder.Property(P => P.Codigo).ValueGeneratedOnAdd();
            builder.Property(P => P.Login);
            builder.Property(P => P.Senha);
            builder.Property(P => P.Email);
        }
    }
}
