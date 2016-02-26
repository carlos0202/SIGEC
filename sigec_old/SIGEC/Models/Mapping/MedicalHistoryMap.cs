using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SIGEC.Models.Mapping
{
    public class MedicalHistoryMap : EntityTypeConfiguration<MedicalHistory>
    {
        public MedicalHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.GDM)
                .HasMaxLength(200);

            this.Property(t => t.menarche)
                .HasMaxLength(200);

            this.Property(t => t.menstruation)
                .HasMaxLength(200);

            this.Property(t => t.preclampsia)
                .HasMaxLength(200);

            this.Property(t => t.allergies)
                .HasMaxLength(200);

            this.Property(t => t.psychiatric)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("MedicalHistory");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.alcohol).HasColumnName("alcohol");
            this.Property(t => t.CCF).HasColumnName("CCF");
            this.Property(t => t.childhoodIllnesses).HasColumnName("childhoodIllnesses");
            this.Property(t => t.createDate).HasColumnName("createDate");
            this.Property(t => t.currentCondition).HasColumnName("currentCondition");
            this.Property(t => t.diabetes).HasColumnName("diabetes");
            this.Property(t => t.drugs).HasColumnName("drugs");
            this.Property(t => t.gastrointestinal).HasColumnName("gastrointestinal");
            this.Property(t => t.GDM).HasColumnName("GDM");
            this.Property(t => t.HTN).HasColumnName("HTN");
            this.Property(t => t.menarche).HasColumnName("menarche");
            this.Property(t => t.menstruation).HasColumnName("menstruation");
            this.Property(t => t.naturalMedicines).HasColumnName("naturalMedicines");
            this.Property(t => t.others).HasColumnName("others");
            this.Property(t => t.preclampsia).HasColumnName("preclampsia");
            this.Property(t => t.respiratory).HasColumnName("respiratory");
            this.Property(t => t.skinAppendages).HasColumnName("skinAppendages");
            this.Property(t => t.surgeries).HasColumnName("surgeries");
            this.Property(t => t.tobacco).HasColumnName("tobacco");
            this.Property(t => t.transfusions).HasColumnName("transfusions");
            this.Property(t => t.urinaryReproductive).HasColumnName("urinaryReproductive");
            this.Property(t => t.patientID).HasColumnName("patientID");
            this.Property(t => t.completed).HasColumnName("completed");
            this.Property(t => t.allergies).HasColumnName("allergies");
            this.Property(t => t.psychiatric).HasColumnName("psychiatric");

            // Relationships
            this.HasRequired(t => t.Patient)
                .WithMany(t => t.MedicalHistories)
                .HasForeignKey(d => d.patientID);

        }
    }
}
