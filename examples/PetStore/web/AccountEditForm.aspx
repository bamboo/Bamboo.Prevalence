<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
	Account account = SessionState.Account;
	ContactInfo contact = account.getContactInfo();
	CreditCard card = account.getCreditCard();
	Category[] categories = ((PetStore)Prevayler.system()).getCategories();
%>
<html>

<head>
<title>Prevayler Pet Store - Account Creation Confirmation</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>

<form method="post" action="accountConfirm.aspx">

<p>Your Account Information</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />

<p>Contact Information</p>

<p>First name: <input type="text" name="firstName" value="<%=contact.firstName%>"><br>
Last name: <input type="text" name="lastName" value="<%=contact.lastName%>"><br>
Phone: <input type="text" name="phone" value="<%=contact.phone%>"><br>
e-mail: <input type="text" name="email" value="<%=contact.email%>"><br>
Street: <input type="text" name="street" value="<%=contact.street%>"><br>
City: <input type="text" name="city" value="<%=contact.city%>"><br>
State/Province: <select name="state">
						<option <%= (contact.state.Equals("California"))?"selected":"" %>>California</option>
						<option <%= (contact.state.Equals("New York"))?"selected":"" %>>New York</option>
						<option <%= (contact.state.Equals("Texas"))?"selected":"" %>>Texas</option>
					</select>
<br>
Postal code: <input type="text" name="postalCode" value="<%=contact.postalcode%>"><br>
Country: <select name="country">
						<option <%= (contact.country.Equals("United State"))?"selected":"" %>>United States</option>
						<option <%= (contact.country.Equals( "Japan"))?"selected":"" %>>Japan</option>
						<option <%= (contact.country.Equals("China"))?"selected":"" %>>China</option>
						<option <%= (contact.country.Equals("Canada"))?"selected":"" %>>Canada</option>
			</select>
</p>
<p>Credit Card Information</p>
<p>Credit Card Number: <input type="text" name="cardNumber" value="<%=card.number%>"><br>
Credit Card Type:	<select name="cardType">
						<option <%= (card.type.Equals("Duke Express"))?"selected":"" %>>Duke Express</option>
						<option <%= (card.type.Equals("Java(TM) card"))?"selected":"" %>>Java(TM) card</option>
						<option <%= (card.type.Equals("Meow Card"))?"selected":"" %>>Meow Card</option>
						</select>
<br>
Credit Card Expiry Date: <select name= "cardDateMonth">
<%	for (int i = 1; i <= 12; i++) {
%>	
			<option <%=(card.date.Month == i)?"selected":"" %>><%=i%></option>
<%
	}
%>
		</select> / <select name = "cardDateYear">

<% for(int i = 2002; i <= 2005; i++) {
%>		
			<option <%= (card.date.Year == i)?"selected":"" %>><%=i%></option>
<%
	}
%>
</select>
</p>
<p>Profile Information</p>
<p>
My favorite category is : <select name="category">
<%  Category category = null;
	String selected = "";
	for (int i = 0; i < categories.Length; i++) {
		    category = categories[i];
		    selected = category.Equals(account.getPreferredCategory())?"selected":"";
%>		
<option value="<%=i%>" <%=selected%>><%=category.name%></option>
<%	}
%>
	</select>
	
<br>
<input type="checkbox" name="listFeature" value="true" <%=account.isListFeatureActive()?"checked":""%>> Yes, I want to enable the MyList feature. MyList makes your favorite items and categories more prominent as you shop.<br>
<input type="checkbox" name="bannerFeature" value="true" <%=account.isBannerFeatureActive()?"checked":""%>>Yes, I want to enable the pet tips banners. Java Pet Store will display pet tips as you shop, which are based on your favorite items and categories.</p>

<input type="hidden" name="login" value="<%=account.getLogin()%>">
<input type="submit" value="Save Account">

</form>
</body>
</html>