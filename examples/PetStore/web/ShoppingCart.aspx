<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
    ShoppingCart cart = SessionState.ShoppingCart;
    if (cart == null){
    	cart = new ShoppingCart();
    	SessionState.ShoppingCart = cart;
    }
    Account account = SessionState.Account;
    Category myCategory = null;
    if (account != null && account.isBannerFeatureActive())
    	myCategory = account.getPreferredCategory();
%>


<html>

<head>
<title>Prevayler Pet Store - Shopping Cart</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Shopping Cart</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<p>
<%
	if (cart.getCartItems().Length == 0) {
%>
	<p>Your Shopping Cart is empty</p>
<%
}
%>
<form method="POST" action="recalculate.aspx">
<%	
    CartItem item;
    double totalPrice = 0d;
    CartItem[] items = cart.getCartItems();
    for(int i = 0; i < items.Length; i++){ 
		item = items[i];
		totalPrice += ( item.price * item.getQuantity());
%>	
		<a href="removeItem.aspx?itemId=<%=i%>">Remove</a> <%=item.item.name%> <input type="text" name="<%=i%>" value="<%=item.getQuantity()%>" size="2" maxlength="2"> <%=item.price%><br>
					
<%	
    }
%>
<input type="submit" name="button" value="Recalculate"> Total: <%=totalPrice%>
</form>
</p>
<p><a href="PurchaseOrderForm.aspx">Check out</a></p>
<p><a href="Categories.aspx">Shopping</a></p>
<p align="center">
<%if (  myCategory != null  ) {
		Response.Write("Banner : Thanks for buying "+ myCategory.name +" at Presto.");
}%>
</p>
</body>
</html>