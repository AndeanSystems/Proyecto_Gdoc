using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class DocumentoAdjuntoMap : EntityTypeConfiguration<DocumentoAdjunto>
    {
        public DocumentoAdjuntoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDDoctoAdjunto);

            // Properties
            this.Property(t => t.TipoOperacion)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.TipoDoctoAdjunto)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("DocumentoAdjunto");
            this.Property(t => t.IDDoctoAdjunto).HasColumnName("IDDoctoAdjunto");
            this.Property(t => t.CodigoOperacion).HasColumnName("CodigoOperacion");
            this.Property(t => t.TipoOperacion).HasColumnName("TipoOperacion");
            this.Property(t => t.CodigoDoctoAdjunto).HasColumnName("CodigoDoctoAdjunto");
            this.Property(t => t.TipoDoctoAdjunto).HasColumnName("TipoDoctoAdjunto");
            this.Property(t => t.CodigoComentarioMesa).HasColumnName("CodigoComentarioMesa");
            this.Property(t => t.EstadoDoctoAdjunto).HasColumnName("EstadoDoctoAdjunto");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.DocumentoAdjuntoes)
                .HasForeignKey(d => d.CodigoOperacion);

        }
    }
}
