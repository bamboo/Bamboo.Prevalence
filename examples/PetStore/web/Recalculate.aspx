<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
    ShoppingCart cart = SessionState.ShoppingCart;
    if (cart == null) cart = new ShoppingCart();
    SessionState.ShoppingCart = cart;

	int quantity = 0;
	for (int i = 0; i < cart.getCartItems().Length; i++) 
	{
		quantity = int.Parse(Request.Form[""+i]);
    	cart.getCartItems()[i].setQuantity(quantity);    	
    }

    Response.Redirect("ShoppingCart.aspx");
%>