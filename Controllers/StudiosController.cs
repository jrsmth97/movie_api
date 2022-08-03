using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
    public class StudiosController : ControllerBase
    {
        private readonly IStudioRepository _studioRepository;
        private readonly ILogger<StudiosController> _logger;
        private readonly IConfiguration _configuration;

        public StudiosController(
            IStudioRepository studioRepository,
            ILogger<StudiosController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _studioRepository = studioRepository;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("/api/studios")]
        public async Task<IActionResult> GetStudios()
        {
            IList<Studios> studios = await _studioRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/studios] All Studios data accessed");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                studios
            ));
        }

        [Authorize]
        [HttpGet("/api/studios/{id}")]
        public async Task<IActionResult> GetStudio(int id)
        {
            Studios studio = await _studioRepository.GetAsync(id);
            if (studio == null) {
                _logger.LogInformation("[GET: /api/studios/{id}] Studio data not found ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Studio not found" },
                    StatusCodes.Status404NotFound
                ));
            }

            _logger.LogInformation("[GET: /api/studios/{id}] Studio data accessed ( id : " + id + ")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                studio
            ));
        }

        [Authorize]
        [HttpPost("/api/studios")]
        public async Task<IActionResult> AddStudio(Studios studio)
        {
            StudiosValidation Obj = new StudiosValidation();
            ValidationResult Result = Obj.Validate(studio);

            if (Result.IsValid == false) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.CREATE_FAILED,
                    Result,
                    StatusCodes.Status400BadRequest
                ));
            }
            
            _logger.LogInformation($"[POST: /api/studios] Studiocreation requested => '{JsonConvert.SerializeObject(studio)}'");
            Studios createStudio = await _studioRepository.CreateAsync(studio);

            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.CREATE_OK,
                createStudio
            ));
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/studios/delete/{id}")]
        public async Task<IActionResult> DeleteStudio(int id)
        {
            Studios studio = await _studioRepository.DeleteAsync(id);
           
            if (studio == null) 
            {
                _logger.LogInformation("[DELETE: /api/studios/delete/{id}] not exist studio ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Studio not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[DELETE: /api/studios/delete/{id}] Deleting Studio ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                studio
            ));

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/studios/delete/{id}")]
        public async Task<IActionResult> SoftDeleteStudio(int id)
        {
            Studios studio = await _studioRepository.SoftDeleteAsync(id);
           
            if (studio == null) 
            {
                _logger.LogInformation("[PATCH: /api/studios/delete/{id}] not exist Studio ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Studio not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/studios/delete/{id}] Deleting Studio ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                studio
            ));
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/studios/{id}")]
        public async Task<IActionResult> UpdateStudio(int id, Studios studio)
        {
            Studios updateStudio = await _studioRepository.UpdateAsync(id, studio);
             
            if (updateStudio == null) 
            {
                _logger.LogInformation("[PATCH: /api/studios/{id}] not exist Studio ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Studio not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/studios/{id}] Updating Studio ( id : "+ id +")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.UPDATE_OK,
                updateStudio
            ));
        }
    }
}