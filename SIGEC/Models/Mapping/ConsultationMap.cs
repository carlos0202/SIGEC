using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class ConsultationMap : EntityTypeConfiguration<Consultation>
    {
        public ConsultationMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.referredTo)
                .HasMaxLength(60);

            // Table & Column Mappings
            this.ToTable("Consultations");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.doctorID).HasColumnName("doctorID");
            this.Property(t => t.appointmentID).HasColumnName("appointmentID");
            this.Property(t => t.ended).HasColumnName("ended");
            this.Property(t => t.reason).HasColumnName("reason");
            this.Property(t => t.treatment).HasColumnName("treatment");
            this.Property(t => t.observations).HasColumnName("observations");
            this.Property(t => t.referredTo).HasColumnName("referredTo");

            // Relationships
            this.HasMany(t => t.Analyses)
                .WithMany(t => t.Consultations)
                .Map(m =>
                    {
                        m.ToTable("Consultations_Analysis");
                        m.MapLeftKey("consultationID");
                        m.MapRightKey("analysisID");
                    });

            this.HasMany(t => t.Procedures)
                .WithMany(t => t.Consultations)
                .Map(m =>
                    {
                        m.ToTable("Consultations_Procedures");
                        m.MapLeftKey("consultationID");
                        m.MapRightKey("procedureID");
                    });

            this.HasMany(t => t.Studies)
                .WithMany(t => t.Consultations)
                .Map(m =>
                    {
                        m.ToTable("Consultations_Studies");
                        m.MapLeftKey("consultationID");
                        m.MapRightKey("studyID");
                    });

            this.HasOptional(t => t.Appointment)
                .WithMany(t => t.Consultations)
                .HasForeignKey(d => d.appointmentID);
            this.HasRequired(t => t.Doctor)
                .WithMany(t => t.Consultations)
                .HasForeignKey(d => d.doctorID);
            this.HasRequired(t => t.Patient)
                .WithMany(t => t.Consultations)
                .HasForeignKey(d => d.patientID);

        }
    }
}
