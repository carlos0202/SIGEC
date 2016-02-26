using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class InsurerMap : EntityTypeConfiguration<Insurer>
    {
        public InsurerMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RNC)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Insurers");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.RNC).HasColumnName("RNC");
            this.Property(t => t.addressID).HasColumnName("addressID");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.createdBy).HasColumnName("createdBy");

            // Relationships
            this.HasMany(t => t.Phones)
                .WithMany(t => t.Insurers)
                .Map(m =>
                    {
                        m.ToTable("Insurers_Phones");
                        m.MapLeftKey("insurerID");
                        m.MapRightKey("phoneID");
                    });

            this.HasOptional(t => t.Address)
                .WithMany(t => t.Insurers)
                .HasForeignKey(d => d.addressID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Insurers)
                .HasForeignKey(d => d.createdBy);

        }
    }
}
