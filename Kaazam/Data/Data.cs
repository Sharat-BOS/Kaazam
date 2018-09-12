using Kaazam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaazam.Data
{
    public interface IHumanRepository
    {
        Task<List<Human>> GetHumanAsync();
        Task<Human> GetHumanAsync(int id);
        //Task<List<Human>> GetHumansByEpisodeAsync(string episode);
    }

    public interface IStarshipRepository
    {
        Task<List<Starship>> GetStarshipAsync();
        Task<Starship> GetStarshipAsync(int id);
        //Task<List<Droid>> GetDroidsByStarshipIdAsync();
        //Task<List<Human>> GetHumansByStarshipIdAsync();
        //Task<List<Character>> GetPassengersByStarshipIdAsync();      
    }
    

    public class HumanRepository : IHumanRepository
    {
        private List<Human> _humans;

        public HumanRepository()
        {
            _humans = new List<Human>{
                new Human()
                {
                    Id = 1,
                    Name = "Boba Fett",
                    HomePlanet="Kamino",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                new Human()
                {
                    Id = 2,
                    Name = "Darth Caedus",
                    HomePlanet="Kamino",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                 new Human()
                {
                    Id = 3,
                    Name = " Han Solo",
                    HomePlanet="Alderan",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                new Human()
                {
                    Id = 4,
                    Name = "Leia Organa Solo",
                    HomePlanet="Alderan",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                 new Human()
                {
                    Id = 5,
                    Name = "Jabba the Hutt",
                    HomePlanet="Alderan",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                  new Human()
                {
                    Id = 6,
                    Name = "Chubeca",
                    HomePlanet="Alderan",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                   new Human()
                {
                    Id = 7,
                    Name = "Han Solo",
                    HomePlanet="Alderan",
                    AppearsIn=["JEDI","NEWHOPE","EMPIRE","PHANTOM"],                    
                    TotalCredits=1000,
                    Height=165
                },
                new Human()
                {
                    Id = 8,
                    Name = "Obi-Wan Kenobi",
                    HomePlanet="Tatooine",
                    AppearsIn=["JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                },
                 new Human()
                {
                    Id = 9,
                    Name = "Luke Skywalker",
                    HomePlanet="Tatooine",
                    AppearsIn=["CLONEWARS","EMPIRE","PHANTOM"],
                    Friends=[],
                    TotalCredits=1000,
                    Height=165
                }

            };
        }

        public Task<List<Human>> GetHumanAsync()
        {
            return Task.FromResult(_humans);
        }

        public Task<Human> GetHumanAsync(int id)
        {
            return Task.FromResult(_humans.FirstOrDefault(x => x.Id == id));
        }
        //public Task<Human> GetHumansByEpisodeAsync(int id)
        //{
        //    return Task.FromResult(_humans..FirstOrDefault(x => x.Id == id));
        //}

        public Task<Human> AddFriendAsync(int id, List<Human> friends)
        {
            var human = _humans.FirstOrDefault(x => x.Id == id);
            foreach (Human person in friends)
            {
                human.Friends.Add(person);
            }
            return Task.FromResult(human);
        }
        public Task<Human> AddStarshipAsync(int id, List<Starship> starships)
        {
            var human = _humans.FirstOrDefault(x => x.Id == id);
            foreach (Starship ship in starships)
            {
                human.Starships.Add(ship);
            }
            return Task.FromResult(human);
        }
    }

    public interface IDroidRepository
    {
        Task<List<Droid>> DroidAsync();
        Task<Droid> GetDroidAsync(int id);

        Task<Droid> AddFriendAsync(int id, List<Human> friends);

        Task<Droid> AddStarshipAsync(int id, List<Starship> starships);
    }
    public class DroidRepository : IDroidRepository
    {
        private List<Droid> _droids;

        public DroidRepository()
        {
            _droids = new List<Droid>{
                new Droid()
                {
                    Id = 10,
                    Name = "C3P0",                   
                    AppearsIn=new List<string>(){"JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM" },                   
                    PrimaryFunction="Translator"
                     // Friends=[],
                    //Starships=[]
                },
                new Droid()
                {
                    Id = 11,
                    Name = "R2D2",
                    AppearsIn=new List<string>(){"JEDI","NEWHOPE","CLONEWARS","EMPIRE","PHANTOM" },
                    PrimaryFunction="Repair Droid"
                     // Friends=[],
                    //Starships=[]
                },
                new Droid()
                {
                    Id = 12,
                    Name = "R2D3",
                    AppearsIn=new List<string>(){"JEDI","NEWHOPE","CLONEWARS"},
                    PrimaryFunction="Repair Droid"
                     // Friends=[],
                    //Starships=[]
                },
                new Droid()
                {
                    Id = 10,
                    Name = "Battle Droid",
                   AppearsIn=new List<string>(){"JEDI","CLONEWARS","PHANTOM" },
                    PrimaryFunction="Battle Droid",
                    // Friends=[],
                    //Starships=[]

                },
            };
        }

        public Task<List<Droid>> DroidAsync()
        {
            return Task.FromResult(_droids);
        }

        public Task<Droid> GetDroidAsync(int id)
        {
            return Task.FromResult(_droids.FirstOrDefault(x => x.Id == id));
        }

        public Task<Droid> AddFriendAsync(int id,List<Human> friends)
        {
            var droid = _droids.FirstOrDefault(x => x.Id == id);
            foreach (Human person in friends) {
                droid.Friends.Add(person);
            }            
            return Task.FromResult(droid);
        }
        public Task<Droid> AddStarshipAsync(int id, List<Starship> starships)
        {
            var droid = _droids.FirstOrDefault(x => x.Id == id);
            foreach (Starship ship in starships)
            {
                droid.Starships.Add(ship);
            }
            return Task.FromResult(droid);
        }
    }

    public class StarshipRepository : IStarshipRepository
    {
        private List<Starship> _starships;

        public StarshipRepository()
        {
            _starships = new List<Starship>{
                new Starship()
                {
                    Id = 1,                                      
                    Name = "TIE Interceptor",
                    // Droids=[],
                    //Humans=[]
                },
                new Starship()
                {
                    Id = 2,                    
                    Name = "X-wing Starfighter",
                    // Droids=[],
                    //Humans=[]
                },
                new Starship()
                {
                    Id = 3,                    
                    Name = "Millennium Falcon",
                    // Droids=[],
                    //Humans=[]
                }
            };
        }

        public Task<Starship> GetStarshipAsync(int id)
        {
            return Task.FromResult(_starships.FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Starship>> GetStarshipAsync()
        {
            return Task.FromResult(_starships);
        }

        //public Task<List<Starship>> GetStarshipsByHumanIdAsync(int episodeId)
        //{
        //    return Task.FromResult(_starships.Where(x => x.HumanId == episodeId).ToList());
        //}

        //public Task<Starship> AddStarshipAsync(Starship starship)
        //{
        //    starship.Id = _starships.Count + 1;
        //    _starships.Add(starship);
        //    return Task.FromResult(_starships.FirstOrDefault(x=>x.Id== starship.Id));            
        //}
        //public Task<List<Starship>> DeleteStarshipAsync(int starshipId)
        //{
        //    var starship = _starships.FirstOrDefault(x => x.Id == starshipId);
        //    _starships.Remove(starship);
        //    return Task.FromResult(_starships);
        //}

    }
}
