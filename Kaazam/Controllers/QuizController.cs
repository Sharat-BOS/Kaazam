using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Kaazam.ViewModels;
using GraphQL;
using GraphQL.Types;
using Kaazam.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kaazam.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : Controller
    {

        // GET: api/quiz/
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num=10)
        {
            var sampleQuizzies = new List<QuizViewModel>();
            //add first sample quiz
            sampleQuizzies.Add(new QuizViewModel()
            {
                Id = 1,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Anime-related personality test",
                CreatedDate = DateTime.Now,
                LastModifiedDate= DateTime.Now
            });


            for (int i = 2; i <= num; i++) {
                sampleQuizzies.Add(new QuizViewModel()
                {
                    Id = i,
                    Title = String.Format("Sample Quiz {0}", i),
                    Description = "This is sample quiz",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(
                sampleQuizzies,
                new JsonSerializerSettings() {
                    Formatting = Formatting.Indented
                });
            
         }

        
        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num=10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(t => t.Title), new JsonSerializerSettings() { Formatting = Formatting.Indented });            
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;
            return new JsonResult(sampleQuizzes.OrderBy(t => Guid.NewGuid()), new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }       
}
