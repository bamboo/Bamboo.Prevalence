<%@ Page Language="C#" %>
<%@ import Namespace="Bamboo.Prevalence" %>
<%@ import Namespace="Bamboo.Prevalence.Configuration" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList.Queries" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList.Commands" %>
<script runat="server">

    // Insert page code here
    //
    
    void Page_Load(Object sender, EventArgs e) {
        PrevalenceEngine engine = PrevalenceSettings.GetEngine("todo");
        gridTasks.DataSource = engine.ExecuteQuery(new AllTasksQuery());
        gridTasks.DataBind();
    }

</script>
<html>
<head>
</head>
<body>
    <form runat="server">
        <asp:DataGrid id="gridTasks" runat="server" BorderStyle="None" BorderWidth="1px" BorderColor="#CCCCCC" BackColor="White" CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" Width="90%">
            <FooterStyle forecolor="#000066" backcolor="White"></FooterStyle>
            <HeaderStyle font-bold="True" forecolor="White" backcolor="#006699"></HeaderStyle>
            <PagerStyle horizontalalign="Left" forecolor="#000066" backcolor="White" mode="NumericPages"></PagerStyle>
            <SelectedItemStyle font-bold="True" forecolor="White" backcolor="#669999"></SelectedItemStyle>
            <ItemStyle forecolor="#000066"></ItemStyle>
            <Columns>
                <asp:BoundColumn DataField="ID" HeaderText="ID" ItemStyle-HorizontalAlign="center"></asp:BoundColumn>
                <asp:BoundColumn DataField="CreatedTime" HeaderText="Creation Date"></asp:BoundColumn>
                <asp:BoundColumn DataField="Summary" HeaderText="Summary"></asp:BoundColumn>
                <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>
                <asp:BoundColumn DataField="Priority" HeaderText="Priority"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <!-- Insert content here -->
    </form>
    <center><a href="NewTask.aspx">New Task</a> 
    </center>
</body>
</html>
