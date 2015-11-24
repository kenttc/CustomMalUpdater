using System;
using System.Collections.Generic;

namespace CustomMalUpdaterDomain.Interfaces
{
   public interface IAnimeRepo
   {

       void UpdateSynopsisFileLastGenerated(int id, DateTime date);
       
       /// <summary>
       /// gets anime entries where synopsis file hasn't been generated yet
       /// </summary>
       /// <returns></returns>
       List<AnimeEntry> GetNewEntriesToGenerate();


       void UpdateDetails(AnimeEntry anime);

       /// <summary>
       /// get list of anime that DataUpdatedDate column has been set to null
       /// </summary>
       /// <returns></returns>
       List<AnimeEntry> GetDetailsToUpdate();
       /// <summary>
       /// get an ordered list of DISTINCT search terms inserted in the db where is downloadeded is marked false, or null 
       /// </summary>
       /// <returns></returns>
       List<string> GetSearchTerms();
       int AddEntries(List<AnimeEntry> entries);

       void MarkQueryDownloaded(string query);

       /// <summary>
       /// search local db for anime entries that match either title, english title, or synonyms
       /// </summary>
       /// <param name="query"></param>
       /// <returns></returns>
       List<AnimeEntry> Search(string query);
       /// <summary>
       /// add new search term to be searched from the api
       /// </summary>
       /// <param name="searchTerm"></param>
       /// <returns></returns>
       string AddNewSearchTerm(string searchTerm);
       /// <summary>
       /// mark anime entry update for new details from api
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
       string MarkForDataUpdate(int id);

       List<AnimeEntry> GetNotInDb(List<AnimeEntry> entries);
   }
}
