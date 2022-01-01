using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public class LevelDataConstants 
    {
        public enum LevelName { RootLevel, StartMenu, Apartment, None };
        public enum LevelType { Menu, GameLevel, None };
    }

    public class LevelData : ScriptableObject
    {
        public LevelDataConstants.LevelName Name;
        public LevelDataConstants.LevelType Type;

        public static LevelData GetLevelData(LevelDataConstants.LevelName Name)
        {
            LevelData levelData = null;

            if (Name == LevelDataConstants.LevelName.RootLevel)
            {
                levelData = CreateInstance<LevelData>();
                levelData.Type = LevelDataConstants.LevelType.None;
                levelData.Name = LevelDataConstants.LevelName.RootLevel;
            }
            else if (Name == LevelDataConstants.LevelName.StartMenu)
            {
                levelData = CreateInstance<LevelData>();
                levelData.Type = LevelDataConstants.LevelType.Menu;
                levelData.Name = LevelDataConstants.LevelName.StartMenu;
            }
            else if (Name == LevelDataConstants.LevelName.Apartment)
            {
                levelData = CreateInstance<LevelData>();
                levelData.Type = LevelDataConstants.LevelType.GameLevel;
                levelData.Name = LevelDataConstants.LevelName.Apartment;
            }

            return levelData;
        }
    }
}

