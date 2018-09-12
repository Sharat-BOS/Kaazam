using Kaazam.Data;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;

namespace Kaazam.Models
{

    public class Character
    {
        #region Constructor
        public Character()
        {

        }
        #endregion
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Character> Friends { get; set; }
        public List<Episode> AppearsIn { get; set; }
        public int StarshipId { get; set; }
        public List<Starship> TravelsIn { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
        #endregion
    }
    public class Droid : Character
    {
        #region Constructor
        public Droid()
        {

        }
        #endregion
        #region Properties     
        //public int Id { get; set; }
        //public int Name { get; set; }
        //public List<Character> Friends { get; set; }
        //public List<Episode> AppearsIn { get; set; }        
        public string PrimaryFunction { get; set; }
        #endregion
    }

    public class Human : Character
    {
        #region Constructor
        public Human()
        {

        }
        //public int Id { get; set; }
        //public int name { get; set; }
        //public List<Character> friends { get; set; }
        //public List<Episodes> appearsIn { get; set; }   

        public string HomePlanet { get; set; }
        public int TotalCredits { get; set; }
        #endregion
        #region Properties      
        public int Height { get; set; } //in Centimeters
        #endregion
    }


    //public class DroidInputType : InputObjectGraphType
    //{
    //    public DroidInputType()
    //    {
    //        Name = "DroidInput";
    //        Field<NonNullGraphType<StringGraphType>>("Name");
    //        Field<ListGraphType<Character>>("Friends");
    //        Field<ListGraphType<Episode>>("AppearsIn");         
    //        Field<ListGraphType<Starship>>("Starships");
    //        Field<StringGraphType>("PrimaryFunction");
    //    }
    //}

    //public class HumanInputType : InputObjectGraphType
    //{
    //    public HumanInputType()
    //    {
    //        Name = "HumanInput";
    //        Field<NonNullGraphType<StringGraphType>>("Name");
    //        Field<ListGraphType<Character>>("Friends");
    //        Field<List<Episode>>("AppearsIn");
    //        Field<List<Starship>>("Starships");
    //        Field<IntGraphType>("TotalCredits");
    //        Field<IntGraphType>("Height");
    //    }
    //}

    public class Episode
    {
        #region Constructor
        public Episode()
        {

        }
        #endregion
        #region Properties
        public int Id { get; set; }
        public string EpisodeName { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        #endregion
    }

    //public enum Episode {
    //    NEWHOPE,EMPIRE,JEDI
    //}

    public class Starship
    {
        #region Constructor
        public Starship()
        {
        }
        #endregion
        #region Properties  
        public int Id { get; set; }
        public int Name { get; set; }              
        public float Length { get; set; } //in Meters    
        #endregion
    }
    

    

    public class StarshipType : ObjectGraphType<Starship>
    {
        public StarshipType(IDroidRepository droidRepository, IHumanRepository humanRepository, IStarshipRepository starshipRepository)
        {
            Field(x => x.Id).Description("Starship id.");
            Field(x => x.Name, nullable: false).Description("Starship Name.");
            Field(x => x.Length, nullable: true).Description("Starship Length.");
            //Field<ListGraphType<DroidType>>("droids",
            //resolve: context => starshipRepository.GetDroidsByStarshipIdAsync(context.Source.Id).Result.ToList());
            //Field<ListGraphType<HumanType>>("humans",
            //resolve: context => starshipRepository.GetHumansByStarshipIdAsync(context.Source.Id).Result.ToList());
           // Field<ListGraphType<Character>>("passengers",
           //resolve: context => starshipRepository.GetPassengerByStarshipIdAsync(context.Source.Id).Result.ToList());
        }
       
    }
    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType(IDroidRepository droidRepository, IStarshipRepository starshipRepository, ICharacterRepository characterRepository)
        {
            Field(x => x.Id).Description("Character id.");
            Field(x => x.Name).Description("Character Name.");
            Field(x => x.PrimaryFunction, nullable: true).Description("Character PrimaryFunction.");
            Field(x => x.AppearsIn, nullable: true).Description("Character AppearsIn Episodes");
            //Field<ListGraphType<Character>>("friends",
            //resolve: context => characterRepository.GetFriendsByCharacterIdAsync(context.Source.Id).Result.ToList());
           // Field<ListGraphType<Starship>>("starships",
           //resolve: context => starshipRepository.GetStarshipsByCharacterIdAsync(context.Source.Id).Result.ToList());
        }
    }

    public class HumanType : ObjectGraphType<Human>
    {
        public HumanType(IStarshipRepository starshipRepository, ICharacterRepository characterRepository)
        {
            Field(x => x.Id).Description("Character id.");
            Field(x => x.Name).Description("Character Name.");          
            Field(x => x.AppearsIn, nullable: true).Description("Character AppearsIn Episodes");           
            //Field<ListGraphType<Character>>("friends",
            //resolve: context => characterRepository.GetFriendsByCharacterIdAsync(context.Source.Id).Result.ToList());
            //Field<ListGraphType<Starship>>("starships",
            //resolve: context => starshipRepository.GetStarshipsByCharacterIdAsync(context.Source.Id).Result.ToList());
        }
    }

   




    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery(IStarshipRepository starshipRepository, ICharacterRepository characterRepository, IDroidRepository droidRepository, IHumanRepository humanRepository)
        {
            Field<StarshipType>(
                "starship",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Episode id" }
                ),
                resolve: context => starshipRepository.GetStarshipAsync(context.GetArgument<int>("id")).Result
            );

           // Field<CharacterType>(
           //    "character",
           //    arguments: new QueryArguments(
           //        new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Character id" }
           //    ),
           //    resolve: context => characterRepository.GetCharacterAsync(context.GetArgument<int>("id")).Result
           //);

            Field<DroidType>(
               "droid",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Character id" }
               ),
               resolve: context => droidRepository.GetDroidAsync(context.GetArgument<int>("id")).Result
           );

            Field<HumanType>(
              "human",
              arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "Character id" }
              ),
              resolve: context => droidRepository.GetDroidAsync(context.GetArgument<int>("id")).Result
            );            
        }
    }


    public class StarWarsMutation : ObjectGraphType
    {
        public StarWarsMutation(IStarshipRepository starshipRepository, ICharacterRepository characterRepository, IDroidRepository droidRepository, IHumanRepository humanRepository)
        {
            Name = "Mutation";

           // Field<EpisodeType>(
           //    "episode",
           //    arguments: new QueryArguments(
           //        new QueryArgument<NonNullGraphType<EpisodeInputType>> { Name = "Episode"}
           //    ),
           //     resolve: context =>
           //     {
           //         var episode = context.GetArgument<Episode>("episode");
           //         return episodeRepository.Add(episode);
           //     }
           //);

          //  Field<DroidType>(
          //     "createDroid",
          //     arguments: new QueryArguments(
          //         new QueryArgument<NonNullGraphType<DroidInputType>> { Name = "Droid" }
          //     ),
          //     resolve: context =>
          //     {
          //         var droid = context.GetArgument<Droid>("droid");
          //         return droidRepository.Add(droid);
          //     }
          // );

          //  Field<HumanType>(
          //    "createHuman",
          //    arguments: new QueryArguments(
          //        new QueryArgument<NonNullGraphType<HumanInputType>> { Name = "Human" }
          //    ),
          //    resolve: context =>
          //    {
          //        var human = context.GetArgument<Human>("human");
          //        return humanRepository.Add(human);
          //    }
          //);

        }
    }


    public class StarWarsSchema : Schema
    {
        public StarWarsSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<StarWarsQuery>();
            Mutation = resolver.Resolve<StarWarsMutation>();

        }

        //public StarWarsSchema(Func<Type, GraphType> resolveType)
        //    : base(resolveType)
        //{
        //    Query = (StarWarsQuery)resolveType(typeof(StarWarsQuery));          
        //}
    }




    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public string Variables { get; set; }
    }


}
