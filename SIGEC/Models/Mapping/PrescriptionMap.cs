using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class PrescriptionMap : EntityTypeConfiguration<Prescription>
    {
        public PrescriptionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.patientID, t.consultationID });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.notes)
                .IsRequired();

            this.Property(t => t.patientID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.consultationID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Prescriptions");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.notes).HasColumnName("notes");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.consultationID).HasColumnName("consultationID");

            // Relationships
            this.HasRequired(t => t.Consultation)
                .WithMany(t => t.Prescriptions)
                .HasForeignKey(d => d.consultationID);

        }
    }
}
