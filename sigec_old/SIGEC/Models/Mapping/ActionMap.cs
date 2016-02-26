using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class ActionMap : EntityTypeConfiguration<Action>
    {
        public ActionMap()
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
            this.ToTable("Actions");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.controllerID).HasColumnName("controllerID");
            this.Property(t => t.displayName).HasColumnName("displayName");

            // Relationships
            this.HasMany(t => t.webpages_Roles)
                .WithMany(t => t.Actions)
                .Map(m =>
                    {
                        m.ToTable("RolesActions");
                        m.MapLeftKey("ActionID");
                        m.MapRightKey("RoleID");
                    });

            this.HasRequired(t => t.Menu)
                .WithMany(t => t.Actions)
                .HasForeignKey(d => d.controllerID);

        }
    }
}
