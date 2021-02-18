using System;
using System.Web.Security;
using System.Web.UI;

namespace FolioWebApplication
{
    public partial class Default1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.Web.Security.Roles.GetRolesForUser().Length == 0)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack)
            {
                DataBind();
            }
        }
    }
}
