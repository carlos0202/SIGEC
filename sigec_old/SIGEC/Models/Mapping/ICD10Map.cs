using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class ICD10Map : EntityTypeConfiguration<ICD10>
    {
        public ICD10Map()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description_es)
                .HasMaxLength(400);

            this.Property(t => t.Description_en)
                .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("ICD10");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Description_es).HasColumnName("Description_es");
            this.Property(t => t.Description_en).HasColumnName("Description_en");
        }
    }
}
