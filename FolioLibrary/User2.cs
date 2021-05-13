using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.users -> diku_mod_users.users
    // User2 -> User
    [CustomValidation(typeof(User2), nameof(ValidateUser2)), DisplayColumn(nameof(Username)), DisplayName("Users"), JsonConverter(typeof(JsonPathJsonConverter<User2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("users", Schema = "uc")]
    public partial class User2
    {
        public static ValidationResult ValidateUser2(User2 user2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (user2.Username != null && fsc.AnyUser2s($"id <> \"{user2.Id}\" and username == \"{user2.Username}\"")) return new ValidationResult("Username already exists");
            if (user2.ExternalSystemId != null && fsc.AnyUser2s($"id <> \"{user2.Id}\" and externalSystemId == \"{user2.ExternalSystemId}\"")) return new ValidationResult("External System Id already exists");
            if (user2.Barcode != null && fsc.AnyUser2s($"id <> \"{user2.Id}\" and barcode == \"{user2.Barcode}\"")) return new ValidationResult("Barcode already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.User.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("username"), Display(Order = 2), JsonProperty("username"), StringLength(1024)]
        public virtual string Username { get; set; }

        [Column("external_system_id"), Display(Name = "External System Id", Order = 3), JsonProperty("externalSystemId"), StringLength(1024)]
        public virtual string ExternalSystemId { get; set; }

        [Column("barcode"), Display(Order = 4), JsonProperty("barcode"), StringLength(1024)]
        public virtual string Barcode { get; set; }

        [Column("active"), Display(Order = 5), JsonProperty("active")]
        public virtual bool? Active { get; set; }

        [Column("type"), JsonProperty("type"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string Type { get; set; }

        [Display(Order = 7)]
        public virtual Group2 Group { get; set; }

        [Column("group_id"), Display(Name = "Group", Order = 8), JsonProperty("patronGroup"), Required]
        public virtual Guid? GroupId { get; set; }

        [Column("name"), Display(Order = 9), Editable(false), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("last_name"), Display(Name = "Last Name", Order = 10), JsonProperty("personal.lastName"), Required, StringLength(1024)]
        public virtual string LastName { get; set; }

        [Column("first_name"), Display(Name = "First Name", Order = 11), JsonProperty("personal.firstName"), StringLength(1024)]
        public virtual string FirstName { get; set; }

        [Column("middle_name"), Display(Name = "Middle Name", Order = 12), JsonProperty("personal.middleName"), StringLength(1024)]
        public virtual string MiddleName { get; set; }

        [Column("preferred_first_name"), Display(Name = "Preferred First Name", Order = 13), JsonProperty("personal.preferredFirstName"), StringLength(1024)]
        public virtual string PreferredFirstName { get; set; }

        [Column("email"), DataType(DataType.EmailAddress), Display(Name = "Email Address", Order = 14), JsonProperty("personal.email"), RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"), Required, StringLength(1024)]
        public virtual string EmailAddress { get; set; }

        [Column("phone"), DataType(DataType.PhoneNumber), Display(Name = "Phone Number", Order = 15), JsonProperty("personal.phone"), RegularExpression(@"^(\+\d{1,3} ?)?(\(?\d{3}\)?[ \-]?)?(\d{2})?\d[ \-]?\d{4}$"), StringLength(1024)]
        public virtual string PhoneNumber { get; set; }

        [Column("mobile_phone"), DataType(DataType.PhoneNumber), Display(Name = "Mobile Phone Number", Order = 16), JsonProperty("personal.mobilePhone"), RegularExpression(@"^(\+\d{1,3} ?)?(\(?\d{3}\)?[ \-]?)?(\d{2})?\d[ \-]?\d{4}$"), StringLength(1024)]
        public virtual string MobilePhoneNumber { get; set; }

        [Column("date_of_birth"), DataType(DataType.Date), Display(Name = "Birth Date", Order = 17), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("personal.dateOfBirth")]
        public virtual DateTime? BirthDate { get; set; }

        [Display(Name = "Preferred Contact Type", Order = 18)]
        public virtual ContactType PreferredContactType { get; set; }

        [Column("preferred_contact_type_id"), Display(Name = "Preferred Contact Type", Order = 19), JsonProperty("personal.preferredContactTypeId"), Required, StringLength(1024)]
        public virtual string PreferredContactTypeId { get; set; }

        [Column("enrollment_date"), DataType(DataType.Date), Display(Name = "Start Date", Order = 20), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("enrollmentDate")]
        public virtual DateTime? StartDate { get; set; }

        [Column("expiration_date"), DataType(DataType.Date), Display(Name = "End Date", Order = 21), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("expirationDate")]
        public virtual DateTime? EndDate { get; set; }

        [Column("source"), Display(Order = 22), JsonProperty("customFields.source"), RegularExpression(@"^(Library|University)$"), Required, StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("category"), Display(Order = 23), Editable(false), JsonProperty("customFields.category"), StringLength(1024)]
        public virtual string Category { get; set; }

        [Column("status"), Display(Order = 24), Editable(false), JsonProperty("customFields.status"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("statuses"), Display(Order = 25), Editable(false), JsonProperty("customFields.statuses"), StringLength(1024)]
        public virtual string Statuses { get; set; }

        [Column("staff_status"), Display(Name = "Staff Status", Order = 26), Editable(false), JsonProperty("customFields.staffStatus"), StringLength(1024)]
        public virtual string StaffStatus { get; set; }

        [Column("staff_privileges"), Display(Name = "Staff Privileges", Order = 27), Editable(false), JsonProperty("customFields.staffPrivileges"), StringLength(1024)]
        public virtual string StaffPrivileges { get; set; }

        [Column("staff_division"), Display(Name = "Staff Division", Order = 28), Editable(false), JsonProperty("customFields.staffDivision"), StringLength(1024)]
        public virtual string StaffDivision { get; set; }

        [Column("staff_department"), Display(Name = "Staff Department", Order = 29), Editable(false), JsonProperty("customFields.staffDepartment"), StringLength(1024)]
        public virtual string StaffDepartment { get; set; }

        [Column("student_id"), Display(Name = "Student Id", Order = 30), Editable(false), JsonProperty("customFields.studentId"), StringLength(1024)]
        public virtual string StudentId { get; set; }

        [Column("student_status"), Display(Name = "Student Status", Order = 31), Editable(false), JsonProperty("customFields.studentStatus"), StringLength(1024)]
        public virtual string StudentStatus { get; set; }

        [Column("student_restriction"), Display(Name = "Student Restriction", Order = 32), Editable(false), JsonProperty("customFields.studentRestriction")]
        public virtual bool? StudentRestriction { get; set; }

        [Column("student_division"), Display(Name = "Student Division", Order = 33), Editable(false), JsonProperty("customFields.studentDivision"), StringLength(1024)]
        public virtual string StudentDivision { get; set; }

        [Column("student_department"), Display(Name = "Student Department", Order = 34), Editable(false), JsonProperty("customFields.studentDepartment"), StringLength(1024)]
        public virtual string StudentDepartment { get; set; }

        [Column("deceased"), Display(Order = 35), JsonProperty("customFields.deceased")]
        public virtual bool? Deceased { get; set; }

        [Column("collections"), Display(Order = 36), JsonProperty("customFields.collections")]
        public virtual bool? Collections { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 37), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 38), InverseProperty("User2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 39), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 41), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 42), InverseProperty("User2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 43), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(User), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 45), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Acquisitions Units", Order = 46)]
        public virtual ICollection<AcquisitionsUnit2> AcquisitionsUnit2s { get; set; }

        [Display(Name = "Acquisitions Units 1", Order = 47)]
        public virtual ICollection<AcquisitionsUnit2> AcquisitionsUnit2s1 { get; set; }

        [Display(Order = 48)]
        public virtual ICollection<Address> Addresses { get; set; }

        [Display(Name = "Addresses 1", Order = 49)]
        public virtual ICollection<Address> Addresses1 { get; set; }

        [Display(Name = "Address Types", Order = 50)]
        public virtual ICollection<AddressType2> AddressType2s { get; set; }

        [Display(Name = "Address Types 1", Order = 51)]
        public virtual ICollection<AddressType2> AddressType2s1 { get; set; }

        [Display(Name = "Alternative Title Types", Order = 52)]
        public virtual ICollection<AlternativeTitleType2> AlternativeTitleType2s { get; set; }

        [Display(Name = "Alternative Title Types 1", Order = 53)]
        public virtual ICollection<AlternativeTitleType2> AlternativeTitleType2s1 { get; set; }

        [Display(Name = "Auth Attempts", Order = 54)]
        public virtual ICollection<AuthAttempt2> AuthAttempt2s { get; set; }

        [Display(Name = "Auth Credentials Historys", Order = 55)]
        public virtual ICollection<AuthCredentialsHistory2> AuthCredentialsHistory2s { get; set; }

        [Display(Name = "Auth Credentials Historys 1", Order = 56)]
        public virtual ICollection<AuthCredentialsHistory2> AuthCredentialsHistory2s1 { get; set; }

        [Display(Name = "Auth Credentials Historys 2", Order = 57)]
        public virtual ICollection<AuthCredentialsHistory2> AuthCredentialsHistory2s2 { get; set; }

        [Display(Name = "Batch Groups", Order = 58)]
        public virtual ICollection<BatchGroup2> BatchGroup2s { get; set; }

        [Display(Name = "Batch Groups 1", Order = 59)]
        public virtual ICollection<BatchGroup2> BatchGroup2s1 { get; set; }

        [Display(Name = "Batch Voucher Exports", Order = 60)]
        public virtual ICollection<BatchVoucherExport2> BatchVoucherExport2s { get; set; }

        [Display(Name = "Batch Voucher Exports 1", Order = 61)]
        public virtual ICollection<BatchVoucherExport2> BatchVoucherExport2s1 { get; set; }

        [Display(Name = "Batch Voucher Export Configs", Order = 62)]
        public virtual ICollection<BatchVoucherExportConfig2> BatchVoucherExportConfig2s { get; set; }

        [Display(Name = "Batch Voucher Export Configs 1", Order = 63)]
        public virtual ICollection<BatchVoucherExportConfig2> BatchVoucherExportConfig2s1 { get; set; }

        [Display(Name = "Blocks", Order = 64)]
        public virtual ICollection<Block2> Block2s { get; set; }

        [Display(Name = "Blocks 1", Order = 65)]
        public virtual ICollection<Block2> Block2s1 { get; set; }

        [Display(Name = "Blocks 2", Order = 66)]
        public virtual ICollection<Block2> Block2s2 { get; set; }

        [Display(Name = "Block Conditions", Order = 67)]
        public virtual ICollection<BlockCondition2> BlockCondition2s { get; set; }

        [Display(Name = "Block Conditions 1", Order = 68)]
        public virtual ICollection<BlockCondition2> BlockCondition2s1 { get; set; }

        [Display(Name = "Block Limits", Order = 69)]
        public virtual ICollection<BlockLimit2> BlockLimit2s { get; set; }

        [Display(Name = "Block Limits 1", Order = 70)]
        public virtual ICollection<BlockLimit2> BlockLimit2s1 { get; set; }

        [Display(Name = "Budgets", Order = 71)]
        public virtual ICollection<Budget2> Budget2s { get; set; }

        [Display(Name = "Budgets 1", Order = 72)]
        public virtual ICollection<Budget2> Budget2s1 { get; set; }

        [Display(Name = "Call Number Types", Order = 73)]
        public virtual ICollection<CallNumberType2> CallNumberType2s { get; set; }

        [Display(Name = "Call Number Types 1", Order = 74)]
        public virtual ICollection<CallNumberType2> CallNumberType2s1 { get; set; }

        [Display(Name = "Campuss", Order = 75)]
        public virtual ICollection<Campus2> Campus2s { get; set; }

        [Display(Name = "Campuss 1", Order = 76)]
        public virtual ICollection<Campus2> Campus2s1 { get; set; }

        [Display(Name = "Cancellation Reasons", Order = 77)]
        public virtual ICollection<CancellationReason2> CancellationReason2s { get; set; }

        [Display(Name = "Cancellation Reasons 1", Order = 78)]
        public virtual ICollection<CancellationReason2> CancellationReason2s1 { get; set; }

        [Display(Name = "Categorys", Order = 79)]
        public virtual ICollection<Category2> Category2s { get; set; }

        [Display(Name = "Categorys 1", Order = 80)]
        public virtual ICollection<Category2> Category2s1 { get; set; }

        [Display(Name = "Check Ins", Order = 81)]
        public virtual ICollection<CheckIn2> CheckIn2s { get; set; }

        [Display(Name = "Classification Types", Order = 82)]
        public virtual ICollection<ClassificationType2> ClassificationType2s { get; set; }

        [Display(Name = "Classification Types 1", Order = 83)]
        public virtual ICollection<ClassificationType2> ClassificationType2s1 { get; set; }

        [Display(Name = "Comments", Order = 84)]
        public virtual ICollection<Comment2> Comment2s { get; set; }

        [Display(Name = "Comments 1", Order = 85)]
        public virtual ICollection<Comment2> Comment2s1 { get; set; }

        [Display(Name = "Configurations", Order = 86)]
        public virtual ICollection<Configuration2> Configuration2s { get; set; }

        [Display(Name = "Configurations 1", Order = 87)]
        public virtual ICollection<Configuration2> Configuration2s1 { get; set; }

        [Display(Name = "Contacts", Order = 88)]
        public virtual ICollection<Contact2> Contact2s { get; set; }

        [Display(Name = "Contacts 1", Order = 89)]
        public virtual ICollection<Contact2> Contact2s1 { get; set; }

        [Display(Name = "Contact Addresses", Order = 90)]
        public virtual ICollection<ContactAddress> ContactAddresses { get; set; }

        [Display(Name = "Contact Addresses 1", Order = 91)]
        public virtual ICollection<ContactAddress> ContactAddresses1 { get; set; }

        [Display(Name = "Contact Emails", Order = 92)]
        public virtual ICollection<ContactEmail> ContactEmails { get; set; }

        [Display(Name = "Contact Emails 1", Order = 93)]
        public virtual ICollection<ContactEmail> ContactEmails1 { get; set; }

        [Display(Name = "Contact Phone Numbers", Order = 94)]
        public virtual ICollection<ContactPhoneNumber> ContactPhoneNumbers { get; set; }

        [Display(Name = "Contact Phone Numbers 1", Order = 95)]
        public virtual ICollection<ContactPhoneNumber> ContactPhoneNumbers1 { get; set; }

        [Display(Name = "Contact URLs", Order = 96)]
        public virtual ICollection<ContactUrl> ContactUrls { get; set; }

        [Display(Name = "Contact URLs 1", Order = 97)]
        public virtual ICollection<ContactUrl> ContactUrls1 { get; set; }

        [Display(Name = "Contributor Name Types", Order = 98)]
        public virtual ICollection<ContributorNameType2> ContributorNameType2s { get; set; }

        [Display(Name = "Contributor Name Types 1", Order = 99)]
        public virtual ICollection<ContributorNameType2> ContributorNameType2s1 { get; set; }

        [Display(Name = "Contributor Types", Order = 100)]
        public virtual ICollection<ContributorType2> ContributorType2s { get; set; }

        [Display(Name = "Contributor Types 1", Order = 101)]
        public virtual ICollection<ContributorType2> ContributorType2s1 { get; set; }

        [Display(Name = "Custom Fields", Order = 102)]
        public virtual ICollection<CustomField2> CustomField2s { get; set; }

        [Display(Name = "Custom Fields 1", Order = 103)]
        public virtual ICollection<CustomField2> CustomField2s1 { get; set; }

        [Display(Name = "Departments", Order = 104)]
        public virtual ICollection<Department2> Department2s { get; set; }

        [Display(Name = "Departments 1", Order = 105)]
        public virtual ICollection<Department2> Department2s1 { get; set; }

        [Display(Name = "Documents", Order = 106)]
        public virtual ICollection<Document2> Document2s { get; set; }

        [Display(Name = "Documents 1", Order = 107)]
        public virtual ICollection<Document2> Document2s1 { get; set; }

        [Display(Name = "Documents 2", Order = 108)]
        public virtual ICollection<Document2> Document2s2 { get; set; }

        [Display(Name = "Documents 3", Order = 109)]
        public virtual ICollection<Document2> Document2s3 { get; set; }

        [Display(Name = "Electronic Access Relationships", Order = 110)]
        public virtual ICollection<ElectronicAccessRelationship2> ElectronicAccessRelationship2s { get; set; }

        [Display(Name = "Electronic Access Relationships 1", Order = 111)]
        public virtual ICollection<ElectronicAccessRelationship2> ElectronicAccessRelationship2s1 { get; set; }

        [Display(Name = "Event Logs", Order = 112)]
        public virtual ICollection<EventLog2> EventLog2s { get; set; }

        [Display(Name = "Event Logs 1", Order = 113)]
        public virtual ICollection<EventLog2> EventLog2s1 { get; set; }

        [Display(Name = "Event Logs 2", Order = 114)]
        public virtual ICollection<EventLog2> EventLog2s2 { get; set; }

        [Display(Name = "Expense Classs", Order = 115)]
        public virtual ICollection<ExpenseClass2> ExpenseClass2s { get; set; }

        [Display(Name = "Expense Classs 1", Order = 116)]
        public virtual ICollection<ExpenseClass2> ExpenseClass2s1 { get; set; }

        [Display(Name = "Export Config Credentials", Order = 117)]
        public virtual ICollection<ExportConfigCredential2> ExportConfigCredential2s { get; set; }

        [Display(Name = "Export Config Credentials 1", Order = 118)]
        public virtual ICollection<ExportConfigCredential2> ExportConfigCredential2s1 { get; set; }

        [Display(Name = "Fees", Order = 119)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Fees 1", Order = 120)]
        public virtual ICollection<Fee2> Fee2s1 { get; set; }

        [Display(Name = "Fees 2", Order = 121)]
        public virtual ICollection<Fee2> Fee2s2 { get; set; }

        [Display(Name = "Fee Types", Order = 122)]
        public virtual ICollection<FeeType2> FeeType2s { get; set; }

        [Display(Name = "Fee Types 1", Order = 123)]
        public virtual ICollection<FeeType2> FeeType2s1 { get; set; }

        [Display(Name = "Finance Groups", Order = 124)]
        public virtual ICollection<FinanceGroup2> FinanceGroup2s { get; set; }

        [Display(Name = "Finance Groups 1", Order = 125)]
        public virtual ICollection<FinanceGroup2> FinanceGroup2s1 { get; set; }

        [Display(Name = "Fiscal Years", Order = 126)]
        public virtual ICollection<FiscalYear2> FiscalYear2s { get; set; }

        [Display(Name = "Fiscal Years 1", Order = 127)]
        public virtual ICollection<FiscalYear2> FiscalYear2s1 { get; set; }

        [Display(Name = "Fixed Due Date Schedules", Order = 128)]
        public virtual ICollection<FixedDueDateSchedule2> FixedDueDateSchedule2s { get; set; }

        [Display(Name = "Fixed Due Date Schedules 1", Order = 129)]
        public virtual ICollection<FixedDueDateSchedule2> FixedDueDateSchedule2s1 { get; set; }

        [Display(Order = 130)]
        public virtual ICollection<Format> Formats { get; set; }

        [Display(Name = "Formats 1", Order = 131)]
        public virtual ICollection<Format> Formats1 { get; set; }

        [Display(Name = "Funds", Order = 132)]
        public virtual ICollection<Fund2> Fund2s { get; set; }

        [Display(Name = "Funds 1", Order = 133)]
        public virtual ICollection<Fund2> Fund2s1 { get; set; }

        [Display(Name = "Groups", Order = 134)]
        public virtual ICollection<Group2> Group2s { get; set; }

        [Display(Name = "Groups 1", Order = 135)]
        public virtual ICollection<Group2> Group2s1 { get; set; }

        [Display(Name = "Holdings", Order = 136)]
        public virtual ICollection<Holding2> Holding2s { get; set; }

        [Display(Name = "Holdings 1", Order = 137)]
        public virtual ICollection<Holding2> Holding2s1 { get; set; }

        [Display(Name = "Holding Note Types", Order = 138)]
        public virtual ICollection<HoldingNoteType2> HoldingNoteType2s { get; set; }

        [Display(Name = "Holding Note Types 1", Order = 139)]
        public virtual ICollection<HoldingNoteType2> HoldingNoteType2s1 { get; set; }

        [Display(Name = "Holding Types", Order = 140)]
        public virtual ICollection<HoldingType2> HoldingType2s { get; set; }

        [Display(Name = "Holding Types 1", Order = 141)]
        public virtual ICollection<HoldingType2> HoldingType2s1 { get; set; }

        [Display(Name = "Id Types", Order = 142)]
        public virtual ICollection<IdType2> IdType2s { get; set; }

        [Display(Name = "Id Types 1", Order = 143)]
        public virtual ICollection<IdType2> IdType2s1 { get; set; }

        [Display(Name = "Ill Policys", Order = 144)]
        public virtual ICollection<IllPolicy2> IllPolicy2s { get; set; }

        [Display(Name = "Ill Policys 1", Order = 145)]
        public virtual ICollection<IllPolicy2> IllPolicy2s1 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Instance2> Instance2s { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Instance2> Instance2s1 { get; set; }

        [Display(Name = "Instance Note Types", Order = 148)]
        public virtual ICollection<InstanceNoteType2> InstanceNoteType2s { get; set; }

        [Display(Name = "Instance Note Types 1", Order = 149)]
        public virtual ICollection<InstanceNoteType2> InstanceNoteType2s1 { get; set; }

        [Display(Name = "Instance Types", Order = 150)]
        public virtual ICollection<InstanceType2> InstanceType2s { get; set; }

        [Display(Name = "Instance Types 1", Order = 151)]
        public virtual ICollection<InstanceType2> InstanceType2s1 { get; set; }

        [Display(Name = "Institutions", Order = 152)]
        public virtual ICollection<Institution2> Institution2s { get; set; }

        [Display(Name = "Institutions 1", Order = 153)]
        public virtual ICollection<Institution2> Institution2s1 { get; set; }

        [Display(Name = "Interfaces", Order = 154)]
        public virtual ICollection<Interface2> Interface2s { get; set; }

        [Display(Name = "Interfaces 1", Order = 155)]
        public virtual ICollection<Interface2> Interface2s1 { get; set; }

        [Display(Name = "Invoices", Order = 156)]
        public virtual ICollection<Invoice2> Invoice2s { get; set; }

        [Display(Name = "Invoices 1", Order = 157)]
        public virtual ICollection<Invoice2> Invoice2s1 { get; set; }

        [Display(Name = "Invoices 2", Order = 158)]
        public virtual ICollection<Invoice2> Invoice2s2 { get; set; }

        [Display(Name = "Invoice Items", Order = 159)]
        public virtual ICollection<InvoiceItem2> InvoiceItem2s { get; set; }

        [Display(Name = "Invoice Items 1", Order = 160)]
        public virtual ICollection<InvoiceItem2> InvoiceItem2s1 { get; set; }

        [Display(Name = "Issuance Modes", Order = 161)]
        public virtual ICollection<IssuanceMode> IssuanceModes { get; set; }

        [Display(Name = "Issuance Modes 1", Order = 162)]
        public virtual ICollection<IssuanceMode> IssuanceModes1 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item2> Item2s { get; set; }

        [Display(Name = "Items 1", Order = 164)]
        public virtual ICollection<Item2> Item2s1 { get; set; }

        [Display(Name = "Items 2", Order = 165)]
        public virtual ICollection<Item2> Item2s2 { get; set; }

        [Display(Name = "Item Damaged Statuss", Order = 166)]
        public virtual ICollection<ItemDamagedStatus2> ItemDamagedStatus2s { get; set; }

        [Display(Name = "Item Damaged Statuss 1", Order = 167)]
        public virtual ICollection<ItemDamagedStatus2> ItemDamagedStatus2s1 { get; set; }

        [Display(Name = "Item Note Types", Order = 168)]
        public virtual ICollection<ItemNoteType2> ItemNoteType2s { get; set; }

        [Display(Name = "Item Note Types 1", Order = 169)]
        public virtual ICollection<ItemNoteType2> ItemNoteType2s1 { get; set; }

        [Display(Name = "Job Executions", Order = 170)]
        public virtual ICollection<JobExecution2> JobExecution2s { get; set; }

        [Display(Name = "Ledgers", Order = 171)]
        public virtual ICollection<Ledger2> Ledger2s { get; set; }

        [Display(Name = "Ledgers 1", Order = 172)]
        public virtual ICollection<Ledger2> Ledger2s1 { get; set; }

        [Display(Name = "Ledger Rollovers", Order = 173)]
        public virtual ICollection<LedgerRollover2> LedgerRollover2s { get; set; }

        [Display(Name = "Ledger Rollovers 1", Order = 174)]
        public virtual ICollection<LedgerRollover2> LedgerRollover2s1 { get; set; }

        [Display(Name = "Ledger Rollover Errors", Order = 175)]
        public virtual ICollection<LedgerRolloverError2> LedgerRolloverError2s { get; set; }

        [Display(Name = "Ledger Rollover Errors 1", Order = 176)]
        public virtual ICollection<LedgerRolloverError2> LedgerRolloverError2s1 { get; set; }

        [Display(Name = "Ledger Rollover Progresss", Order = 177)]
        public virtual ICollection<LedgerRolloverProgress2> LedgerRolloverProgress2s { get; set; }

        [Display(Name = "Ledger Rollover Progresss 1", Order = 178)]
        public virtual ICollection<LedgerRolloverProgress2> LedgerRolloverProgress2s1 { get; set; }

        [Display(Name = "Librarys", Order = 179)]
        public virtual ICollection<Library2> Library2s { get; set; }

        [Display(Name = "Librarys 1", Order = 180)]
        public virtual ICollection<Library2> Library2s1 { get; set; }

        [Display(Name = "Loans", Order = 181)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        [Display(Name = "Loans 1", Order = 182)]
        public virtual ICollection<Loan2> Loan2s1 { get; set; }

        [Display(Name = "Loans 2", Order = 183)]
        public virtual ICollection<Loan2> Loan2s2 { get; set; }

        [Display(Name = "Loans 3", Order = 184)]
        public virtual ICollection<Loan2> Loan2s3 { get; set; }

        [Display(Name = "Loan Policys", Order = 185)]
        public virtual ICollection<LoanPolicy2> LoanPolicy2s { get; set; }

        [Display(Name = "Loan Policys 1", Order = 186)]
        public virtual ICollection<LoanPolicy2> LoanPolicy2s1 { get; set; }

        [Display(Name = "Loan Types", Order = 187)]
        public virtual ICollection<LoanType2> LoanType2s { get; set; }

        [Display(Name = "Loan Types 1", Order = 188)]
        public virtual ICollection<LoanType2> LoanType2s1 { get; set; }

        [Display(Name = "Locations", Order = 189)]
        public virtual ICollection<Location2> Location2s { get; set; }

        [Display(Name = "Locations 1", Order = 190)]
        public virtual ICollection<Location2> Location2s1 { get; set; }

        [Display(Name = "Location Settings", Order = 191)]
        public virtual ICollection<LocationSetting> LocationSettings { get; set; }

        [Display(Name = "Location Settings 1", Order = 192)]
        public virtual ICollection<LocationSetting> LocationSettings1 { get; set; }

        [Display(Name = "Logins", Order = 193)]
        public virtual ICollection<Login2> Login2s { get; set; }

        [Display(Name = "Logins 1", Order = 194)]
        public virtual ICollection<Login2> Login2s1 { get; set; }

        [Display(Name = "Logins 2", Order = 195)]
        public virtual ICollection<Login2> Login2s2 { get; set; }

        [Display(Name = "Lost Item Fee Policys", Order = 196)]
        public virtual ICollection<LostItemFeePolicy2> LostItemFeePolicy2s { get; set; }

        [Display(Name = "Lost Item Fee Policys 1", Order = 197)]
        public virtual ICollection<LostItemFeePolicy2> LostItemFeePolicy2s1 { get; set; }

        [Display(Name = "Manual Block Templates", Order = 198)]
        public virtual ICollection<ManualBlockTemplate2> ManualBlockTemplate2s { get; set; }

        [Display(Name = "Manual Block Templates 1", Order = 199)]
        public virtual ICollection<ManualBlockTemplate2> ManualBlockTemplate2s1 { get; set; }

        [Display(Name = "Material Types", Order = 200)]
        public virtual ICollection<MaterialType2> MaterialType2s { get; set; }

        [Display(Name = "Material Types 1", Order = 201)]
        public virtual ICollection<MaterialType2> MaterialType2s1 { get; set; }

        [Display(Name = "Nature Of Content Terms", Order = 202)]
        public virtual ICollection<NatureOfContentTerm2> NatureOfContentTerm2s { get; set; }

        [Display(Name = "Nature Of Content Terms 1", Order = 203)]
        public virtual ICollection<NatureOfContentTerm2> NatureOfContentTerm2s1 { get; set; }

        [Display(Name = "Notes", Order = 204)]
        public virtual ICollection<Note3> Note3s { get; set; }

        [Display(Name = "Notes 1", Order = 205)]
        public virtual ICollection<Note3> Note3s1 { get; set; }

        [Display(Name = "Note Types", Order = 206)]
        public virtual ICollection<NoteType2> NoteType2s { get; set; }

        [Display(Name = "Note Types 1", Order = 207)]
        public virtual ICollection<NoteType2> NoteType2s1 { get; set; }

        [Display(Name = "Orders", Order = 208)]
        public virtual ICollection<Order2> Order2s { get; set; }

        [Display(Name = "Orders 1", Order = 209)]
        public virtual ICollection<Order2> Order2s1 { get; set; }

        [Display(Name = "Orders 2", Order = 210)]
        public virtual ICollection<Order2> Order2s2 { get; set; }

        [Display(Name = "Orders 3", Order = 211)]
        public virtual ICollection<Order2> Order2s3 { get; set; }

        [Display(Name = "Order Items", Order = 212)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Order Items 1", Order = 213)]
        public virtual ICollection<OrderItem2> OrderItem2s1 { get; set; }

        [Display(Name = "Organizations", Order = 214)]
        public virtual ICollection<Organization2> Organization2s { get; set; }

        [Display(Name = "Organizations 1", Order = 215)]
        public virtual ICollection<Organization2> Organization2s1 { get; set; }

        [Display(Name = "Organization Addresses", Order = 216)]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

        [Display(Name = "Organization Addresses 1", Order = 217)]
        public virtual ICollection<OrganizationAddress> OrganizationAddresses1 { get; set; }

        [Display(Name = "Organization Emails", Order = 218)]
        public virtual ICollection<OrganizationEmail> OrganizationEmails { get; set; }

        [Display(Name = "Organization Emails 1", Order = 219)]
        public virtual ICollection<OrganizationEmail> OrganizationEmails1 { get; set; }

        [Display(Name = "Organization Phone Numbers", Order = 220)]
        public virtual ICollection<OrganizationPhoneNumber> OrganizationPhoneNumbers { get; set; }

        [Display(Name = "Organization Phone Numbers 1", Order = 221)]
        public virtual ICollection<OrganizationPhoneNumber> OrganizationPhoneNumbers1 { get; set; }

        [Display(Name = "Organization URLs", Order = 222)]
        public virtual ICollection<OrganizationUrl> OrganizationUrls { get; set; }

        [Display(Name = "Organization URLs 1", Order = 223)]
        public virtual ICollection<OrganizationUrl> OrganizationUrls1 { get; set; }

        [Display(Name = "Overdue Fine Policys", Order = 224)]
        public virtual ICollection<OverdueFinePolicy2> OverdueFinePolicy2s { get; set; }

        [Display(Name = "Overdue Fine Policys 1", Order = 225)]
        public virtual ICollection<OverdueFinePolicy2> OverdueFinePolicy2s1 { get; set; }

        [Display(Name = "Owners", Order = 226)]
        public virtual ICollection<Owner2> Owner2s { get; set; }

        [Display(Name = "Owners 1", Order = 227)]
        public virtual ICollection<Owner2> Owner2s1 { get; set; }

        [Display(Name = "Patron Action Sessions", Order = 228)]
        public virtual ICollection<PatronActionSession2> PatronActionSession2s { get; set; }

        [Display(Name = "Patron Action Sessions 1", Order = 229)]
        public virtual ICollection<PatronActionSession2> PatronActionSession2s1 { get; set; }

        [Display(Name = "Patron Action Sessions 2", Order = 230)]
        public virtual ICollection<PatronActionSession2> PatronActionSession2s2 { get; set; }

        [Display(Name = "Patron Notice Policys", Order = 231)]
        public virtual ICollection<PatronNoticePolicy2> PatronNoticePolicy2s { get; set; }

        [Display(Name = "Patron Notice Policys 1", Order = 232)]
        public virtual ICollection<PatronNoticePolicy2> PatronNoticePolicy2s1 { get; set; }

        [Display(Name = "Payments", Order = 233)]
        public virtual ICollection<Payment2> Payment2s { get; set; }

        [Display(Name = "Payment Methods", Order = 234)]
        public virtual ICollection<PaymentMethod2> PaymentMethod2s { get; set; }

        [Display(Name = "Payment Methods 1", Order = 235)]
        public virtual ICollection<PaymentMethod2> PaymentMethod2s1 { get; set; }

        [Display(Name = "Permissions", Order = 236)]
        public virtual ICollection<Permission2> Permission2s { get; set; }

        [Display(Name = "Permissions 1", Order = 237)]
        public virtual ICollection<Permission2> Permission2s1 { get; set; }

        [Display(Name = "Permissions Users", Order = 238)]
        public virtual ICollection<PermissionsUser2> PermissionsUser2s { get; set; }

        [Display(Name = "Permissions Users 1", Order = 239)]
        public virtual ICollection<PermissionsUser2> PermissionsUser2s1 { get; set; }

        [Display(Name = "Permissions Users 2", Order = 240)]
        public virtual ICollection<PermissionsUser2> PermissionsUser2s2 { get; set; }

        [Display(Name = "Preceding Succeeding Titles", Order = 241)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s { get; set; }

        [Display(Name = "Preceding Succeeding Titles 1", Order = 242)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s1 { get; set; }

        [Display(Order = 243)]
        public virtual ICollection<Printer> Printers { get; set; }

        [Display(Name = "Printers 1", Order = 244)]
        public virtual ICollection<Printer> Printers1 { get; set; }

        [Display(Name = "Proxys", Order = 245)]
        public virtual ICollection<Proxy2> Proxy2s { get; set; }

        [Display(Name = "Proxys 1", Order = 246)]
        public virtual ICollection<Proxy2> Proxy2s1 { get; set; }

        [Display(Name = "Proxys 2", Order = 247)]
        public virtual ICollection<Proxy2> Proxy2s2 { get; set; }

        [Display(Name = "Proxys 3", Order = 248)]
        public virtual ICollection<Proxy2> Proxy2s3 { get; set; }

        [Display(Name = "Records", Order = 249)]
        public virtual ICollection<Record2> Record2s { get; set; }

        [Display(Name = "Records 1", Order = 250)]
        public virtual ICollection<Record2> Record2s1 { get; set; }

        [Display(Name = "Refund Reasons", Order = 251)]
        public virtual ICollection<RefundReason2> RefundReason2s { get; set; }

        [Display(Name = "Refund Reasons 1", Order = 252)]
        public virtual ICollection<RefundReason2> RefundReason2s1 { get; set; }

        [Display(Order = 253)]
        public virtual ICollection<Relationship> Relationships { get; set; }

        [Display(Name = "Relationships 1", Order = 254)]
        public virtual ICollection<Relationship> Relationships1 { get; set; }

        [Display(Name = "Relationship Types", Order = 255)]
        public virtual ICollection<RelationshipType> RelationshipTypes { get; set; }

        [Display(Name = "Relationship Types 1", Order = 256)]
        public virtual ICollection<RelationshipType> RelationshipTypes1 { get; set; }

        [Display(Name = "Requests", Order = 257)]
        public virtual ICollection<Request2> Request2s { get; set; }

        [Display(Name = "Requests 1", Order = 258)]
        public virtual ICollection<Request2> Request2s1 { get; set; }

        [Display(Name = "Requests 2", Order = 259)]
        public virtual ICollection<Request2> Request2s2 { get; set; }

        [Display(Name = "Requests 3", Order = 260)]
        public virtual ICollection<Request2> Request2s3 { get; set; }

        [Display(Name = "Requests 4", Order = 261)]
        public virtual ICollection<Request2> Request2s4 { get; set; }

        [Display(Name = "Request Policys", Order = 262)]
        public virtual ICollection<RequestPolicy2> RequestPolicy2s { get; set; }

        [Display(Name = "Request Policys 1", Order = 263)]
        public virtual ICollection<RequestPolicy2> RequestPolicy2s1 { get; set; }

        [Display(Name = "Scheduled Notices", Order = 264)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        [Display(Name = "Scheduled Notices 1", Order = 265)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s1 { get; set; }

        [Display(Name = "Scheduled Notices 2", Order = 266)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s2 { get; set; }

        [Display(Name = "Service Points", Order = 267)]
        public virtual ICollection<ServicePoint2> ServicePoint2s { get; set; }

        [Display(Name = "Service Points 1", Order = 268)]
        public virtual ICollection<ServicePoint2> ServicePoint2s1 { get; set; }

        [Display(Name = "Service Point Users", Order = 269)]
        public virtual ICollection<ServicePointUser2> ServicePointUser2s { get; set; }

        [Display(Name = "Service Point Users 1", Order = 270)]
        public virtual ICollection<ServicePointUser2> ServicePointUser2s1 { get; set; }

        [Display(Name = "Service Point Users 2", Order = 271)]
        public virtual ICollection<ServicePointUser2> ServicePointUser2s2 { get; set; }

        [Display(Order = 272)]
        public virtual ICollection<Setting> Settings { get; set; }

        [Display(Name = "Settings 1", Order = 273)]
        public virtual ICollection<Setting> Settings1 { get; set; }

        [Display(Name = "Snapshots", Order = 274)]
        public virtual ICollection<Snapshot2> Snapshot2s { get; set; }

        [Display(Name = "Snapshots 1", Order = 275)]
        public virtual ICollection<Snapshot2> Snapshot2s1 { get; set; }

        [Display(Name = "Sources", Order = 276)]
        public virtual ICollection<Source2> Source2s { get; set; }

        [Display(Name = "Sources 1", Order = 277)]
        public virtual ICollection<Source2> Source2s1 { get; set; }

        [Display(Name = "Staff Slips", Order = 278)]
        public virtual ICollection<StaffSlip2> StaffSlip2s { get; set; }

        [Display(Name = "Staff Slips 1", Order = 279)]
        public virtual ICollection<StaffSlip2> StaffSlip2s1 { get; set; }

        [Display(Name = "Statistical Codes", Order = 280)]
        public virtual ICollection<StatisticalCode2> StatisticalCode2s { get; set; }

        [Display(Name = "Statistical Codes 1", Order = 281)]
        public virtual ICollection<StatisticalCode2> StatisticalCode2s1 { get; set; }

        [Display(Name = "Statistical Code Types", Order = 282)]
        public virtual ICollection<StatisticalCodeType2> StatisticalCodeType2s { get; set; }

        [Display(Name = "Statistical Code Types 1", Order = 283)]
        public virtual ICollection<StatisticalCodeType2> StatisticalCodeType2s1 { get; set; }

        [Display(Name = "Statuses 1", Order = 284)]
        public virtual ICollection<Status> Statuses1 { get; set; }

        [Display(Name = "Statuses 2", Order = 285)]
        public virtual ICollection<Status> Statuses2 { get; set; }

        [Display(Name = "Tags", Order = 286)]
        public virtual ICollection<Tag2> Tag2s { get; set; }

        [Display(Name = "Tags 1", Order = 287)]
        public virtual ICollection<Tag2> Tag2s1 { get; set; }

        [Display(Name = "Templates", Order = 288)]
        public virtual ICollection<Template2> Template2s { get; set; }

        [Display(Name = "Templates 1", Order = 289)]
        public virtual ICollection<Template2> Template2s1 { get; set; }

        [Display(Name = "Titles", Order = 290)]
        public virtual ICollection<Title2> Title2s { get; set; }

        [Display(Name = "Titles 1", Order = 291)]
        public virtual ICollection<Title2> Title2s1 { get; set; }

        [Display(Name = "Transactions", Order = 292)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Transactions 1", Order = 293)]
        public virtual ICollection<Transaction2> Transaction2s1 { get; set; }

        [Display(Name = "Transfer Accounts", Order = 294)]
        public virtual ICollection<TransferAccount2> TransferAccount2s { get; set; }

        [Display(Name = "Transfer Accounts 1", Order = 295)]
        public virtual ICollection<TransferAccount2> TransferAccount2s1 { get; set; }

        [Display(Name = "Users", Order = 296)]
        public virtual ICollection<User2> User2s { get; set; }

        [Display(Name = "Users 1", Order = 297)]
        public virtual ICollection<User2> User2s1 { get; set; }

        [Display(Name = "User Acquisitions Units", Order = 298)]
        public virtual ICollection<UserAcquisitionsUnit2> UserAcquisitionsUnit2s { get; set; }

        [Display(Name = "User Acquisitions Units 1", Order = 299)]
        public virtual ICollection<UserAcquisitionsUnit2> UserAcquisitionsUnit2s1 { get; set; }

        [Display(Name = "User Acquisitions Units 2", Order = 300)]
        public virtual ICollection<UserAcquisitionsUnit2> UserAcquisitionsUnit2s2 { get; set; }

        [Display(Name = "User Addresses", Order = 301), JsonProperty("personal.addresses")]
        public virtual ICollection<UserAddress> UserAddresses { get; set; }

        [Display(Name = "User Departments", Order = 302), JsonConverter(typeof(ArrayJsonConverter<List<UserDepartment>, UserDepartment>), "DepartmentId"), JsonProperty("departments")]
        public virtual ICollection<UserDepartment> UserDepartments { get; set; }

        [Display(Name = "User Request Preferences", Order = 303)]
        public virtual ICollection<UserRequestPreference2> UserRequestPreference2s { get; set; }

        [Display(Name = "User Request Preferences 1", Order = 304)]
        public virtual ICollection<UserRequestPreference2> UserRequestPreference2s1 { get; set; }

        [Display(Name = "User Request Preferences 2", Order = 305)]
        public virtual ICollection<UserRequestPreference2> UserRequestPreference2s2 { get; set; }

        [Display(Name = "User Summarys", Order = 306)]
        public virtual ICollection<UserSummary2> UserSummary2s { get; set; }

        [Display(Name = "User Summarys 1", Order = 307)]
        public virtual ICollection<UserSummary2> UserSummary2s1 { get; set; }

        [Display(Name = "User Summarys 2", Order = 308)]
        public virtual ICollection<UserSummary2> UserSummary2s2 { get; set; }

        [Display(Name = "User Tags", Order = 309), JsonConverter(typeof(ArrayJsonConverter<List<UserTag>, UserTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<UserTag> UserTags { get; set; }

        [Display(Name = "Vouchers", Order = 310)]
        public virtual ICollection<Voucher2> Voucher2s { get; set; }

        [Display(Name = "Vouchers 1", Order = 311)]
        public virtual ICollection<Voucher2> Voucher2s1 { get; set; }

        [Display(Name = "Voucher Items", Order = 312)]
        public virtual ICollection<VoucherItem2> VoucherItem2s { get; set; }

        [Display(Name = "Voucher Items 1", Order = 313)]
        public virtual ICollection<VoucherItem2> VoucherItem2s1 { get; set; }

        [Display(Name = "Waive Reasons", Order = 314)]
        public virtual ICollection<WaiveReason2> WaiveReason2s { get; set; }

        [Display(Name = "Waive Reasons 1", Order = 315)]
        public virtual ICollection<WaiveReason2> WaiveReason2s1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Username)} = {Username}, {nameof(ExternalSystemId)} = {ExternalSystemId}, {nameof(Barcode)} = {Barcode}, {nameof(Active)} = {Active}, {nameof(Type)} = {Type}, {nameof(GroupId)} = {GroupId}, {nameof(Name)} = {Name}, {nameof(LastName)} = {LastName}, {nameof(FirstName)} = {FirstName}, {nameof(MiddleName)} = {MiddleName}, {nameof(PreferredFirstName)} = {PreferredFirstName}, {nameof(EmailAddress)} = {EmailAddress}, {nameof(PhoneNumber)} = {PhoneNumber}, {nameof(MobilePhoneNumber)} = {MobilePhoneNumber}, {nameof(BirthDate)} = {BirthDate}, {nameof(PreferredContactTypeId)} = {PreferredContactTypeId}, {nameof(StartDate)} = {StartDate}, {nameof(EndDate)} = {EndDate}, {nameof(Source)} = {Source}, {nameof(Category)} = {Category}, {nameof(Status)} = {Status}, {nameof(Statuses)} = {Statuses}, {nameof(StaffStatus)} = {StaffStatus}, {nameof(StaffPrivileges)} = {StaffPrivileges}, {nameof(StaffDivision)} = {StaffDivision}, {nameof(StaffDepartment)} = {StaffDepartment}, {nameof(StudentId)} = {StudentId}, {nameof(StudentStatus)} = {StudentStatus}, {nameof(StudentRestriction)} = {StudentRestriction}, {nameof(StudentDivision)} = {StudentDivision}, {nameof(StudentDepartment)} = {StudentDepartment}, {nameof(Deceased)} = {Deceased}, {nameof(Collections)} = {Collections}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(UserAddresses)} = {(UserAddresses != null ? $"{{ {string.Join(", ", UserAddresses)} }}" : "")}, {nameof(UserDepartments)} = {(UserDepartments != null ? $"{{ {string.Join(", ", UserDepartments)} }}" : "")}, {nameof(UserTags)} = {(UserTags != null ? $"{{ {string.Join(", ", UserTags)} }}" : "")} }}";

        public static User2 FromJObject(JObject jObject) => jObject != null ? new User2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Username = (string)jObject.SelectToken("username"),
            ExternalSystemId = (string)jObject.SelectToken("externalSystemId"),
            Barcode = (string)jObject.SelectToken("barcode"),
            Active = (bool?)jObject.SelectToken("active"),
            Type = (string)jObject.SelectToken("type"),
            GroupId = (Guid?)jObject.SelectToken("patronGroup"),
            Name = $"{jObject.SelectToken("personal.firstName")}{(jObject.SelectToken("personal.middleName") != null ? $" {jObject.SelectToken("personal.middleName")}" : "")} {jObject.SelectToken("personal.lastName")}",
            LastName = (string)jObject.SelectToken("personal.lastName"),
            FirstName = (string)jObject.SelectToken("personal.firstName"),
            MiddleName = (string)jObject.SelectToken("personal.middleName"),
            PreferredFirstName = (string)jObject.SelectToken("personal.preferredFirstName"),
            EmailAddress = (string)jObject.SelectToken("personal.email"),
            PhoneNumber = (string)jObject.SelectToken("personal.phone"),
            MobilePhoneNumber = (string)jObject.SelectToken("personal.mobilePhone"),
            BirthDate = ((DateTime?)jObject.SelectToken("personal.dateOfBirth"))?.ToLocalTime(),
            PreferredContactTypeId = (string)jObject.SelectToken("personal.preferredContactTypeId"),
            StartDate = ((DateTime?)jObject.SelectToken("enrollmentDate"))?.ToLocalTime(),
            EndDate = ((DateTime?)jObject.SelectToken("expirationDate"))?.ToLocalTime(),
            Source = (string)jObject.SelectToken("customFields.source"),
            Category = (string)jObject.SelectToken("customFields.category"),
            Status = (string)jObject.SelectToken("customFields.status"),
            Statuses = (string)jObject.SelectToken("customFields.statuses"),
            StaffStatus = (string)jObject.SelectToken("customFields.staffStatus"),
            StaffPrivileges = (string)jObject.SelectToken("customFields.staffPrivileges"),
            StaffDivision = (string)jObject.SelectToken("customFields.staffDivision"),
            StaffDepartment = (string)jObject.SelectToken("customFields.staffDepartment"),
            StudentId = (string)jObject.SelectToken("customFields.studentId"),
            StudentStatus = (string)jObject.SelectToken("customFields.studentStatus"),
            StudentRestriction = (bool?)jObject.SelectToken("customFields.studentRestriction"),
            StudentDivision = (string)jObject.SelectToken("customFields.studentDivision"),
            StudentDepartment = (string)jObject.SelectToken("customFields.studentDepartment"),
            Deceased = (bool?)jObject.SelectToken("customFields.deceased"),
            Collections = (bool?)jObject.SelectToken("customFields.collections"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            UserAddresses = jObject.SelectToken("personal.addresses")?.Where(jt => jt.HasValues).Select(jt => UserAddress.FromJObject((JObject)jt)).ToArray(),
            UserDepartments = jObject.SelectToken("departments")?.Where(jt => jt.HasValues).Select(jt => UserDepartment.FromJObject((JValue)jt)).ToArray(),
            UserTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => UserTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("username", Username),
            new JProperty("externalSystemId", ExternalSystemId),
            new JProperty("barcode", Barcode),
            new JProperty("active", Active),
            new JProperty("type", Type),
            new JProperty("patronGroup", GroupId),
            new JProperty("personal", new JObject(
                new JProperty("lastName", LastName),
                new JProperty("firstName", FirstName),
                new JProperty("middleName", MiddleName),
                new JProperty("preferredFirstName", PreferredFirstName),
                new JProperty("email", EmailAddress),
                new JProperty("phone", PhoneNumber),
                new JProperty("mobilePhone", MobilePhoneNumber),
                new JProperty("dateOfBirth", BirthDate?.ToUniversalTime()),
                new JProperty("preferredContactTypeId", PreferredContactTypeId),
                new JProperty("addresses", UserAddresses?.Select(ua => ua.ToJObject())))),
            new JProperty("enrollmentDate", StartDate?.ToUniversalTime()),
            new JProperty("expirationDate", EndDate?.ToUniversalTime()),
            new JProperty("customFields", new JObject(
                new JProperty("source", Source),
                new JProperty("category", Category),
                new JProperty("status", Status),
                new JProperty("statuses", Statuses),
                new JProperty("staffStatus", StaffStatus),
                new JProperty("staffPrivileges", StaffPrivileges),
                new JProperty("staffDivision", StaffDivision),
                new JProperty("staffDepartment", StaffDepartment),
                new JProperty("studentId", StudentId),
                new JProperty("studentStatus", StudentStatus),
                new JProperty("studentRestriction", StudentRestriction),
                new JProperty("studentDivision", StudentDivision),
                new JProperty("studentDepartment", StudentDepartment),
                new JProperty("deceased", Deceased),
                new JProperty("collections", Collections))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("departments", UserDepartments?.Select(ud => ud.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", UserTags?.Select(ut => ut.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
