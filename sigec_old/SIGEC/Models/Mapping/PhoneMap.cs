using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class PhoneMap : EntityTypeConfiguration<Phone>
    {
        public PhoneMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.number)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.notes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Phones");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.number).HasColumnName("number");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.notes).HasColumnName("notes");

            // Relationships
            this.HasMany(t => t.Users)
                .WithMany(t => t.Phones)
                .Map(m =>
                    {
                        m.ToTable("Users_Phones");
                        m.MapLeftKey("phoneID");
                        m.MapRightKey("userID");
                    });


        }
    }
}
