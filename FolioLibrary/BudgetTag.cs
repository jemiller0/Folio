using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.budget_tags -> diku_mod_finance_storage.budget
    // BudgetTag -> Budget
    [DisplayColumn(nameof(Id)), DisplayName("Budget Tags"), Table("budget_tags", Schema = "uc")]
    public partial class BudgetTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Budget2 Budget { get; set; }

        [Column("budget_id"), Display(Name = "Budget", Order = 3), Required]
        public virtual Guid? BudgetId { get; set; }

        [Display(Order = 4)]
        public virtual Tag2 Tag { get; set; }

        [Column("tag_id"), Display(Name = "Tag", Order = 5), Required]
        public virtual Guid? TagId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BudgetId)} = {BudgetId}, {nameof(TagId)} = {TagId} }}";

        public static BudgetTag FromJObject(JValue jObject) => jObject != null ? new BudgetTag
        {
            TagId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(TagId);
    }
}
