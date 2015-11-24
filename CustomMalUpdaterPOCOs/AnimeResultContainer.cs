using System.Xml.Serialization;

namespace CustomMalUpdaterDomain
{
    [XmlRoot("anime")]
    public class AnimeResultContainer
    {
        [XmlElement("entry")]
        public AnimeEntry[] Entries { get; set; }
    }
}
