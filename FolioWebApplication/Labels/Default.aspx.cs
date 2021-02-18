using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FolioWebApplication.Labels
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("Edit.aspx");
            //if (!IsPostBack)
            //{
            //    DataBind();
            //}
        }

        protected void FindRadButton_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), null, $"window.open('View.aspx?barcode={BarcodeRadTextBox.Text}', '_blank');", true);

            Session.Remove("Label");
            Response.Redirect("Edit.aspx?Barcode=" + BarcodeRadTextBox.Text);
        }
    }
}
