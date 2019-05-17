
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.login"  %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<script src="jquery-1.4.1.js" type="text/javascript"></script>

<script type="text/javascript">

    function InitAjax() {

        var ajax = false;

        try {

            ajax = new ActiveXObject("Msxml2.XMLHTTP");

        }

        catch (e) {

            try {

                ajax = new ActiveXObject("Microsoft.XMLHTTP");

            }

            catch (E) {

                ajax = false;

            }

        }

        if (!ajax && typeof XMLHttpRequest != 'undefined') {

            ajax = new XMLHttpRequest();

        }

        return ajax;

    }

    function Login() {

        try {

            var strName = 'ccc\\userb';

            var strPWD = '123321';

            var location = 'http://xqx2012/blog';

            var ajax = new InitAjax();

            ajax.open('post', location, false, strName, strPWD);

            ajax.send();

            if (ajax.status == 200) {

                window.location.href = location;

            }

        }

        catch (e)

    { }

    }

</script>
    </head>
<body>
    <form id="form1" runat="server">
    <div>
<input type="button" value="登录" onclick="Login()" />
        </div>
        </form>
    </body>
    </html>