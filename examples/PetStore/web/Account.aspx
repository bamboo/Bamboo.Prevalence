<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<html>

<head>
<title>Prevayler Pet Store - Account</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Your Account Information</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<%
	Account account = (Account) SessionState.Account;
	System.Globalization.GregorianCalendar calendar = new System.Globalization.GregorianCalendar();
	ContactInfo contact = account.getContactInfo();
	CreditCard card = account.getCreditCard();
%>

<p>Contact Information</p>
<p>First name: <%=contact.firstName%><br>
Last name: <%=contact.lastName%><br>
Phone: <%=contact.phone%><br>
e-mail: <%=contact.email%><br>
Street: <%=contact.street%><br>
City: <%=contact.city%><br>
State/Province: <%=contact.state%><br>
Postal code: <%=contact.postalcode%><br>
Country: <%=contact.country%></p>

<p>Credit Card Information</p>
<p>Credit Card Number: <%=card.number%><br>
Credit Card Type: <%=card.type%><br>
Credit Card Expiry Date: <%=calendar.GetMonth(card.date)%> / <%=calendar.GetYear(card.date)%></p>

<p>Profile Information</p>
My favorite category is : <%=account.getPreferredCategory().name%><br>
<%=account.isListFeatureActive()?"Yes, I":"No, I don't"%> want to enable the MyList feature. <i>MyList makes your favorite items and categories more prominent as you shop.</i><br>
<%=account.isBannerFeatureActive()?"Yes, I":"No, I don't"%> want to enable the pet tips banners. <i>Java Pet Store will display pet tips as you shop, which are based on your favorite items and categories.</i></p>

<p><a href="AccountEditForm.aspx">Edit your account</a></p>
</body>
</html>

