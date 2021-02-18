using System;

namespace FolioWebApplication
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageLiteral.Text = (string)Session["ErrorMessage"];
            Session.Remove("ErrorMessage");
        }
    }
}
