#region license
// Bamboo.Prevalence - a .NET object prevalence engine
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Based on the original concept and implementation of Prevayler (TM)
// by Klaus Wuestefeld. Visit http://www.prevayler.org for details.
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// http://bbooprevalence.sourceforge.net
// mailto:rodrigobamboo@users.sourceforge.net
#endregion

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
