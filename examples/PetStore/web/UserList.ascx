<%@ Control Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%
Account listAccount = SessionState.Account;

if (listAccount != null && listAccount.isListFeatureActive()) 
{
	int listCategoryIndex;
	Product[] listProducts;
	PetStore ps = (PetStore) Prevayler.system();
	Category preferredCategory = listAccount.getPreferredCategory();
	listCategoryIndex =  Array.IndexOf(ps.getCategories(), preferredCategory);
	listProducts = preferredCategory.products;
	
	Product listProduct;
	for(int i = 0; i < listProducts.Length; i++) 
	{ 
		listProduct = listProducts[i];
		litOutput.Text += String.Format("<a href=\"Items.aspx?categoryId={0}&productId={1}\">{2}</a><br>", listCategoryIndex, i, listProduct.name);
	}
	txtOutput.Visible = true;
}
%>
<asp:PlaceHolder id="txtOutput" runat="server" visible="false">
<p align=right>My List</p>
<p align=right><asp:Literal id="litOutput" runat="server"></asp:Literal></p>
</asp:PlaceHolder>
