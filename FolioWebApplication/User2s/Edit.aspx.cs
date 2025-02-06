using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.User2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void User2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var u2 = id == null && (string)Session["User2sPermission"] == "Edit" ? new User2 { Active = true, PreferredContactTypeId = "002", StartDate = DateTime.Now.Date, Source = "Library" } : folioServiceContext.FindUser2(id, true);
            if (u2 == null) Response.Redirect("Default.aspx");
            u2.Content = u2.Content != null ? JsonConvert.DeserializeObject<JToken>(u2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            User2FormView.DataSource = new[] { u2 };
            Title = $"User {u2.Username}";
        }

        protected void GroupRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = folioServiceContext.Group2s(orderBy: "group").ToArray();
        }

        protected void PreferredContactTypeRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new[] { new ContactType { Id = "002", Name = "Email" }, new ContactType { Id = "001", Name = "Mail" }, new ContactType { Id = "003", Name = "Text message" } };
        }

        protected void SourceRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new string[] { "Library", "University" };
        }

        protected void User2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)User2FormView.DataKey.Value;
            var u2 = id != null ? folioServiceContext.FindUser2(id) : new User2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            u2.Username = Global.Trim((string)e.NewValues["Username"]);
            u2.ExternalSystemId = Global.Trim((string)e.NewValues["ExternalSystemId"]);
            u2.Barcode = Global.Trim((string)e.NewValues["Barcode"]);
            u2.Active = (bool?)e.NewValues["Active"];
            u2.GroupId = (Guid?)Guid.Parse((string)e.NewValues["GroupId"]);
            u2.LastName = Global.Trim((string)e.NewValues["LastName"]);
            u2.FirstName = Global.Trim((string)e.NewValues["FirstName"]);
            u2.MiddleName = Global.Trim((string)e.NewValues["MiddleName"]);
            u2.PreferredFirstName = Global.Trim((string)e.NewValues["PreferredFirstName"]);
            u2.EmailAddress = Global.Trim((string)e.NewValues["EmailAddress"]);
            u2.PhoneNumber = Global.Trim((string)e.NewValues["PhoneNumber"]);
            u2.MobilePhoneNumber = Global.Trim((string)e.NewValues["MobilePhoneNumber"]);
            u2.BirthDate = (DateTime?)e.NewValues["BirthDate"];
            u2.PreferredContactTypeId = (string)e.NewValues["PreferredContactTypeId"];
            u2.ProfilePictureLink = Global.Trim((string)e.NewValues["ProfilePictureLink"]);
            u2.StartDate = (DateTime?)e.NewValues["StartDate"];
            u2.EndDate = (DateTime?)e.NewValues["EndDate"];
            u2.Source = Global.Trim((string)e.NewValues["Source"]);
            u2.CategoryCode = Global.Trim((string)e.NewValues["CategoryCode"]);
            u2.Deceased = (bool?)e.NewValues["Deceased"];
            u2.Collections = (bool?)e.NewValues["Collections"];
            u2.LastWriteTime = DateTime.Now;
            u2.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = User2.ValidateUser2(u2, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)User2FormView.FindControl("User2CustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(u2); else folioServiceContext.Update(u2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={u2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void User2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void User2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)User2FormView.DataKey.Value;
            try
            {
                if (folioServiceContext.AnyAcquisitionMethod2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisition method");
                if (folioServiceContext.AnyAcquisitionMethod2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisition method");
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
                if (folioServiceContext.AnyActualCostRecord2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a actual cost record");
                if (folioServiceContext.AnyActualCostRecord2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a actual cost record");
                if (folioServiceContext.AnyAddressType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a address type");
                if (folioServiceContext.AnyAddressType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a address type");
                if (folioServiceContext.AnyAlternativeTitleType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a alternative title type");
                if (folioServiceContext.AnyAlternativeTitleType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a alternative title type");
                if (folioServiceContext.AnyBatchGroup2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch group");
                if (folioServiceContext.AnyBatchGroup2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch group");
                if (folioServiceContext.AnyBlock2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block");
                if (folioServiceContext.AnyBlock2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block");
                if (folioServiceContext.AnyBlock2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block");
                if (folioServiceContext.AnyBlockCondition2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block condition");
                if (folioServiceContext.AnyBlockCondition2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block condition");
                if (folioServiceContext.AnyBlockLimit2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block limit");
                if (folioServiceContext.AnyBlockLimit2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a block limit");
                if (folioServiceContext.AnyBoundWithPart2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a bound with part");
                if (folioServiceContext.AnyBoundWithPart2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a bound with part");
                if (folioServiceContext.AnyBudget2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a budget");
                if (folioServiceContext.AnyBudget2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a budget");
                if (folioServiceContext.AnyCallNumberType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a call number type");
                if (folioServiceContext.AnyCallNumberType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a call number type");
                if (folioServiceContext.AnyCampus2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a campus");
                if (folioServiceContext.AnyCampus2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a campus");
                if (folioServiceContext.AnyCancellationReason2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a cancellation reason");
                if (folioServiceContext.AnyCancellationReason2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a cancellation reason");
                if (folioServiceContext.AnyCategory2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a category");
                if (folioServiceContext.AnyCategory2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a category");
                if (folioServiceContext.AnyCheckIn2s($"performedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a check in");
                if (folioServiceContext.AnyClassificationType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a classification type");
                if (folioServiceContext.AnyClassificationType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a classification type");
                if (folioServiceContext.AnyComment2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a comment");
                if (folioServiceContext.AnyComment2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a comment");
                if (folioServiceContext.AnyConfiguration2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a configuration");
                if (folioServiceContext.AnyConfiguration2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a configuration");
                if (folioServiceContext.AnyContact2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contact");
                if (folioServiceContext.AnyContact2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contact");
                if (folioServiceContext.AnyContributorNameType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contributor name type");
                if (folioServiceContext.AnyContributorNameType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contributor name type");
                if (folioServiceContext.AnyContributorType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contributor type");
                if (folioServiceContext.AnyContributorType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a contributor type");
                if (folioServiceContext.AnyCustomField2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a custom field");
                if (folioServiceContext.AnyCustomField2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a custom field");
                if (folioServiceContext.AnyDepartment2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a department");
                if (folioServiceContext.AnyDepartment2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a department");
                if (folioServiceContext.AnyElectronicAccessRelationship2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a electronic access relationship");
                if (folioServiceContext.AnyElectronicAccessRelationship2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a electronic access relationship");
                if (folioServiceContext.AnyExpenseClass2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a expense class");
                if (folioServiceContext.AnyExpenseClass2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a expense class");
                if (folioServiceContext.AnyFee2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyFee2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyFee2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fee");
                if (folioServiceContext.AnyFeeType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fee type");
                if (folioServiceContext.AnyFeeType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fee type");
                if (folioServiceContext.AnyFinanceGroup2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a finance group");
                if (folioServiceContext.AnyFinanceGroup2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a finance group");
                if (folioServiceContext.AnyFiscalYear2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fiscal year");
                if (folioServiceContext.AnyFiscalYear2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fiscal year");
                if (folioServiceContext.AnyFixedDueDateSchedule2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fixed due date schedule");
                if (folioServiceContext.AnyFixedDueDateSchedule2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fixed due date schedule");
                if (folioServiceContext.AnyFormats($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a format");
                if (folioServiceContext.AnyFormats($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a format");
                if (folioServiceContext.AnyFund2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fund");
                if (folioServiceContext.AnyFund2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a fund");
                if (folioServiceContext.AnyGroup2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a group");
                if (folioServiceContext.AnyGroup2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a group");
                if (folioServiceContext.AnyHolding2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding");
                if (folioServiceContext.AnyHolding2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding");
                if (folioServiceContext.AnyHoldingNoteType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding note type");
                if (folioServiceContext.AnyHoldingNoteType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding note type");
                if (folioServiceContext.AnyHoldingType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding type");
                if (folioServiceContext.AnyHoldingType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a holding type");
                if (folioServiceContext.AnyIdType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a id type");
                if (folioServiceContext.AnyIdType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a id type");
                if (folioServiceContext.AnyIllPolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ill policy");
                if (folioServiceContext.AnyIllPolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ill policy");
                if (folioServiceContext.AnyInstance2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance");
                if (folioServiceContext.AnyInstance2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance");
                if (folioServiceContext.AnyInstanceNoteType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance note type");
                if (folioServiceContext.AnyInstanceNoteType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance note type");
                if (folioServiceContext.AnyInstanceType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance type");
                if (folioServiceContext.AnyInstanceType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a instance type");
                if (folioServiceContext.AnyInstitution2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a institution");
                if (folioServiceContext.AnyInstitution2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a institution");
                if (folioServiceContext.AnyInterface2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a interface");
                if (folioServiceContext.AnyInterface2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a interface");
                if (folioServiceContext.AnyInvoice2s($"approvedBy == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a invoice");
                if (folioServiceContext.AnyInvoice2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a invoice");
                if (folioServiceContext.AnyInvoice2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a invoice");
                if (folioServiceContext.AnyInvoiceItem2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a invoice item");
                if (folioServiceContext.AnyInvoiceItem2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a invoice item");
                if (folioServiceContext.AnyIssuanceModes($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a issuance mode");
                if (folioServiceContext.AnyIssuanceModes($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a issuance mode");
                if (folioServiceContext.AnyItem2s($"lastCheckIn.staffMemberId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item");
                if (folioServiceContext.AnyItem2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item");
                if (folioServiceContext.AnyItem2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item");
                if (folioServiceContext.AnyItemDamagedStatus2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item damaged status");
                if (folioServiceContext.AnyItemDamagedStatus2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item damaged status");
                if (folioServiceContext.AnyItemNoteType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item note type");
                if (folioServiceContext.AnyItemNoteType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a item note type");
                if (folioServiceContext.AnyLedger2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger");
                if (folioServiceContext.AnyLedger2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger");
                if (folioServiceContext.AnyLibrary2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a library");
                if (folioServiceContext.AnyLibrary2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a library");
                if (folioServiceContext.AnyLoan2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyLoan2s($"proxyUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyLoan2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyLoan2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyLoanPolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan policy");
                if (folioServiceContext.AnyLoanPolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan policy");
                if (folioServiceContext.AnyLoanType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan type");
                if (folioServiceContext.AnyLoanType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a loan type");
                if (folioServiceContext.AnyLocation2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a location");
                if (folioServiceContext.AnyLocation2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a location");
                if (folioServiceContext.LocationSettings().Any(cu => cu.CreationUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a location setting");
                if (folioServiceContext.LocationSettings().Any(lwu => lwu.LastWriteUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a location setting");
                if (folioServiceContext.AnyLostItemFeePolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a lost item fee policy");
                if (folioServiceContext.AnyLostItemFeePolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a lost item fee policy");
                if (folioServiceContext.AnyManualBlockTemplate2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a manual block template");
                if (folioServiceContext.AnyManualBlockTemplate2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a manual block template");
                if (folioServiceContext.AnyMaterialType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a material type");
                if (folioServiceContext.AnyMaterialType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a material type");
                if (folioServiceContext.AnyNatureOfContentTerm2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a nature of content term");
                if (folioServiceContext.AnyNatureOfContentTerm2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a nature of content term");
                if (folioServiceContext.AnyNote2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note");
                if (folioServiceContext.AnyNote2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note");
                if (folioServiceContext.AnyNoteType2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note type");
                if (folioServiceContext.AnyNoteType2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note type");
                if (folioServiceContext.AnyOrder2s($"approvedById == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order");
                if (folioServiceContext.AnyOrder2s($"assignedTo == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order");
                if (folioServiceContext.AnyOrder2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order");
                if (folioServiceContext.AnyOrder2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order");
                if (folioServiceContext.AnyOrderItem2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order item");
                if (folioServiceContext.AnyOrderItem2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a order item");
                if (folioServiceContext.AnyOrganization2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a organization");
                if (folioServiceContext.AnyOrganization2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a organization");
                if (folioServiceContext.AnyOverdueFinePolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a overdue fine policy");
                if (folioServiceContext.AnyOverdueFinePolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a overdue fine policy");
                if (folioServiceContext.AnyOwner2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a owner");
                if (folioServiceContext.AnyOwner2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a owner");
                if (folioServiceContext.AnyPatronActionSession2s($"patronId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a patron action session");
                if (folioServiceContext.AnyPatronActionSession2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a patron action session");
                if (folioServiceContext.AnyPatronActionSession2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a patron action session");
                if (folioServiceContext.AnyPatronNoticePolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a patron notice policy");
                if (folioServiceContext.AnyPatronNoticePolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a patron notice policy");
                if (folioServiceContext.AnyPayment2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a payment");
                if (folioServiceContext.AnyPaymentMethod2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a payment method");
                if (folioServiceContext.AnyPaymentMethod2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a payment method");
                if (folioServiceContext.AnyPermission2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a permission");
                if (folioServiceContext.AnyPermission2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a permission");
                if (folioServiceContext.AnyPermissionsUser2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a permissions user");
                if (folioServiceContext.AnyPermissionsUser2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a permissions user");
                if (folioServiceContext.AnyPermissionsUser2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a permissions user");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.AnyPrecedingSucceedingTitle2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a preceding succeeding title");
                if (folioServiceContext.Printers().Any(cu => cu.CreationUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a printer");
                if (folioServiceContext.Printers().Any(lwu => lwu.LastWriteUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a printer");
                if (folioServiceContext.AnyProxy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a proxy");
                if (folioServiceContext.AnyProxy2s($"proxyUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a proxy");
                if (folioServiceContext.AnyProxy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a proxy");
                if (folioServiceContext.AnyProxy2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a proxy");
                if (folioServiceContext.AnyReceiving2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a receiving");
                if (folioServiceContext.AnyReceiving2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a receiving");
                if (folioServiceContext.AnyRecord2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRecord2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRefundReason2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a refund reason");
                if (folioServiceContext.AnyRefundReason2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a refund reason");
                if (folioServiceContext.AnyRelationships($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationships($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationshipTypes($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship type");
                if (folioServiceContext.AnyRelationshipTypes($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship type");
                if (folioServiceContext.AnyRequest2s($"printDetails.requesterId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"cancelledByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"proxyUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"requesterId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequestPolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request policy");
                if (folioServiceContext.AnyRequestPolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request policy");
                if (folioServiceContext.AnyRollover2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover");
                if (folioServiceContext.AnyRollover2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover");
                if (folioServiceContext.AnyRolloverBudget2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover budget");
                if (folioServiceContext.AnyRolloverBudget2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover budget");
                if (folioServiceContext.AnyRolloverError2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover error");
                if (folioServiceContext.AnyRolloverError2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover error");
                if (folioServiceContext.AnyRolloverProgress2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover progress");
                if (folioServiceContext.AnyRolloverProgress2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a rollover progress");
                if (folioServiceContext.AnyScheduledNotice2s($"recipientUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a scheduled notice");
                if (folioServiceContext.AnyScheduledNotice2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a scheduled notice");
                if (folioServiceContext.AnyScheduledNotice2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a scheduled notice");
                if (folioServiceContext.AnyServicePoint2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a service point");
                if (folioServiceContext.AnyServicePoint2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a service point");
                if (folioServiceContext.AnyServicePointUser2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a service point user");
                if (folioServiceContext.AnyServicePointUser2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a service point user");
                if (folioServiceContext.AnyServicePointUser2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a service point user");
                if (folioServiceContext.Settings().Any(cu => cu.CreationUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a setting");
                if (folioServiceContext.Settings().Any(lwu => lwu.LastWriteUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a setting");
                if (folioServiceContext.AnySnapshot2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a snapshot");
                if (folioServiceContext.AnySnapshot2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a snapshot");
                if (folioServiceContext.AnySource2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a source");
                if (folioServiceContext.AnySource2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a source");
                if (folioServiceContext.AnyStaffSlip2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a staff slip");
                if (folioServiceContext.AnyStaffSlip2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a staff slip");
                if (folioServiceContext.AnyStatisticalCode2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a statistical code");
                if (folioServiceContext.AnyStatisticalCode2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a statistical code");
                if (folioServiceContext.AnyStatisticalCodeType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a statistical code type");
                if (folioServiceContext.AnyStatisticalCodeType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a statistical code type");
                if (folioServiceContext.AnyStatuses($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a status");
                if (folioServiceContext.AnyStatuses($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a status");
                if (folioServiceContext.AnyTag2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a tag");
                if (folioServiceContext.AnyTag2s($" == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a tag");
                if (folioServiceContext.AnyTemplate2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a template");
                if (folioServiceContext.AnyTemplate2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a template");
                if (folioServiceContext.AnyTitle2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a title");
                if (folioServiceContext.AnyTitle2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a title");
                if (folioServiceContext.AnyTransaction2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a transaction");
                if (folioServiceContext.AnyTransaction2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a transaction");
                if (folioServiceContext.AnyTransferAccount2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a transfer account");
                if (folioServiceContext.AnyTransferAccount2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a transfer account");
                if (folioServiceContext.AnyUser2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user");
                if (folioServiceContext.AnyUser2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user");
                if (folioServiceContext.AnyUserAcquisitionsUnit2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user acquisitions unit");
                if (folioServiceContext.AnyUserAcquisitionsUnit2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user acquisitions unit");
                if (folioServiceContext.AnyUserAcquisitionsUnit2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user acquisitions unit");
                if (folioServiceContext.AnyUserRequestPreference2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user request preference");
                if (folioServiceContext.AnyUserRequestPreference2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user request preference");
                if (folioServiceContext.AnyUserRequestPreference2s($"userId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a user request preference");
                if (folioServiceContext.AnyVoucher2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a voucher");
                if (folioServiceContext.AnyVoucher2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a voucher");
                if (folioServiceContext.AnyVoucherItem2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a voucher item");
                if (folioServiceContext.AnyVoucherItem2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a voucher item");
                if (folioServiceContext.AnyWaiveReason2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a waive reason");
                if (folioServiceContext.AnyWaiveReason2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a waive reason");
                folioServiceContext.DeleteUser2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        protected void Block2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Block2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Type", "type" }, { "Description", "desc" }, { "Code", "code" }, { "StaffInformation", "staffInformation" }, { "PatronMessage", "patronMessage" }, { "ExpirationDate", "expirationDate" }, { "Borrowing", "borrowing" }, { "Renewals", "renewals" }, { "Requests", "requests" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(Block2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(Block2s2RadGrid, "Type", "type"),
                Global.GetCqlFilter(Block2s2RadGrid, "Description", "desc"),
                Global.GetCqlFilter(Block2s2RadGrid, "Code", "code"),
                Global.GetCqlFilter(Block2s2RadGrid, "StaffInformation", "staffInformation"),
                Global.GetCqlFilter(Block2s2RadGrid, "PatronMessage", "patronMessage"),
                Global.GetCqlFilter(Block2s2RadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Block2s2RadGrid, "Borrowing", "borrowing"),
                Global.GetCqlFilter(Block2s2RadGrid, "Renewals", "renewals"),
                Global.GetCqlFilter(Block2s2RadGrid, "Requests", "requests"),
                Global.GetCqlFilter(Block2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Block2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Block2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Block2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Block2s2RadGrid.DataSource = folioServiceContext.Block2s(where, Block2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Block2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Block2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Block2s2RadGrid.PageSize * Block2s2RadGrid.CurrentPageIndex, Block2s2RadGrid.PageSize, true);
            Block2s2RadGrid.VirtualItemCount = folioServiceContext.CountBlock2s(where);
            if (Block2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Block2s2RadGrid.AllowFilteringByColumn = Block2s2RadGrid.VirtualItemCount > 10;
                Block2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["Block2sPermission"] != null && Block2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"performedByUserId == \"{id}\"",
                Global.GetCqlFilter(CheckIn2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "OccurredDateTime", "occurredDateTime"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "RequestQueueSize", "requestQueueSize"),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ItemLocation.Name", "itemLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(CheckIn2sRadGrid, "ServicePoint.Name", "servicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints)
            }.Where(s => s != null)));
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(where, CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = folioServiceContext.CountCheckIn2s(where);
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = User2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Fee2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(Fee2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(Fee2s2RadGrid, "Amount", "amount"),
                Global.GetCqlFilter(Fee2s2RadGrid, "RemainingAmount", "remaining"),
                Global.GetCqlFilter(Fee2s2RadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Fee2s2RadGrid, "PaymentStatusName", "paymentStatus.name"),
                Global.GetCqlFilter(Fee2s2RadGrid, "Title", "title"),
                Global.GetCqlFilter(Fee2s2RadGrid, "CallNumber", "callNumber"),
                Global.GetCqlFilter(Fee2s2RadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(Fee2s2RadGrid, "MaterialType", "materialType"),
                Global.GetCqlFilter(Fee2s2RadGrid, "ItemStatusName", "itemStatus.name"),
                Global.GetCqlFilter(Fee2s2RadGrid, "Location", "location"),
                Global.GetCqlFilter(Fee2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Fee2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Fee2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Fee2s2RadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Fee2s2RadGrid, "ReturnedTime", "returnedDate"),
                Global.GetCqlFilter(Fee2s2RadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(Fee2s2RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Fee2s2RadGrid, "MaterialType1.Name", "materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(Fee2s2RadGrid, "FeeType.Name", "feeFineId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(Fee2s2RadGrid, "Owner.Name", "ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(Fee2s2RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Fee2s2RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances)
            }.Where(s => s != null)));
            Fee2s2RadGrid.DataSource = folioServiceContext.Fee2s(where, Fee2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2s2RadGrid.PageSize * Fee2s2RadGrid.CurrentPageIndex, Fee2s2RadGrid.PageSize, true);
            Fee2s2RadGrid.VirtualItemCount = folioServiceContext.CountFee2s(where);
            if (Fee2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2s2RadGrid.AllowFilteringByColumn = Fee2s2RadGrid.VirtualItemCount > 10;
                Fee2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Fee2s2Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Payment2sPermission"] == null) return;
            var rg = (RadGrid)sender;
            var id = (Guid?)((GridDataItem)rg.Parent.Parent).GetDataKeyValue("Id");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePoint", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"accountId == \"{id}\"",
                Global.GetCqlFilter(rg, "Id", "id"),
                Global.GetCqlFilter(rg, "CreationTime", "dateAction"),
                Global.GetCqlFilter(rg, "TypeAction", "typeAction"),
                Global.GetCqlFilter(rg, "Comments", "comments"),
                Global.GetCqlFilter(rg, "Notify", "notify"),
                Global.GetCqlFilter(rg, "Amount", "amountAction"),
                Global.GetCqlFilter(rg, "RemainingAmount", "balance"),
                Global.GetCqlFilter(rg, "TransactionInformation", "transactionInformation"),
                Global.GetCqlFilter(rg, "ServicePoint", "createdAt"),
                Global.GetCqlFilter(rg, "Source", "source"),
                Global.GetCqlFilter(rg, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(rg, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            rg.DataSource = folioServiceContext.Payment2s(where, rg.MasterTableView.SortExpressions.Count > 0 ? $"{d[rg.MasterTableView.SortExpressions[0].FieldName]}{(rg.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, rg.PageSize * rg.CurrentPageIndex, rg.PageSize, true);
            rg.VirtualItemCount = folioServiceContext.CountPayment2s(where);
            if (rg.MasterTableView.FilterExpression == "")
            {
                rg.AllowFilteringByColumn = rg.VirtualItemCount > 10;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "Enclosure", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNumber", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "FiscalYearId", "fiscalYearId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "NextInvoiceLineNumber", "nextInvoiceLineNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"approvedBy == \"{id}\"",
                Global.GetCqlFilter(Invoice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountingCode", "accountingCode"),
                Global.GetCqlFilter(Invoice2sRadGrid, "AdjustmentsTotal", "adjustmentsTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "BatchGroup.Name", "batchGroupId", "name", folioServiceContext.FolioServiceClient.BatchGroups),
                Global.GetCqlFilter(Invoice2sRadGrid, "BillTo.Id", "billTo", "id", folioServiceContext.FolioServiceClient.Configurations),
                Global.GetCqlFilter(Invoice2sRadGrid, "CheckSubscriptionOverlap", "chkSubscriptionOverlap"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CancellationNote", "cancellationNote"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Currency", "currency"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Enclosure", "enclosureNeeded"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExchangeRate", "exchangeRate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ExportToAccounting", "exportToAccounting"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Number", "folioInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "InvoiceDate", "invoiceDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LockTotal", "lockTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Note", "note"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDueDate", "paymentDue"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentDate", "paymentDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentTerms", "paymentTerms"),
                Global.GetCqlFilter(Invoice2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Invoice2sRadGrid, "SubTotal", "subTotal"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Total", "total"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VendorInvoiceNumber", "vendorInvoiceNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementNumber", "disbursementNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "VoucherNumber", "voucherNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Payment.Amount", "paymentId", "amount", folioServiceContext.FolioServiceClient.Transactions),
                Global.GetCqlFilter(Invoice2sRadGrid, "DisbursementDate", "disbursementDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "Vendor.Name", "vendorId", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Invoice2sRadGrid, "FiscalYear.Name", "fiscalYearId", "name", folioServiceContext.FolioServiceClient.FiscalYears),
                Global.GetCqlFilter(Invoice2sRadGrid, "AccountNumber", "accountNo"),
                Global.GetCqlFilter(Invoice2sRadGrid, "ManualPayment", "manualPayment"),
                Global.GetCqlFilter(Invoice2sRadGrid, "NextInvoiceLineNumber", "nextInvoiceLineNumber"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Invoice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(where, Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = folioServiceContext.CountInvoice2s(where);
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Loan2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "IsDcb", "isDcb" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" }, { "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number" }, { "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"proxyUserId == \"{id}\"",
                Global.GetCqlFilter(Loan2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Loan2s1RadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s1RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Loan2s1RadGrid, "ItemEffectiveLocationAtCheckOut.Name", "itemEffectiveLocationIdAtCheckOut", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Loan2s1RadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Loan2s1RadGrid, "LoanTime", "loanDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "ReturnTime", "returnDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "SystemReturnTime", "systemReturnDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "Action", "action"),
                Global.GetCqlFilter(Loan2s1RadGrid, "ActionComment", "actionComment"),
                Global.GetCqlFilter(Loan2s1RadGrid, "ItemStatus", "itemStatus"),
                Global.GetCqlFilter(Loan2s1RadGrid, "RenewalCount", "renewalCount"),
                Global.GetCqlFilter(Loan2s1RadGrid, "LoanPolicy.Name", "loanPolicyId", "name", folioServiceContext.FolioServiceClient.LoanPolicies),
                Global.GetCqlFilter(Loan2s1RadGrid, "CheckoutServicePoint.Name", "checkoutServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2s1RadGrid, "CheckinServicePoint.Name", "checkinServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2s1RadGrid, "Group.Name", "patronGroupIdAtCheckout", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(Loan2s1RadGrid, "DueDateChangedByRecall", "dueDateChangedByRecall"),
                Global.GetCqlFilter(Loan2s1RadGrid, "IsDcb", "isDcb"),
                Global.GetCqlFilter(Loan2s1RadGrid, "DeclaredLostDate", "declaredLostDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "ClaimedReturnedDate", "claimedReturnedDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "OverdueFinePolicy.Name", "overdueFinePolicyId", "name", folioServiceContext.FolioServiceClient.OverdueFinePolicies),
                Global.GetCqlFilter(Loan2s1RadGrid, "LostItemPolicy.Name", "lostItemPolicyId", "name", folioServiceContext.FolioServiceClient.LostItemFeePolicies),
                Global.GetCqlFilter(Loan2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s1RadGrid, "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled"),
                Global.GetCqlFilter(Loan2s1RadGrid, "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled"),
                Global.GetCqlFilter(Loan2s1RadGrid, "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate"),
                Global.GetCqlFilter(Loan2s1RadGrid, "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number"),
                Global.GetCqlFilter(Loan2s1RadGrid, "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date")
            }.Where(s => s != null)));
            Loan2s1RadGrid.DataSource = folioServiceContext.Loan2s(where, Loan2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2s1RadGrid.PageSize * Loan2s1RadGrid.CurrentPageIndex, Loan2s1RadGrid.PageSize, true);
            Loan2s1RadGrid.VirtualItemCount = folioServiceContext.CountLoan2s(where);
            if (Loan2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2s1RadGrid.AllowFilteringByColumn = Loan2s1RadGrid.VirtualItemCount > 10;
                Loan2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Loan2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "IsDcb", "isDcb" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" }, { "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number" }, { "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(Loan2s3RadGrid, "Id", "id"),
                Global.GetCqlFilter(Loan2s3RadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s3RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Loan2s3RadGrid, "ItemEffectiveLocationAtCheckOut.Name", "itemEffectiveLocationIdAtCheckOut", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Loan2s3RadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Loan2s3RadGrid, "LoanTime", "loanDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "ReturnTime", "returnDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "SystemReturnTime", "systemReturnDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "Action", "action"),
                Global.GetCqlFilter(Loan2s3RadGrid, "ActionComment", "actionComment"),
                Global.GetCqlFilter(Loan2s3RadGrid, "ItemStatus", "itemStatus"),
                Global.GetCqlFilter(Loan2s3RadGrid, "RenewalCount", "renewalCount"),
                Global.GetCqlFilter(Loan2s3RadGrid, "LoanPolicy.Name", "loanPolicyId", "name", folioServiceContext.FolioServiceClient.LoanPolicies),
                Global.GetCqlFilter(Loan2s3RadGrid, "CheckoutServicePoint.Name", "checkoutServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2s3RadGrid, "CheckinServicePoint.Name", "checkinServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2s3RadGrid, "Group.Name", "patronGroupIdAtCheckout", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(Loan2s3RadGrid, "DueDateChangedByRecall", "dueDateChangedByRecall"),
                Global.GetCqlFilter(Loan2s3RadGrid, "IsDcb", "isDcb"),
                Global.GetCqlFilter(Loan2s3RadGrid, "DeclaredLostDate", "declaredLostDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "ClaimedReturnedDate", "claimedReturnedDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "OverdueFinePolicy.Name", "overdueFinePolicyId", "name", folioServiceContext.FolioServiceClient.OverdueFinePolicies),
                Global.GetCqlFilter(Loan2s3RadGrid, "LostItemPolicy.Name", "lostItemPolicyId", "name", folioServiceContext.FolioServiceClient.LostItemFeePolicies),
                Global.GetCqlFilter(Loan2s3RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s3RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2s3RadGrid, "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled"),
                Global.GetCqlFilter(Loan2s3RadGrid, "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled"),
                Global.GetCqlFilter(Loan2s3RadGrid, "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate"),
                Global.GetCqlFilter(Loan2s3RadGrid, "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number"),
                Global.GetCqlFilter(Loan2s3RadGrid, "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date")
            }.Where(s => s != null)));
            Loan2s3RadGrid.DataSource = folioServiceContext.Loan2s(where, Loan2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2s3RadGrid.PageSize * Loan2s3RadGrid.CurrentPageIndex, Loan2s3RadGrid.PageSize, true);
            Loan2s3RadGrid.VirtualItemCount = folioServiceContext.CountLoan2s(where);
            if (Loan2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2s3RadGrid.AllowFilteringByColumn = Loan2s3RadGrid.VirtualItemCount > 10;
                Loan2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2s3RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "NextPolNumber", "nextPolNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"approvedById == \"{id}\"",
                Global.GetCqlFilter(Order2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Order2sRadGrid, "Approved", "approved"),
                Global.GetCqlFilter(Order2sRadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "AssignedTo.Username", "assignedTo", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonReason", "closeReason.reason"),
                Global.GetCqlFilter(Order2sRadGrid, "CloseReasonNote", "closeReason.note"),
                Global.GetCqlFilter(Order2sRadGrid, "OrderDate", "dateOrdered"),
                Global.GetCqlFilter(Order2sRadGrid, "Manual", "manualPo"),
                Global.GetCqlFilter(Order2sRadGrid, "Number", "poNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OrderType", "orderType"),
                Global.GetCqlFilter(Order2sRadGrid, "Reencumber", "reEncumber"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingInterval", "ongoing.interval"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingIsSubscription", "ongoing.isSubscription"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingManualRenewal", "ongoing.manualRenewal"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingNotes", "ongoing.notes"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewPeriod", "ongoing.reviewPeriod"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingRenewalDate", "ongoing.renewalDate"),
                Global.GetCqlFilter(Order2sRadGrid, "OngoingReviewDate", "ongoing.reviewDate"),
                Global.GetCqlFilter(Order2sRadGrid, "Template.Name", "template", "templateName", folioServiceContext.FolioServiceClient.OrderTemplates),
                Global.GetCqlFilter(Order2sRadGrid, "Vendor.Name", "vendor", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Order2sRadGrid, "Status", "workflowStatus"),
                Global.GetCqlFilter(Order2sRadGrid, "NextPolNumber", "nextPolNumber"),
                Global.GetCqlFilter(Order2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Order2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Order2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(where, Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = folioServiceContext.CountOrder2s(where);
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Order2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "NextPolNumber", "nextPolNumber" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"assignedTo == \"{id}\"",
                Global.GetCqlFilter(Order2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Order2s1RadGrid, "Approved", "approved"),
                Global.GetCqlFilter(Order2s1RadGrid, "ApprovedBy.Username", "approvedById", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2s1RadGrid, "ApprovalDate", "approvalDate"),
                Global.GetCqlFilter(Order2s1RadGrid, "CloseReasonReason", "closeReason.reason"),
                Global.GetCqlFilter(Order2s1RadGrid, "CloseReasonNote", "closeReason.note"),
                Global.GetCqlFilter(Order2s1RadGrid, "OrderDate", "dateOrdered"),
                Global.GetCqlFilter(Order2s1RadGrid, "Manual", "manualPo"),
                Global.GetCqlFilter(Order2s1RadGrid, "Number", "poNumber"),
                Global.GetCqlFilter(Order2s1RadGrid, "OrderType", "orderType"),
                Global.GetCqlFilter(Order2s1RadGrid, "Reencumber", "reEncumber"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingInterval", "ongoing.interval"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingIsSubscription", "ongoing.isSubscription"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingManualRenewal", "ongoing.manualRenewal"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingNotes", "ongoing.notes"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingReviewPeriod", "ongoing.reviewPeriod"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingRenewalDate", "ongoing.renewalDate"),
                Global.GetCqlFilter(Order2s1RadGrid, "OngoingReviewDate", "ongoing.reviewDate"),
                Global.GetCqlFilter(Order2s1RadGrid, "Template.Name", "template", "templateName", folioServiceContext.FolioServiceClient.OrderTemplates),
                Global.GetCqlFilter(Order2s1RadGrid, "Vendor.Name", "vendor", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(Order2s1RadGrid, "Status", "workflowStatus"),
                Global.GetCqlFilter(Order2s1RadGrid, "NextPolNumber", "nextPolNumber"),
                Global.GetCqlFilter(Order2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Order2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Order2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Order2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Order2s1RadGrid.DataSource = folioServiceContext.Order2s(where, Order2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2s1RadGrid.PageSize * Order2s1RadGrid.CurrentPageIndex, Order2s1RadGrid.PageSize, true);
            Order2s1RadGrid.VirtualItemCount = folioServiceContext.CountOrder2s(where);
            if (Order2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Order2s1RadGrid.AllowFilteringByColumn = Order2s1RadGrid.VirtualItemCount > 10;
                Order2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PatronActionSession2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronActionSession2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "SessionId", "sessionId" }, { "PatronId", "patronId" }, { "LoanId", "loanId" }, { "ActionType", "actionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"patronId == \"{id}\"",
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "SessionId", "sessionId"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "ActionType", "actionType"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PatronActionSession2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PatronActionSession2sRadGrid.DataSource = folioServiceContext.PatronActionSession2s(where, PatronActionSession2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PatronActionSession2sRadGrid.PageSize * PatronActionSession2sRadGrid.CurrentPageIndex, PatronActionSession2sRadGrid.PageSize, true);
            PatronActionSession2sRadGrid.VirtualItemCount = folioServiceContext.CountPatronActionSession2s(where);
            if (PatronActionSession2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PatronActionSession2sRadGrid.AllowFilteringByColumn = PatronActionSession2sRadGrid.VirtualItemCount > 10;
                PatronActionSession2sPanel.Visible = User2FormView.DataKey.Value != null && Session["PatronActionSession2sPermission"] != null && PatronActionSession2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Payment2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePoint", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(Payment2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Payment2sRadGrid, "CreationTime", "dateAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "TypeAction", "typeAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "Comments", "comments"),
                Global.GetCqlFilter(Payment2sRadGrid, "Notify", "notify"),
                Global.GetCqlFilter(Payment2sRadGrid, "Amount", "amountAction"),
                Global.GetCqlFilter(Payment2sRadGrid, "RemainingAmount", "balance"),
                Global.GetCqlFilter(Payment2sRadGrid, "TransactionInformation", "transactionInformation"),
                Global.GetCqlFilter(Payment2sRadGrid, "ServicePoint", "createdAt"),
                Global.GetCqlFilter(Payment2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(Payment2sRadGrid, "PaymentMethod", "paymentMethod"),
                Global.GetCqlFilter(Payment2sRadGrid, "Fee.Title", "accountId", "title", folioServiceContext.FolioServiceClient.Fees)
            }.Where(s => s != null)));
            Payment2sRadGrid.DataSource = folioServiceContext.Payment2s(where, Payment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Payment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Payment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Payment2sRadGrid.PageSize * Payment2sRadGrid.CurrentPageIndex, Payment2sRadGrid.PageSize, true);
            Payment2sRadGrid.VirtualItemCount = folioServiceContext.CountPayment2s(where);
            if (Payment2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Payment2sRadGrid.AllowFilteringByColumn = Payment2sRadGrid.VirtualItemCount > 10;
                Payment2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Payment2sPermission"] != null && Payment2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PermissionsUser2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PermissionsUser2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(PermissionsUser2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(PermissionsUser2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(PermissionsUser2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(PermissionsUser2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(PermissionsUser2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            PermissionsUser2s2RadGrid.DataSource = folioServiceContext.PermissionsUser2s(where, PermissionsUser2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PermissionsUser2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PermissionsUser2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PermissionsUser2s2RadGrid.PageSize * PermissionsUser2s2RadGrid.CurrentPageIndex, PermissionsUser2s2RadGrid.PageSize, true);
            PermissionsUser2s2RadGrid.VirtualItemCount = folioServiceContext.CountPermissionsUser2s(where);
            if (PermissionsUser2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                PermissionsUser2s2RadGrid.AllowFilteringByColumn = PermissionsUser2s2RadGrid.VirtualItemCount > 10;
                PermissionsUser2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["PermissionsUser2sPermission"] != null && PermissionsUser2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void PreferredEmailCommunicationsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PreferredEmailCommunicationsPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindUser2(id, true).PreferredEmailCommunications ?? new PreferredEmailCommunication[] { };
            PreferredEmailCommunicationsRadGrid.DataSource = l;
            PreferredEmailCommunicationsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            PreferredEmailCommunicationsPanel.Visible = User2FormView.DataKey.Value != null && ((string)Session["PreferredEmailCommunicationsPermission"] == "Edit" || Session["PreferredEmailCommunicationsPermission"] != null && l.Any());
        }

        protected void Proxy2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Proxy2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"proxyUserId == \"{id}\"",
                Global.GetCqlFilter(Proxy2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2s1RadGrid, "RequestForSponsor", "requestForSponsor"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "NotificationsTo", "notificationsTo"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "AccrueTo", "accrueTo"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "Status", "status"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Proxy2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Proxy2s1RadGrid.DataSource = folioServiceContext.Proxy2s(where, Proxy2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Proxy2s1RadGrid.PageSize * Proxy2s1RadGrid.CurrentPageIndex, Proxy2s1RadGrid.PageSize, true);
            Proxy2s1RadGrid.VirtualItemCount = folioServiceContext.CountProxy2s(where);
            if (Proxy2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Proxy2s1RadGrid.AllowFilteringByColumn = Proxy2s1RadGrid.VirtualItemCount > 10;
                Proxy2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Proxy2sPermission"] != null && Proxy2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Proxy2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Proxy2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(Proxy2s3RadGrid, "Id", "id"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2s3RadGrid, "RequestForSponsor", "requestForSponsor"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "NotificationsTo", "notificationsTo"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "AccrueTo", "accrueTo"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "Status", "status"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Proxy2s3RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Proxy2s3RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            Proxy2s3RadGrid.DataSource = folioServiceContext.Proxy2s(where, Proxy2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Proxy2s3RadGrid.PageSize * Proxy2s3RadGrid.CurrentPageIndex, Proxy2s3RadGrid.PageSize, true);
            Proxy2s3RadGrid.VirtualItemCount = folioServiceContext.CountProxy2s(where);
            if (Proxy2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Proxy2s3RadGrid.AllowFilteringByColumn = Proxy2s3RadGrid.VirtualItemCount > 10;
                Proxy2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Proxy2sPermission"] != null && Proxy2s3RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"printDetails.requesterId == \"{id}\"",
                Global.GetCqlFilter(Request2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2sRadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2sRadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2sRadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2sRadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Request2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(Request2sRadGrid, "CancellationReason.Name", "cancellationReasonId", "name", folioServiceContext.FolioServiceClient.CancellationReasons),
                Global.GetCqlFilter(Request2sRadGrid, "CancelledByUser.Username", "cancelledByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "CancellationAdditionalInformation", "cancellationAdditionalInformation"),
                Global.GetCqlFilter(Request2sRadGrid, "CancelledDate", "cancelledDate"),
                Global.GetCqlFilter(Request2sRadGrid, "Position", "position"),
                Global.GetCqlFilter(Request2sRadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(Request2sRadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterFirstName", "requester.firstName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterLastName", "requester.lastName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterMiddleName", "requester.middleName"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterBarcode", "requester.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "RequesterPatronGroup", "requester.patronGroup"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyFirstName", "proxy.firstName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyLastName", "proxy.lastName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyMiddleName", "proxy.middleName"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyBarcode", "proxy.barcode"),
                Global.GetCqlFilter(Request2sRadGrid, "ProxyPatronGroup", "proxy.patronGroup"),
                Global.GetCqlFilter(Request2sRadGrid, "FulfillmentPreference", "fulfillmentPreference"),
                Global.GetCqlFilter(Request2sRadGrid, "DeliveryAddressType.Name", "deliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(Request2sRadGrid, "RequestExpirationDate", "requestExpirationDate"),
                Global.GetCqlFilter(Request2sRadGrid, "HoldShelfExpirationDate", "holdShelfExpirationDate"),
                Global.GetCqlFilter(Request2sRadGrid, "PickupServicePoint.Name", "pickupServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Request2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Request2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Request2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsPrintCount", "printDetails.printCount"),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsIsPrinted", "printDetails.isPrinted"),
                Global.GetCqlFilter(Request2sRadGrid, "PrintDetailsPrintEventDate", "printDetails.printEventDate"),
                Global.GetCqlFilter(Request2sRadGrid, "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexShelvingOrder", "searchIndex.shelvingOrder"),
                Global.GetCqlFilter(Request2sRadGrid, "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName"),
                Global.GetCqlFilter(Request2sRadGrid, "ItemLocationCode", "itemLocationCode")
            }.Where(s => s != null)));
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(where, Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = folioServiceContext.CountRequest2s(where);
            if (Request2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Request2sRadGrid.AllowFilteringByColumn = Request2sRadGrid.VirtualItemCount > 10;
                Request2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"cancelledByUserId == \"{id}\"",
                Global.GetCqlFilter(Request2s1RadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2s1RadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2s1RadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s1RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2s1RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2s1RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Request2s1RadGrid, "Status", "status"),
                Global.GetCqlFilter(Request2s1RadGrid, "CancellationReason.Name", "cancellationReasonId", "name", folioServiceContext.FolioServiceClient.CancellationReasons),
                Global.GetCqlFilter(Request2s1RadGrid, "CancellationAdditionalInformation", "cancellationAdditionalInformation"),
                Global.GetCqlFilter(Request2s1RadGrid, "CancelledDate", "cancelledDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "Position", "position"),
                Global.GetCqlFilter(Request2s1RadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(Request2s1RadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequesterFirstName", "requester.firstName"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequesterLastName", "requester.lastName"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequesterMiddleName", "requester.middleName"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequesterBarcode", "requester.barcode"),
                Global.GetCqlFilter(Request2s1RadGrid, "RequesterPatronGroup", "requester.patronGroup"),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyFirstName", "proxy.firstName"),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyLastName", "proxy.lastName"),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyMiddleName", "proxy.middleName"),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyBarcode", "proxy.barcode"),
                Global.GetCqlFilter(Request2s1RadGrid, "ProxyPatronGroup", "proxy.patronGroup"),
                Global.GetCqlFilter(Request2s1RadGrid, "FulfillmentPreference", "fulfillmentPreference"),
                Global.GetCqlFilter(Request2s1RadGrid, "DeliveryAddressType.Name", "deliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(Request2s1RadGrid, "RequestExpirationDate", "requestExpirationDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "HoldShelfExpirationDate", "holdShelfExpirationDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "PickupServicePoint.Name", "pickupServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Request2s1RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s1RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s1RadGrid, "PrintDetailsPrintCount", "printDetails.printCount"),
                Global.GetCqlFilter(Request2s1RadGrid, "PrintDetailsRequester.Username", "printDetails.requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s1RadGrid, "PrintDetailsIsPrinted", "printDetails.isPrinted"),
                Global.GetCqlFilter(Request2s1RadGrid, "PrintDetailsPrintEventDate", "printDetails.printEventDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate"),
                Global.GetCqlFilter(Request2s1RadGrid, "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber"),
                Global.GetCqlFilter(Request2s1RadGrid, "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix"),
                Global.GetCqlFilter(Request2s1RadGrid, "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix"),
                Global.GetCqlFilter(Request2s1RadGrid, "SearchIndexShelvingOrder", "searchIndex.shelvingOrder"),
                Global.GetCqlFilter(Request2s1RadGrid, "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName"),
                Global.GetCqlFilter(Request2s1RadGrid, "ItemLocationCode", "itemLocationCode")
            }.Where(s => s != null)));
            Request2s1RadGrid.DataSource = folioServiceContext.Request2s(where, Request2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2s1RadGrid.PageSize * Request2s1RadGrid.CurrentPageIndex, Request2s1RadGrid.PageSize, true);
            Request2s1RadGrid.VirtualItemCount = folioServiceContext.CountRequest2s(where);
            if (Request2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Request2s1RadGrid.AllowFilteringByColumn = Request2s1RadGrid.VirtualItemCount > 10;
                Request2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2s1RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"proxyUserId == \"{id}\"",
                Global.GetCqlFilter(Request2s3RadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2s3RadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2s3RadGrid, "Requester.Username", "requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s3RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2s3RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2s3RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Request2s3RadGrid, "Status", "status"),
                Global.GetCqlFilter(Request2s3RadGrid, "CancellationReason.Name", "cancellationReasonId", "name", folioServiceContext.FolioServiceClient.CancellationReasons),
                Global.GetCqlFilter(Request2s3RadGrid, "CancelledByUser.Username", "cancelledByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s3RadGrid, "CancellationAdditionalInformation", "cancellationAdditionalInformation"),
                Global.GetCqlFilter(Request2s3RadGrid, "CancelledDate", "cancelledDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "Position", "position"),
                Global.GetCqlFilter(Request2s3RadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(Request2s3RadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequesterFirstName", "requester.firstName"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequesterLastName", "requester.lastName"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequesterMiddleName", "requester.middleName"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequesterBarcode", "requester.barcode"),
                Global.GetCqlFilter(Request2s3RadGrid, "RequesterPatronGroup", "requester.patronGroup"),
                Global.GetCqlFilter(Request2s3RadGrid, "ProxyFirstName", "proxy.firstName"),
                Global.GetCqlFilter(Request2s3RadGrid, "ProxyLastName", "proxy.lastName"),
                Global.GetCqlFilter(Request2s3RadGrid, "ProxyMiddleName", "proxy.middleName"),
                Global.GetCqlFilter(Request2s3RadGrid, "ProxyBarcode", "proxy.barcode"),
                Global.GetCqlFilter(Request2s3RadGrid, "ProxyPatronGroup", "proxy.patronGroup"),
                Global.GetCqlFilter(Request2s3RadGrid, "FulfillmentPreference", "fulfillmentPreference"),
                Global.GetCqlFilter(Request2s3RadGrid, "DeliveryAddressType.Name", "deliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(Request2s3RadGrid, "RequestExpirationDate", "requestExpirationDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "HoldShelfExpirationDate", "holdShelfExpirationDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "PickupServicePoint.Name", "pickupServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Request2s3RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s3RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s3RadGrid, "PrintDetailsPrintCount", "printDetails.printCount"),
                Global.GetCqlFilter(Request2s3RadGrid, "PrintDetailsRequester.Username", "printDetails.requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s3RadGrid, "PrintDetailsIsPrinted", "printDetails.isPrinted"),
                Global.GetCqlFilter(Request2s3RadGrid, "PrintDetailsPrintEventDate", "printDetails.printEventDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate"),
                Global.GetCqlFilter(Request2s3RadGrid, "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber"),
                Global.GetCqlFilter(Request2s3RadGrid, "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix"),
                Global.GetCqlFilter(Request2s3RadGrid, "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix"),
                Global.GetCqlFilter(Request2s3RadGrid, "SearchIndexShelvingOrder", "searchIndex.shelvingOrder"),
                Global.GetCqlFilter(Request2s3RadGrid, "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName"),
                Global.GetCqlFilter(Request2s3RadGrid, "ItemLocationCode", "itemLocationCode")
            }.Where(s => s != null)));
            Request2s3RadGrid.DataSource = folioServiceContext.Request2s(where, Request2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2s3RadGrid.PageSize * Request2s3RadGrid.CurrentPageIndex, Request2s3RadGrid.PageSize, true);
            Request2s3RadGrid.VirtualItemCount = folioServiceContext.CountRequest2s(where);
            if (Request2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Request2s3RadGrid.AllowFilteringByColumn = Request2s3RadGrid.VirtualItemCount > 10;
                Request2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2s3RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Request2s4RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestLevel", "requestLevel" }, { "RequestType", "requestType" }, { "EcsRequestPhase", "ecsRequestPhase" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "InstanceId", "instanceId" }, { "HoldingId", "holdingsRecordId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "InstanceTitle", "instance.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfillmentPreference", "fulfillmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "PrintDetailsPrintCount", "printDetails.printCount" }, { "PrintDetailsRequesterId", "printDetails.requesterId" }, { "PrintDetailsIsPrinted", "printDetails.isPrinted" }, { "PrintDetailsPrintEventDate", "printDetails.printEventDate" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" }, { "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber" }, { "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix" }, { "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix" }, { "SearchIndexShelvingOrder", "searchIndex.shelvingOrder" }, { "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName" }, { "ItemLocationCode", "itemLocationCode" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"requesterId == \"{id}\"",
                Global.GetCqlFilter(Request2s4RadGrid, "Id", "id"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequestLevel", "requestLevel"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequestType", "requestType"),
                Global.GetCqlFilter(Request2s4RadGrid, "EcsRequestPhase", "ecsRequestPhase"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequestDate", "requestDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "PatronComments", "patronComments"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s4RadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(Request2s4RadGrid, "Holding.ShortId", "holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(Request2s4RadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Request2s4RadGrid, "Status", "status"),
                Global.GetCqlFilter(Request2s4RadGrid, "CancellationReason.Name", "cancellationReasonId", "name", folioServiceContext.FolioServiceClient.CancellationReasons),
                Global.GetCqlFilter(Request2s4RadGrid, "CancelledByUser.Username", "cancelledByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s4RadGrid, "CancellationAdditionalInformation", "cancellationAdditionalInformation"),
                Global.GetCqlFilter(Request2s4RadGrid, "CancelledDate", "cancelledDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "Position", "position"),
                Global.GetCqlFilter(Request2s4RadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(Request2s4RadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequesterFirstName", "requester.firstName"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequesterLastName", "requester.lastName"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequesterMiddleName", "requester.middleName"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequesterBarcode", "requester.barcode"),
                Global.GetCqlFilter(Request2s4RadGrid, "RequesterPatronGroup", "requester.patronGroup"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyFirstName", "proxy.firstName"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyLastName", "proxy.lastName"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyMiddleName", "proxy.middleName"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyBarcode", "proxy.barcode"),
                Global.GetCqlFilter(Request2s4RadGrid, "ProxyPatronGroup", "proxy.patronGroup"),
                Global.GetCqlFilter(Request2s4RadGrid, "FulfillmentPreference", "fulfillmentPreference"),
                Global.GetCqlFilter(Request2s4RadGrid, "DeliveryAddressType.Name", "deliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(Request2s4RadGrid, "RequestExpirationDate", "requestExpirationDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "HoldShelfExpirationDate", "holdShelfExpirationDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "PickupServicePoint.Name", "pickupServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Request2s4RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s4RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s4RadGrid, "PrintDetailsPrintCount", "printDetails.printCount"),
                Global.GetCqlFilter(Request2s4RadGrid, "PrintDetailsRequester.Username", "printDetails.requesterId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Request2s4RadGrid, "PrintDetailsIsPrinted", "printDetails.isPrinted"),
                Global.GetCqlFilter(Request2s4RadGrid, "PrintDetailsPrintEventDate", "printDetails.printEventDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate"),
                Global.GetCqlFilter(Request2s4RadGrid, "SearchIndexCallNumberComponentsCallNumber", "searchIndex.callNumberComponents.callNumber"),
                Global.GetCqlFilter(Request2s4RadGrid, "SearchIndexCallNumberComponentsPrefix", "searchIndex.callNumberComponents.prefix"),
                Global.GetCqlFilter(Request2s4RadGrid, "SearchIndexCallNumberComponentsSuffix", "searchIndex.callNumberComponents.suffix"),
                Global.GetCqlFilter(Request2s4RadGrid, "SearchIndexShelvingOrder", "searchIndex.shelvingOrder"),
                Global.GetCqlFilter(Request2s4RadGrid, "SearchIndexPickupServicePointName", "searchIndex.pickupServicePointName"),
                Global.GetCqlFilter(Request2s4RadGrid, "ItemLocationCode", "itemLocationCode")
            }.Where(s => s != null)));
            Request2s4RadGrid.DataSource = folioServiceContext.Request2s(where, Request2s4RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2s4RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2s4RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2s4RadGrid.PageSize * Request2s4RadGrid.CurrentPageIndex, Request2s4RadGrid.PageSize, true);
            Request2s4RadGrid.VirtualItemCount = folioServiceContext.CountRequest2s(where);
            if (Request2s4RadGrid.MasterTableView.FilterExpression == "")
            {
                Request2s4RadGrid.AllowFilteringByColumn = Request2s4RadGrid.VirtualItemCount > 10;
                Request2s4Panel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2s4RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "SessionId", "sessionId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"recipientUserId == \"{id}\"",
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Loan.Id", "loanId", "id", folioServiceContext.FolioServiceClient.Loans),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Request.Id", "requestId", "id", folioServiceContext.FolioServiceClient.Requests),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "Payment.Id", "feeFineActionId", "id", folioServiceContext.FolioServiceClient.Payments),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "SessionId", "sessionId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NextRunTime", "nextRunTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "TriggeringEvent", "triggeringEvent"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTiming", "noticeConfig.timing"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigTemplate.Name", "noticeConfig.templateId", "name", folioServiceContext.FolioServiceClient.Templates),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigFormat", "noticeConfig.format"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ScheduledNotice2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(where, ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = folioServiceContext.CountScheduledNotice2s(where);
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = User2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ServicePointUser2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ServicePointUser2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "DefaultServicePointId", "defaultServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "DefaultServicePoint.Name", "defaultServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ServicePointUser2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ServicePointUser2s2RadGrid.DataSource = folioServiceContext.ServicePointUser2s(where, ServicePointUser2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePointUser2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePointUser2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ServicePointUser2s2RadGrid.PageSize * ServicePointUser2s2RadGrid.CurrentPageIndex, ServicePointUser2s2RadGrid.PageSize, true);
            ServicePointUser2s2RadGrid.VirtualItemCount = folioServiceContext.CountServicePointUser2s(where);
            if (ServicePointUser2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                ServicePointUser2s2RadGrid.AllowFilteringByColumn = ServicePointUser2s2RadGrid.VirtualItemCount > 10;
                ServicePointUser2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["ServicePointUser2sPermission"] != null && ServicePointUser2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void UserAcquisitionsUnit2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserAcquisitionsUnit2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "AcquisitionsUnitId", "acquisitionsUnitId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "AcquisitionsUnit.Name", "acquisitionsUnitId", "name", folioServiceContext.FolioServiceClient.AcquisitionsUnits),
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            UserAcquisitionsUnit2s2RadGrid.DataSource = folioServiceContext.UserAcquisitionsUnit2s(where, UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserAcquisitionsUnit2s2RadGrid.PageSize * UserAcquisitionsUnit2s2RadGrid.CurrentPageIndex, UserAcquisitionsUnit2s2RadGrid.PageSize, true);
            UserAcquisitionsUnit2s2RadGrid.VirtualItemCount = folioServiceContext.CountUserAcquisitionsUnit2s(where);
            if (UserAcquisitionsUnit2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                UserAcquisitionsUnit2s2RadGrid.AllowFilteringByColumn = UserAcquisitionsUnit2s2RadGrid.VirtualItemCount > 10;
                UserAcquisitionsUnit2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["UserAcquisitionsUnit2sPermission"] != null && UserAcquisitionsUnit2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void UserAddressesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserAddressesPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindUser2(id, true).UserAddresses ?? new UserAddress[] { };
            UserAddressesRadGrid.DataSource = l;
            UserAddressesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            UserAddressesPanel.Visible = User2FormView.DataKey.Value != null && ((string)Session["UserAddressesPermission"] == "Edit" || Session["UserAddressesPermission"] != null && l.Any());
        }

        protected void UserDepartmentsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserDepartmentsPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindUser2(id, true).UserDepartments ?? new UserDepartment[] { };
            UserDepartmentsRadGrid.DataSource = l;
            UserDepartmentsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            UserDepartmentsPanel.Visible = User2FormView.DataKey.Value != null && ((string)Session["UserDepartmentsPermission"] == "Edit" || Session["UserDepartmentsPermission"] != null && l.Any());
        }

        protected void UserRequestPreference2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserRequestPreference2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "HoldShelf", "holdShelf" }, { "Delivery", "delivery" }, { "DefaultServicePointId", "defaultServicePointId" }, { "DefaultDeliveryAddressTypeId", "defaultDeliveryAddressTypeId" }, { "Fulfillment", "fulfillment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"userId == \"{id}\"",
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "Id", "id"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "HoldShelf", "holdShelf"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "Delivery", "delivery"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "DefaultServicePoint.Name", "defaultServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "DefaultDeliveryAddressType.Name", "defaultDeliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "Fulfillment", "fulfillment"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserRequestPreference2s2RadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            UserRequestPreference2s2RadGrid.DataSource = folioServiceContext.UserRequestPreference2s(where, UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserRequestPreference2s2RadGrid.PageSize * UserRequestPreference2s2RadGrid.CurrentPageIndex, UserRequestPreference2s2RadGrid.PageSize, true);
            UserRequestPreference2s2RadGrid.VirtualItemCount = folioServiceContext.CountUserRequestPreference2s(where);
            if (UserRequestPreference2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                UserRequestPreference2s2RadGrid.AllowFilteringByColumn = UserRequestPreference2s2RadGrid.VirtualItemCount > 10;
                UserRequestPreference2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["UserRequestPreference2sPermission"] != null && UserRequestPreference2s2RadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void UserTagsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserTagsPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindUser2(id, true).UserTags ?? new UserTag[] { };
            UserTagsRadGrid.DataSource = l;
            UserTagsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            UserTagsPanel.Visible = User2FormView.DataKey.Value != null && ((string)Session["UserTagsPermission"] == "Edit" || Session["UserTagsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
