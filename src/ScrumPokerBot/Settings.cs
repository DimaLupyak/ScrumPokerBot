using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ScrumPokerBot
{
    [XmlRoot("Settings")]
    [XmlInclude(typeof(Voice))]
    public class Settings
    {

        [XmlArray("VoiceList"), XmlArrayItem("Voice")]
        public List<Voice> VoiceList { get; set; }

        private XmlSerializer SettingSerializer = new XmlSerializer(typeof(Settings));
        private string SettingFile { get; set; }

        private Settings() { }

        public Settings(string file)
        {
            SettingFile = file;
            Load();
        }

        public void Save()
        {
            using (FileStream fs = new FileStream(SettingFile, FileMode.OpenOrCreate))
            {
                SettingSerializer.Serialize(fs, this);
            }
        }
        private void Load()
        {

            using (FileStream fs = new FileStream(SettingFile, FileMode.OpenOrCreate))
            {
                try
                {
                    var settings = (Settings)SettingSerializer.Deserialize(fs);
                    VoiceList = settings.VoiceList;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (VoiceList == null)
                        VoiceList = new List<Voice>();
                }
            }
        }
    }

    [XmlType("Voice")]
    public class Voice
    {
        [XmlElement("Users")]
        public List<string> Users { get; set; }
        [XmlElement("Question")]
        public string Question { get; set; }
        [XmlElement("Answers")]
        public string[] Answers { get; set; }
        [XmlElement("VotesFile")]
        public SerializableDictionary<string, int> Votes { get; set; }

        [XmlElement("MessageId")]
        public int MessageId { get; set; }
        [XmlElement("IsOpened")]
        public bool IsOpened { get; set; }

        public Voice() { }
        public Voice(int mId, string question, string[] answers)
        {
            MessageId = mId;
            Question = question;
            Answers = answers;
            Users = new List<string>();
            Votes = new SerializableDictionary<string, int>();
        }
    }

    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
