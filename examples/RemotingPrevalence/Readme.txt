This sample demonstrates:

	* the use of PrevalenceActivator.CreateTransparentEngine to automatically add prevalence
	support to a serializable class derived from System.MarshalByRefObject

	* how you can combine prevalent classes with .NET remoting to create object servers

	* how to use the PassThrough attribute to prevent the engine from treating a method call
	as a command

	* how to declare events in a prevalent class (you have to implement the special
	methods add/remove, that's the only way to apply the attribute NonSerialized
	to the delegate)

How to use

	* build the sample by invoking Nant, this will create 3 files: AddressBook.dll, client.exe, server.exe

	* run server.exe

	* run client.exe (run several instances at once, it makes it more fun)