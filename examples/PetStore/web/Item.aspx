<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<script runat="server">
Item item;
int categoryIndex;
int productIndex;
int itemIndex;
void Page_Load(Object sender, EventArgs e)
{
	categoryIndex = int.Parse(Request.QueryString["categoryId"]);
	productIndex = int.Parse(Request.QueryString["productId"]);
	itemIndex = int.Parse(Request.QueryString["itemId"]);
	item = ((PetStore)Prevayler.system()).getCategories()[categoryIndex].products[productIndex].items[itemIndex];
}
</script>
<html>

<head>
<title>Prevayler Pet Store - Item</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Item</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<p><%=item.name%></p>
<p><%=item.description%><br>
<%=item.listPrice%><br>
<%=item.yourPrice%><br>
<img src="<%=item.photo%>"><br>
		<a href="AddItem.aspx?categoryId=<%=categoryIndex%>&productId=<%=productIndex%>&itemId=<%=itemIndex%>">Add to cart</a>
</p>
</body>
</html>