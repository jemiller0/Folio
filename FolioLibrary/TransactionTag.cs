using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.transaction_tags -> uchicago_mod_finance_storage.transaction
    // TransactionTag -> Transaction
    [DisplayColumn(nameof(Content)), DisplayName("Transaction Tags"), Table("transaction_tags", Schema = "uc")]
    public partial class TransactionTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Transaction2 Transaction { get; set; }

        [Column("transaction_id"), Display(Name = "Transaction", Order = 3), Required]
        public virtual Guid? TransactionId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TransactionId)} = {TransactionId}, {nameof(Content)} = {Content} }}";

        public static TransactionTag FromJObject(JValue jObject) => jObject != null ? new TransactionTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
