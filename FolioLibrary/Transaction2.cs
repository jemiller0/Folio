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
    // uc.transactions -> uchicago_mod_finance_storage.transaction
    // Transaction2 -> Transaction
    [DisplayColumn(nameof(Amount)), DisplayName("Transactions"), JsonConverter(typeof(JsonPathJsonConverter<Transaction2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("transactions", Schema = "uc")]
    public partial class Transaction2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Transaction.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), JsonProperty("_version"), ScaffoldColumn(false)]
        public virtual int? Version { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 3), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount"), Required]
        public virtual decimal? Amount { get; set; }

        [Display(Name = "Awaiting Payment Encumbrance", Order = 4), InverseProperty("Transaction2s")]
        public virtual Transaction2 AwaitingPaymentEncumbrance { get; set; }

        [Column("awaiting_payment_encumbrance_id"), Display(Name = "Awaiting Payment Encumbrance", Order = 5), JsonProperty("awaitingPayment.encumbranceId")]
        public virtual Guid? AwaitingPaymentEncumbranceId { get; set; }

        [Column("awaiting_payment_release_encumbrance"), Display(Name = "Awaiting Payment Release Encumbrance", Order = 6), JsonProperty("awaitingPayment.releaseEncumbrance")]
        public virtual bool? AwaitingPaymentReleaseEncumbrance { get; set; }

        [Column("currency"), Display(Order = 7), JsonProperty("currency"), Required, StringLength(1024)]
        public virtual string Currency { get; set; }

        [Column("description"), Display(Order = 8), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("encumbrance_amount_awaiting_payment"), DataType(DataType.Currency), Display(Name = "Awaiting Payment Amount", Order = 9), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("encumbrance.amountAwaitingPayment")]
        public virtual decimal? AwaitingPaymentAmount { get; set; }

        [Column("encumbrance_amount_credited"), DataType(DataType.Currency), Display(Name = "Encumbrance Amount Credited", Order = 10), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("encumbrance.amountCredited")]
        public virtual decimal? EncumbranceAmountCredited { get; set; }

        [Column("encumbrance_amount_expended"), DataType(DataType.Currency), Display(Name = "Expended Amount", Order = 11), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("encumbrance.amountExpended")]
        public virtual decimal? ExpendedAmount { get; set; }

        [Column("encumbrance_initial_amount_encumbered"), DataType(DataType.Currency), Display(Name = "Initial Encumbered Amount", Order = 12), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("encumbrance.initialAmountEncumbered"), Required]
        public virtual decimal? InitialEncumberedAmount { get; set; }

        [Column("encumbrance_status"), Display(Order = 13), JsonProperty("encumbrance.status"), RegularExpression(@"^(Released|Unreleased|Pending)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("encumbrance_order_type"), Display(Name = "Order Type", Order = 14), JsonProperty("encumbrance.orderType"), RegularExpression(@"^(One-Time|Ongoing)$"), Required, StringLength(1024)]
        public virtual string OrderType { get; set; }

        [Column("encumbrance_order_status"), Display(Name = "Order Status", Order = 15), JsonProperty("encumbrance.orderStatus"), RegularExpression(@"^(Pending|Open|Closed)$"), StringLength(1024)]
        public virtual string OrderStatus { get; set; }

        [Column("encumbrance_subscription"), Display(Order = 16), JsonProperty("encumbrance.subscription")]
        public virtual bool? Subscription { get; set; }

        [Column("encumbrance_re_encumber"), Display(Name = "Re Encumber", Order = 17), JsonProperty("encumbrance.reEncumber")]
        public virtual bool? ReEncumber { get; set; }

        [Display(Order = 18)]
        public virtual Order2 Order { get; set; }

        [Column("encumbrance_source_purchase_order_id"), Display(Name = "Order", Order = 19), JsonProperty("encumbrance.sourcePurchaseOrderId"), Required]
        public virtual Guid? OrderId { get; set; }

        [Display(Name = "Order Item", Order = 20)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("encumbrance_source_po_line_id"), Display(Name = "Order Item", Order = 21), JsonProperty("encumbrance.sourcePoLineId"), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Name = "Expense Class", Order = 22)]
        public virtual ExpenseClass2 ExpenseClass { get; set; }

        [Column("expense_class_id"), Display(Name = "Expense Class", Order = 23), JsonProperty("expenseClassId")]
        public virtual Guid? ExpenseClassId { get; set; }

        [Display(Name = "Fiscal Year", Order = 24), InverseProperty("Transaction2s1")]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 25), JsonProperty("fiscalYearId"), Required]
        public virtual Guid? FiscalYearId { get; set; }

        [Display(Name = "From Fund", Order = 26), InverseProperty("Transaction2s")]
        public virtual Fund2 FromFund { get; set; }

        [Column("from_fund_id"), Display(Name = "From Fund", Order = 27), JsonProperty("fromFundId")]
        public virtual Guid? FromFundId { get; set; }

        [Column("invoice_cancelled"), Display(Name = "Invoice Cancelled", Order = 28), JsonProperty("invoiceCancelled")]
        public virtual bool? InvoiceCancelled { get; set; }

        [Display(Name = "Payment Encumbrance", Order = 29), InverseProperty("Transaction2s1")]
        public virtual Transaction2 PaymentEncumbrance { get; set; }

        [Column("payment_encumbrance_id"), Display(Name = "Payment Encumbrance", Order = 30), JsonProperty("paymentEncumbranceId")]
        public virtual Guid? PaymentEncumbranceId { get; set; }

        [Column("source"), Display(Order = 31), JsonProperty("source"), RegularExpression(@"^(User|PoLine|Invoice)$"), Required, StringLength(1024)]
        public virtual string Source { get; set; }

        [Display(Name = "Source Fiscal Year", Order = 32), InverseProperty("Transaction2s")]
        public virtual FiscalYear2 SourceFiscalYear { get; set; }

        [Column("source_fiscal_year_id"), Display(Name = "Source Fiscal Year", Order = 33), JsonProperty("sourceFiscalYearId")]
        public virtual Guid? SourceFiscalYearId { get; set; }

        [Display(Order = 34)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("source_invoice_id"), Display(Name = "Invoice", Order = 35), JsonProperty("sourceInvoiceId")]
        public virtual Guid? InvoiceId { get; set; }

        [Display(Name = "Invoice Item", Order = 36)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("source_invoice_line_id"), Display(Name = "Invoice Item", Order = 37), JsonProperty("sourceInvoiceLineId")]
        public virtual Guid? InvoiceItemId { get; set; }

        [Display(Name = "To Fund", Order = 38), InverseProperty("Transaction2s1")]
        public virtual Fund2 ToFund { get; set; }

        [Column("to_fund_id"), Display(Name = "To Fund", Order = 39), JsonProperty("toFundId")]
        public virtual Guid? ToFundId { get; set; }

        [Column("transaction_type"), Display(Name = "Transaction Type", Order = 40), JsonProperty("transactionType"), RegularExpression(@"^(Allocation|Credit|Encumbrance|Payment|Pending payment|Rollover transfer|Transfer)$"), Required, StringLength(1024)]
        public virtual string TransactionType { get; set; }

        [Column("voided_amount"), DataType(DataType.Currency), Display(Name = "Voided Amount", Order = 41), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("voidedAmount")]
        public virtual decimal? VoidedAmount { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 42), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 43), InverseProperty("Transaction2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 44), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 46), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 47), InverseProperty("Transaction2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 48), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Transaction), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 50), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Invoices", Order = 51)]
        public virtual ICollection<Invoice2> Invoice2s { get; set; }

        [Display(Name = "Invoice Adjustment Funds", Order = 52)]
        public virtual ICollection<InvoiceAdjustmentFund> InvoiceAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Adjustment Funds", Order = 53)]
        public virtual ICollection<InvoiceItemAdjustmentFund> InvoiceItemAdjustmentFunds { get; set; }

        [Display(Name = "Invoice Item Funds", Order = 54)]
        public virtual ICollection<InvoiceItemFund> InvoiceItemFunds { get; set; }

        [Display(Name = "Order Item Funds", Order = 55)]
        public virtual ICollection<OrderItemFund> OrderItemFunds { get; set; }

        [Display(Name = "Transactions", Order = 56)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        [Display(Name = "Transactions 1", Order = 57)]
        public virtual ICollection<Transaction2> Transaction2s1 { get; set; }

        [Display(Name = "Transaction Tags", Order = 58), JsonConverter(typeof(ArrayJsonConverter<List<TransactionTag>, TransactionTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<TransactionTag> TransactionTags { get; set; }

        [Display(Name = "Voucher Items", Order = 59)]
        public virtual ICollection<VoucherItem2> VoucherItem2s { get; set; }

        [Display(Name = "Voucher Item Funds", Order = 60)]
        public virtual ICollection<VoucherItemFund> VoucherItemFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(Amount)} = {Amount}, {nameof(AwaitingPaymentEncumbranceId)} = {AwaitingPaymentEncumbranceId}, {nameof(AwaitingPaymentReleaseEncumbrance)} = {AwaitingPaymentReleaseEncumbrance}, {nameof(Currency)} = {Currency}, {nameof(Description)} = {Description}, {nameof(AwaitingPaymentAmount)} = {AwaitingPaymentAmount}, {nameof(EncumbranceAmountCredited)} = {EncumbranceAmountCredited}, {nameof(ExpendedAmount)} = {ExpendedAmount}, {nameof(InitialEncumberedAmount)} = {InitialEncumberedAmount}, {nameof(Status)} = {Status}, {nameof(OrderType)} = {OrderType}, {nameof(OrderStatus)} = {OrderStatus}, {nameof(Subscription)} = {Subscription}, {nameof(ReEncumber)} = {ReEncumber}, {nameof(OrderId)} = {OrderId}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(ExpenseClassId)} = {ExpenseClassId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(FromFundId)} = {FromFundId}, {nameof(InvoiceCancelled)} = {InvoiceCancelled}, {nameof(PaymentEncumbranceId)} = {PaymentEncumbranceId}, {nameof(Source)} = {Source}, {nameof(SourceFiscalYearId)} = {SourceFiscalYearId}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(ToFundId)} = {ToFundId}, {nameof(TransactionType)} = {TransactionType}, {nameof(VoidedAmount)} = {VoidedAmount}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(TransactionTags)} = {(TransactionTags != null ? $"{{ {string.Join(", ", TransactionTags)} }}" : "")} }}";

        public static Transaction2 FromJObject(JObject jObject) => jObject != null ? new Transaction2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            AwaitingPaymentEncumbranceId = (Guid?)jObject.SelectToken("awaitingPayment.encumbranceId"),
            AwaitingPaymentReleaseEncumbrance = (bool?)jObject.SelectToken("awaitingPayment.releaseEncumbrance"),
            Currency = (string)jObject.SelectToken("currency"),
            Description = (string)jObject.SelectToken("description"),
            AwaitingPaymentAmount = (decimal?)jObject.SelectToken("encumbrance.amountAwaitingPayment"),
            EncumbranceAmountCredited = (decimal?)jObject.SelectToken("encumbrance.amountCredited"),
            ExpendedAmount = (decimal?)jObject.SelectToken("encumbrance.amountExpended"),
            InitialEncumberedAmount = (decimal?)jObject.SelectToken("encumbrance.initialAmountEncumbered"),
            Status = (string)jObject.SelectToken("encumbrance.status"),
            OrderType = (string)jObject.SelectToken("encumbrance.orderType"),
            OrderStatus = (string)jObject.SelectToken("encumbrance.orderStatus"),
            Subscription = (bool?)jObject.SelectToken("encumbrance.subscription"),
            ReEncumber = (bool?)jObject.SelectToken("encumbrance.reEncumber"),
            OrderId = (Guid?)jObject.SelectToken("encumbrance.sourcePurchaseOrderId"),
            OrderItemId = (Guid?)jObject.SelectToken("encumbrance.sourcePoLineId"),
            ExpenseClassId = (Guid?)jObject.SelectToken("expenseClassId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            FromFundId = (Guid?)jObject.SelectToken("fromFundId"),
            InvoiceCancelled = (bool?)jObject.SelectToken("invoiceCancelled"),
            PaymentEncumbranceId = (Guid?)jObject.SelectToken("paymentEncumbranceId"),
            Source = (string)jObject.SelectToken("source"),
            SourceFiscalYearId = (Guid?)jObject.SelectToken("sourceFiscalYearId"),
            InvoiceId = (Guid?)jObject.SelectToken("sourceInvoiceId"),
            InvoiceItemId = (Guid?)jObject.SelectToken("sourceInvoiceLineId"),
            ToFundId = (Guid?)jObject.SelectToken("toFundId"),
            TransactionType = (string)jObject.SelectToken("transactionType"),
            VoidedAmount = (decimal?)jObject.SelectToken("voidedAmount"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            TransactionTags = jObject.SelectToken("tags.tagList")?.Select(jt => TransactionTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("amount", Amount),
            new JProperty("awaitingPayment", new JObject(
                new JProperty("encumbranceId", AwaitingPaymentEncumbranceId),
                new JProperty("releaseEncumbrance", AwaitingPaymentReleaseEncumbrance))),
            new JProperty("currency", Currency),
            new JProperty("description", Description),
            new JProperty("encumbrance", new JObject(
                new JProperty("amountAwaitingPayment", AwaitingPaymentAmount),
                new JProperty("amountCredited", EncumbranceAmountCredited),
                new JProperty("amountExpended", ExpendedAmount),
                new JProperty("initialAmountEncumbered", InitialEncumberedAmount),
                new JProperty("status", Status),
                new JProperty("orderType", OrderType),
                new JProperty("orderStatus", OrderStatus),
                new JProperty("subscription", Subscription),
                new JProperty("reEncumber", ReEncumber),
                new JProperty("sourcePurchaseOrderId", OrderId),
                new JProperty("sourcePoLineId", OrderItemId))),
            new JProperty("expenseClassId", ExpenseClassId),
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("fromFundId", FromFundId),
            new JProperty("invoiceCancelled", InvoiceCancelled),
            new JProperty("paymentEncumbranceId", PaymentEncumbranceId),
            new JProperty("source", Source),
            new JProperty("sourceFiscalYearId", SourceFiscalYearId),
            new JProperty("sourceInvoiceId", InvoiceId),
            new JProperty("sourceInvoiceLineId", InvoiceItemId),
            new JProperty("toFundId", ToFundId),
            new JProperty("transactionType", TransactionType),
            new JProperty("voidedAmount", VoidedAmount),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("tags", new JObject(
                new JProperty("tagList", TransactionTags?.Select(tt => tt.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
