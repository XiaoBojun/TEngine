using ProtoBuf;

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Fantasy;
using Fantasy.Network.Interface;
using Fantasy.Serialize;
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantOverriddenMember
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618

namespace Fantasy
{	
	/// <summary>
	///  登录游戏
	/// </summary>
	[ProtoContract]
	public partial class C2G_LoginRequest : AMessage, IRequest, IProto
	{
		public static C2G_LoginRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<C2G_LoginRequest>();
		}
		public override void Dispose()
		{
			UserName = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<C2G_LoginRequest>(this);
#endif
		}
		[ProtoIgnore]
		public G2C_LoginResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.C2G_LoginRequest; }
		[ProtoMember(1)]
		public string UserName { get; set; }
	}
	[ProtoContract]
	public partial class G2C_LoginResponse : AMessage, IResponse, IProto
	{
		public static G2C_LoginResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2C_LoginResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2C_LoginResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.G2C_LoginResponse; }
		[ProtoMember(1)]
		public uint ErrorCode { get; set; }
	}
	/// <summary>
	///  退出游戏
	/// </summary>
	[ProtoContract]
	public partial class C2G_ExitRequest : AMessage, IRequest, IProto
	{
		public static C2G_ExitRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<C2G_ExitRequest>();
		}
		public override void Dispose()
		{
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<C2G_ExitRequest>(this);
#endif
		}
		[ProtoIgnore]
		public G2C_ExitResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.C2G_ExitRequest; }
	}
	[ProtoContract]
	public partial class G2C_ExitResponse : AMessage, IResponse, IProto
	{
		public static G2C_ExitResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2C_ExitResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2C_ExitResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.G2C_ExitResponse; }
		[ProtoMember(1)]
		public uint ErrorCode { get; set; }
	}
	/// <summary>
	///  发送一个聊天消息给Chat服务器，中间是经过Gate中转的
	/// </summary>
	[ProtoContract]
	public partial class C2Chat_SendMessageRequest : AMessage, ICustomRouteRequest, IProto
	{
		public static C2Chat_SendMessageRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<C2Chat_SendMessageRequest>();
		}
		public override void Dispose()
		{
			ChatInfoTree = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<C2Chat_SendMessageRequest>(this);
#endif
		}
		[ProtoIgnore]
		public Chat2C_SendMessageResponse ResponseType { get; set; }
		public uint OpCode() { return OuterOpcode.C2Chat_SendMessageRequest; }
		[ProtoIgnore]
		public int RouteType => Fantasy.RouteType.ChatRoute;
		[ProtoMember(1)]
		public ChatInfoTree ChatInfoTree { get; set; }
	}
	[ProtoContract]
	public partial class Chat2C_SendMessageResponse : AMessage, ICustomRouteResponse, IProto
	{
		public static Chat2C_SendMessageResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Chat2C_SendMessageResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Chat2C_SendMessageResponse>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Chat2C_SendMessageResponse; }
		[ProtoMember(1)]
		public uint ErrorCode { get; set; }
	}
	[ProtoContract]
	public partial class Chat2C_Message : AMessage, ICustomRouteMessage, IProto
	{
		public static Chat2C_Message Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Chat2C_Message>();
		}
		public override void Dispose()
		{
			ChatInfoTree = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Chat2C_Message>(this);
#endif
		}
		public uint OpCode() { return OuterOpcode.Chat2C_Message; }
		[ProtoIgnore]
		public int RouteType => Fantasy.RouteType.ChatRoute;
		[ProtoMember(1)]
		public ChatInfoTree ChatInfoTree { get; set; }
	}
	/// <summary>
	///  聊天消息树
	/// </summary>
	[ProtoContract]
	public partial class ChatInfoTree : AMessage, IProto
	{
		public static ChatInfoTree Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<ChatInfoTree>();
		}
		public override void Dispose()
		{
			ChatChannelType = default;
			ChatChannelId = default;
			UnitId = default;
			UserName = default;
			Target.Clear();
			Node.Clear();
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<ChatInfoTree>(this);
#endif
		}
		[ProtoMember(1)]
		public int ChatChannelType { get; set; }
		[ProtoMember(2)]
		public long ChatChannelId { get; set; }
		[ProtoMember(3)]
		public long UnitId { get; set; }
		[ProtoMember(4)]
		public string UserName { get; set; }
		[ProtoMember(5)]
		public List<long> Target = new List<long>();
		[ProtoMember(6)]
		public List<ChatInfoNode> Node = new List<ChatInfoNode>();
	}
	/// <summary>
	///  聊天信息节点
	/// </summary>
	[ProtoContract]
	public partial class ChatInfoNode : AMessage, IProto
	{
		public static ChatInfoNode Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<ChatInfoNode>();
		}
		public override void Dispose()
		{
			ChatNodeType = default;
			ChatNodeEvent = default;
			Content = default;
			Color = default;
			Data = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<ChatInfoNode>(this);
#endif
		}
		[ProtoMember(1)]
		public int ChatNodeType { get; set; }
		[ProtoMember(2)]
		public int ChatNodeEvent { get; set; }
		[ProtoMember(3)]
		public string Content { get; set; }
		[ProtoMember(4)]
		public string Color { get; set; }
		[ProtoMember(5)]
		public byte[] Data { get; set; }
	}
	/// <summary>
	///  聊天位置信息节点
	/// </summary>
	[ProtoContract]
	public partial class ChatPositionNode : AMessage, IProto
	{
		public static ChatPositionNode Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<ChatPositionNode>();
		}
		public override void Dispose()
		{
			MapName = default;
			PosX = default;
			PosY = default;
			PosZ = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<ChatPositionNode>(this);
#endif
		}
		[ProtoMember(1)]
		public string MapName { get; set; }
		[ProtoMember(2)]
		public float PosX { get; set; }
		[ProtoMember(3)]
		public float PosY { get; set; }
		[ProtoMember(4)]
		public float PosZ { get; set; }
	}
	/// <summary>
	///  聊天位置信息节点
	/// </summary>
	[ProtoContract]
	public partial class ChatOpenUINode : AMessage, IProto
	{
		public static ChatOpenUINode Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<ChatOpenUINode>();
		}
		public override void Dispose()
		{
			UIName = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<ChatOpenUINode>(this);
#endif
		}
		[ProtoMember(1)]
		public string UIName { get; set; }
	}
	/// <summary>
	///  聊天连接信息节点
	/// </summary>
	[ProtoContract]
	public partial class ChatLinkNode : AMessage, IProto
	{
		public static ChatLinkNode Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<ChatLinkNode>();
		}
		public override void Dispose()
		{
			Link = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<ChatLinkNode>(this);
#endif
		}
		[ProtoMember(1)]
		public string Link { get; set; }
	}
	/// <summary>
	///  装备信息实体
	/// </summary>
	[ProtoContract]
	public partial class Item : AMessage, IProto
	{
		public static Item Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Item>();
		}
		public override void Dispose()
		{
			Level = default;
			Name = default;
			HP = default;
			MP = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Item>(this);
#endif
		}
		[ProtoMember(1)]
		public string Level { get; set; }
		[ProtoMember(2)]
		public string Name { get; set; }
		[ProtoMember(3)]
		public string HP { get; set; }
		[ProtoMember(4)]
		public string MP { get; set; }
	}
}
