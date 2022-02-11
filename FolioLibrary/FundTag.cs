using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fund_tags -> uchicago_mod_finance_storage.fund
    // FundTag -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Fund Tags"), Table("fund_tags", Schema = "uc")]
    public partial class FundTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Order = 4)]
        public virtual Tag2 Tag { get; set; }

        [Column("tag_id"), Display(Name = "Tag", Order = 5), Required]
        public virtual Guid? TagId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(TagId)} = {TagId} }}";

        public static FundTag FromJObject(JValue jObject) => jObject != null ? new FundTag
        {
            TagId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(TagId);
    }
}
