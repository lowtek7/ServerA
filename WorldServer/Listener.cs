using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using MemoryPack;
using Network.NetCommand;
using Network.Packet;
using Network.Packet.Handler;
using Vim.Math3d;
using World.Component;
using World.Extensions;
using World.System;
using WorldServer.Utility;

namespace WorldServer;

public class Listener : INetEventListener
{
	private readonly PacketManager packetManager = new PacketManager();

	private byte[] buffer = new byte[4096];

	private NetManager server;

	private readonly Queue<INetCommand> commandQueue = new Queue<INetCommand>();

	private uint CurrentTime => DateTime.UtcNow.ToUnixTime();

	private readonly World.World _world = new World.World();

	public void Init(NetManager serverInstance)
	{
		packetManager.Init();
		server = serverInstance;
	}

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
		var opcodeValue = reader.GetShort();
		var opcode = (RAMG.Packets.Opcode) opcodeValue;

		reader.GetBytes(buffer, length);

		var body = buffer.AsSpan(new Range(0, length));
		var command = packetManager.ToCommand(opcode, body);

		commandQueue.Enqueue(command);

		Console.WriteLine($"OnNetworkReceive. Peer Id : {peer.Id}, Remote Id : {peer.RemoteId}, length : {length}, opcode : {opcode}");
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

	public void Update()
	{
		// Queue를 비워주면서 동작.
		while (commandQueue.TryDequeue(out var command))
		{
			var opcode = (RAMG.Packets.Opcode) command.Opcode;

			switch (opcode)
			{
				case RAMG.Packets.Opcode.CMD_ENTITY_MOVE:
				{
					var peers = server.ConnectedPeerList;

					if (command is RAMG.Packets.CMD_ENTITY_MOVE entityMove)
					{
						var netId = entityMove.Id;
						var senderIndex = peers.FindIndex(x => x.Id == netId);

						if (senderIndex >= 0)
						{
							var velocity = new Vector3(entityMove.VelocityX, entityMove.VelocityY, entityMove.VelocityZ);

							if (_world.TryGetEntity(netId, out var entity))
							{
								var movementComponent = entity.GetOrCreateComponent<MovementComponent>();

								entity.Position = new Vector3(entityMove.X, entityMove.Y, entityMove.Z);

								if (!movementComponent.Velocity.IsAlmostCloseTo(velocity))
								{
									movementComponent.Velocity = velocity;

									foreach (var peer in peers)
									{
										if (peer.Id == netId)
										{
											continue;
										}

										if (peer.ConnectionState == ConnectionState.Connected)
										{
											var targetEntityMove = RAMG.Packets.CMD_ENTITY_MOVE.Create();

											targetEntityMove.Id = entityMove.Id;
											targetEntityMove.Time = entityMove.Time;
											targetEntityMove.X = entity.Position.X;
											targetEntityMove.Y = entity.Position.Y;
											targetEntityMove.Z = entity.Position.Z;
											targetEntityMove.VelocityX = entityMove.VelocityX;
											targetEntityMove.VelocityY = entityMove.VelocityY;
											targetEntityMove.VelocityZ = entityMove.VelocityZ;
											targetEntityMove.MoveType = entityMove.MoveType;

											SendInternal(peer, targetEntityMove);
										}
									}
								}
							}
						}
					}

					break;
				}
				case RAMG.Packets.Opcode.CMD_ENTITY_POS_SYNC:
				{
					var peers = server.ConnectedPeerList;

					if (command is RAMG.Packets.CMD_ENTITY_POS_SYNC entityPosSync)
					{
						var netId = entityPosSync.Id;
						var senderIndex = peers.FindIndex(x => x.Id == netId);

						if (senderIndex >= 0)
						{
							if (_world.TryGetEntity(netId, out var entity))
							{
								var movementComponent = entity.GetOrCreateComponent<MovementComponent>();

								entity.Position = new Vector3(entityPosSync.X, entityPosSync.Y, entityPosSync.Z);
								movementComponent.Velocity = Vector3.Zero;

								foreach (var peer in peers)
								{
									if (peer.Id == netId)
									{
										continue;
									}

									if (peer.ConnectionState == ConnectionState.Connected)
									{
										var targetEntityPosSync = RAMG.Packets.CMD_ENTITY_POS_SYNC.Create();

										targetEntityPosSync.Id = entityPosSync.Id;
										targetEntityPosSync.Time = entityPosSync.Time;
										targetEntityPosSync.X = entity.Position.X;
										targetEntityPosSync.Y = entity.Position.Y;
										targetEntityPosSync.Z = entity.Position.Z;

										SendInternal(peer, targetEntityPosSync);
									}
								}
							}
						}
					}

					break;
				}
				case RAMG.Packets.Opcode.CMD_ENTITY_ROTATE:
				{
					var peers = server.ConnectedPeerList;

					if (command is RAMG.Packets.CMD_ENTITY_ROTATE entityRotate)
					{
						var netId = entityRotate.Id;

						foreach (var peer in peers)
						{
							if (peer.Id == netId)
							{
								continue;
							}

							if (peer.ConnectionState == ConnectionState.Connected)
							{
								var targetEntityRotate = RAMG.Packets.CMD_ENTITY_ROTATE.Create();

								targetEntityRotate.Id = entityRotate.Id;
								targetEntityRotate.Time = entityRotate.Time;
								targetEntityRotate.X = entityRotate.X;
								targetEntityRotate.Y = entityRotate.Y;
								targetEntityRotate.Z = entityRotate.Z;
								targetEntityRotate.W = entityRotate.W;

								SendInternal(peer, targetEntityRotate);
							}
						}
					}

					break;
				}
				case RAMG.Packets.Opcode.SCMD_PLAYER_LOGIN:
				{
					var peers = server.ConnectedPeerList;

					if (command is RAMG.Packets.SCMD_PLAYER_LOGIN playerLogin)
					{
						var netId = playerLogin.Id;

						_world.TryCreateEntity(netId);

						foreach (var peer in peers)
						{
							if (peer.Id == netId)
							{
								foreach (var targetPeer in peers)
								{
									if (targetPeer.Id == netId)
									{
										continue;
									}

									var playerServerJoin = RAMG.Packets.CCMD_PLAYER_WORLD_JOIN.Create();

									playerServerJoin.Id = targetPeer.Id;
									playerServerJoin.Time = CurrentTime;

									SendInternal(peer, playerServerJoin);
								}

								continue;
							}

							if (peer.ConnectionState == ConnectionState.Connected)
							{
								var playerServerJoin = RAMG.Packets.CCMD_PLAYER_WORLD_JOIN.Create();

								playerServerJoin.Id = netId;
								playerServerJoin.Time = CurrentTime;

								SendInternal(peer, playerServerJoin);
							}
						}
					}

					break;
				}
			}

			if (command is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	private void SendInternal(NetPeer peer, INetCommand command)
	{
		// 일단 임시로 메모리 스트림 사용...
		using (var ms = new MemoryStream())
		{
			using (var binaryWriter = new BinaryWriter(ms))
			{
				// Writer를 사용해 패킷 생성
				var commandBytes = MemoryPackSerializer.Serialize(command.GetType(), command,
					MemoryPackSerializerOptions.Utf8);
				var length = Convert.ToUInt16(commandBytes.Length);
				var opcode = command.Opcode;

				binaryWriter.Write(length);
				binaryWriter.Write(opcode);
				binaryWriter.Write(commandBytes);

				// 패킷 보내기
				var data = ms.ToArray();
				peer.Send(data, DeliveryMethod.ReliableOrdered);

				if (command is IDisposable disposable)
				{
					// 커맨드를 Dispose
					disposable.Dispose();
				}
			}
		}
	}
}
