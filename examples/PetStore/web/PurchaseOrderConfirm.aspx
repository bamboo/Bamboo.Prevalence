<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Import Namespace="PetStoreWeb.Commands" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<html>

<head>
<title>Prevayler Pet Store - Purchase Order Confirmation</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Purchase Order Confirmation</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />

<%
Account account = SessionState.Account;
ShoppingCart cart = SessionState.ShoppingCart;

String billingFirstName = Request.Form["billingFirstName"];
String billingLastName = Request.Form["billingLastName"];
String billingPhone = Request.Form["billingPhone"];
String billingEmail = Request.Form["billingEmail"];
String billingStreet = Request.Form["billingStreet"];
String billingCity = Request.Form["billingCity"];
String billingState = Request.Form["billingState"];
String billingPostalCode = Request.Form["billingPostalCode"];
String billingCountry = Request.Form["billingCountry"];
ContactInfo billingInfo = new ContactInfo(billingFirstName, billingLastName, billingPhone, billingEmail, billingStreet, billingCity, billingState, billingPostalCode, billingCountry);

String shippingFirstName = Request.Form["shippingFirstName"];
String shippingLastName = Request.Form["shippingLastName"];
String shippingPhone = Request.Form["shippingPhone"];
String shippingEmail = Request.Form["shippingEmail"];
String shippingStreet = Request.Form["shippingStreet"];
String shippingCity = Request.Form["shippingCity"];
String shippingState= Request.Form["shippingState"];
String shippingPostalCode = Request.Form["shippingPostalCode"];
String shippingCountry = Request.Form["shippingCountry"];
ContactInfo shippingInfo = new ContactInfo(shippingFirstName, shippingLastName, shippingPhone, shippingEmail, shippingStreet, shippingCity, shippingState, shippingPostalCode, shippingCountry);

PurchaseOrderCreateCommand command = new PurchaseOrderCreateCommand(account, shippingInfo, billingInfo, cart.getCartItems());
PurchaseOrder purchaseOrder = (PurchaseOrder) Prevayler.Execute(command);

SessionState.ShoppingCart = null;
%>

<p>Your Order is Complete.</p>
<p>Your order Id is <%= purchaseOrder.id %>.</p>
<p>You should receive a confirmation e-mail soon at <%= purchaseOrder.billingInfo.email %>.</p>
<p>Thank you for shopping with Presto.</p>

</body>
</html>