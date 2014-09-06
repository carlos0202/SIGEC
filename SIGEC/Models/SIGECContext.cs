using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SIGEC.Models.Mapping;

namespace SIGEC.Models
{
    public partial class SIGECContext : DbContext
    {
        static SIGECContext()
        {
            Database.SetInitializer<SIGECContext>(null);
        }

        public SIGECContext()
            : base("Name=SIGECContext")
        {
        }

        public DbSet<Action> Actions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Analysis> Analyses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Consultations_Payment> Consultations_Payment { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<DoctorRule> DoctorRules { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<ICD10> ICD10 { get; set; }
        public DbSet<InheritedDiseas> InheritedDiseases { get; set; }
        public DbSet<Insurer> Insurers { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescriptions_Medicines> Prescriptions_Medicines { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActionMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AnalysisMap());
            modelBuilder.Configurations.Add(new AppointmentMap());
            modelBuilder.Configurations.Add(new ConsultationMap());
            modelBuilder.Configurations.Add(new Consultations_PaymentMap());
            modelBuilder.Configurations.Add(new DiseaseMap());
            modelBuilder.Configurations.Add(new DoctorRuleMap());
            modelBuilder.Configurations.Add(new DoctorMap());
            modelBuilder.Configurations.Add(new ICD10Map());
            modelBuilder.Configurations.Add(new InheritedDiseasMap());
            modelBuilder.Configurations.Add(new InsurerMap());
            modelBuilder.Configurations.Add(new MedicalHistoryMap());
            modelBuilder.Configurations.Add(new MedicineMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new PatientMap());
            modelBuilder.Configurations.Add(new PhoneMap());
            modelBuilder.Configurations.Add(new PrescriptionMap());
            modelBuilder.Configurations.Add(new Prescriptions_MedicinesMap());
            modelBuilder.Configurations.Add(new ProcedureMap());
            modelBuilder.Configurations.Add(new StudyMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new webpages_MembershipMap());
            modelBuilder.Configurations.Add(new webpages_OAuthMembershipMap());
            modelBuilder.Configurations.Add(new webpages_RolesMap());
        }
    }
}
