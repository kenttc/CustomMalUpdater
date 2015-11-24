

using System.Xml.Serialization;

namespace UtilityDomain.Interface
{
    public interface IXmlSerializerAndDeserializer
    {
        T ConvertFromXml<T>(string xml);
        string ConvertToXml<T>(T obj, XmlSerializerNamespaces xsns, bool omitXmlDeclaration = false);
        BoolResult CheckValidXml(string xml);
    }
}
