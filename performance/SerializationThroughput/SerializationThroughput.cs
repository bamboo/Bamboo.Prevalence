using System;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Integer
{
	private int _value;

	public Integer(int value)
	{
		_value = value;
	}	
}

public class SerializationThroughput
{
	static Random rand = new Random();

	static public void Main(string[] args)
	{
		BinaryFormatter formatter = new BinaryFormatter();

		Console.WriteLine("record size\trecords written\tbytes per record\trecords per sec\tintegers per sec");

		for (int n=1; n<50000; n=(int)(1.2*n)+1)
		{
			using (FileStream fos = new FileStream("tmp.tmp", FileMode.Create, FileAccess.Write))
			{
				Thread.Sleep(1000); //Wait for any disk activity to stop.
				Integer[] d = new Integer[n];
				for (int k=0; k<n; k++)
				{
					d[k] = new Integer((int)(rand.NextDouble()*k));
				}

				start();
				int max = 1000;
				int i = 0;
				while (i<max && elapsed()<10)
				{
					formatter.Serialize(fos, d);
					fos.Flush();
					FlushFileBuffers(fos.Handle); // Forces the OS to flush 
					i++;
				}

				double t =elapsed();
				long f = fos.Length;
				Console.WriteLine(
					n +"\t"+
					i + "\t"+
					f / i +"\t"+
					i / t +"\t"+
					n * i / t);
			}
		}
	}

	static long t0;

	static void start()
	{
		t0 = Environment.TickCount;
	}

	static double elapsed()
	{
		return (Environment.TickCount - t0)/1000.0;
	}

	[DllImport("Kernel32.DLL", EntryPoint="FlushFileBuffers", SetLastError=true,CallingConvention=CallingConvention.Winapi)]
	static extern int FlushFileBuffers(IntPtr handle);
}
