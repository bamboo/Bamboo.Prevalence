<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<script runat="server">
Product[] products;
int categoryIndex;
void Page_Load(Object sender, EventArgs e)
{
	categoryIndex = int.Parse(Request.QueryString["categoryId"]);
	products = ((PetStore)Prevayler.system()).getCategories()[categoryIndex].products;
}
</script>
<html>

<head>
<title>Prevayler Pet Store - Products</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>
<body>
<p>Products for this category</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<%	 
	Product product;
	for(int i = 0; i < products.Length; i++){ 
		product = products[i];
%>
		<p>
		<a href="Items.aspx?categoryId=<%=categoryIndex%>&productId=<%=i%>"><%=product.name%></a><br>			
		<%=product.description%>
		</p>
<%	
	}
%>
</body>
</html>