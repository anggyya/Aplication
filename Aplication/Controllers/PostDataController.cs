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
        public IHttpActionResult GetPagePost(string page = null, string pagesize = null)
        {
            try
            {
                if (!int.TryParse(page, out int pageNumber) || !int.TryParse(pagesize, out int pageSizeNumber))
                    return BadRequest("Data yang diinput salah, tolong masukkan nilai integer.");

                if (pageNumber <= 0 || pageSizeNumber <= 0)
                    return BadRequest("Data yang di input salah, tolong masukan lebih dari 0");

                var response = _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts").Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Gagal mengambil data dari link json");
                }

                var jsonString = response.Content.ReadAsStringAsync().Result;
                var allPost = JsonConvert.DeserializeObject<List<PostData>>(jsonString);

                var postViewModels = allPost.Select(p => new PostDataView
                {
                    Id = p.Id,
                    Title = p.Title
                }).ToList();

                var totalItems = postViewModels.Count;
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSizeNumber);

                var pageData = postViewModels.Skip((pageNumber - 1) * pageSizeNumber).Take(pageSizeNumber).ToArray();

                var result = new
                {
                    data = pageData
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}