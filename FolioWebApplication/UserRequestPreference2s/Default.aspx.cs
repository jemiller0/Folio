using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.UserRequestPreference2s
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRequestPreference2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void UserRequestPreference2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "HoldShelf", "holdShelf" }, { "Delivery", "delivery" }, { "DefaultServicePointId", "defaultServicePointId" }, { "DefaultDeliveryAddressTypeId", "defaultDeliveryAddressTypeId" }, { "Fulfillment", "fulfillment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "HoldShelf", "holdShelf"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Delivery", "delivery"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "DefaultServicePoint.Name", "defaultServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "DefaultDeliveryAddressType.Name", "defaultDeliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Fulfillment", "fulfillment"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            UserRequestPreference2sRadGrid.DataSource = folioServiceContext.UserRequestPreference2s(out var i, where, UserRequestPreference2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserRequestPreference2sRadGrid.PageSize * UserRequestPreference2sRadGrid.CurrentPageIndex, UserRequestPreference2sRadGrid.PageSize, true);
            UserRequestPreference2sRadGrid.VirtualItemCount = i;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"UserRequestPreference2s.txt\"");
            Response.BufferOutput = false;
            Response.Write("Id\tUser\tUserId\tHoldShelf\tDelivery\tDefaultServicePoint\tDefaultServicePointId\tDefaultDeliveryAddressType\tDefaultDeliveryAddressTypeId\tFulfillment\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "HoldShelf", "holdShelf" }, { "Delivery", "delivery" }, { "DefaultServicePointId", "defaultServicePointId" }, { "DefaultDeliveryAddressTypeId", "defaultDeliveryAddressTypeId" }, { "Fulfillment", "fulfillment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "HoldShelf", "holdShelf"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Delivery", "delivery"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "DefaultServicePoint.Name", "defaultServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "DefaultDeliveryAddressType.Name", "defaultDeliveryAddressTypeId", "addressType", folioServiceContext.FolioServiceClient.AddressTypes),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "Fulfillment", "fulfillment"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(UserRequestPreference2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            foreach (var urp2 in folioServiceContext.UserRequestPreference2s(where, UserRequestPreference2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
                Response.Write($"{urp2.Id}\t{Global.TextEncode(urp2.User?.Username)}\t{urp2.UserId}\t{urp2.HoldShelf}\t{urp2.Delivery}\t{Global.TextEncode(urp2.DefaultServicePoint?.Name)}\t{urp2.DefaultServicePointId}\t{Global.TextEncode(urp2.DefaultDeliveryAddressType?.Name)}\t{urp2.DefaultDeliveryAddressTypeId}\t{Global.TextEncode(urp2.Fulfillment)}\t{urp2.CreationTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(urp2.CreationUser?.Username)}\t{urp2.CreationUserId}\t{urp2.LastWriteTime:M/d/yyyy HH:mm:ss}\t{Global.TextEncode(urp2.LastWriteUser?.Username)}\t{urp2.LastWriteUserId}\r\n");
            Response.End();
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
