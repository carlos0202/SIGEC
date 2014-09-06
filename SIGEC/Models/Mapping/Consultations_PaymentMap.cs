using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class Consultations_PaymentMap : EntityTypeConfiguration<Consultations_Payment>
    {
        public Consultations_PaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.paymentForm)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Consultations_Payment");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.consultationID).HasColumnName("consultationID");
            this.Property(t => t.price).HasColumnName("price");
            this.Property(t => t.discount).HasColumnName("discount");
            this.Property(t => t.insurer).HasColumnName("insurer");
            this.Property(t => t.netAmount).HasColumnName("netAmount");
            this.Property(t => t.total).HasColumnName("total");
            this.Property(t => t.paymentForm).HasColumnName("paymentForm");

            // Relationships
            this.HasRequired(t => t.Consultation)
                .WithMany(t => t.Consultations_Payment)
                .HasForeignKey(d => d.consultationID);

        }
    }
}
