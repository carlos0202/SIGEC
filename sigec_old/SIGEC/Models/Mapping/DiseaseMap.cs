using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class DiseaseMap : EntityTypeConfiguration<Disease>
    {
        public DiseaseMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.diagnoseCode)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Diseases");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.diagnoseCode).HasColumnName("diagnoseCode");
            this.Property(t => t.observations).HasColumnName("observations");
            this.Property(t => t.startTime).HasColumnName("startTime");
            this.Property(t => t.endTime).HasColumnName("endTime");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.consultationID).HasColumnName("consultationID");

            // Relationships
            this.HasOptional(t => t.Consultation)
                .WithMany(t => t.Diseases)
                .HasForeignKey(d => d.consultationID);
            this.HasRequired(t => t.ICD10)
                .WithMany(t => t.Diseases)
                .HasForeignKey(d => d.diagnoseCode);
            this.HasRequired(t => t.Patient)
                .WithMany(t => t.Diseases)
                .HasForeignKey(d => d.patientID);

        }
    }
}
