// ************************************************************************************
// WEB524 Project Template V3 2224 == 316c494a-4a07-4439-9422-a2bbca7fbfc5
// Do not change or remove the line above.
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

using AutoMapper;
using S2022A6LLC.EntityModels;
using S2022A6LLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace S2022A6LLC.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // If necessary, add constructor code here

            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {
                // Define the mappings below, for example...
                // cfg.CreateMap<SourceType, DestinationType>();
                // cfg.CreateMap<Product, ProductBaseViewModel>();

                // Object mapper definitions

                cfg.CreateMap<Models.RegisterViewModel, Models.RegisterViewModelForm>();
                cfg.CreateMap<Genre, GenreBaseViewModel>();
                // Artists
                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                // Albums
                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();

                // Tracks
                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<Track, TrackAudioBaseViewModel>();
                cfg.CreateMap<TrackWithDetailViewModel, TrackEditFormViewModel>();

                // Artist Media Items
                cfg.CreateMap<ArtistMediaItem, ArtistMediaItemBaseViewModel>();
                cfg.CreateMap<ArtistMediaItem, ArtistMediaItemContentViewModel>();
                cfg.CreateMap<Artist, ArtistWithMediaInfo>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }


        // Add your methods below and call them from controllers. Ensure that your methods accept
        // and deliver ONLY view model objects and collections. When working with collections, the
        // return type is almost always IEnumerable<T>.
        //
        // Remember to use the suggested naming convention, for example:
        // ProductGetAll(), ProductGetById(), ProductAdd(), ProductEdit(), and ProductDelete().



        // *** Add your methods above this line **

        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            var genres = ds.Genres.OrderBy(gr => gr.Name);
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(genres);
        }

        // ****************************************
        // ************ Artist Methods ************
        // ****************************************
        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            var artists = ds.Artists.OrderBy(art => art.Name);

            if (artists == null)
            {
                return null;
            }

            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(artists);
        }

        public ArtistWithDetailViewModel ArtistGetByIdWithDetail(int id)
        {
            var artist = ds.Artists.Include("Albums")
                .SingleOrDefault(art => art.Id == id);

            if (artist == null)
            {
                return null;
            }
            return mapper.Map<Artist, ArtistWithDetailViewModel>(artist);
        }

        public ArtistWithDetailViewModel ArtistAdd(ArtistAddViewModel newArtist)
        {
            var addedArtist = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(newArtist));
            addedArtist.Executive = User.Name;
            ds.SaveChanges();

            return addedArtist == null ?
                null
                : mapper.Map<Artist, ArtistWithDetailViewModel>(addedArtist);
        }

        // ****************************************
        // ****** Artist Media Items Methods ******
        // ****************************************

        public ArtistWithMediaInfo ArtistGetByIdWithMediaInfo(int id)
        {
            var o = ds.Artists.Include("ArtistMediaItems").SingleOrDefault(p => p.Id == id);

            return (o == null) ? null : mapper.Map<Artist, ArtistWithMediaInfo>(o);
        }
        public ArtistWithDetailViewModel ArtistAddArtistMediaItem(ArtistMediaItemAddViewModel newItem)
        {
            // find the artist
            var artist = ds.Artists.Find(newItem.ArtistId);

            if (artist == null) { return null; }

            var addedItem = new ArtistMediaItem();
            ds.ArtistMediaItems.Add(addedItem);

            // First, extract the bytes from the HttpPostedFile object
            byte[] photoBytes = new byte[newItem.MediaItemUpload.ContentLength];
            newItem.MediaItemUpload.InputStream.Read(photoBytes, 0, newItem.MediaItemUpload.ContentLength);


            // edit some properties
            // Then, configure the new object's properties
            addedItem.Content = photoBytes;
            addedItem.ContentType = newItem.MediaItemUpload.ContentType;

            // edit some properties
            addedItem.Caption = newItem.Caption;

            addedItem.Artist = artist;

            ds.SaveChanges();

            return (addedItem == null) ? null : mapper.Map<Artist, ArtistWithDetailViewModel>(artist);

        }

        public ArtistMediaItemContentViewModel ArtistMediaItemGetById(string stringId)
        {
            var o = ds.ArtistMediaItems.SingleOrDefault(p => p.StringId == stringId);

            return (o == null) ? null : mapper.Map<ArtistMediaItem, ArtistMediaItemContentViewModel>(o);
        }

        // ****************************************
        // ************ Album Methods ************
        // ****************************************
        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            var albums = ds.Albums.OrderBy(alb => alb.Name);

            if (albums == null)
            {
                return null;
            }

            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(albums);
        }

        public AlbumWithDetailViewModel AlbumGetByIdWithDetail(int id)
        {
            var album = ds.Albums.Include("Artists")
                                 .Include("Tracks")
                                 .SingleOrDefault(alb => alb.Id == id);

            if (album == null)
            {
                return null;
            }

            var result = mapper.Map<Album, AlbumWithDetailViewModel>(album);

            return result;
        }

        public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel addAlbum)
        {
            // if artists found, set the artists property

            var addedAlbum = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(addAlbum));

            // Set the coordinator property
            addedAlbum.Coordinator = User.Name;
            ds.SaveChanges();
            return mapper.Map<Album, AlbumWithDetailViewModel>(addedAlbum);

        }


        // ****************************************
        // ************ Tracks Methods ************
        // ****************************************
        public IEnumerable<TrackBaseViewModel> TrackGetAll()
        {
            var tracks = ds.Tracks.OrderBy(tr => tr.Name);

            if (tracks == null)
            {
                return null;
            }

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(tracks);
        }

        public IEnumerable<TrackWithDetailViewModel> TrackGetAllWithDetail()
        {
            var tracks = ds.Tracks.Include("Albums")
                .OrderBy(tr => tr.Name);

            if (tracks == null)
            {
                return null;
            }

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackWithDetailViewModel>>(tracks);
        }

        public TrackWithDetailViewModel TrackGetByIdWithDetail (int id)
        {
            var track = ds.Tracks.SingleOrDefault(tr => tr.Id == id);

            if (track == null)
            {
                return null;
            }

            var result = mapper.Map<Track, TrackWithDetailViewModel>(track);

            return result;
        }

        public TrackAudioBaseViewModel TrackAudioGetById (int id)
        {
            var track = ds.Tracks.Find(id);
            if (track == null) return null;
            return mapper.Map<Track, TrackAudioBaseViewModel>(track);
        }

        public TrackWithDetailViewModel TrackAdd(TrackAddViewModel newTrack)
        {
            // find the album
            var album = ds.Albums.Find(newTrack.AlbumId);

            if (album == null)
            {
                return null;
            }

            // add the found albums to a list of albums
            ICollection<Album> albumsCol = new List<Album> { album };

            var addedTrack = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(newTrack));

            // First, extract the bytes from the HttpPostedFile object
            byte[] audioBytes = new byte[newTrack.AudioUpload.ContentLength];
            newTrack.AudioUpload.InputStream.Read(audioBytes, 0, newTrack.AudioUpload.ContentLength);

            // Then, configure the new object's properties
            addedTrack.Audio = audioBytes;
            addedTrack.AudioType = newTrack.AudioUpload.ContentType;

            addedTrack.Albums = albumsCol;

            addedTrack.Clerk = User.Name;

            ds.SaveChanges();

            return addedTrack == null ? null : mapper.Map<Track, TrackWithDetailViewModel>(addedTrack);
        }

        public TrackWithDetailViewModel TrackEdit(TrackEditViewModel editedTrack)
        {
            var track = ds.Tracks.Find(editedTrack.Id);

            if (track == null)
            {
                return null;
            }

            // update track data
            // First, extract the bytes from the HttpPostedFile object
            byte[] audioBytes = new byte[editedTrack.AudioUpload.ContentLength];
            editedTrack.AudioUpload.InputStream.Read(audioBytes, 0, editedTrack.AudioUpload.ContentLength);

            // Then, configure the new object's properties
            track.Audio = audioBytes;
            track.AudioType = editedTrack.AudioUpload.ContentType;

            ds.Entry(track).CurrentValues.SetValues(editedTrack);

            ds.SaveChanges();

            return track == null ? null : mapper.Map<Track, TrackWithDetailViewModel>(track);

        }

        public bool TrackDelete(int id)
        {
            // Attempt to fetch the object to be deleted
            var itemToDelete = ds.Tracks.Find(id);

            if (itemToDelete == null)
            {
                return false;
            }
            else
            {
                // Remove the object
                ds.Tracks.Remove(itemToDelete);
                ds.SaveChanges();

                return true;
            }
        }

        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        // Add some programmatically-generated objects to the data store
        // You can write one method or many methods but remember to
        // check for existing data first.  You will call this/these method(s)
        // from a controller action.

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool done = false;

            // *** Role claims ***
            if (ds.RoleClaims.Count() == 0)
            {
                // Add role claims here
                ds.RoleClaims.Add(new RoleClaim { Name = "Clerk" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Coordinator" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Executive" });
                ds.RoleClaims.Add(new RoleClaim { Name = "Staff" });

                ds.SaveChanges();
                done = true;
            }

            // *** Genres ***
            if (ds.Genres.Count() == 0)
            {
                // Add genres here
                ds.Genres.Add(new Genre { Name = "Alternative" });
                ds.Genres.Add(new Genre { Name = "Classical" });
                ds.Genres.Add(new Genre { Name = "Country" });
                ds.Genres.Add(new Genre { Name = "Easy Listening" });
                ds.Genres.Add(new Genre { Name = "Hip-Hop/Rap" });
                ds.Genres.Add(new Genre { Name = "Jazz" });
                ds.Genres.Add(new Genre { Name = "Pop" });
                ds.Genres.Add(new Genre { Name = "R&B" });
                ds.Genres.Add(new Genre { Name = "Rock" });
                ds.Genres.Add(new Genre { Name = "Soundtrack" });

                ds.SaveChanges();
                done = true;
            }

            // *** Artists ***
            if (ds.Artists.Count() == 0)
            {
                // Add artists here

                ds.Artists.Add(new Artist
                {
                    Name = "The Beatles",
                    BirthOrStartDate = new DateTime(1962, 8, 15),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/9/9f/Beatles_ad_1965_just_the_beatles_crop.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Adele",
                    BirthName = "Adele Adkins",
                    BirthOrStartDate = new DateTime(1988, 5, 5),
                    Executive = user,
                    Genre = "Pop",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/Adele_2016.jpg/800px-Adele_2016.jpg"
                });

                ds.Artists.Add(new Artist
                {
                    Name = "Bryan Adams",
                    BirthOrStartDate = new DateTime(1959, 11, 5),
                    Executive = user,
                    Genre = "Rock",
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/7/7e/Bryan_Adams_Hamburg_MG_0631_flickr.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // *** Albums ***
            if (ds.Albums.Count() == 0)
            {
                // Add albums here

                // For "Bryan Adams"
                var bryan = ds.Artists.SingleOrDefault(a => a.Name == "Bryan Adams");

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "Reckless",
                    ReleaseDate = new DateTime(1984, 11, 5),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/5/56/Bryan_Adams_-_Reckless.jpg"
                });

                ds.Albums.Add(new Album
                {
                    Artists = new List<Artist> { bryan },
                    Name = "So Far So Good",
                    ReleaseDate = new DateTime(1993, 11, 2),
                    Coordinator = user,
                    Genre = "Rock",
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/pt/a/ab/So_Far_so_Good_capa.jpg"
                });

                ds.SaveChanges();
                done = true;
            }

            // *** Tracks ***
            if (ds.Tracks.Count() == 0)
            {
                // Add tracks

                // For "Reckless"
                var reck = ds.Albums.SingleOrDefault(a => a.Name == "Reckless");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Run To You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Heaven",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Somebody",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Summer of '69",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { reck },
                    Name = "Kids Wanna Rock",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                // For "So Far So Good"
                var so = ds.Albums.SingleOrDefault(a => a.Name == "So Far So Good");

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Straight from the Heart",
                    Composers = "Bryan Adams, Eric Kagna",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "It's Only Love",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "This Time",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "(Everything I Do) I Do It for You",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.Tracks.Add(new Track
                {
                    Albums = new List<Album> { so },
                    Name = "Heat of the Night",
                    Composers = "Bryan Adams, Jim Vallance",
                    Clerk = user,
                    Genre = "Rock"
                });

                ds.SaveChanges();
                done = true;
            }

            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}