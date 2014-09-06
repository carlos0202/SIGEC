using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class DoctorRuleMap : EntityTypeConfiguration<DoctorRule>
    {
        public DoctorRuleMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.availableDays)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DoctorRules");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.doctorID).HasColumnName("doctorID");
            this.Property(t => t.consultationPrice).HasColumnName("consultationPrice");
            this.Property(t => t.maxPatients).HasColumnName("maxPatients");
            this.Property(t => t.consultationStart).HasColumnName("consultationStart");
            this.Property(t => t.consultationEnd).HasColumnName("consultationEnd");
            this.Property(t => t.availableDays).HasColumnName("availableDays");

            // Relationships
            this.HasRequired(t => t.Doctor)
                .WithMany(t => t.DoctorRules)
                .HasForeignKey(d => d.doctorID);

        }
    }
}
