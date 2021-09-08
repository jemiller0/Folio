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
    [Table("job_execution_progress", Schema = "diku_mod_source_record_manager")]
    public partial class JobExecutionProgress
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.JobExecutionProgress.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(JobExecutionProgress), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Name = "Job Execution", Order = 3)]
        public virtual JobExecution JobExecution { get; set; }

        [Column("jobexecutionid"), Display(Name = "Job Execution", Order = 4), ForeignKey("JobExecution")]
        public virtual Guid? Jobexecutionid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Jobexecutionid)} = {Jobexecutionid} }}";

        public static JobExecutionProgress FromJObject(JObject jObject) => jObject != null ? new JobExecutionProgress
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            Jobexecutionid = (Guid?)jObject.SelectToken("jobExecutionId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
