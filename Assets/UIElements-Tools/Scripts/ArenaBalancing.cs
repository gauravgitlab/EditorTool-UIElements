

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum eArena { Dirt = 0, Sand, BlackStone, Stone, Metal, Road }
public enum eEnemyType
{
	Zombie_Big_Hands, Zombie_Boss_1, Zombie1_Female,
	Zombie1_Male, Zombie2_Female, Zombie2_Male,
	Zombie_Army, Zombie_Big_Head, Zombie_Boss_2,
	Zombie_Boss_3, Zombie_Cop
}

public interface IData
{

}

public enum eLevelType
{
	ExitBottom,
	ExitTop,
	ExtraBoundary,
	LeftRight,
	TopBottom
}

public class ArenaData : IData
{
	[JsonProperty("Arenas")]
	public List<Arena> ArenaDataList = new List<Arena>();

	public Arena GetArena(eArena arenaType)
	{
		var index = ArenaDataList.FindIndex(x => x.ArenaType == arenaType);
		return index != -1 ? ArenaDataList[index] : null;
	}
}

public class Arena
{
	public eArena ArenaType;
    public string Name;
    public List<ChapterLevel> Levels;
	public List<eEnemyType> Zombies;
}


public class ChapterLevel
{
    public int LevelID;
    public string LevelName;
    public string BestTime;
}



public class LevelAreaData : IData
{
	[JsonProperty("LevelArea")] 
	public LevelArea LevelArea;
}

public class LevelArea
{
	public List<string> ExitBottom;
	public List<string> ExitTop;
	public List<string> ExtraBoundary;
	public List<string> LeftRight;
	public List<string> TopBottom;

	public string GetLevelName(eLevelType levelType)
	{
		switch (levelType)
		{
			case eLevelType.ExitBottom:
				return ExitBottom[UnityEngine.Random.Range(0, ExitBottom.Count)];
			case eLevelType.ExitTop:
				return ExitTop[UnityEngine.Random.Range(0, ExitTop.Count)];
			case eLevelType.ExtraBoundary:
				return ExtraBoundary[UnityEngine.Random.Range(0, ExtraBoundary.Count)];
			case eLevelType.LeftRight:
				return LeftRight[UnityEngine.Random.Range(0, LeftRight.Count)];
			case eLevelType.TopBottom:
				return TopBottom[UnityEngine.Random.Range(0, TopBottom.Count)];
			default:
				throw new ArgumentOutOfRangeException(nameof(levelType), levelType, null);
		}
	}
}