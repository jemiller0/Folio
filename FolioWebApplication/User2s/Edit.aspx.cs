using FolioLibrary;
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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            u2.StartDate = (DateTime?)e.NewValues["StartDate"];
            u2.EndDate = (DateTime?)e.NewValues["EndDate"];
            u2.Source = Global.Trim((string)e.NewValues["Source"]);
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
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
                if (folioServiceContext.AnyAddressType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a address type");
                if (folioServiceContext.AnyAddressType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a address type");
                if (folioServiceContext.AnyAlternativeTitleType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a alternative title type");
                if (folioServiceContext.AnyAlternativeTitleType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a alternative title type");
                if (folioServiceContext.AnyBatchGroup2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch group");
                if (folioServiceContext.AnyBatchGroup2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch group");
                if (folioServiceContext.AnyBatchVoucherExport2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch voucher export");
                if (folioServiceContext.AnyBatchVoucherExport2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch voucher export");
                if (folioServiceContext.AnyBatchVoucherExportConfig2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch voucher export config");
                if (folioServiceContext.AnyBatchVoucherExportConfig2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a batch voucher export config");
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
                if (folioServiceContext.AnyLedgerRollover2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover");
                if (folioServiceContext.AnyLedgerRollover2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover");
                if (folioServiceContext.AnyLedgerRolloverError2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover error");
                if (folioServiceContext.AnyLedgerRolloverError2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover error");
                if (folioServiceContext.AnyLedgerRolloverProgress2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover progress");
                if (folioServiceContext.AnyLedgerRolloverProgress2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a ledger rollover progress");
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
                if (folioServiceContext.AnyNote3s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note");
                if (folioServiceContext.AnyNote3s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note");
                if (folioServiceContext.AnyNoteType2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note type");
                if (folioServiceContext.AnyNoteType2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a note type");
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
                if (folioServiceContext.Permission2s().Any(cu => cu.CreationUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a permission");
                if (folioServiceContext.Permission2s().Any(lwu => lwu.LastWriteUserId == id)) throw new Exception("User cannot be deleted because it is being referenced by a permission");
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
                if (folioServiceContext.AnyRecord2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRecord2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a record");
                if (folioServiceContext.AnyRefundReason2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a refund reason");
                if (folioServiceContext.AnyRefundReason2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a refund reason");
                if (folioServiceContext.AnyRelationships($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationships($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship");
                if (folioServiceContext.AnyRelationshipTypes($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship type");
                if (folioServiceContext.AnyRelationshipTypes($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a relationship type");
                if (folioServiceContext.AnyRequest2s($"cancelledByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"proxyUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"requesterId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequest2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request");
                if (folioServiceContext.AnyRequestPolicy2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request policy");
                if (folioServiceContext.AnyRequestPolicy2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a request policy");
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
                if (folioServiceContext.AnyTag2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a tag");
                if (folioServiceContext.AnyTag2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a tag");
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
            Block2s2RadGrid.DataSource = folioServiceContext.Block2s(out var i, Global.GetCqlFilter(Block2s2RadGrid, d, $"userId == \"{id}\""), Block2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Block2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Block2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Block2s2RadGrid.PageSize * Block2s2RadGrid.CurrentPageIndex, Block2s2RadGrid.PageSize, true);
            Block2s2RadGrid.VirtualItemCount = i;
            if (Block2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Block2s2RadGrid.AllowFilteringByColumn = Block2s2RadGrid.VirtualItemCount > 10;
                Block2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["Block2sPermission"] != null && Block2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void CheckIn2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["CheckIn2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "OccurredDateTime", "occurredDateTime" }, { "ItemId", "itemId" }, { "ItemStatusPriorToCheckIn", "itemStatusPriorToCheckIn" }, { "RequestQueueSize", "requestQueueSize" }, { "ItemLocationId", "itemLocationId" }, { "ServicePointId", "servicePointId" }, { "PerformedByUserId", "performedByUserId" } };
            CheckIn2sRadGrid.DataSource = folioServiceContext.CheckIn2s(out var i, Global.GetCqlFilter(CheckIn2sRadGrid, d, $"performedByUserId == \"{id}\""), CheckIn2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[CheckIn2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(CheckIn2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, CheckIn2sRadGrid.PageSize * CheckIn2sRadGrid.CurrentPageIndex, CheckIn2sRadGrid.PageSize, true);
            CheckIn2sRadGrid.VirtualItemCount = i;
            if (CheckIn2sRadGrid.MasterTableView.FilterExpression == "")
            {
                CheckIn2sRadGrid.AllowFilteringByColumn = CheckIn2sRadGrid.VirtualItemCount > 10;
                CheckIn2sPanel.Visible = User2FormView.DataKey.Value != null && Session["CheckIn2sPermission"] != null && CheckIn2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Fee2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Fee2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Amount", "amount" }, { "RemainingAmount", "remaining" }, { "StatusName", "status.name" }, { "PaymentStatusName", "paymentStatus.name" }, { "Title", "title" }, { "CallNumber", "callNumber" }, { "Barcode", "barcode" }, { "MaterialType", "materialType" }, { "ItemStatusName", "itemStatus.name" }, { "Location", "location" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "DueTime", "dueDate" }, { "ReturnedTime", "returnedDate" }, { "LoanId", "loanId" }, { "UserId", "userId" }, { "ItemId", "itemId" }, { "MaterialTypeId", "materialTypeId" }, { "FeeTypeId", "feeFineId" }, { "OwnerId", "ownerId" }, { "HoldingId", "holdingsRecordId" }, { "InstanceId", "instanceId" } };
            Fee2s2RadGrid.DataSource = folioServiceContext.Fee2s(out var i, Global.GetCqlFilter(Fee2s2RadGrid, d, $"userId == \"{id}\""), Fee2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Fee2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Fee2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Fee2s2RadGrid.PageSize * Fee2s2RadGrid.CurrentPageIndex, Fee2s2RadGrid.PageSize, true);
            Fee2s2RadGrid.VirtualItemCount = i;
            if (Fee2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Fee2s2RadGrid.AllowFilteringByColumn = Fee2s2RadGrid.VirtualItemCount > 10;
                Fee2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["Fee2sPermission"] != null && Fee2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Fee2s2Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Payment2sPermission"] == null) return;
            var rg = (RadGrid)sender;
            var id = (Guid?)((GridDataItem)rg.Parent.Parent).GetDataKeyValue("Id");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePointId", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            rg.DataSource = folioServiceContext.Payment2s(out var i, Global.GetCqlFilter(rg, d, $"accountId == \"{id}\""), rg.MasterTableView.SortExpressions.Count > 0 ? $"{d[rg.MasterTableView.SortExpressions[0].FieldName]}{(rg.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, rg.PageSize * rg.CurrentPageIndex, rg.PageSize, true);
            rg.VirtualItemCount = i;
            if (rg.MasterTableView.FilterExpression == "")
            {
                rg.AllowFilteringByColumn = rg.VirtualItemCount > 10;
            }
        }

        protected void Invoice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Invoice2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "AccountingCode", "accountingCode" }, { "AdjustmentsTotal", "adjustmentsTotal" }, { "ApprovedById", "approvedBy" }, { "ApprovalDate", "approvalDate" }, { "BatchGroupId", "batchGroupId" }, { "BillToId", "billTo" }, { "CheckSubscriptionOverlap", "chkSubscriptionOverlap" }, { "CancellationNote", "cancellationNote" }, { "Currency", "currency" }, { "EnclosureNeeded", "enclosureNeeded" }, { "ExchangeRate", "exchangeRate" }, { "ExportToAccounting", "exportToAccounting" }, { "Number", "folioInvoiceNo" }, { "InvoiceDate", "invoiceDate" }, { "LockTotal", "lockTotal" }, { "Note", "note" }, { "PaymentDueDate", "paymentDue" }, { "PaymentDate", "paymentDate" }, { "PaymentTerms", "paymentTerms" }, { "PaymentMethod", "paymentMethod" }, { "Status", "status" }, { "Source", "source" }, { "SubTotal", "subTotal" }, { "Total", "total" }, { "VendorInvoiceNo", "vendorInvoiceNo" }, { "DisbursementNumber", "disbursementNumber" }, { "VoucherNumber", "voucherNumber" }, { "PaymentId", "paymentId" }, { "DisbursementDate", "disbursementDate" }, { "VendorId", "vendorId" }, { "AccountNumber", "accountNo" }, { "ManualPayment", "manualPayment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Invoice2sRadGrid.DataSource = folioServiceContext.Invoice2s(out var i, Global.GetCqlFilter(Invoice2sRadGrid, d, $"approvedBy == \"{id}\""), Invoice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Invoice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Invoice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Invoice2sRadGrid.PageSize * Invoice2sRadGrid.CurrentPageIndex, Invoice2sRadGrid.PageSize, true);
            Invoice2sRadGrid.VirtualItemCount = i;
            if (Invoice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Invoice2sRadGrid.AllowFilteringByColumn = Invoice2sRadGrid.VirtualItemCount > 10;
                Invoice2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Invoice2sPermission"] != null && Invoice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Loan2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2s1RadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2s1RadGrid, d, $"proxyUserId == \"{id}\""), Loan2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2s1RadGrid.PageSize * Loan2s1RadGrid.CurrentPageIndex, Loan2s1RadGrid.PageSize, true);
            Loan2s1RadGrid.VirtualItemCount = i;
            if (Loan2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2s1RadGrid.AllowFilteringByColumn = Loan2s1RadGrid.VirtualItemCount > 10;
                Loan2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Loan2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2s3RadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2s3RadGrid, d, $"userId == \"{id}\""), Loan2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2s3RadGrid.PageSize * Loan2s3RadGrid.CurrentPageIndex, Loan2s3RadGrid.PageSize, true);
            Loan2s3RadGrid.VirtualItemCount = i;
            if (Loan2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2s3RadGrid.AllowFilteringByColumn = Loan2s3RadGrid.VirtualItemCount > 10;
                Loan2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2s3RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Order2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2sRadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2sRadGrid, d, $"approvedById == \"{id}\""), Order2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2sRadGrid.PageSize * Order2sRadGrid.CurrentPageIndex, Order2sRadGrid.PageSize, true);
            Order2sRadGrid.VirtualItemCount = i;
            if (Order2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Order2sRadGrid.AllowFilteringByColumn = Order2sRadGrid.VirtualItemCount > 10;
                Order2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Order2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Order2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Approved", "approved" }, { "ApprovedById", "approvedById" }, { "ApprovalDate", "approvalDate" }, { "AssignedToId", "assignedTo" }, { "BillToId", "billTo" }, { "CloseReasonReason", "closeReason.reason" }, { "CloseReasonNote", "closeReason.note" }, { "OrderDate", "dateOrdered" }, { "Manual", "manualPo" }, { "Number", "poNumber" }, { "OrderType", "orderType" }, { "Reencumber", "reEncumber" }, { "OngoingInterval", "ongoing.interval" }, { "OngoingIsSubscription", "ongoing.isSubscription" }, { "OngoingManualRenewal", "ongoing.manualRenewal" }, { "OngoingNotes", "ongoing.notes" }, { "OngoingReviewPeriod", "ongoing.reviewPeriod" }, { "OngoingRenewalDate", "ongoing.renewalDate" }, { "OngoingReviewDate", "ongoing.reviewDate" }, { "ShipToId", "shipTo" }, { "TemplateId", "template" }, { "VendorId", "vendor" }, { "Status", "workflowStatus" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Order2s1RadGrid.DataSource = folioServiceContext.Order2s(out var i, Global.GetCqlFilter(Order2s1RadGrid, d, $"assignedTo == \"{id}\""), Order2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Order2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Order2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Order2s1RadGrid.PageSize * Order2s1RadGrid.CurrentPageIndex, Order2s1RadGrid.PageSize, true);
            Order2s1RadGrid.VirtualItemCount = i;
            if (Order2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Order2s1RadGrid.AllowFilteringByColumn = Order2s1RadGrid.VirtualItemCount > 10;
                Order2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Order2sPermission"] != null && Order2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void PatronActionSession2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PatronActionSession2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "PatronId", "patronId" }, { "LoanId", "loanId" }, { "ActionType", "actionType" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PatronActionSession2sRadGrid.DataSource = folioServiceContext.PatronActionSession2s(out var i, Global.GetCqlFilter(PatronActionSession2sRadGrid, d, $"patronId == \"{id}\""), PatronActionSession2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PatronActionSession2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PatronActionSession2sRadGrid.PageSize * PatronActionSession2sRadGrid.CurrentPageIndex, PatronActionSession2sRadGrid.PageSize, true);
            PatronActionSession2sRadGrid.VirtualItemCount = i;
            if (PatronActionSession2sRadGrid.MasterTableView.FilterExpression == "")
            {
                PatronActionSession2sRadGrid.AllowFilteringByColumn = PatronActionSession2sRadGrid.VirtualItemCount > 10;
                PatronActionSession2sPanel.Visible = User2FormView.DataKey.Value != null && Session["PatronActionSession2sPermission"] != null && PatronActionSession2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Payment2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Payment2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "CreationTime", "dateAction" }, { "TypeAction", "typeAction" }, { "Comments", "comments" }, { "Notify", "notify" }, { "Amount", "amountAction" }, { "RemainingAmount", "balance" }, { "TransactionInformation", "transactionInformation" }, { "ServicePointId", "createdAt" }, { "Source", "source" }, { "PaymentMethod", "paymentMethod" }, { "FeeId", "accountId" }, { "UserId", "userId" } };
            Payment2sRadGrid.DataSource = folioServiceContext.Payment2s(out var i, Global.GetCqlFilter(Payment2sRadGrid, d, $"userId == \"{id}\""), Payment2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Payment2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Payment2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Payment2sRadGrid.PageSize * Payment2sRadGrid.CurrentPageIndex, Payment2sRadGrid.PageSize, true);
            Payment2sRadGrid.VirtualItemCount = i;
            if (Payment2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Payment2sRadGrid.AllowFilteringByColumn = Payment2sRadGrid.VirtualItemCount > 10;
                Payment2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Payment2sPermission"] != null && Payment2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void PermissionsUser2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["PermissionsUser2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            PermissionsUser2s2RadGrid.DataSource = folioServiceContext.PermissionsUser2s(out var i, Global.GetCqlFilter(PermissionsUser2s2RadGrid, d, $"userId == \"{id}\""), PermissionsUser2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[PermissionsUser2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(PermissionsUser2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, PermissionsUser2s2RadGrid.PageSize * PermissionsUser2s2RadGrid.CurrentPageIndex, PermissionsUser2s2RadGrid.PageSize, true);
            PermissionsUser2s2RadGrid.VirtualItemCount = i;
            if (PermissionsUser2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                PermissionsUser2s2RadGrid.AllowFilteringByColumn = PermissionsUser2s2RadGrid.VirtualItemCount > 10;
                PermissionsUser2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["PermissionsUser2sPermission"] != null && PermissionsUser2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Proxy2s1RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Proxy2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Proxy2s1RadGrid.DataSource = folioServiceContext.Proxy2s(out var i, Global.GetCqlFilter(Proxy2s1RadGrid, d, $"proxyUserId == \"{id}\""), Proxy2s1RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2s1RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2s1RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Proxy2s1RadGrid.PageSize * Proxy2s1RadGrid.CurrentPageIndex, Proxy2s1RadGrid.PageSize, true);
            Proxy2s1RadGrid.VirtualItemCount = i;
            if (Proxy2s1RadGrid.MasterTableView.FilterExpression == "")
            {
                Proxy2s1RadGrid.AllowFilteringByColumn = Proxy2s1RadGrid.VirtualItemCount > 10;
                Proxy2s1Panel.Visible = User2FormView.DataKey.Value != null && Session["Proxy2sPermission"] != null && Proxy2s1RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Proxy2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Proxy2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "RequestForSponsor", "requestForSponsor" }, { "NotificationsTo", "notificationsTo" }, { "AccrueTo", "accrueTo" }, { "Status", "status" }, { "ExpirationDate", "expirationDate" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Proxy2s3RadGrid.DataSource = folioServiceContext.Proxy2s(out var i, Global.GetCqlFilter(Proxy2s3RadGrid, d, $"userId == \"{id}\""), Proxy2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Proxy2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Proxy2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Proxy2s3RadGrid.PageSize * Proxy2s3RadGrid.CurrentPageIndex, Proxy2s3RadGrid.PageSize, true);
            Proxy2s3RadGrid.VirtualItemCount = i;
            if (Proxy2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Proxy2s3RadGrid.AllowFilteringByColumn = Proxy2s3RadGrid.VirtualItemCount > 10;
                Proxy2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Proxy2sPermission"] != null && Proxy2s3RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Request2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Request2sRadGrid.DataSource = folioServiceContext.Request2s(out var i, Global.GetCqlFilter(Request2sRadGrid, d, $"cancelledByUserId == \"{id}\""), Request2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2sRadGrid.PageSize * Request2sRadGrid.CurrentPageIndex, Request2sRadGrid.PageSize, true);
            Request2sRadGrid.VirtualItemCount = i;
            if (Request2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Request2sRadGrid.AllowFilteringByColumn = Request2sRadGrid.VirtualItemCount > 10;
                Request2sPanel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Request2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Request2s2RadGrid.DataSource = folioServiceContext.Request2s(out var i, Global.GetCqlFilter(Request2s2RadGrid, d, $"proxyUserId == \"{id}\""), Request2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2s2RadGrid.PageSize * Request2s2RadGrid.CurrentPageIndex, Request2s2RadGrid.PageSize, true);
            Request2s2RadGrid.VirtualItemCount = i;
            if (Request2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                Request2s2RadGrid.AllowFilteringByColumn = Request2s2RadGrid.VirtualItemCount > 10;
                Request2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void Request2s3RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Request2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "RequestType", "requestType" }, { "RequestDate", "requestDate" }, { "PatronComments", "patronComments" }, { "RequesterId", "requesterId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "Status", "status" }, { "CancellationReasonId", "cancellationReasonId" }, { "CancelledByUserId", "cancelledByUserId" }, { "CancellationAdditionalInformation", "cancellationAdditionalInformation" }, { "CancelledDate", "cancelledDate" }, { "Position", "position" }, { "ItemTitle", "item.title" }, { "ItemBarcode", "item.barcode" }, { "RequesterFirstName", "requester.firstName" }, { "RequesterLastName", "requester.lastName" }, { "RequesterMiddleName", "requester.middleName" }, { "RequesterBarcode", "requester.barcode" }, { "RequesterPatronGroup", "requester.patronGroup" }, { "ProxyFirstName", "proxy.firstName" }, { "ProxyLastName", "proxy.lastName" }, { "ProxyMiddleName", "proxy.middleName" }, { "ProxyBarcode", "proxy.barcode" }, { "ProxyPatronGroup", "proxy.patronGroup" }, { "FulfilmentPreference", "fulfilmentPreference" }, { "DeliveryAddressTypeId", "deliveryAddressTypeId" }, { "RequestExpirationDate", "requestExpirationDate" }, { "HoldShelfExpirationDate", "holdShelfExpirationDate" }, { "PickupServicePointId", "pickupServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AwaitingPickupRequestClosedDate", "awaitingPickupRequestClosedDate" } };
            Request2s3RadGrid.DataSource = folioServiceContext.Request2s(out var i, Global.GetCqlFilter(Request2s3RadGrid, d, $"requesterId == \"{id}\""), Request2s3RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Request2s3RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Request2s3RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Request2s3RadGrid.PageSize * Request2s3RadGrid.CurrentPageIndex, Request2s3RadGrid.PageSize, true);
            Request2s3RadGrid.VirtualItemCount = i;
            if (Request2s3RadGrid.MasterTableView.FilterExpression == "")
            {
                Request2s3RadGrid.AllowFilteringByColumn = Request2s3RadGrid.VirtualItemCount > 10;
                Request2s3Panel.Visible = User2FormView.DataKey.Value != null && Session["Request2sPermission"] != null && Request2s3RadGrid.VirtualItemCount > 0;
            }
        }

        protected void ScheduledNotice2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ScheduledNotice2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LoanId", "loanId" }, { "RequestId", "requestId" }, { "PaymentId", "feeFineActionId" }, { "RecipientUserId", "recipientUserId" }, { "NextRunTime", "nextRunTime" }, { "TriggeringEvent", "triggeringEvent" }, { "NoticeConfigTiming", "noticeConfig.timing" }, { "NoticeConfigRecurringPeriodDuration", "noticeConfig.recurringPeriod.duration" }, { "NoticeConfigRecurringPeriodInterval", "noticeConfig.recurringPeriod.intervalId" }, { "NoticeConfigTemplateId", "noticeConfig.templateId" }, { "NoticeConfigFormat", "noticeConfig.format" }, { "NoticeConfigSendInRealTime", "noticeConfig.sendInRealTime" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ScheduledNotice2sRadGrid.DataSource = folioServiceContext.ScheduledNotice2s(out var i, Global.GetCqlFilter(ScheduledNotice2sRadGrid, d, $"recipientUserId == \"{id}\""), ScheduledNotice2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ScheduledNotice2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ScheduledNotice2sRadGrid.PageSize * ScheduledNotice2sRadGrid.CurrentPageIndex, ScheduledNotice2sRadGrid.PageSize, true);
            ScheduledNotice2sRadGrid.VirtualItemCount = i;
            if (ScheduledNotice2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ScheduledNotice2sRadGrid.AllowFilteringByColumn = ScheduledNotice2sRadGrid.VirtualItemCount > 10;
                ScheduledNotice2sPanel.Visible = User2FormView.DataKey.Value != null && Session["ScheduledNotice2sPermission"] != null && ScheduledNotice2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void ServicePointUser2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ServicePointUser2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "DefaultServicePointId", "defaultServicePointId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            ServicePointUser2s2RadGrid.DataSource = folioServiceContext.ServicePointUser2s(out var i, Global.GetCqlFilter(ServicePointUser2s2RadGrid, d, $"userId == \"{id}\""), ServicePointUser2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ServicePointUser2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ServicePointUser2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ServicePointUser2s2RadGrid.PageSize * ServicePointUser2s2RadGrid.CurrentPageIndex, ServicePointUser2s2RadGrid.PageSize, true);
            ServicePointUser2s2RadGrid.VirtualItemCount = i;
            if (ServicePointUser2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                ServicePointUser2s2RadGrid.AllowFilteringByColumn = ServicePointUser2s2RadGrid.VirtualItemCount > 10;
                ServicePointUser2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["ServicePointUser2sPermission"] != null && ServicePointUser2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void UserAcquisitionsUnit2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserAcquisitionsUnit2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "AcquisitionsUnitId", "acquisitionsUnitId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            UserAcquisitionsUnit2s2RadGrid.DataSource = folioServiceContext.UserAcquisitionsUnit2s(out var i, Global.GetCqlFilter(UserAcquisitionsUnit2s2RadGrid, d, $"userId == \"{id}\""), UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserAcquisitionsUnit2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserAcquisitionsUnit2s2RadGrid.PageSize * UserAcquisitionsUnit2s2RadGrid.CurrentPageIndex, UserAcquisitionsUnit2s2RadGrid.PageSize, true);
            UserAcquisitionsUnit2s2RadGrid.VirtualItemCount = i;
            if (UserAcquisitionsUnit2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                UserAcquisitionsUnit2s2RadGrid.AllowFilteringByColumn = UserAcquisitionsUnit2s2RadGrid.VirtualItemCount > 10;
                UserAcquisitionsUnit2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["UserAcquisitionsUnit2sPermission"] != null && UserAcquisitionsUnit2s2RadGrid.VirtualItemCount > 0;
            }
        }

        protected void UserRequestPreference2s2RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserRequestPreference2sPermission"] == null) return;
            var id = (Guid?)User2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "HoldShelf", "holdShelf" }, { "Delivery", "delivery" }, { "DefaultServicePointId", "defaultServicePointId" }, { "DefaultDeliveryAddressTypeId", "defaultDeliveryAddressTypeId" }, { "Fulfillment", "fulfillment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            UserRequestPreference2s2RadGrid.DataSource = folioServiceContext.UserRequestPreference2s(out var i, Global.GetCqlFilter(UserRequestPreference2s2RadGrid, d, $"userId == \"{id}\""), UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2s2RadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserRequestPreference2s2RadGrid.PageSize * UserRequestPreference2s2RadGrid.CurrentPageIndex, UserRequestPreference2s2RadGrid.PageSize, true);
            UserRequestPreference2s2RadGrid.VirtualItemCount = i;
            if (UserRequestPreference2s2RadGrid.MasterTableView.FilterExpression == "")
            {
                UserRequestPreference2s2RadGrid.AllowFilteringByColumn = UserRequestPreference2s2RadGrid.VirtualItemCount > 10;
                UserRequestPreference2s2Panel.Visible = User2FormView.DataKey.Value != null && Session["UserRequestPreference2sPermission"] != null && UserRequestPreference2s2RadGrid.VirtualItemCount > 0;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
