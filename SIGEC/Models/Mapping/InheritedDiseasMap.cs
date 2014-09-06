using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class InheritedDiseasMap : EntityTypeConfiguration<InheritedDiseas>
    {
        public InheritedDiseasMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.disease)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.familyMember)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("InheritedDiseases");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.disease).HasColumnName("disease");
            this.Property(t => t.familyMember).HasColumnName("familyMember");
            this.Property(t => t.medicalHistoryID).HasColumnName("medicalHistoryID");

            // Relationships
            this.HasRequired(t => t.MedicalHistory)
                .WithMany(t => t.InheritedDiseases)
                .HasForeignKey(d => d.medicalHistoryID);

        }
    }
}
