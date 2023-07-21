using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using Network.Packet;
using Network.Packet.Handler;

namespace WorldServer;

public class Listener : INetEventListener
{
	private readonly PacketManager packetManager = new PacketManager();

	private byte[] buffer = new byte[4096];

	public void OnPeerConnected(NetPeer peer)
	{
		Console.WriteLine($"Peer Connected. Peer Id : {peer.Id}, Remote Id : {peer.RemoteId}");

	}

	public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
	{
		Console.WriteLine($"Peer Disconnected. Peer Id : {peer.Id}, Remote Id : {peer.RemoteId}, ErrorCode : {disconnectInfo.SocketErrorCode}");
	}

	public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
	{
	}

	public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
	{
		var length = reader.GetUShort();
		var opcode = reader.GetShort();

		reader.GetBytes(buffer, length);

		var body = buffer.AsSpan(new Range(0, length));
		var command = packetManager.ToCommand((Opcode) opcode, body);
		Console.WriteLine($"OnNetworkReceive. Peer Id : {peer.Id}, Remote Id : {peer.RemoteId}, length : {length}, opcode : {opcode}");

		if (command is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
	{
	}

	public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
	{
	}

	public void OnConnectionRequest(ConnectionRequest request)
	{
		request.Accept();
	}

	public Listener()
	{
		packetManager.Init();
	}
}
