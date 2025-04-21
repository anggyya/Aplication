using Aplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Aplication.Controllers
{
    [RoutePrefix("api/postpagedata")]
    public class PostDataController : ApiController
    {
        private readonly HttpClient _httpClient;

        public PostDataController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult GetPagePost(int page = 1, int pagesize = 10)
        {
            if (page <= 0 || pagesize <= 0)
                return BadRequest("Data yang di input salah, tolong masukan lebih dari 0");

            var response = _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts").Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Gagal mengambil data dari link json");
            }

            var jsonString = response.Content.ReadAsStringAsync().Result;
            var allPost = JsonConvert.DeserializeObject<List<PostDataModel>>(jsonString);

            var postViewModels = allPost.Select(p => new PostDataViewModel
            {
                Id = p.Id,
                Title = p.Title
            }).ToList();

            var totalItems = postViewModels.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pagesize);

            var pageData = postViewModels.Skip((page - 1) * pagesize).Take(pagesize).ToArray();

            var result = new
            {
                data = pageData
            };

            return Ok(result);
        }
    }
}