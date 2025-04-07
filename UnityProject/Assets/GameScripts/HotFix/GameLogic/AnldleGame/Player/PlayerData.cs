using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace GameLogic
{
    public class Skill
    {
        [XmlAttribute("ID")]
        public int ID;

        public int damageLevel;
        public int speedLevel;
        public bool unlocked;

        public Skill()
        {
        }

        public Skill(int ID, int damageLevel, int speedLevel, bool unlocked)
        {
            this.ID = ID;
            this.damageLevel = damageLevel;
            this.speedLevel = speedLevel;
            this.unlocked = unlocked;
        }
    }

//all public variables in this class will be serialized, and saved to a xml file
    [XmlRoot("PlayerData")]
    public class PlayerData
    {
        public int level;

        public int money;

        public int activatedSkillID;

        public DateTime quitTime;

        [XmlArray("Skills")]
        [XmlArrayItem("Skill")]
        public Skill[] skills;

        public void Reset() //function used when starting a new game
        {
            level = 1;

            money = 0;

            activatedSkillID = 0;

            skills = new Skill[AnldleGame.Instance.activeSkills.Length];

            quitTime = DateTime.Now;

            for (int i = 0; i < AnldleGame.Instance.activeSkills.Length; i++)
            {
                Skill skill = new Skill(i, 0, 0, false);

                skills[i] = skill;
            }
        }

        public void Save() //save the game in a xml file
        {
            PrepareToSave();

            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

            using (FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.xml", FileMode.Create))
            {
                serializer.Serialize(stream, this);
            }
        }

        public void SaveForWeb() //save method for web player
        {
            PrepareToSave();

            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);

                PlayerPrefs.SetString("PlayerData", writer.ToString());

                PlayerPrefs.Save (); //By default Unity writes the saved data to disk on application quit. Uncomment this line to write to disk instantly.
            }
        }

        private void PrepareToSave() //get all kinds of game information
        {
            level = AnldleGame_Data.Instance.Level;

            money = AnldleGame_Data.Instance.Money;

            activatedSkillID = AnldleGame_Data.Instance.activatedSkillID;

            skills = new Skill[AnldleGame.Instance.activeSkills.Length];

            quitTime = DateTime.Now;

            for (int i = 0; i < AnldleGame.Instance.activeSkills.Length; i++)
            {
                Skill skill = new Skill(AnldleGame.Instance.activeSkills[i].ID, AnldleGame.Instance.activeSkills[i].DamageLevel, AnldleGame.Instance.activeSkills[i].SpeedLevel,
                    AnldleGame.Instance.activeSkills[i].Unlocked);

                skills[i] = skill;
            }
        }

        public static PlayerData Load() //load the game from the xml file
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

            using (FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.xml", FileMode.Open))
            {
                return serializer.Deserialize(stream) as PlayerData;
            }
        }

        public static PlayerData LoadForWeb() //load method for web player, as the Load() method won't work on web platform
        {
            string savedGame = PlayerPrefs.GetString("PlayerData");

            var serializer = new XmlSerializer(typeof(PlayerData));

            return serializer.Deserialize(new StringReader(savedGame)) as PlayerData;
        }
    }
}