using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.User2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
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

        protected void User2sRadGrid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "InitInsert") Response.Redirect("Edit.aspx");
        }

        protected void User2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Username", "username" }, { "ExternalSystemId", "externalSystemId" }, { "Barcode", "barcode" }, { "Active", "active" }, { "GroupId", "patronGroup" }, { "LastName", "personal.lastName" }, { "FirstName", "personal.firstName" }, { "MiddleName", "personal.middleName" }, { "PreferredFirstName", "personal.preferredFirstName" }, { "EmailAddress", "personal.email" }, { "PhoneNumber", "personal.phone" }, { "MobilePhoneNumber", "personal.mobilePhone" }, { "BirthDate", "personal.dateOfBirth" }, { "PreferredContactTypeId", "personal.preferredContactTypeId" }, { "StartDate", "enrollmentDate" }, { "EndDate", "expirationDate" }, { "Source", "customFields.source" }, { "CategoryCode", "customFields.category" }, { "CategoryId", "customFields.category_2" }, { "Status", "customFields.status" }, { "Statuses", "customFields.statuses" }, { "StaffStatus", "customFields.staffStatus" }, { "StaffPrivileges", "customFields.staffPrivileges" }, { "StaffDivision", "customFields.staffDivision" }, { "StaffDepartment", "customFields.staffDepartment" }, { "StudentId", "customFields.studentId" }, { "StudentStatus", "customFields.studentStatus" }, { "StudentRestriction", "customFields.studentRestriction" }, { "StudentDivision", "customFields.studentDivision" }, { "StudentDepartment", "customFields.studentDepartment" }, { "Deceased", "customFields.deceased" }, { "Collections", "customFields.collections" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(User2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(User2sRadGrid, "Username", "username"),
                Global.GetCqlFilter(User2sRadGrid, "ExternalSystemId", "externalSystemId"),
                Global.GetCqlFilter(User2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(User2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(User2sRadGrid, "Group.Name", "patronGroup", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(User2sRadGrid, "Name", ""),
                Global.GetCqlFilter(User2sRadGrid, "LastName", "personal.lastName"),
                Global.GetCqlFilter(User2sRadGrid, "FirstName", "personal.firstName"),
                Global.GetCqlFilter(User2sRadGrid, "MiddleName", "personal.middleName"),
                Global.GetCqlFilter(User2sRadGrid, "PreferredFirstName", "personal.preferredFirstName"),
                Global.GetCqlFilter(User2sRadGrid, "EmailAddress", "personal.email"),
                Global.GetCqlFilter(User2sRadGrid, "PhoneNumber", "personal.phone"),
                Global.GetCqlFilter(User2sRadGrid, "MobilePhoneNumber", "personal.mobilePhone"),
                Global.GetCqlFilter(User2sRadGrid, "BirthDate", "personal.dateOfBirth"),
                Global.GetCqlFilter(User2sRadGrid, "StartDate", "enrollmentDate"),
                Global.GetCqlFilter(User2sRadGrid, "EndDate", "expirationDate"),
                Global.GetCqlFilter(User2sRadGrid, "Source", "customFields.source"),
                Global.GetCqlFilter(User2sRadGrid, "CategoryCode", "customFields.category"),
                Global.GetCqlFilter(User2sRadGrid, "Status", "customFields.status"),
                Global.GetCqlFilter(User2sRadGrid, "Statuses", "customFields.statuses"),
                Global.GetCqlFilter(User2sRadGrid, "StaffStatus", "customFields.staffStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StaffPrivileges", "customFields.staffPrivileges"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDivision", "customFields.staffDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDepartment", "customFields.staffDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "StudentId", "customFields.studentId"),
                Global.GetCqlFilter(User2sRadGrid, "StudentStatus", "customFields.studentStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StudentRestriction", "customFields.studentRestriction"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDivision", "customFields.studentDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDepartment", "customFields.studentDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "Deceased", "customFields.deceased"),
                Global.GetCqlFilter(User2sRadGrid, "Collections", "customFields.collections"),
                Global.GetCqlFilter(User2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(User2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            User2sRadGrid.DataSource = folioServiceContext.User2s(out var i, where, User2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[User2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(User2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, User2sRadGrid.PageSize * User2sRadGrid.CurrentPageIndex, User2sRadGrid.PageSize, true);
            User2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"User2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tUsername\tExternalSystemId\tBarcode\tActive\tGroup\tGroupId\tName\tLastName\tFirstName\tMiddleName\tPreferredFirstName\tEmailAddress\tPhoneNumber\tMobilePhoneNumber\tBirthDate\tPreferredContactType\tPreferredContactTypeId\tStartDate\tEndDate\tSource\tCategoryCode\tCategory\tCategoryId\tStatus\tStatuses\tStaffStatus\tStaffPrivileges\tStaffDivision\tStaffDepartment\tStudentId\tStudentStatus\tStudentRestriction\tStudentDivision\tStudentDepartment\tDeceased\tCollections\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Username", "username" }, { "ExternalSystemId", "externalSystemId" }, { "Barcode", "barcode" }, { "Active", "active" }, { "GroupId", "patronGroup" }, { "LastName", "personal.lastName" }, { "FirstName", "personal.firstName" }, { "MiddleName", "personal.middleName" }, { "PreferredFirstName", "personal.preferredFirstName" }, { "EmailAddress", "personal.email" }, { "PhoneNumber", "personal.phone" }, { "MobilePhoneNumber", "personal.mobilePhone" }, { "BirthDate", "personal.dateOfBirth" }, { "PreferredContactTypeId", "personal.preferredContactTypeId" }, { "StartDate", "enrollmentDate" }, { "EndDate", "expirationDate" }, { "Source", "customFields.source" }, { "CategoryCode", "customFields.category" }, { "CategoryId", "customFields.category_2" }, { "Status", "customFields.status" }, { "Statuses", "customFields.statuses" }, { "StaffStatus", "customFields.staffStatus" }, { "StaffPrivileges", "customFields.staffPrivileges" }, { "StaffDivision", "customFields.staffDivision" }, { "StaffDepartment", "customFields.staffDepartment" }, { "StudentId", "customFields.studentId" }, { "StudentStatus", "customFields.studentStatus" }, { "StudentRestriction", "customFields.studentRestriction" }, { "StudentDivision", "customFields.studentDivision" }, { "StudentDepartment", "customFields.studentDepartment" }, { "Deceased", "customFields.deceased" }, { "Collections", "customFields.collections" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(User2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(User2sRadGrid, "Username", "username"),
                Global.GetCqlFilter(User2sRadGrid, "ExternalSystemId", "externalSystemId"),
                Global.GetCqlFilter(User2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(User2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(User2sRadGrid, "Group.Name", "patronGroup", "group", folioServiceContext.FolioServiceClient.Groups),
                Global.GetCqlFilter(User2sRadGrid, "Name", ""),
                Global.GetCqlFilter(User2sRadGrid, "LastName", "personal.lastName"),
                Global.GetCqlFilter(User2sRadGrid, "FirstName", "personal.firstName"),
                Global.GetCqlFilter(User2sRadGrid, "MiddleName", "personal.middleName"),
                Global.GetCqlFilter(User2sRadGrid, "PreferredFirstName", "personal.preferredFirstName"),
                Global.GetCqlFilter(User2sRadGrid, "EmailAddress", "personal.email"),
                Global.GetCqlFilter(User2sRadGrid, "PhoneNumber", "personal.phone"),
                Global.GetCqlFilter(User2sRadGrid, "MobilePhoneNumber", "personal.mobilePhone"),
                Global.GetCqlFilter(User2sRadGrid, "BirthDate", "personal.dateOfBirth"),
                Global.GetCqlFilter(User2sRadGrid, "StartDate", "enrollmentDate"),
                Global.GetCqlFilter(User2sRadGrid, "EndDate", "expirationDate"),
                Global.GetCqlFilter(User2sRadGrid, "Source", "customFields.source"),
                Global.GetCqlFilter(User2sRadGrid, "CategoryCode", "customFields.category"),
                Global.GetCqlFilter(User2sRadGrid, "Status", "customFields.status"),
                Global.GetCqlFilter(User2sRadGrid, "Statuses", "customFields.statuses"),
                Global.GetCqlFilter(User2sRadGrid, "StaffStatus", "customFields.staffStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StaffPrivileges", "customFields.staffPrivileges"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDivision", "customFields.staffDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDepartment", "customFields.staffDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "StudentId", "customFields.studentId"),
                Global.GetCqlFilter(User2sRadGrid, "StudentStatus", "customFields.studentStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StudentRestriction", "customFields.studentRestriction"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDivision", "customFields.studentDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDepartment", "customFields.studentDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "Deceased", "customFields.deceased"),
                Global.GetCqlFilter(User2sRadGrid, "Collections", "customFields.collections"),
                Global.GetCqlFilter(User2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(User2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var u2 in folioServiceContext.User2s(where, User2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[User2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(User2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{u2.Id}\t{Global.TextEncode(u2.Username)}\t{Global.TextEncode(u2.ExternalSystemId)}\t{Global.TextEncode(u2.Barcode)}\t{u2.Active}\t{Global.TextEncode(u2.Group?.Name)}\t{u2.GroupId}\t{Global.TextEncode(u2.Name)}\t{Global.TextEncode(u2.LastName)}\t{Global.TextEncode(u2.FirstName)}\t{Global.TextEncode(u2.MiddleName)}\t{Global.TextEncode(u2.PreferredFirstName)}\t{Global.TextEncode(u2.EmailAddress)}\t{Global.TextEncode(u2.PhoneNumber)}\t{Global.TextEncode(u2.MobilePhoneNumber)}\t{u2.BirthDate:M/d/yyyy}\t{Global.TextEncode(u2.PreferredContactType?.Name)}\t{Global.TextEncode(u2.PreferredContactTypeId)}\t{u2.StartDate:M/d/yyyy}\t{u2.EndDate:M/d/yyyy}\t{Global.TextEncode(u2.Source)}\t{Global.TextEncode(u2.CategoryCode)}\t{Global.TextEncode(u2.Category?.Name)}\t{Global.TextEncode(u2.CategoryId)}\t{Global.TextEncode(u2.Status)}\t{Global.TextEncode(u2.Statuses)}\t{Global.TextEncode(u2.StaffStatus)}\t{Global.TextEncode(u2.StaffPrivileges)}\t{Global.TextEncode(u2.StaffDivision)}\t{Global.TextEncode(u2.StaffDepartment)}\t{Global.TextEncode(u2.StudentId)}\t{Global.TextEncode(u2.StudentStatus)}\t{u2.StudentRestriction}\t{Global.TextEncode(u2.StudentDivision)}\t{Global.TextEncode(u2.StudentDepartment)}\t{u2.Deceased}\t{u2.Collections}\t{u2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(u2.CreationUser?.Username)}\t{u2.CreationUserId}\t{u2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(u2.LastWriteUser?.Username)}\t{u2.LastWriteUserId}\r\n");
            Response.End();
        }

        protected void User2sRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var gei = (GridEditableItem)e.Item;
            var id = (Guid?)gei.GetDataKeyValue("Id");
            try
            {
                if (folioServiceContext.AnyAcquisitionMethod2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisition method");
                if (folioServiceContext.AnyAcquisitionMethod2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisition method");
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.createdByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
                if (folioServiceContext.AnyAcquisitionsUnit2s($"metadata.updatedByUserId == \"{id}\"")) throw new Exception("User cannot be deleted because it is being referenced by a acquisitions unit");
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
                var cv = (CustomValidator)gei.FindControl("DeleteCustomValidator");
                cv.IsValid = false;
                e.Canceled = true;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
