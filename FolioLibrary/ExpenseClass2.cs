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
    // uc.expense_classes -> diku_mod_finance_storage.expense_class
    // ExpenseClass2 -> ExpenseClass
    [DisplayColumn(nameof(Name)), DisplayName("Expense Classes"), JsonConverter(typeof(JsonPathJsonConverter<ExpenseClass2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("expense_classes", Schema = "uc")]
    public partial class ExpenseClass2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ExpenseClass.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("code"), Display(Order = 2), JsonProperty("code"), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("external_account_number_ext"), Display(Name = "Account Number Extension", Order = 3), JsonProperty("externalAccountNumberExt"), StringLength(1024)]
        public virtual string AccountNumberExtension { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 6), InverseProperty("ExpenseClass2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 10), InverseProperty("ExpenseClass2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 11), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ExpenseClass), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 13), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Budget Expense Classs", Order = 14)]
        public virtual ICollection<BudgetExpenseClass2> BudgetExpenseClass2s { get; set; }

        [Display(Name = "Invoice Adjustment Funds", Order = 15)]
        public virtual ICollection<InvoiceAdjustmentFund> InvoiceAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Adjustment Funds", Order = 16)]
        public virtual ICollection<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Funds", Order = 17)]
        public virtual ICollection<InvoiceItemFund> InvoiceItemFunds { get; set; }

        [Display(Name = "Order Item Funds", Order = 18)]
        public virtual ICollection<OrderItemFund> OrderItemFunds { get; set; }

        [Display(Name = "Transactions", Order = 19)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Voucher Item Funds", Order = 20)]
        public virtual ICollection<VoucherItemFund> VoucherItemFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(AccountNumberExtension)} = {AccountNumberExtension}, {nameof(Name)} = {Name}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static ExpenseClass2 FromJObject(JObject jObject) => jObject != null ? new ExpenseClass2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Code = (string)jObject.SelectToken("code"),
            AccountNumberExtension = (string)jObject.SelectToken("externalAccountNumberExt"),
            Name = (string)jObject.SelectToken("name"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("code", Code),
            new JProperty("externalAccountNumberExt", AccountNumberExtension),
            new JProperty("name", Name),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
