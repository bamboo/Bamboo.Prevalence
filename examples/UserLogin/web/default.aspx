<script runat="server">
void Page_Load(object sender, EventArgs e)
{
	Bamboo.Prevalence.Examples.UserLogin.User user = (Bamboo.Prevalence.Examples.UserLogin.User)Context.User;
	_name.Text = user.FullName; 
}
</script>
<html>
<body>
<center>
Hello, <asp:Label id="_name" runat="server" />!
</center>
</body>
</html>
