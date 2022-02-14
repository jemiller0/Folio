using System;
using Telerik.Web.UI;

namespace FolioWebApplication
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var rmi = RadMenu2.Items.FindItemByText("Inventory");
                if (Session["LabelsPermission"] != null)
                {
                    rmi.Visible = true;
                    rmi.Items.Insert(rmi.Items.IndexOf(rmi.Items.FindItemByText("Libraries")), new RadMenuItem("Labels", "~/Labels/Default.aspx"));
                }
            }
        }

        protected void RadScriptManager1_AsyncPostBackError(object sender, System.Web.UI.AsyncPostBackErrorEventArgs e)
        {
            //RadScriptManager1.AsyncPostBackErrorMessage = (e.Exception.InnerException?.Message ?? e.Exception.Message).Replace("A task was canceled.", "Request timed out.");
            RadScriptManager1.AsyncPostBackErrorMessage = (e.Exception.InnerException ?? e.Exception).ToString();
            //Server.ClearError();
        }
    }
}
