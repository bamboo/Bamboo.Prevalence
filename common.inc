<?xml version="1.0"?>
<project name="Common Build Properties">
	<property name='nunit.framework.dll' value='${nant.location}/nunit.framework.dll' />
	
	<if test="${not property::exists('nant.tasks.nunit2')}">
		<echo>nunit2 task is not available. Unit tests won't be executed.</echo>
		<property name="nant.tasks.nunit2" value="False" />
	</if>
</project>
