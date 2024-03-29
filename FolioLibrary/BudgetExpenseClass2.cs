using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.budget_expense_classes -> uchicago_mod_finance_storage.budget_expense_class
    // BudgetExpenseClass2 -> BudgetExpenseClass
    [DisplayColumn(nameof(Id)), DisplayName("Budget Expense Classes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("budget_expense_classes", Schema = "uc")]
    public partial class BudgetExpenseClass2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BudgetExpenseClass.json")))
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

        [Display(Order = 3)]
        public virtual Budget2 Budget { get; set; }

        [Column("budget_id"), Display(Name = "Budget", Order = 4), JsonProperty("budgetId")]
        public virtual Guid? BudgetId { get; set; }

        [Display(Name = "Expense Class", Order = 5)]
        public virtual ExpenseClass2 ExpenseClass { get; set; }

        [Column("expense_class_id"), Display(Name = "Expense Class", Order = 6), JsonProperty("expenseClassId")]
        public virtual Guid? ExpenseClassId { get; set; }

        [Column("status"), Display(Order = 7), JsonProperty("status"), RegularExpression(@"^(Active|Inactive)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("content"), CustomValidation(typeof(BudgetExpenseClass), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 8), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(BudgetId)} = {BudgetId}, {nameof(ExpenseClassId)} = {ExpenseClassId}, {nameof(Status)} = {Status}, {nameof(Content)} = {Content} }}";

        public static BudgetExpenseClass2 FromJObject(JObject jObject) => jObject != null ? new BudgetExpenseClass2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            BudgetId = (Guid?)jObject.SelectToken("budgetId"),
            ExpenseClassId = (Guid?)jObject.SelectToken("expenseClassId"),
            Status = (string)jObject.SelectToken("status"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("budgetId", BudgetId),
            new JProperty("expenseClassId", ExpenseClassId),
            new JProperty("status", Status)).RemoveNullAndEmptyProperties();
    }
}
