<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<html>
<head><title>Presto Error Page</title></head>
<body>
<p>The following error happened:<br>
<% if (exception != null) {
	out.print(exception.getClass().getName() + " : ");
	out.print(exception.getMessage());
}
%>
</p>
<p>Please, <a href="javascript:history.back()">click here</a> to try again.</p>
</body>
</html>