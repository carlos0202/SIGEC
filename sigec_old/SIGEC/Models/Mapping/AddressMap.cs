using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.city)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.municipality)
                .HasMaxLength(50);

            this.Property(t => t.number)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.sector)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.street)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.country)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Addresses");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.city).HasColumnName("city");
            this.Property(t => t.municipality).HasColumnName("municipality");
            this.Property(t => t.number).HasColumnName("number");
            this.Property(t => t.sector).HasColumnName("sector");
            this.Property(t => t.street).HasColumnName("street");
            this.Property(t => t.country).HasColumnName("country");
        }
    }
}
