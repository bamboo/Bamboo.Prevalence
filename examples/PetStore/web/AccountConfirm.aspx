<%@ Page Language="C#" %>
<%@ Import Namespace="PetStoreWeb.Components" %>
<%@ Import Namespace="PetStoreWeb.Utils" %>
<%@ Import Namespace="PetStoreWeb.Commands" %>
<%@ Register TagPrefix="ps" TagName="Category" Src="UserList.ascx" %>
<%@ Register TagPrefix="ps" TagName="Navigation" Src="Navigation.ascx" %>
<%
    PetStore ps = (PetStore) Prevayler.system();
	Category[] categories = ps.getCategories();

	String login = Request.Form["login"];
	Account account = ps.getAccount(login);
	
	String firstName = Request.Form["firstName"];
	String lastName = Request.Form["lastName"];
	String phone = Request.Form["phone"];
	String email = Request.Form["email"];
	String street = Request.Form["street"];
	String city = Request.Form["city"];
	String state = Request.Form["state"];
	String postalCode = Request.Form["postalCode"];
	String country = Request.Form["country"];
	ContactInfo info = new ContactInfo(firstName, lastName, phone, email, street, city, state, postalCode, country);
	
	String cardNumber = Request.Form["cardNumber"];
	String cardType = Request.Form["cardType"];
	String cardDateMonth = Request.Form["cardDateMonth"];
	String cardDateYear = Request.Form["cardDateYear"];
	DateTime cardDate = new DateTime(Convert.ToInt32(cardDateYear), Convert.ToInt32(cardDateMonth), 1);
	CreditCard card = new CreditCard(cardNumber, cardType, cardDate);
	
	Category category = categories[int.Parse(Request.Form["category"])];
	bool listFeature = Convert.ToBoolean(Request.Form["listFeature"]);
	bool bannerFeature = Convert.ToBoolean(Request.Form["bannerFeature"]);
	
	AccountUpdateCommand command = new AccountUpdateCommand(account, info, card, category, listFeature, bannerFeature);
	Prevayler.Execute(command);
	SessionState.Account = account;
%>
<html>

<head>
<title>Prevayler Pet Store - Account Creation Confirmation</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>

<body>
<p>Your Account Information</p>
<ps:Navigation id="ctrlNav" runat="server" />
<ps:Category id="ctrlList" runat="server" />
<p>Account updated successfully</p>
</body>
</html>