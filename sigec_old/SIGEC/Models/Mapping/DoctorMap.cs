using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class DoctorMap : EntityTypeConfiguration<Doctor>
    {
        public DoctorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.speciality)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Doctors");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.speciality).HasColumnName("speciality");
            this.Property(t => t.userID).HasColumnName("userID");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Doctors)
                .HasForeignKey(d => d.userID);

        }
    }
}
