<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<script runat="server">
Category[] categories = ((PetStore)Prevayler.system()).getCategories();
</script>
<html>
<head>
<title>Bamboo.Prevalence Pet Store - Categories</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Category</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<p>
<%	
    Category category;
    for(int i = 0; i < categories.Length; i++){ 
		category = categories[i];
%>	
		<a href="Products.aspx?categoryId=<%=i%>"><%=category.name%></a><br />			
<%	
    }
%>
</p>
</body>
</html>