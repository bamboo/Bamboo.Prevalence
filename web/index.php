<html>
<head>
<title>Bamboo.Prevalence - a .NET object prevalence engine</title>
</head>
<body>
<table width="100%">
<tr>
<td><A href="http://sourceforge.net"> <IMG src="http://sourceforge.net/sflogo.php?group_id=61693&amp;type=5" width="210" height="62" border="0" alt="SourceForge Logo"></A></td>
<td><div align="right"><span align="right">02-13-2003</span></div></td>
</tr> 
</table>
<h1>Bamboo.Prevalence - a .NET object prevalence engine</h1>
<h2>Latest News</h2>
<p>
Last week I've improved the VersionMigration tool that ships with
Bamboo.Prevalence (now it's possible to rename types and even move them to
different assemblies). Also added a not so nice GUI to the tool :-).
</p>
<p>
I've also implemented a simple XPath navigator for object graphs that will
probably ship in the next release (in about a week or so).
</p>
<p>
Bamboo.Prevalence has been in production for
about 4 months in a corporate intranet and for about 3 months in a big
website. The thought of going back to SQL and relational databases gives me
the creeps :-)
</p>
<p>
The not so distant future:
<ul>
<li>
I'm following the evolution of prevayler closely and I intend to add
replication support to Bamboo.Prevalence so people can enjoy the full speed
of objects in a clustered environment
</li>
<li>
Make the code simpler (any tips?): something I want to do is to decouple
the command logging from the snapshot taking from the engine: this will allow engines to use
different storage mechanisms and different snapshot formats (well, today it
is possible to do that by using different IFormatter implementations but
that's just not documented).
</li>
<li>
An Object Query Tool: something like a simple query analyzer for
object graphs. The query language being XPath. People miss something like
Query Analyzer when they migrate to an object only environment and It's
something good to demo
</li>
</ul>

<h2>Introduction</h2>
<p>Bamboo.Prevalence is a .NET implementation of the object prevalence concept 
  brought to life by Klaus Wuestefeld in his <a href="http://www.prevayler.org/">Prevayler 
  system</a>.</p>
<p>Basically, Bamboo.Prevalence aims to provide transparent object persistence 
  and synchronization to deterministic systems targeting the CLR (Common Language 
  Runtime). No relational databases. No object-to-relational mapping goo. No SQL. 
  Just you and your objects, isn't life great? 
<p>For the rationale behind object prevalence please read the <a href="http://www.prevayler.org/wiki.jsp?topic=ObjectPrevalenceSkepticalFAQ" target="_blank">Object 
  Prevalence Skeptical FAQ</a>. 
<h2>What's New</h2>
<p>Bamboo.Prevalence is not a simple port of existing implementations but an attempt 
  to extend them in several useful ways and, above all, to integrate seamlessly 
  with the .NET framework technologies (ASP.NET, WebForms, etc).</p>
<p>One such a way Bamboo.Prevalence extends the existing implementations is by 
  introducing the concept of query objects: objects that model queries to the 
  system state - query objects are guaranteed not to apply changes to the system. 
  Commands objects, in turn, always apply changes to the system. With both concepts 
  in place: query objects and command objects the Bamboo.Prevalence engine is 
  able to provide a much more scalable synchronization solution by making use 
  of the reader/writer lock design pattern.</p>
<p>
  Bamboo.Prevalence is implemented in C# and intended to be usable from any CLR 
  implementation. </p>
<h2>History</h2>
<p><b>09/15/2002</b> Bamboo.Prevalence now supports transparent prevalence. What 
  does it mean? It means that if you don't want to you don't need to write command 
  and/or query objects, the Bamboo.Prevalence engine will take care of mapping 
  method calls to command/query objects as needed. Take a look <a href="MyFirstPrevalentSystem.htm">here</a>. 
  Thanks to Jesse Ezell for the ideas and talks! </p>
<h2>System Requirements</h2>
<p>All you need to start using Bamboo.Prevalence in your projects is the .NET 
  framework runtime.</p>
<p>If you want to build Bamboo.Prevalence you are going to need:</p>
<ul>
  <li><a href="http://nant.sourceforge.net/">NAnt</a></li>
  <li><a href="http://nunit.sourceforget.net/">NUnit</a></li>
</ul>
<h2>Getting Started</h2>
<ol>
  <li>Read the <a href="http://www.prevayler.org/wiki.jsp?topic=ObjectPrevalenceSkepticalFAQ" target="_blank">Object 
    Prevalence Skeptical FAQ</a></li>
  <li><a target="_blank" href="http://sourceforge.net/project/showfiles.php?group_id=61693&release_id=109035"> 
    Download Bamboo.Prevalence</a></li>
  <li>Take a look at the included examples and test cases, you can start by taking a look
  at <a href="MyFirstPrevalentSystem.htm">MyFirstPrevalentSystem</a></li>
  <li>Start writing your own prevalent system classes</li>
  <li>Subscribe to the <a href="https://sourceforge.net/mail/?group_id=61693">mailing 
    lists</a></li>
  <li>Post questions, suggestions and comments (bug reports are always welcome 
    as well)</li>
</ol>
I'll be adding a tutorial section to this web site soon. 
<h2>Developers</h2>
<ul>
  <li><a href="mailto:rodrigobamboo@users.sourceforge.net">Rodrigo B. de Oliveira</a></li>
</ul>
<p>Do you want to help? Drop me a line.</p>
<h2>Thanks and Acknowledgements</h2>
<ul>
  <li><b>Klaus Wuestefeld and the Prevayler team</b> for starting it all</li>
  <li><b>Jesse Ezell</b> for the great idea of using ContextBoundObject and context 
    attributes to enable transparent prevalence totally removing the need of handcoding 
    command and query objects. Later on we decided on using proxies. </li>
</ul>
<h2>Other Resources</h2>
<p><a href="http://sourceforge.net/projects/bbooprevalence">Project site at SourceForge</a></p>
<p><a href="http://www.prevayler.org">www.prevayler.org</a></p>
<hr width="90%" align="center" />
<p align="center"><font size="-1"> Bamboo.Prevalence - a .NET object prevalence 
  engine<br />
  Copyright &copy; 2002 Rodrigo B. de Oliveira</font></p>


</body>
</html>