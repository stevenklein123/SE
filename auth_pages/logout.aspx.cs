using System;
using System.Web;
using System.Web.UI;

namespace Project_Tracking.auth_pages
{
    public partial class logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            Session.RemoveAll();

            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = "";

                Response.Cookies["ASP.NET_SessionId"].Expires =
                    DateTime.Now.AddDays(-1);
            }

            Response.Write(@"
<!DOCTYPE html>
<html>
<head>
    <title>Logout</title>
</head>

<body>

<script>

    // CLEAR STORAGE
    localStorage.clear();

    sessionStorage.clear();

    // REMOVE CHATBASE
    if(window.chatbase){

        try{
            window.chatbase('reset');
        }
        catch(e){}
    }

    // REMOVE IFRAMES
    document.querySelectorAll('iframe')
        .forEach(x => x.remove());

    // REMOVE CHATBASE SCRIPTS
    document.querySelectorAll('script')
        .forEach(x => {

            if(x.src.includes('chatbase')){
                x.remove();
            }

        });

    // HARD REDIRECT
    window.location.replace('../auth_pages/login.aspx');

</script>

</body>
</html>
");
        }
    }
}