using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class Prescriptions_MedicinesMap : EntityTypeConfiguration<Prescriptions_Medicines>
    {
        public Prescriptions_MedicinesMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.administration)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Prescriptions_Medicines");
            this.Property(t => t.prescriptionID).HasColumnName("prescriptionID");
            this.Property(t => t.medicineID).HasColumnName("medicineID");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.consultationID).HasColumnName("consultationID");
            this.Property(t => t.administration).HasColumnName("administration");
            this.Property(t => t.ID).HasColumnName("ID");

            // Relationships
            this.HasRequired(t => t.Medicine)
                .WithMany(t => t.Prescriptions_Medicines)
                .HasForeignKey(d => d.medicineID);
            this.HasRequired(t => t.Prescription)
                .WithMany(t => t.Prescriptions_Medicines)
                .HasForeignKey(d => new { d.prescriptionID, d.patientID, d.consultationID });

        }
    }
}
