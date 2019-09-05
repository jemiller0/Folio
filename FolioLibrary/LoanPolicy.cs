using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("loan_policy", Schema = "diku_mod_circulation_storage")]
    public partial class LoanPolicy
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LoanPolicy.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(LoanPolicy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Fixed Due Date Schedule", Order = 5), InverseProperty("LoanPolicies")]
        public virtual FixedDueDateSchedule FixedDueDateSchedule { get; set; }

        [Column("loanspolicy_fixedduedatescheduleid"), Display(Name = "Fixed Due Date Schedule", Order = 6), Editable(false), ForeignKey("FixedDueDateSchedule")]
        public virtual Guid? LoanspolicyFixedduedatescheduleid { get; set; }

        [Display(Name = "Fixed Due Date Schedule 1", Order = 7), InverseProperty("LoanPolicies1")]
        public virtual FixedDueDateSchedule FixedDueDateSchedule1 { get; set; }

        [Column("renewalspolicy_alternatefixedduedatescheduleid"), Display(Name = "Fixed Due Date Schedule 1", Order = 8), Editable(false), ForeignKey("FixedDueDateSchedule1")]
        public virtual Guid? RenewalspolicyAlternatefixedduedatescheduleid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LoanspolicyFixedduedatescheduleid)} = {LoanspolicyFixedduedatescheduleid}, {nameof(RenewalspolicyAlternatefixedduedatescheduleid)} = {RenewalspolicyAlternatefixedduedatescheduleid} }}";
    }
}
