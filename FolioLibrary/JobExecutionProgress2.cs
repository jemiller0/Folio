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
    // uc.job_execution_progresses -> diku_mod_source_record_manager.job_execution_progress
    // JobExecutionProgress2 -> JobExecutionProgress
    [DisplayColumn(nameof(Id)), DisplayName("Job Execution Progresses"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("job_execution_progresses", Schema = "uc")]
    public partial class JobExecutionProgress2
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

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Job Execution", Order = 2)]
        public virtual JobExecution2 JobExecution { get; set; }

        [Column("job_execution_id"), Display(Name = "Job Execution", Order = 3), JsonProperty("jobExecutionId")]
        public virtual Guid? JobExecutionId { get; set; }

        [Column("currently_succeeded"), Display(Name = "Currently Succeeded", Order = 4), JsonProperty("currentlySucceeded")]
        public virtual int? CurrentlySucceeded { get; set; }

        [Column("currently_failed"), Display(Name = "Currently Failed", Order = 5), JsonProperty("currentlyFailed")]
        public virtual int? CurrentlyFailed { get; set; }

        [Column("total"), Display(Order = 6), JsonProperty("total")]
        public virtual int? Total { get; set; }

        [Column("content"), CustomValidation(typeof(JobExecutionProgress), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 7), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(JobExecutionId)} = {JobExecutionId}, {nameof(CurrentlySucceeded)} = {CurrentlySucceeded}, {nameof(CurrentlyFailed)} = {CurrentlyFailed}, {nameof(Total)} = {Total}, {nameof(Content)} = {Content} }}";

        public static JobExecutionProgress2 FromJObject(JObject jObject) => jObject != null ? new JobExecutionProgress2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            JobExecutionId = (Guid?)jObject.SelectToken("jobExecutionId"),
            CurrentlySucceeded = (int?)jObject.SelectToken("currentlySucceeded"),
            CurrentlyFailed = (int?)jObject.SelectToken("currentlyFailed"),
            Total = (int?)jObject.SelectToken("total"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("jobExecutionId", JobExecutionId),
            new JProperty("currentlySucceeded", CurrentlySucceeded),
            new JProperty("currentlyFailed", CurrentlyFailed),
            new JProperty("total", Total)).RemoveNullAndEmptyProperties();
    }
}
