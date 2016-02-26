using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.displayName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menus");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.displayName).HasColumnName("displayName");
        }
    }
}
