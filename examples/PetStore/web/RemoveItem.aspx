<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
	int cartItemIndex = int.Parse(Request.QueryString["itemId"]);
    SessionState.ShoppingCart.removeItem(cartItemIndex);
    Response.Redirect("ShoppingCart.aspx");
%>