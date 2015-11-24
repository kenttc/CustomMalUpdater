



using CustomMalUpdaterDomain;
using CustomMalUpdaterDomain.Interfaces;

namespace CustomMalUpdaterServices
{
    using UtilityDomain.Interface;

    public class MalDataParser : IMalDataParser
    {
        private readonly IXmlSerializerAndDeserializer _xmlSerializerAndDeserializer;

        public MalDataParser(IXmlSerializerAndDeserializer xmlSerializerAndDeserializer)
        {
            _xmlSerializerAndDeserializer = xmlSerializerAndDeserializer;
        }

        public AnimeResultContainer ParseFromXml(string xml)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(xml);
            sb.Replace("<title>", "<title><![CDATA[");
            sb.Replace("</title>", "]]></title>");
            sb.Replace("<english>", "<english><![CDATA[");
            sb.Replace("</english>", "]]></english>");
            sb.Replace("<synonyms>", "<synonyms><![CDATA[");
            sb.Replace("</synonyms>", "]]></synonyms>");
            sb.Replace("<synopsis>", "<synopsis><![CDATA[");
            sb.Replace("</synopsis>", "]]></synopsis>");
            
            return _xmlSerializerAndDeserializer.ConvertFromXml<AnimeResultContainer>(sb.ToString());
        }
    }
}
