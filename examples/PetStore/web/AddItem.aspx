<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
    if (SessionState.ShoppingCart == null) {
    	SessionState.ShoppingCart = new ShoppingCart();
    }

	int categoryIndex = int.Parse(Request.QueryString["categoryId"]);
	int productIndex = int.Parse(Request.QueryString["productId"]);
	int itemIndex = int.Parse(Request.QueryString["itemId"]);

    Item item = ((PetStore)Prevayler.system()).getCategories()[categoryIndex].products[productIndex].items[itemIndex];

    SessionState.ShoppingCart.addItem(new CartItem(item, 1, item.listPrice));
    //Response.Write(categoryIndex);
    //Response.Write("<br />");
    //Response.Write(productIndex);
    //Response.Write("<br />");
    //Response.Write(itemIndex);
    //Response.Write("<br />");
    Response.Redirect("ShoppingCart.aspx");
%>