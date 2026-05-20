using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using DocumentManagementSystem.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using DocumentManagementSystem.Business;
using Microsoft.IdentityModel.Tokens;
using Nest;
using OCRWorker.Model;

namespace DocumentManagementSystem.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        IBusinessLayer _bridge;
        private readonly IElasticClient _elastic;
        public DocumentController(IElasticClient elastic)
        {
            _elastic = elastic;   
        }

        private readonly IBusinessLayer _bridge = new BusinessLayer();
        
        [HttpGet]
        public ActionResult<IEnumerable<DocumentData>> GetDocuments()
        {
            var shownDocuments = _bridge.getAllDocuments();
            return Ok(shownDocuments ?? new List<DocumentData>());
        }


        [HttpPost]
        public async Task<IActionResult> NewDocument()
        {
            var form = await Request.ReadFormAsync();
            var file = form["file"];
            var name = form["name"];
            var tags = form["tags"];
            //check if required parts are there
            if (file.IsNullOrEmpty() || name.IsNullOrEmpty()) {
                return StatusCode(400);
            }
            //convert file to byte array
            byte[] data = new byte[file.Count];
            for (int i = 0; i < file.Count; i++)
            {
                data[i] = Convert.ToByte(file[i]);
            }

            if (_bridge.newDocument(name!, data, tags))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteDocument([FromRoute(Name = "id")]string id) 
        {
            //find document with this id and pass it on to delete it
            if (_bridge.deleteDocument(id))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DocumentData> GetDocument([FromRoute(Name = "id")] string id)
        {
            //find specific document and return it
            var res = _bridge.getDocumentById(id);
            if (res != null)
            {
                return Ok(res);
            }
            return StatusCode(404);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDocument([FromRoute(Name = "id")] string id) 
        {
            //find specific document and change to body input
            return StatusCode(501); //Not Implemented
        }

        [HttpGet("{id}/data")]
        public IActionResult GetMetadata([FromRoute(Name = "id")] string id) 
        {
            //find specific document and return its metadata
            return StatusCode(501);//Not Implemented
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, CancellationToken token)
        {
            var response = await _elastic.SearchAsync<DocIndex>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Content)
                        .Query(query)
                    )
                ),
                token
            );

            return Ok(response.Documents);
        }

    }
}

//for complicated body reading
/*using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                // Do something
            }*/
