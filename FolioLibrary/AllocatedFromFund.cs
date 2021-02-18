using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.allocated_from_funds -> diku_mod_finance_storage.fund
    // AllocatedFromFund -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Allocated From Funds"), Table("allocated_from_funds", Schema = "uc")]
    public partial class AllocatedFromFund
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2), InverseProperty("AllocatedFromFunds1")]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "From Fund", Order = 4), InverseProperty("AllocatedFromFunds")]
        public virtual Fund2 FromFund { get; set; }

        [Column("from_fund_id"), Display(Name = "From Fund", Order = 5), Required]
        public virtual Guid? FromFundId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(FromFundId)} = {FromFundId} }}";

        public static AllocatedFromFund FromJObject(JValue jObject) => jObject != null ? new AllocatedFromFund
        {
            FromFundId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(FromFundId);
    }
}
