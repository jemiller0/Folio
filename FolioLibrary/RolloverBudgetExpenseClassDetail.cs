using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.rollover_budget_expense_class_details -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudgetExpenseClassDetail -> RolloverBudget
    [DisplayColumn(nameof(Id)), DisplayName("Rollover Budget Expense Class Details"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollover_budget_expense_class_details", Schema = "uc")]
    public partial class RolloverBudgetExpenseClassDetail
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Rollover Budget", Order = 2)]
        public virtual RolloverBudget2 RolloverBudget { get; set; }

        [Column("rollover_budget_id"), Display(Name = "Rollover Budget", Order = 3)]
        public virtual Guid? RolloverBudgetId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id")]
        public virtual Guid? Id2 { get; set; }

        [Column("expense_class_name"), Display(Name = "Expense Class Name", Order = 5), JsonProperty("expenseClassName"), StringLength(1024)]
        public virtual string ExpenseClassName { get; set; }

        [Column("expense_class_code"), Display(Name = "Expense Class Code", Order = 6), JsonProperty("expenseClassCode"), StringLength(1024)]
        public virtual string ExpenseClassCode { get; set; }

        [Column("expense_class_status"), Display(Name = "Expense Class Status", Order = 7), JsonProperty("expenseClassStatus"), StringLength(1024)]
        public virtual string ExpenseClassStatus { get; set; }

        [Column("encumbered"), Display(Order = 8), JsonProperty("encumbered")]
        public virtual decimal? Encumbered { get; set; }

        [Column("awaiting_payment"), Display(Name = "Awaiting Payment", Order = 9), JsonProperty("awaitingPayment")]
        public virtual decimal? AwaitingPayment { get; set; }

        [Column("credited"), Display(Order = 10), JsonProperty("credited")]
        public virtual decimal? Credited { get; set; }

        [Column("percentage_credited"), Display(Name = "Percentage Credited", Order = 11), JsonProperty("percentageCredited")]
        public virtual decimal? PercentageCredited { get; set; }

        [Column("expended"), Display(Order = 12), JsonProperty("expended")]
        public virtual decimal? Expended { get; set; }

        [Column("percentage_expended"), Display(Name = "Percentage Expended", Order = 13), JsonProperty("percentageExpended")]
        public virtual decimal? PercentageExpended { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RolloverBudgetId)} = {RolloverBudgetId}, {nameof(Id2)} = {Id2}, {nameof(ExpenseClassName)} = {ExpenseClassName}, {nameof(ExpenseClassCode)} = {ExpenseClassCode}, {nameof(ExpenseClassStatus)} = {ExpenseClassStatus}, {nameof(Encumbered)} = {Encumbered}, {nameof(AwaitingPayment)} = {AwaitingPayment}, {nameof(Credited)} = {Credited}, {nameof(PercentageCredited)} = {PercentageCredited}, {nameof(Expended)} = {Expended}, {nameof(PercentageExpended)} = {PercentageExpended} }}";

        public static RolloverBudgetExpenseClassDetail FromJObject(JObject jObject) => jObject != null ? new RolloverBudgetExpenseClassDetail
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            ExpenseClassName = (string)jObject.SelectToken("expenseClassName"),
            ExpenseClassCode = (string)jObject.SelectToken("expenseClassCode"),
            ExpenseClassStatus = (string)jObject.SelectToken("expenseClassStatus"),
            Encumbered = (decimal?)jObject.SelectToken("encumbered"),
            AwaitingPayment = (decimal?)jObject.SelectToken("awaitingPayment"),
            Credited = (decimal?)jObject.SelectToken("credited"),
            PercentageCredited = (decimal?)jObject.SelectToken("percentageCredited"),
            Expended = (decimal?)jObject.SelectToken("expended"),
            PercentageExpended = (decimal?)jObject.SelectToken("percentageExpended")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("expenseClassName", ExpenseClassName),
            new JProperty("expenseClassCode", ExpenseClassCode),
            new JProperty("expenseClassStatus", ExpenseClassStatus),
            new JProperty("encumbered", Encumbered),
            new JProperty("awaitingPayment", AwaitingPayment),
            new JProperty("credited", Credited),
            new JProperty("percentageCredited", PercentageCredited),
            new JProperty("expended", Expended),
            new JProperty("percentageExpended", PercentageExpended)).RemoveNullAndEmptyProperties();
    }
}
