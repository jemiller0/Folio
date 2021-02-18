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
    // uc.payments -> diku_mod_feesfines.feefineactions
    // Payment2 -> Payment
    [DisplayColumn(nameof(Id)), DisplayName("Payments"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("payments", Schema = "uc")]
    public partial class Payment2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Payment.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("date_action"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 2), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("dateAction")]
        public virtual DateTime? CreationTime { get; set; }

        [Column("type_action"), Display(Name = "Type Action", Order = 3), JsonProperty("typeAction"), StringLength(1024)]
        public virtual string TypeAction { get; set; }

        [Column("comments"), Display(Order = 4), JsonProperty("comments"), StringLength(1024)]
        public virtual string Comments { get; set; }

        [Column("notify"), Display(Order = 5), JsonProperty("notify")]
        public virtual bool? Notify { get; set; }

        [Column("amount_action"), DataType(DataType.Currency), Display(Order = 6), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amountAction")]
        public virtual decimal? Amount { get; set; }

        [Column("balance"), DataType(DataType.Currency), Display(Name = "Remaining Amount", Order = 7), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("balance")]
        public virtual decimal? RemainingAmount { get; set; }

        [Column("transaction_information"), Display(Name = "Transaction Information", Order = 8), JsonProperty("transactionInformation"), StringLength(1024)]
        public virtual string TransactionInformation { get; set; }

        [Column("created_at"), Display(Name = "Created At", Order = 9), JsonProperty("createdAt"), StringLength(1024)]
        public virtual string CreatedAt { get; set; }

        [Column("source"), Display(Order = 10), JsonProperty("source"), StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("payment_method"), Display(Name = "Payment Method", Order = 11), JsonProperty("paymentMethod"), StringLength(1024)]
        public virtual string PaymentMethod { get; set; }

        [Display(Order = 12)]
        public virtual Fee2 Fee { get; set; }

        [Column("fee_id"), Display(Name = "Fee", Order = 13), JsonProperty("accountId"), Required]
        public virtual Guid? FeeId { get; set; }

        [Display(Order = 14)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 15), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("content"), CustomValidation(typeof(Payment), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Scheduled Notices", Order = 17)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(CreationTime)} = {CreationTime}, {nameof(TypeAction)} = {TypeAction}, {nameof(Comments)} = {Comments}, {nameof(Notify)} = {Notify}, {nameof(Amount)} = {Amount}, {nameof(RemainingAmount)} = {RemainingAmount}, {nameof(TransactionInformation)} = {TransactionInformation}, {nameof(CreatedAt)} = {CreatedAt}, {nameof(Source)} = {Source}, {nameof(PaymentMethod)} = {PaymentMethod}, {nameof(FeeId)} = {FeeId}, {nameof(UserId)} = {UserId}, {nameof(Content)} = {Content} }}";

        public static Payment2 FromJObject(JObject jObject) => jObject != null ? new Payment2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            CreationTime = ((DateTime?)jObject.SelectToken("dateAction"))?.ToLocalTime(),
            TypeAction = (string)jObject.SelectToken("typeAction"),
            Comments = (string)jObject.SelectToken("comments"),
            Notify = (bool?)jObject.SelectToken("notify"),
            Amount = (decimal?)jObject.SelectToken("amountAction"),
            RemainingAmount = (decimal?)jObject.SelectToken("balance"),
            TransactionInformation = (string)jObject.SelectToken("transactionInformation"),
            CreatedAt = (string)jObject.SelectToken("createdAt"),
            Source = (string)jObject.SelectToken("source"),
            PaymentMethod = (string)jObject.SelectToken("paymentMethod"),
            FeeId = (Guid?)jObject.SelectToken("accountId"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("dateAction", CreationTime?.ToUniversalTime()),
            new JProperty("typeAction", TypeAction),
            new JProperty("comments", Comments),
            new JProperty("notify", Notify),
            new JProperty("amountAction", Amount),
            new JProperty("balance", RemainingAmount),
            new JProperty("transactionInformation", TransactionInformation),
            new JProperty("createdAt", CreatedAt),
            new JProperty("source", Source),
            new JProperty("paymentMethod", PaymentMethod),
            new JProperty("accountId", FeeId),
            new JProperty("userId", UserId)).RemoveNullAndEmptyProperties();
    }
}
