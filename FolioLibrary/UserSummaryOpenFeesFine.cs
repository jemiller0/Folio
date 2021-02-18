using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_summary_open_fees_fines -> diku_mod_patron_blocks.user_summary
    // UserSummaryOpenFeesFine -> UserSummary
    [DisplayColumn(nameof(Id)), DisplayName("User Summary Open Fees Fines"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_summary_open_fees_fines", Schema = "uc")]
    public partial class UserSummaryOpenFeesFine
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "User Summary", Order = 2)]
        public virtual UserSummary2 UserSummary { get; set; }

        [Column("user_summary_id"), Display(Name = "User Summary", Order = 3), Required]
        public virtual Guid? UserSummaryId { get; set; }

        [Display(Name = "Fee Fine", Order = 4)]
        public virtual Fee2 FeeFine { get; set; }

        [Column("fee_fine_id"), Display(Name = "Fee Fine", Order = 5), JsonProperty("feeFineId"), Required]
        public virtual Guid? FeeFineId { get; set; }

        [Display(Name = "Fee Fine Type", Order = 6)]
        public virtual FeeType2 FeeFineType { get; set; }

        [Column("fee_fine_type_id"), Display(Name = "Fee Fine Type", Order = 7), JsonProperty("feeFineTypeId"), Required]
        public virtual Guid? FeeFineTypeId { get; set; }

        [Display(Order = 8)]
        public virtual Loan2 Loan { get; set; }

        [Column("loan_id"), Display(Name = "Loan", Order = 9), JsonProperty("loanId")]
        public virtual Guid? LoanId { get; set; }

        [Column("balance"), Display(Order = 10), JsonProperty("balance"), Required]
        public virtual decimal? Balance { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserSummaryId)} = {UserSummaryId}, {nameof(FeeFineId)} = {FeeFineId}, {nameof(FeeFineTypeId)} = {FeeFineTypeId}, {nameof(LoanId)} = {LoanId}, {nameof(Balance)} = {Balance} }}";

        public static UserSummaryOpenFeesFine FromJObject(JObject jObject) => jObject != null ? new UserSummaryOpenFeesFine
        {
            FeeFineId = (Guid?)jObject.SelectToken("feeFineId"),
            FeeFineTypeId = (Guid?)jObject.SelectToken("feeFineTypeId"),
            LoanId = (Guid?)jObject.SelectToken("loanId"),
            Balance = (decimal?)jObject.SelectToken("balance")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("feeFineId", FeeFineId),
            new JProperty("feeFineTypeId", FeeFineTypeId),
            new JProperty("loanId", LoanId),
            new JProperty("balance", Balance)).RemoveNullAndEmptyProperties();
    }
}
