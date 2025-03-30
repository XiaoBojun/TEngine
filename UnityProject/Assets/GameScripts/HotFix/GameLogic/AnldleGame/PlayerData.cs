using System;
using System.Collections;
using System.Collections.Generic;
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

            //skills = new Skill[AnldleGame.Instance.activeSkills.Length];

            quitTime = DateTime.Now;

            // for (int i = 0; i < GameManager.Instance.activeSkills.Length; i++)
            // {
            // 	Skill skill	= new Skill (i, 0, 0, false);
            // 	
            // 	skills[i] = skill;
            // }
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

                //PlayerPrefs.Save (); //By default Unity writes the saved data to disk on application quit. Uncomment this line to write to disk instantly.
            }
        }

        private void PrepareToSave() //get all kinds of game information
        {
            //level = GameManager.Instance.Level;

            money = PlayerStats.Instance.Money;

            //activatedSkillID = PlayerStats.Instance.activeSkill.ID;

            //skills = new Skill[GameManager.Instance.activeSkills.Length];

            quitTime = DateTime.Now;

            // for (int i = 0; i < GameManager.Instance.activeSkills.Length; i++)
            // {
            // 	Skill skill	= new Skill (GameManager.Instance.activeSkills[i].ID, GameManager.Instance.activeSkills[i].DamageLevel, GameManager.Instance.activeSkills[i].SpeedLevel, GameManager.Instance.activeSkills[i].Unlocked);
            // 	
            // 	skills[i] = skill;
            // }
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