using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class MedicineMap : EntityTypeConfiguration<Medicine>
    {
        public MedicineMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.usage)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.dosage)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.genericName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Medicines");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.usage).HasColumnName("usage");
            this.Property(t => t.dosage).HasColumnName("dosage");
            this.Property(t => t.genericName).HasColumnName("genericName");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createdBy).HasColumnName("createdBy");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Medicines)
                .HasForeignKey(d => d.createdBy);

        }
    }
}
