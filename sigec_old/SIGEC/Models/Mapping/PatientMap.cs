using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class PatientMap : EntityTypeConfiguration<Patient>
    {
        public PatientMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Patients");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.createBy).HasColumnName("createBy");
            this.Property(t => t.userID).HasColumnName("userID");
            this.Property(t => t.lastConsultation).HasColumnName("lastConsultation");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Patients)
                .HasForeignKey(d => d.createBy);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Patients1)
                .HasForeignKey(d => d.userID);

        }
    }
}
