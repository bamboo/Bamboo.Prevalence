<%@ Page Language="C#" %>
<%
	Session.Abandon();
	Response.Redirect("Categories.aspx");
%>