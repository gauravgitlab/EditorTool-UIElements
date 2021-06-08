
using System;
using System.Collections.Generic;
using UnityEngine;

[AutoEditorData]
[Serializable]
public class UserData
{
    [SerializeField][AlternatingFormatting(Formatting = new string[] { "color_red_bg", "color_blue_bg" })]
    public List<ChapterData> Chapters = new List<ChapterData>(); 

    public ChapterData GetChapter(eArena arenaType)
    {
        return Chapters.Find(chapter => chapter.ArenaType == arenaType);
    }

    public bool IsChapterExist(ChapterData chapter)
    {
        return Chapters.Find(chap => chap.ArenaType == chapter.ArenaType) != null;
    }

    public void AddChapter(ChapterData chapter)
    {
        Chapters.Add(chapter);
    }

    public void AddLevelInChapter(ChapterData chapterData, int levelId)
    {
        LevelInfo levelInfo = chapterData.GetLevelInfo(levelId);
    }

    public void UpdateLevelInChapter()
    {

    }
}

[AutoEditorData]
[Serializable]
public class ChapterData
{
    [SerializeField]
    public eArena ArenaType;
    
    [AlternatingFormatting(Formatting = new string[] { "color_green_bg", "color_orange_bg" })]
    public List<LevelInfo> LevelSaveData = new List<LevelInfo>();

    public bool IsLevelExist(int levelId)
    {
        return LevelSaveData.Find(level => level.Id == levelId) != null;
    }

    public LevelInfo GetLevelInfo(int levelID)
    {
        return LevelSaveData.Find(level => level.Id == levelID);
    }

    public void SetLevelInfo(LevelInfo levelInfo)
    {
        LevelInfo info = LevelSaveData.Find(level => level.Id == levelInfo.Id);
        if(info == null)
        {
            LevelSaveData.Add(levelInfo);
        }
        else
        {
            if (TimeSpan.Compare(levelInfo.Time, info.Time) == -1)
                info.Set(levelInfo.Kills, levelInfo.Time);
        }
    }

    public int GetLastUnlockLevel()
    {
        return GetLastCompletedLevel() + 1;
    }

    public int GetLastCompletedLevel()
    {
        int lastUnlockLevel = 0;
        for (int i = 0; i < LevelSaveData.Count; i++)
        {
            if (LevelSaveData[i].Id > lastUnlockLevel)
                lastUnlockLevel = LevelSaveData[i].Id;
        }

        return lastUnlockLevel;
    }
}

[AutoEditorData]
[System.Serializable]
public class LevelInfo
{
    public int Id;
    public int Kills;
    public TimeSpan Time;

    public void Set(int kills, TimeSpan time)
    {
        SetKills(kills);
        SetTime(time);
    }

    public void SetKills(int kills)
    {
        Kills = kills;
    }

    public void SetTime(TimeSpan time)
    {
        Time = time;
    }
}
