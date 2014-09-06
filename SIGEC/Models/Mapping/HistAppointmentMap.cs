using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class HistAppointmentMap : EntityTypeConfiguration<HistAppointment>
    {
        public HistAppointmentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.patientID });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.patientID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.finalStatus)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("HistAppointments");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.createUser).HasColumnName("createUser");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.appointmentDate).HasColumnName("appointmentDate");
            this.Property(t => t.finalStatus).HasColumnName("finalStatus");
            this.Property(t => t.doctorID).HasColumnName("doctorID");
        }
    }
}
