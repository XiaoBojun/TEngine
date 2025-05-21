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
	///  Gate登录到Chat服务器
	/// </summary>
	[ProtoContract]
	public partial class G2Chat_LoginRequest : AMessage, IRouteRequest, IProto
	{
		public static G2Chat_LoginRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2Chat_LoginRequest>();
		}
		public override void Dispose()
		{
			UserName = default;
			UnitId = default;
			GateRouteId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2Chat_LoginRequest>(this);
#endif
		}
		[ProtoIgnore]
		public Chat2G_LoginResponse ResponseType { get; set; }
		public uint OpCode() { return InnerOpcode.G2Chat_LoginRequest; }
		[ProtoMember(1)]
		public string UserName { get; set; }
		[ProtoMember(2)]
		public long UnitId { get; set; }
		[ProtoMember(3)]
		public long GateRouteId { get; set; }
	}
	[ProtoContract]
	public partial class Chat2G_LoginResponse : AMessage, IRouteResponse, IProto
	{
		public static Chat2G_LoginResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Chat2G_LoginResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			ChatRouteId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Chat2G_LoginResponse>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.Chat2G_LoginResponse; }
		[ProtoMember(1)]
		public long ChatRouteId { get; set; }
		[ProtoMember(2)]
		public uint ErrorCode { get; set; }
	}
	/// <summary>
	///  Gate通知Chat服务器下线
	/// </summary>
	[ProtoContract]
	public partial class G2Chat_OfflineRequest : AMessage, IRouteRequest, IProto
	{
		public static G2Chat_OfflineRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2Chat_OfflineRequest>();
		}
		public override void Dispose()
		{
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2Chat_OfflineRequest>(this);
#endif
		}
		[ProtoIgnore]
		public Chat2G_OfflineResponse ResponseType { get; set; }
		public uint OpCode() { return InnerOpcode.G2Chat_OfflineRequest; }
	}
	[ProtoContract]
	public partial class Chat2G_OfflineResponse : AMessage, IRouteResponse, IProto
	{
		public static Chat2G_OfflineResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Chat2G_OfflineResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Chat2G_OfflineResponse>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.Chat2G_OfflineResponse; }
		[ProtoMember(1)]
		public uint ErrorCode { get; set; }
	}
	/// <summary>
	///  Chat通知Gate发送一个全服广播的聊天信息
	/// </summary>
	[ProtoContract]
	public partial class Chat2G_ChatMessage : AMessage, IRouteMessage, IProto
	{
		public static Chat2G_ChatMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Chat2G_ChatMessage>();
		}
		public override void Dispose()
		{
			ChatInfoTree = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Chat2G_ChatMessage>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.Chat2G_ChatMessage; }
		[ProtoMember(1)]
		public ChatInfoTree ChatInfoTree { get; set; }
	}
	/// <summary>
	///  其他服务器发送聊天消息到Chat
	/// </summary>
	[ProtoContract]
	public partial class Other2Chat_ChatMessage : AMessage, IRouteMessage, IProto
	{
		public static Other2Chat_ChatMessage Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<Other2Chat_ChatMessage>();
		}
		public override void Dispose()
		{
			ChatInfoTree = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<Other2Chat_ChatMessage>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.Other2Chat_ChatMessage; }
		[ProtoMember(1)]
		public ChatInfoTree ChatInfoTree { get; set; }
	}
	/// <summary>
	///  Gate登录到Map服务器
	/// </summary>
	[ProtoContract]
	public partial class G2M_LoginRequest : AMessage, IRouteRequest, IProto
	{
		public static G2M_LoginRequest Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<G2M_LoginRequest>();
		}
		public override void Dispose()
		{
			ChatUnitRouteId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<G2M_LoginRequest>(this);
#endif
		}
		[ProtoIgnore]
		public M2G_LoginResponse ResponseType { get; set; }
		public uint OpCode() { return InnerOpcode.G2M_LoginRequest; }
		[ProtoMember(1)]
		public long ChatUnitRouteId { get; set; }
	}
	[ProtoContract]
	public partial class M2G_LoginResponse : AMessage, IRouteResponse, IProto
	{
		public static M2G_LoginResponse Create(Scene scene)
		{
			return scene.MessagePoolComponent.Rent<M2G_LoginResponse>();
		}
		public override void Dispose()
		{
			ErrorCode = default;
			MapRouteId = default;
#if FANTASY_NET || FANTASY_UNITY
			GetScene().MessagePoolComponent.Return<M2G_LoginResponse>(this);
#endif
		}
		public uint OpCode() { return InnerOpcode.M2G_LoginResponse; }
		[ProtoMember(1)]
		public long MapRouteId { get; set; }
		[ProtoMember(2)]
		public uint ErrorCode { get; set; }
	}
}
