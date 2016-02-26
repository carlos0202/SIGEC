using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class AppointmentMap : EntityTypeConfiguration<Appointment>
    {
        public AppointmentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.finalStatus)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Appointments");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.createUser).HasColumnName("createUser");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.appointmentDate).HasColumnName("appointmentDate");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.doctorID).HasColumnName("doctorID");
            this.Property(t => t.finalStatus).HasColumnName("finalStatus");

            // Relationships
            this.HasRequired(t => t.Doctor)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.doctorID);
            this.HasRequired(t => t.Patient)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.patientID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.createUser);

        }
    }
}
