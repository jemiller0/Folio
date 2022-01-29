using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.AcquisitionsUnit2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AcquisitionsUnit2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void AcquisitionsUnit2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var au2 = folioServiceContext.FindAcquisitionsUnit2(id, true);
            if (au2 == null) Response.Redirect("Default.aspx");
            au2.Content = au2.Content != null ? JsonConvert.DeserializeObject<JToken>(au2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            AcquisitionsUnit2FormView.DataSource = new[] { au2 };
            Title = $"Acquisitions Unit {au2.Name}";
        }

        protected void UserAcquisitionsUnit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["UserAcquisitionsUnit2sPermission"] == null) return;
            var id = (Guid?)AcquisitionsUnit2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "AcquisitionsUnitId", "acquisitionsUnitId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"acquisitionsUnitId == \"{id}\"",
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserAcquisitionsUnit2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            UserAcquisitionsUnit2sRadGrid.DataSource = folioServiceContext.UserAcquisitionsUnit2s(out var i, where, UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserAcquisitionsUnit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserAcquisitionsUnit2sRadGrid.PageSize * UserAcquisitionsUnit2sRadGrid.CurrentPageIndex, UserAcquisitionsUnit2sRadGrid.PageSize, true);
            UserAcquisitionsUnit2sRadGrid.VirtualItemCount = i;
            if (UserAcquisitionsUnit2sRadGrid.MasterTableView.FilterExpression == "")
            {
                UserAcquisitionsUnit2sRadGrid.AllowFilteringByColumn = UserAcquisitionsUnit2sRadGrid.VirtualItemCount > 10;
                UserAcquisitionsUnit2sPanel.Visible = AcquisitionsUnit2FormView.DataKey.Value != null && Session["UserAcquisitionsUnit2sPermission"] != null && UserAcquisitionsUnit2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
