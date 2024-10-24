﻿// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using LiteNetLib;

namespace WorldServer;

class Program
{
	private static int DefaultPort => 47221;

	static void Main(string[] args)
	{
		var listener = new Listener();
		var server = new NetManager(listener)
		{
			AutoRecycle = true
		};

		server.Start(DefaultPort);
		listener.Init(server);

		Debug.WriteLine("Server Online.");
		while (true)
		{
			server.PollEvents();
			listener.Update();
			Thread.Sleep(15);
		}

		server.Stop();
	}
}
