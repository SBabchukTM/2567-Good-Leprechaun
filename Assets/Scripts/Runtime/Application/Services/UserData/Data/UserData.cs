using System;
using System.Collections.Generic;
using UnityEngine;


namespace Application.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public int Money = 0;
        public List<int> BoughtButtonsId = new List<int>() { 0 };
        public int UsedButtonId = 0;
        public Color UsedButtonColor = Color.blue;
    }
}