


namespace UtilityImplementations
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using UtilityDomain.Interface;
    using UtilityDomain;
    

    public class XmlSerializerAndDeserializer : IXmlSerializerAndDeserializer
    {
        public T ConvertFromXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof (T));
            return (T) serializer.Deserialize(new StringReader(xml));
        }

        public string ConvertToXml<T>(T obj, XmlSerializerNamespaces xsns, bool omitXmlDeclaration = false)
        {

            var stringwriter = new StringWriter();
            var xmlwriter = XmlWriter.Create(stringwriter,
                                             new XmlWriterSettings
                                                 {
                                                     OmitXmlDeclaration = omitXmlDeclaration,
                                                     ConformanceLevel = ConformanceLevel.Auto,
                                                     Indent = true
                                                 });
            var serializer = new XmlSerializer(typeof (T));
            serializer.Serialize(xmlwriter, obj, xsns);
            return stringwriter.ToString();

        }

        public BoolResult CheckValidXml(string xml)
        {
            try
            {
                XElement.Parse(xml);
                return new BoolResult() {ErrorMessage = "", OkResult = true};
            }
            catch (Exception ex)
            {

                return new BoolResult() {ErrorMessage = ex.Message, OkResult = false};
            }
        }
    }
}
