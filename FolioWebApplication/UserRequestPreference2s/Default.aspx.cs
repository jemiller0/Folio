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
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            UserRequestPreference2sRadGrid.DataSource = folioServiceContext.UserRequestPreference2s(out var i, Global.GetCqlFilter(UserRequestPreference2sRadGrid, d), UserRequestPreference2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, UserRequestPreference2sRadGrid.PageSize * UserRequestPreference2sRadGrid.CurrentPageIndex, UserRequestPreference2sRadGrid.PageSize, true);
            UserRequestPreference2sRadGrid.VirtualItemCount = i;
        }

        protected void ExportLinkButton_Click(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 300;
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Charset = "utf-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"UserRequestPreference2s.txt\"");
            Response.BufferOutput = false;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "HoldShelf", "holdShelf" }, { "Delivery", "delivery" }, { "DefaultServicePointId", "defaultServicePointId" }, { "DefaultDeliveryAddressTypeId", "defaultDeliveryAddressTypeId" }, { "Fulfillment", "fulfillment" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            Response.Write("Id\tUser\tUserId\tHoldShelf\tDelivery\tDefaultServicePoint\tDefaultServicePointId\tDefaultDeliveryAddressType\tDefaultDeliveryAddressTypeId\tFulfillment\tCreationTime\tCreationUser\tCreationUserId\tLastWriteTime\tLastWriteUser\tLastWriteUserId\r\n");
            foreach (var urp2 in folioServiceContext.UserRequestPreference2s(Global.GetCqlFilter(UserRequestPreference2sRadGrid, d), UserRequestPreference2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(UserRequestPreference2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, load: true))
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
