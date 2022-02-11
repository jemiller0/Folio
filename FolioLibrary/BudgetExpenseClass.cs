using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("budget_expense_class", Schema = "uchicago_mod_finance_storage")]
    public partial class BudgetExpenseClass
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(BudgetExpenseClass), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Order = 3)]
        public virtual Budget Budget { get; set; }

        [Column("budgetid"), Display(Name = "Budget", Order = 4), ForeignKey("Budget")]
        public virtual Guid? Budgetid { get; set; }

        [Display(Name = "Expense Class", Order = 5)]
        public virtual ExpenseClass ExpenseClass { get; set; }

        [Column("expenseclassid"), Display(Name = "Expense Class", Order = 6), ForeignKey("ExpenseClass")]
        public virtual Guid? Expenseclassid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Budgetid)} = {Budgetid}, {nameof(Expenseclassid)} = {Expenseclassid} }}";

        public static BudgetExpenseClass FromJObject(JObject jObject) => jObject != null ? new BudgetExpenseClass
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            Budgetid = (Guid?)jObject.SelectToken("budgetId"),
            Expenseclassid = (Guid?)jObject.SelectToken("expenseClassId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
