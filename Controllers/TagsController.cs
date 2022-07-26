using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Attributes;
using movie_api.Utils;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagsController> _logger;
        private readonly IConfiguration _configuration;

        public TagsController(
            ITagRepository tagRepository,
            ILogger<TagsController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _tagRepository = tagRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("/api/tags")]
        public async Task<IActionResult> GetTags()
        {
            IList<Tags> tags = await _tagRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/tags] All Tags data accessed");
            return Ok(tags);
        }

        [Authorize]
        [HttpGet("/api/tags/{id}")]
        public async Task<IActionResult> GetTag(int id)
        {
            Tags tag = await _tagRepository.GetAsync(id);
            if (tag == null) {
                _logger.LogInformation("[GET: /api/tags/{id}] tag data not found ( id : " + id + ")");
                return NotFound("Tag not found");
            }

            _logger.LogInformation("[GET: /api/tags/{id}] tag data accessed ( id : " + id + ")");
            return Ok(tag);
        }

        [Authorize]
        [HttpPost("/api/tags")]
        public async Task<IActionResult> AddTag(Tags tag)
        {
            TagsValidation Obj = new TagsValidation();
            ValidationResult Result = Obj.Validate(tag);

            if (Result.IsValid == false) {
                return BadRequest(Result);
            }
            
            _logger.LogInformation($"[POST: /api/tags] tag creation requested => '{JsonConvert.SerializeObject(tag)}'");
            Tags createTag = await _tagRepository.CreateAsync(tag);

            return Ok(createTag);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/tags/delete/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            Tags tag = await _tagRepository.DeleteAsync(id);
           
            if (tag == null) 
            {
                _logger.LogInformation("[DELETE: /api/tags/delete/{id}] tag not exist ( id : " + id + ")");
                return NotFound("tag not found");
            }
            
            _logger.LogInformation("[DELETE: /api/tags/delete/{id}] Deleting Tag ( id : "+id+")");
            return Ok(tag);

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/tags/delete/{id}")]
        public async Task<IActionResult> SoftDeleteTag(int id)
        {
            Tags tag = await _tagRepository.SoftDeleteAsync(id);
           
            if (tag == null) 
            {
                _logger.LogInformation("[PATCH: /api/tags/delete/{id}] not exist Tag ( id : " + id + ")");
                return NotFound("Tag not found");
            }
            
            _logger.LogInformation("[PATCH: /api/tags/delete/{id}] Deleting Tag ( id : "+id+")");
            return Ok(tag);
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/tags/{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Tags tag)
        {
            Tags updateTag = await _tagRepository.UpdateAsync(id, tag);
             
            if (updateTag == null) 
            {
                _logger.LogInformation("[PATCH: /api/tags/{id}] not exist Tag ( id : " + id + ")");
                return NotFound("Tag not found");
            }
            
            _logger.LogInformation("[PATCH: /api/tags/{id}] Updating Tag ( id : "+ id +")");
            return Ok(updateTag);
        }
    }
}