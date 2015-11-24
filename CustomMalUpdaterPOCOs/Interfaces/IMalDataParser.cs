namespace CustomMalUpdaterDomain.Interfaces
{
    public interface IMalDataParser
    {
        AnimeResultContainer ParseFromXml(string xml);
    }
}
