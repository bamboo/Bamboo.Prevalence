<html>
<head>
<title>Bamboo.Prevalence - Frequently Asked Questions</title>
</head>
<body>
<table width="100%">
<tr>
    <td><A href="http://sourceforge.net"> <IMG src="http://sourceforge.net/sflogo.php?group_id=61693&amp;type=5" width="210" height="62" border="0" alt="SourceForge Logo"></A></td>
    <td><div align="right"><span align="right">2003-03-30</span></div></td>
</tr>
</table>

<h1>Bamboo.Prevalence - Frequently Asked Questions</h1>

<!-- Place Holder -->

q) How do I use this thing?

a) The first step to successfully using Bamboo.Prevalence, is to stop thinking in tables and rows. Bamboo still requires a base object which stores all other objects (Similar to the Database). within this base object, we have other objects which serve as containers, managing lots of individual objects. (Our tables), and finally, we have our data objects (our rows)
eg:

<code>
class ObjectStore
{
	private TaskList tasks;
	public ObjectStore()
    {
    	tasks = new TaskList();
    }

	public TaskList Tasks() { return tasks; }
}

class TaskList
{
	private Collection tasks;

	public Add(Task)
	{
		tasks.Add(Task);
    }
}

class Task
{
	public ID;
	public Title;
	public Date;
}
</code>




q) What are the differences between a transparent engine, and a normal engine?

a) There are a number of differences between the two systems.
Firstly, a normal engine does not contain the overheads required by the transparent engine, so this results in a faster runtime (although we're talking about 3 - 5ms difference in execution speed).
Secondly, the proxy sub-system, which the transparent engine uses to transparently create command and query objects, changes its action based on the type of call we're making to the MarshalByRefObject. So nesting methods which should act as commands, underneath properties of an object, will not have the desired effect. Example:

<code>
public class ObjectStore : MarshalByRefObject
{
	private UserManager users;

	public ObjectStore()
	{
		users = new UserManager();
    }

    public UserManager Users
    {
    	get { return users; }
    }
}

public class UserManager
{
	private System.Collections.Hashtable list;

    public UserManager()
    {
        list = new System.Collections.Hashtable();
    }
	public void Add(User user)
	{
		list.add(user.ID, user);
    }

	public User this[int index]
	{
		get { return list[index]; }
    }
}

public class User
{
    private int id;
    private System.Collections.ArrayList emails;

    public User()
    {
    	emails = new System.Collections.ArrayList();
    }

    public int ID
    {
        get { return id; }
    }
    
    public AddEmail(string email)
    {
    	emails.Add(email);
    }
}
</code>

Our MarshalByRefObject is called ObjectStore. ObjectStore creates an instance of a class called UserManager, and enables read access of this object via the property Users. UserManager just controls a hashtable of user objects

Using a transparent engine, the following call would NOT be transparently turned into a command object:

<code>
User user = new User();
user.ID = 1;
system.Users.Add(user);      // transparently turned into a command object;
system.Users[1].AddEmail("sean@arcturus.com.au"); // the object is updated, but the command is never serialised, so if the application crashes. This data is lost.
</code>

Why does this happen? If you examine the PrevalentSystemProxy, the answer lies in the fact that, the engine looks for get_ proprerties, and straight away flags them to be turned into a transparent query.

But we're calling a method named AddEmail() aren't we? Yes, but to get to that method, we are using thser ObjectStore.Users property (get_Users), so our call is transparently turned into a Query, and it is never serialized to the system.

Whats the solution? There are a couple of solutions. One is to specfically code a command object:

<code>
public class UserAddEmail : ICommand
{
	private int id;
	private string email;

	public UserAddEmail(int id, string email)
	{
		this.id = id;
		this.email = email;
	}

	public object Execute(object system)
	{
		((ObjectStore)system).Users[id].AddEmail(email);
		return null;
	}
}
</code>
The other is to modify the code, we don't use C# properties, instead coding seperate get and set methods for any variable we want to get or set (ala Java). 






</body>
</html>
