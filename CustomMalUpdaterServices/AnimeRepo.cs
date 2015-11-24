using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMalUpdaterDatalayer;
using CustomMalUpdaterDomain;
using CustomMalUpdaterDomain.Interfaces;
using UtilityImplementations.Extensions;

namespace CustomMalUpdaterServices
{
    public class AnimeRepo : IAnimeRepo
    {
        public AnimeRepo()
        {
            CreateAutoMapperMappings();
        }

        private static void CreateAutoMapperMappings()
        {
            AutoMapper.Mapper.CreateMap<AnimeEntry, Anime>()
                      .ForMember(x => x.EndDate, opt => opt.MapFrom(src => src.StrEndDate.GetValidDate()))
                      .ForMember(y => y.StartDate, opt => opt.MapFrom(src => src.StrStartDate.GetValidDate()));

            AutoMapper.Mapper.CreateMap<Anime, AnimeEntry>()
                .ForMember(y => y.StrStartDate, opt => opt.MapFrom(src => src.StartDate.Value.ToString("yyyy-MM-dd")))
                .ForMember(y => y.StrEndDate, opt => opt.MapFrom(src => src.EndDate.Value.ToString("yyyy-MM-dd")));

        }

        public void UpdateSynopsisFileLastGenerated(int id, DateTime date)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var dbObj = dbContext.Animes.First(x => x.Id == id);
                dbObj.SynopsisFileLastGenerated = DateTime.Now;
                dbContext.SaveChanges();
            }
        }

        public List<AnimeEntry> GetNewEntriesToGenerate()
        {
            using (var dbContext = new AnimeTempEntities())
            {
         
                var dbResults =
                    dbContext.Animes.Where(x => x.StartDate != null && x.SynopsisFileLastGenerated == null).ToList();
                return dbResults.Select(x =>
                    {
                        var animePoco = new AnimeEntry();
                        AutoMapper.Mapper.Map(x, animePoco);
                        return animePoco;
                    })
                                .ToList();
            }
        }

        public void UpdateDetails(AnimeEntry anime)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                //var dbObjs = dbContext.Animes.Where(x => x.DataUpdatedDate == null).ToList();

                var dbObj = dbContext.Animes.First(x => x.Id == anime.Id);
                AutoMapper.Mapper.Map(anime, dbObj);
                dbObj.DataUpdatedDate = DateTime.Now;
                dbContext.SaveChanges();
            }
        }

        public List<AnimeEntry> GetDetailsToUpdate()
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var dbObjs = dbContext.Animes.Where(x => x.DataUpdatedDate == null).ToList();
                
                return dbObjs.Select(x => new AnimeEntry(x.Id, x.Title, x.EnglishTitle, x.Synonyms,
                                                         Convert.ToString(x.Score), x.Status,
                                                         x.StartDate == null
                                                             ? ""
                                                             : x.StartDate.Value.ToString("yyyy-MM-dd"),
                                                         x.EndDate == null
                                                             ? ""
                                                             : x.EndDate.Value.ToString("yyyy-MM-dd")
                                                         , x.Synopsis, x.ImageUrl, x.SynopsisFileLastGenerated)).ToList();
            }

        }

        public List<string> GetSearchTerms()
        {
            using (var dbContext = new AnimeTempEntities())
            {
                return dbContext.AnimeHomes.Where(
                        x => (x.downloaded == null || x.downloaded == false))
                             .OrderBy(x => x.AnimeTitle)
                             .Select(x => x.AnimeTitle)
                             .Distinct()
                             .ToList();
            }
        }

        public int AddEntries(List<AnimeEntry> entries)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                foreach (var entry in entries)
                {
                    var dbObj = AutoMapper.Mapper.Map<Anime>(entry);
                    dbObj.DataUpdatedDate = DateTime.Now;
                    dbContext.Animes.Add(dbObj);
                    dbContext.SaveChanges();
                }
                return entries.Count;
            }
        }

        public void MarkQueryDownloaded(string query)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var dbObjs = dbContext.AnimeHomes.Where(x => x.AnimeTitle == query).ToList();
                foreach (var obj in dbObjs)
                {
                    obj.downloaded = true;
                }
                dbContext.SaveChanges();
            }
        }

        public List<AnimeEntry> Search(string query)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var dbObjs =
                    dbContext.Animes.Where(
                        x => x.Title.Contains(query) || x.Synonyms.Contains(query) || x.EnglishTitle.Contains(query)).
                              OrderBy(x => x.Title).ToList();

                var animeList = new List<AnimeEntry>();
                foreach (var anime in dbObjs)
              {
                  var animePoco = new AnimeEntry();
                  AutoMapper.Mapper.Map(anime, animePoco);
                    animeList.Add(animePoco);
                }
                return animeList;
             
            }
        }


        public string AddNewSearchTerm(string searchTerm)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var existing = dbContext.AnimeHomes.Any(x => x.AnimeTitle == searchTerm.Trim());
                if (existing)
                    return searchTerm + " already in db";
                
                dbContext.AnimeHomes.Add(new AnimeHome()
                {
                    AnimeTitle = searchTerm,
                    downloaded = false,
                });
                dbContext.SaveChanges();

               return searchTerm   + " saved";
                
            }
        }

        public string MarkForDataUpdate(int id)
        {
            using (var dbContext = new AnimeTempEntities())
            {
                var dbObj = dbContext.Animes.First(x => x.Id == id);
                dbObj.SynopsisFileLastGenerated = null;
                dbObj.DataUpdatedDate = null;
                dbContext.SaveChanges();
                return  id + " flag set for refresh";
            }
         
        }

        public List<AnimeEntry> GetNotInDb(List<AnimeEntry> entries)
        {
            var entryIds = entries.Select(x => x.Id).ToList();

            using (var dbContext = new AnimeTempEntities())
            {
                
                return  entries.Where(x => entryIds.Except(dbContext.Animes.Select(y => y.Id)).Contains(x.Id)).ToList();
            }
        }
    }
}
