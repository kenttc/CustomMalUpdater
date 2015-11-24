namespace CustomMalUpdaterDomain.Interfaces
{
    public interface IAnimeSynopsisManager
    {

        string PlainText(AnimeEntry anime);
        string Html(AnimeEntry anime);
    }
}