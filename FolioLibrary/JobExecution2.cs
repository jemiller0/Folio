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
    // uc.job_executions -> uchicago_mod_source_record_manager.job_executions
    // JobExecution2 -> JobExecution
    [DisplayColumn(nameof(Id)), DisplayName("Job Executions"), JsonConverter(typeof(JsonPathJsonConverter<JobExecution2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("job_executions", Schema = "uc")]
    public partial class JobExecution2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.JobExecution.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("hr_id"), Display(Name = "Hr Id", Order = 2), JsonProperty("hrId"), StringLength(1024)]
        public virtual string HrId { get; set; }

        [Display(Name = "Parent Job", Order = 3), InverseProperty("JobExecution2s")]
        public virtual JobExecution2 ParentJob { get; set; }

        [Column("parent_job_id"), Display(Name = "Parent Job", Order = 4), JsonProperty("parentJobId"), Required]
        public virtual Guid? ParentJobId { get; set; }

        [Column("subordination_type"), Display(Name = "Subordination Type", Order = 5), JsonProperty("subordinationType"), RegularExpression(@"^(CHILD|PARENT_SINGLE|PARENT_MULTIPLE)$"), Required, StringLength(1024)]
        public virtual string SubordinationType { get; set; }

        [Column("job_profile_info_name"), Display(Name = "Job Profile Info Name", Order = 6), JsonProperty("jobProfileInfo.name"), Required, StringLength(1024)]
        public virtual string JobProfileInfoName { get; set; }

        [Column("job_profile_info_data_type"), Display(Name = "Job Profile Info Data Type", Order = 7), JsonProperty("jobProfileInfo.dataType"), RegularExpression(@"^(Delimited|EDIFACT|MARC)$"), StringLength(1024)]
        public virtual string JobProfileInfoDataType { get; set; }

        [Column("job_profile_snapshot_wrapper_profile_id"), Display(Name = "Job Profile Snapshot Wrapper Profile Id", Order = 8), JsonProperty("jobProfileSnapshotWrapper.profileId")]
        public virtual Guid? JobProfileSnapshotWrapperProfileId { get; set; }

        [Column("job_profile_snapshot_wrapper_content_type"), Display(Name = "Job Profile Snapshot Wrapper Content Type", Order = 9), JsonProperty("jobProfileSnapshotWrapper.contentType"), RegularExpression(@"^(JOB_PROFILE|ACTION_PROFILE|MATCH_PROFILE|MAPPING_PROFILE)$"), Required, StringLength(1024)]
        public virtual string JobProfileSnapshotWrapperContentType { get; set; }

        [Column("job_profile_snapshot_wrapper_react_to"), Display(Name = "Job Profile Snapshot Wrapper React To", Order = 10), JsonProperty("jobProfileSnapshotWrapper.reactTo"), RegularExpression(@"^(MATCH|NON_MATCH)$"), StringLength(1024)]
        public virtual string JobProfileSnapshotWrapperReactTo { get; set; }

        [Column("job_profile_snapshot_wrapper_order"), Display(Name = "Job Profile Snapshot Wrapper Order", Order = 11), JsonProperty("jobProfileSnapshotWrapper.order")]
        public virtual int? JobProfileSnapshotWrapperOrder { get; set; }

        [Column("source_path"), Display(Name = "Source Path", Order = 12), JsonProperty("sourcePath"), StringLength(1024)]
        public virtual string SourcePath { get; set; }

        [Column("file_name"), Display(Name = "File Name", Order = 13), JsonProperty("fileName"), StringLength(1024)]
        public virtual string FileName { get; set; }

        [Column("run_by_first_name"), Display(Name = "Run By First Name", Order = 14), JsonProperty("runBy.firstName"), StringLength(1024)]
        public virtual string RunByFirstName { get; set; }

        [Column("run_by_last_name"), Display(Name = "Run By Last Name", Order = 15), JsonProperty("runBy.lastName"), StringLength(1024)]
        public virtual string RunByLastName { get; set; }

        [Display(Name = "Progress Job Execution", Order = 16), InverseProperty("JobExecution2s1")]
        public virtual JobExecution2 ProgressJobExecution { get; set; }

        [Column("progress_job_execution_id"), Display(Name = "Progress Job Execution", Order = 17), JsonProperty("progress.jobExecutionId")]
        public virtual Guid? ProgressJobExecutionId { get; set; }

        [Column("progress_current"), Display(Name = "Progress Current", Order = 18), JsonProperty("progress.current")]
        public virtual int? ProgressCurrent { get; set; }

        [Column("progress_total"), Display(Name = "Progress Total", Order = 19), JsonProperty("progress.total")]
        public virtual int? ProgressTotal { get; set; }

        [Column("started_date"), DataType(DataType.Date), Display(Name = "Started Date", Order = 20), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("startedDate")]
        public virtual DateTime? StartedDate { get; set; }

        [Column("completed_date"), DataType(DataType.Date), Display(Name = "Completed Date", Order = 21), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("completedDate")]
        public virtual DateTime? CompletedDate { get; set; }

        [Column("status"), Display(Order = 22), JsonProperty("status"), RegularExpression(@"^(PARENT|NEW|FILE_UPLOADED|PARSING_IN_PROGRESS|PARSING_FINISHED|PROCESSING_IN_PROGRESS|PROCESSING_FINISHED|COMMIT_IN_PROGRESS|COMMITTED|ERROR|DISCARDED)$"), Required, StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("ui_status"), Display(Name = "Ui Status", Order = 23), JsonProperty("uiStatus"), RegularExpression(@"^(PARENT|INITIALIZATION|PREPARING_FOR_PREVIEW|READY_FOR_PREVIEW|RUNNING|RUNNING_COMPLETE|ERROR|DISCARDED)$"), Required, StringLength(1024)]
        public virtual string UiStatus { get; set; }

        [Column("error_status"), Display(Name = "Error Status", Order = 24), JsonProperty("errorStatus"), RegularExpression(@"^(SNAPSHOT_UPDATE_ERROR|RECORD_UPDATE_ERROR|FILE_PROCESSING_ERROR|INSTANCE_CREATING_ERROR|PROFILE_SNAPSHOT_CREATING_ERROR)$"), StringLength(1024)]
        public virtual string ErrorStatus { get; set; }

        [Display(Order = 25)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 26), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Column("content"), CustomValidation(typeof(JobExecution), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 27), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Job Executions", Order = 28)]
        public virtual ICollection<JobExecution2> JobExecution2s { get; set; }

        [Display(Name = "Job Executions 1", Order = 29)]
        public virtual ICollection<JobExecution2> JobExecution2s1 { get; set; }

        [Display(Name = "Job Execution Progresss", Order = 30)]
        public virtual ICollection<JobExecutionProgress2> JobExecutionProgress2s { get; set; }

        [Display(Name = "Job Execution Source Chunks", Order = 31)]
        public virtual ICollection<JobExecutionSourceChunk2> JobExecutionSourceChunk2s { get; set; }

        [Display(Name = "Job Monitorings", Order = 32)]
        public virtual ICollection<JobMonitoring2> JobMonitoring2s { get; set; }

        [Display(Name = "Journal Records", Order = 33)]
        public virtual ICollection<JournalRecord2> JournalRecord2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HrId)} = {HrId}, {nameof(ParentJobId)} = {ParentJobId}, {nameof(SubordinationType)} = {SubordinationType}, {nameof(JobProfileInfoName)} = {JobProfileInfoName}, {nameof(JobProfileInfoDataType)} = {JobProfileInfoDataType}, {nameof(JobProfileSnapshotWrapperProfileId)} = {JobProfileSnapshotWrapperProfileId}, {nameof(JobProfileSnapshotWrapperContentType)} = {JobProfileSnapshotWrapperContentType}, {nameof(JobProfileSnapshotWrapperReactTo)} = {JobProfileSnapshotWrapperReactTo}, {nameof(JobProfileSnapshotWrapperOrder)} = {JobProfileSnapshotWrapperOrder}, {nameof(SourcePath)} = {SourcePath}, {nameof(FileName)} = {FileName}, {nameof(RunByFirstName)} = {RunByFirstName}, {nameof(RunByLastName)} = {RunByLastName}, {nameof(ProgressJobExecutionId)} = {ProgressJobExecutionId}, {nameof(ProgressCurrent)} = {ProgressCurrent}, {nameof(ProgressTotal)} = {ProgressTotal}, {nameof(StartedDate)} = {StartedDate}, {nameof(CompletedDate)} = {CompletedDate}, {nameof(Status)} = {Status}, {nameof(UiStatus)} = {UiStatus}, {nameof(ErrorStatus)} = {ErrorStatus}, {nameof(UserId)} = {UserId}, {nameof(Content)} = {Content} }}";

        public static JobExecution2 FromJObject(JObject jObject) => jObject != null ? new JobExecution2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            HrId = (string)jObject.SelectToken("hrId"),
            ParentJobId = (Guid?)jObject.SelectToken("parentJobId"),
            SubordinationType = (string)jObject.SelectToken("subordinationType"),
            JobProfileInfoName = (string)jObject.SelectToken("jobProfileInfo.name"),
            JobProfileInfoDataType = (string)jObject.SelectToken("jobProfileInfo.dataType"),
            JobProfileSnapshotWrapperProfileId = (Guid?)jObject.SelectToken("jobProfileSnapshotWrapper.profileId"),
            JobProfileSnapshotWrapperContentType = (string)jObject.SelectToken("jobProfileSnapshotWrapper.contentType"),
            JobProfileSnapshotWrapperReactTo = (string)jObject.SelectToken("jobProfileSnapshotWrapper.reactTo"),
            JobProfileSnapshotWrapperOrder = (int?)jObject.SelectToken("jobProfileSnapshotWrapper.order"),
            SourcePath = (string)jObject.SelectToken("sourcePath"),
            FileName = (string)jObject.SelectToken("fileName"),
            RunByFirstName = (string)jObject.SelectToken("runBy.firstName"),
            RunByLastName = (string)jObject.SelectToken("runBy.lastName"),
            ProgressJobExecutionId = (Guid?)jObject.SelectToken("progress.jobExecutionId"),
            ProgressCurrent = (int?)jObject.SelectToken("progress.current"),
            ProgressTotal = (int?)jObject.SelectToken("progress.total"),
            StartedDate = ((DateTime?)jObject.SelectToken("startedDate"))?.ToUniversalTime(),
            CompletedDate = ((DateTime?)jObject.SelectToken("completedDate"))?.ToUniversalTime(),
            Status = (string)jObject.SelectToken("status"),
            UiStatus = (string)jObject.SelectToken("uiStatus"),
            ErrorStatus = (string)jObject.SelectToken("errorStatus"),
            UserId = (Guid?)jObject.SelectToken("userId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("hrId", HrId),
            new JProperty("parentJobId", ParentJobId),
            new JProperty("subordinationType", SubordinationType),
            new JProperty("jobProfileInfo", new JObject(
                new JProperty("name", JobProfileInfoName),
                new JProperty("dataType", JobProfileInfoDataType))),
            new JProperty("jobProfileSnapshotWrapper", new JObject(
                new JProperty("profileId", JobProfileSnapshotWrapperProfileId),
                new JProperty("contentType", JobProfileSnapshotWrapperContentType),
                new JProperty("reactTo", JobProfileSnapshotWrapperReactTo),
                new JProperty("order", JobProfileSnapshotWrapperOrder))),
            new JProperty("sourcePath", SourcePath),
            new JProperty("fileName", FileName),
            new JProperty("runBy", new JObject(
                new JProperty("firstName", RunByFirstName),
                new JProperty("lastName", RunByLastName))),
            new JProperty("progress", new JObject(
                new JProperty("jobExecutionId", ProgressJobExecutionId),
                new JProperty("current", ProgressCurrent),
                new JProperty("total", ProgressTotal))),
            new JProperty("startedDate", StartedDate?.ToLocalTime()),
            new JProperty("completedDate", CompletedDate?.ToLocalTime()),
            new JProperty("status", Status),
            new JProperty("uiStatus", UiStatus),
            new JProperty("errorStatus", ErrorStatus),
            new JProperty("userId", UserId)).RemoveNullAndEmptyProperties();
    }
}
