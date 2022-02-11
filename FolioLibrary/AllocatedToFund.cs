using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.allocated_to_funds -> uchicago_mod_finance_storage.fund
    // AllocatedToFund -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Allocated To Funds"), Table("allocated_to_funds", Schema = "uc")]
    public partial class AllocatedToFund
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2), InverseProperty("AllocatedToFunds1")]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "To Fund", Order = 4), InverseProperty("AllocatedToFunds")]
        public virtual Fund2 ToFund { get; set; }

        [Column("to_fund_id"), Display(Name = "To Fund", Order = 5), Required]
        public virtual Guid? ToFundId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(ToFundId)} = {ToFundId} }}";

        public static AllocatedToFund FromJObject(JValue jObject) => jObject != null ? new AllocatedToFund
        {
            ToFundId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(ToFundId);
    }
}
