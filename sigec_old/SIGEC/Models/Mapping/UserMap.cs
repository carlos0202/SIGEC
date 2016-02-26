using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.dni)
                .HasMaxLength(20);

            this.Property(t => t.email)
                .HasMaxLength(50);

            this.Property(t => t.firstName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.lastName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.gender)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.maritalStatus)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.username)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.occupation)
                .HasMaxLength(50);

            this.Property(t => t.password)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.salt)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.religion)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Users");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.addressID).HasColumnName("addressID");
            this.Property(t => t.bornDate).HasColumnName("bornDate");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.dni).HasColumnName("dni");
            this.Property(t => t.email).HasColumnName("email");
            this.Property(t => t.firstName).HasColumnName("firstName");
            this.Property(t => t.lastName).HasColumnName("lastName");
            this.Property(t => t.gender).HasColumnName("gender");
            this.Property(t => t.maritalStatus).HasColumnName("maritalStatus");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.occupation).HasColumnName("occupation");
            this.Property(t => t.lastVisit).HasColumnName("lastVisit");
            this.Property(t => t.password).HasColumnName("password");
            this.Property(t => t.salt).HasColumnName("salt");
            this.Property(t => t.superUser).HasColumnName("superUser");
            this.Property(t => t.religion).HasColumnName("religion");

            // Relationships
            this.HasRequired(t => t.Address)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.addressID);

        }
    }
}
