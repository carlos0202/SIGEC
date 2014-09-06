using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class AnalysisMap : EntityTypeConfiguration<Analysis>
    {
        public AnalysisMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Analysis");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.createdBy).HasColumnName("createdBy");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.status).HasColumnName("status");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.Analyses)
                .HasForeignKey(d => d.createdBy);

        }
    }
}
