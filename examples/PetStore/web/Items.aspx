<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<script runat="server">
Item[] items;
int categoryIndex;
int productIndex;
void Page_Load(Object sender, EventArgs e)
{
	categoryIndex = int.Parse(Request.QueryString["categoryId"]);
	productIndex = int.Parse(Request.QueryString["productId"]);
	items = ((PetStore)Prevayler.system()).getCategories()[categoryIndex].products[productIndex].items;
}
</script>
<html>
<head>
<title>Prevayler Pet Store - Items</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Items for this product</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<%	
	Item item;
	for(int i = 0; i < items.Length; i++){ 
		item = items[i];
%>		
		<p>
		<a href="Item.aspx?categoryId=<%=categoryIndex%>&productId=<%=productIndex%>&itemId=<%=i%>"><%=item.name%></a><br>
		<%=item.listPrice%><br>
		<%=item.description%><br>
		<a href="AddItem.aspx?categoryId=<%=categoryIndex%>&productId=<%=productIndex%>&itemId=<%=i%>">Add to cart</a>
		</p>
<%	
	}
%>
</body>
</html>