<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
    Account account = SessionState.Account;
	if (account == null) Response.Redirect("SignInForm.aspx?nextPage=PurchaseOrderForm.aspx");
    ContactInfo contact = account.getContactInfo();
%>
<html>

<head>
<title>Prevayler Pet Store - Purchase Order Information</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>

<form method="post" action="PurchaseOrderConfirm.aspx">

<p>Purchase Order Information</p>
<ps:Navigation id="ctrlNav" runat="server" />

<p>Billing Information</p>

<p>First name: <input type="text" name="billingFirstName" value="<%=contact.firstName%>"><br>
Last name: <input type="text" name="billingLastName" value="<%=contact.lastName%>"><br>
Street: <input type="text" name="billingStreet" value="<%=contact.street%>"><br>
City: <input type="text" name="billingCity" value="<%=contact.city%>"><br>
State/Province: <select name="billingState">
						<option <%= (contact.state.Equals("California"))?"selected":"" %>>California</option>
						<option <%= (contact.state.Equals("New York"))?"selected":"" %>>New York</option>
						<option <%= (contact.state.Equals("Texas"))?"selected":"" %>>Texas</option>
					</select>
<br>
Postal code: <input type="text" name="billingPostalCode" value="<%=contact.postalcode%>"><br>
Country: <select name="billingCountry">
						<option <%= (contact.country.Equals("United State"))?"selected":"" %>>United States</option>
						<option <%= (contact.country.Equals( "Japan"))?"selected":"" %>>Japan</option>
						<option <%= (contact.country.Equals("China"))?"selected":"" %>>China</option>
						<option <%= (contact.country.Equals("Canada"))?"selected":"" %>>Canada</option>
			</select>
Phone: <input type="text" name="billingPhone" value="<%=contact.phone%>"><br>
e-mail: <input type="text" name="billingEmail" value="<%=contact.email%>"><br>
</p>

<p>Shipping Information</p>

<p>First name: <input type="text" name="shippingFirstName" value="<%=contact.firstName%>"><br>
Last name: <input type="text" name="shippingLastName" value="<%=contact.lastName%>"><br>
Street: <input type="text" name="shippingStreet" value="<%=contact.street%>"><br>
City: <input type="text" name="shippingCity" value="<%=contact.city%>"><br>
State/Province: <select name="shippingState">
						<option <%= (contact.state.Equals("California"))?"selected":"" %>>California</option>
						<option <%= (contact.state.Equals("New York"))?"selected":"" %>>New York</option>
						<option <%= (contact.state.Equals("Texas"))?"selected":"" %>>Texas</option>
					</select>
<br>
Postal code: <input type="text" name="shippingPostalCode" value="<%=contact.postalcode%>"><br>
Country: <select name="shippingCountry">
						<option <%= (contact.country.Equals("United State"))?"selected":"" %>>United States</option>
						<option <%= (contact.country.Equals( "Japan"))?"selected":"" %>>Japan</option>
						<option <%= (contact.country.Equals("China"))?"selected":"" %>>China</option>
						<option <%= (contact.country.Equals("Canada"))?"selected":"" %>>Canada</option>
			</select>
Phone: <input type="text" name="shippingPhone" value="<%=contact.phone%>"><br>
e-mail: <input type="text" name="shippingEmail" value="<%=contact.email%>"><br>
</p>

<input type="submit" value="Submit"/>
</form>

</body>
</html>