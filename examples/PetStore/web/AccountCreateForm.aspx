<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Import Namespace="PetStoreWeb.Commands" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<html>

<head>
<title>Prevayler Pet Store - Account Creation</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>

<%
if (!Request.Form["password"].Equals(Request.Form["confirm"])) {
%>
	<p> Password confirmation incorrect. Please, <a href="javascript:history.back()">click here</a> to try again.</p>
	</body>
	</html>
<%
	return;
}

AccountCreateCommand command = new AccountCreateCommand(Request.Form["login"], Request.Form["password"]);
Account account = (Account) Prevayler.Execute(command);
Category[] categories = ((PetStore)Prevayler.system()).getCategories();
%>

<p>Your Account Information</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<p>Contact Information</p>

<form method="POST" action="AccountConfirm.aspx">
<input type="hidden" name="login" value="<%=account.getLogin()%>">
<p>First name: <input type="text" name="firstName" value=""><br>
Last name: <input type="text" name="lastName" value=""><br>
Phone: <input type="text" name="phone" value=""><br>
e-mail: <input type="text" name="email" value=""><br>
Street: <input type="text" name="street" value=""><br>
City: <input type="text" name="city" value=""><br>
State/Province: <select name="state">
						<option>California</option>
						<option>New York</option>
						<option>Texas</option>
					</select>
<br>
Postal code: <input type="text" name="postalCode" value=""><br>
Country: <select name="country">
						<option>United States</option>
						<option>Japan</option>
						<option>China</option>
						<option>Canada</option>
			</select>
</p>
<p>Credit Card Information</p>
<p>Credit Card Number: <input type="text" name="cardNumber" value=""><br>
Credit Card Type:	<select name="cardType">
						<option>Duke Express</option>
						<option>Java(TM) card</option>
						<option>Meow Card</option>
						</select>
<br>
Credit Card Expiry Date: <select name= "cardDateMonth">
<%	for (int i = 1; i <= 12; i++) {
%>	
			<option><%=i%></option>
<%
	}
%>
		</select> / <select name = "cardDateYear">

<% for(int i = 2002; i <= 2005; i++) {
%>		
			<option><%=i%></option>
<%
	}
%>
</select>
</p>
<p>Profile Information</p>
My favorite category is : <select name="category">
<%
	Category category;
	for (int i = 0; i < categories.Length; i++) {
		category = categories[i];
%>
		<option value="<%=i%>"><%=category.name%></option>
<%
	}
%>
	</select>
	
<br>
<input type="checkbox" name="listFeature" value="true"> Yes, I want to enable the MyList feature. <i>MyList makes your favorite items and categories more prominent as you shop.</i><br>
<input type="checkbox" name="bannerFeature" value="true">Yes, I want to enable the pet tips banners. <i>Java Pet Store will display pet tips as you shop, which are based on your favorite items and categories.</i></p>

<input type="submit" value="Create">
</form>
</body>
</html>

