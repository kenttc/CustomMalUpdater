using System.Xml.Serialization;
using System;

namespace CustomMalUpdaterDomain
{
    [XmlType(AnonymousType = true, Namespace = "")]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "entry")]
    public class AnimeEntry
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces _xsns;
        public AnimeEntry()
        {
            _xsns = new XmlSerializerNamespaces();
            _xsns.Add(string.Empty, string.Empty);
        }

        public AnimeEntry(int id, string title, string englishTitle, string synonyms, 
            string score, string status,string strStartDate, string strEndDate, string synopsis, 
            string imageUrl, DateTime? synopsisFileLastGenerated)
        {

            Id = id;
            Title = title;
            EnglishTitle = englishTitle;
            Synonyms = synonyms;
            Score = score;
            Status = status;
            StrStartDate = strStartDate;
            StrEndDate = strEndDate;
            Synopsis = synopsis;
            ImageUrl = imageUrl;
            SynopsisFileLastGenerated = synopsisFileLastGenerated;


        }

        [XmlElement("id")]
        public int Id { get; set; }


        [XmlElement("title")]
        public string Title { get; set; }


        [XmlElement("english")]
        public string EnglishTitle { get; set; }

        [XmlElement("synonyms")]
        public string Synonyms { get; set; }

        [XmlElement("episodes")]
        public int Episodes { get; set; }

        [XmlElement("score")]
        public string Score { get; set; }

        [XmlElement("type")]
        public string AnimeType { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("start_date")]
        public string StrStartDate { get; set; }

        [XmlElement("end_date")]
        public string StrEndDate { get; set; }

        [XmlElement("synopsis")]
        public string Synopsis { get; set; }

        [XmlElement("image")]
        public string ImageUrl { get; set; }

        public DateTime? SynopsisFileLastGenerated { get; set; }
    }
}
