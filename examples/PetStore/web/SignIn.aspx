<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
	String login = Request.Form["login"] + "";
	String password = Request.Form["password"] + "";

    Account account = ((PetStore)Prevayler.system()).getAccount(login);
	if (account == null || !account.isPasswordCorrect(password)) 
	{
%>
<html>
<head>
<title>Prevayler Pet Store - Sign In</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Login / Password invalid. Please, <a href="javascript:history.back()">click here</a> to try again.</p>
</body>
</html>
<%
	}
	else 
	{
		String nextPage = Request.Form["nextPage"];
		SessionState.Account = account;
		Response.Redirect(nextPage);
	}
%>