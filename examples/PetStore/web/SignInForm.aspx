<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<html>

<head>
<title>Prevayler Pet Store - Account Creation / Selection</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Sign-in</p>
<ps:Navigation id="ctrlNav" runat="server" />

<form method="POST" action="SignIn.aspx">
<p>
Login: <input type="text" name="login" value=""><br>
Password: <input type="password" name="password" value=""><br>
<input type="hidden" name="nextPage" value="<%=Request.QueryString["nextPage"]%>"/>
<input type="submit" name="button" value="Login">
</p>
</form>

<p>Create new Account</p>
<form method="POST" action="AccountCreateForm.aspx">
<p>
Login: <input type="text" name="login" value=""><br>
Password: <input type="password" name="password" value=""><br>
Confirm Password: <input type="password" name="confirm" value=""><br>
<input type="submit" name="button" value="Create">
</p>
</form>
</body>
</html>