<%@ Import Namespace="Bamboo.Prevalence.Examples.UserLogin" %>
<%@ Import Namespace="Bamboo.Prevalence.Examples.UserLogin.Web" %>
<script runat="server">

protected UserLoginSystem UserLoginSystem
{	
	get
	{
		return UserLoginApplication.UserLoginSystem;
	}
}

void Page_Load(object sender, EventArgs e)
{
	if (Page.IsPostBack)
	{
		string username = Request["email"];
		string password = Request["password"];
    
		try
		{
			User user = UserLoginSystem.LogonUser(username, password);
			FormsAuthentication.RedirectFromLoginPage(user.Email, _checkRememberMe.Checked);
		}
		catch (ApplicationException x)
		{
			_message.InnerText = x.Message;
		}
	}
}
</script>
<html>
<head>
<title>Bamboo.Prevalence.Examples.UserLogin</title>
</head>
<body>
	<center>
	<div id="_message" runat="server" style="color: red; font-weight: bolder; text-align:center"></div>
	</center>

	<div align="center">If this is your first time here, try logging on with user: administrator,
	password: <empty>.
	</div>
    <form method="post" runat="server">
        <table width="60%" align="center">
            <tbody>
                <tr>
                    <td align="right">
                        Email: 
                    </td>
                    <td align="left">
                        <input type="text" size="20" name="email" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Password: 
                    </td>
                    <td align="left">
                        <input type="password" size="20" name="password" />
                    </td>
                </tr>
                <tr>
                    <td align="middle" colspan="2">
                        <asp:CheckBox id="_checkRememberMe" runat="server" Text="remember me"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td align="middle" colspan="2">
                        <input type="submit" value="Login" />
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
