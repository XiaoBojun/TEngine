// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Fantasy
{
	public enum PokerType
	{
		None = 0,
		Square = 1,
		Club = 2,
		Heart = 3,
		Spade = 4
	}

	public enum PokerValue
	{
		None = 0,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Eight = 8,
		Nine = 9,
		Ten = 10,
		Jack = 11,
		Queen = 12,
		King = 13,
		One = 14,
		Two = 15,
		SJoker = 16,
		LJoker = 17
	}

	public enum PokerLstType
	{
		None = 0,
		Single = 1,
		Pair = 2,
		ThreeWithZero = 3,
		ThreeWithOne = 4,
		ThreeWithPair = 5,
		StraightOne = 6,
		StraightTwo = 7,
		StraightThree = 8,
		PlaneWithOne = 9,
		PlaneWithPair = 10,
		NormalBoom = 11,
		BigBoom = 12
	}

	public enum ErrorCode
	{
		None = 0,
		AcctIsOnline = 1,
		WrongPass = 2,
		SrvDataError = 3,
		OutIndexError = 4,
		OutPokerError = 5,
		UpdateDBError = 6
	}


}