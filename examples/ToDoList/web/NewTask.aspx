<%@ Page Language="C#" %>
<%@ import Namespace="Bamboo.Prevalence" %>
<%@ import Namespace="Bamboo.Prevalence.Configuration" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList.Queries" %>
<%@ import Namespace="Bamboo.Prevalence.Examples.ToDoList.Commands" %>
<script runat="server">    
    void Page_Load(Object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            AddTask();
        }
    }
    
    void AddTask()
    {
        Task task = new Task();
        task.Priority = (Task.TaskPriority)Enum.Parse(typeof(Task.TaskPriority), Request["priority"]);
        task.Description = Request["description"];
		task.Summary = Request["summary"];
        
        PrevalenceEngine engine = PrevalenceSettings.GetEngine("todo");
        engine.ExecuteCommand(new AddTaskCommand(task));
        
        Server.Transfer("Default.aspx");
    }

</script>
<html>
<head>
</head>
<body>
<h2 align="center">New Task</h2>
    <form runat="server">
        <table width="80%" align="center">
            <tbody>
                <tr>
                    <td align="right">
                        Task Priority</td>
                    <td align="left">
                        <select name="priority">
                            <option value="Low">Low</option>
                            <option value="Normal" selected="selected">Normal</option>
                            <option value="High">High</option>
                        </select>
                    </td>
                </tr>
				<tr>
					<td><div align="right">Summary</div></td>
					<td><input name="summary" value="" /></td>
				</tr>
                <tr>
                    
        <td align="middle" colspan="2"> <div align="center">Task Details <br />
            <textarea name="description" rows="10" cols="40"></textarea>
          </div></td>
                </tr>
                <tr>
                    
        <td align="middle" colspan="2"> <div align="center">
            <input type="submit" value="Add" name="cmdSubmit" />
          </div></td>
                </tr>
            </tbody>
        </table>
    </form>
</body>
</html>
