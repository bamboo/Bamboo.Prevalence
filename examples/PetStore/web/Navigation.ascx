<%@ Control Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<p align=right>
<a href="Categories.aspx">Categories</a>		
| <a href="ShoppingCart.aspx">Cart</a> 
<% if (SessionState.Account == null) { %>
	| <a href="SignInForm.aspx?nextPage=Account.aspx">Account</a> 
	| <a href="SignInForm.aspx?nextPage=Categories.aspx">Sign In</a>
<% } else { %>
	| <a href="Account.aspx">Account</a> 
	| <a href="SignOut.aspx">Sign Out</a>
<% } %>
</p>