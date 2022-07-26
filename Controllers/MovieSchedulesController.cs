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

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieSchedulesController : ControllerBase
    {
        private readonly IMovieScheduleRepository _movieScheduleRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly ILogger<MovieSchedulesController> _logger;
        private readonly IConfiguration _configuration;

        public MovieSchedulesController(
            IMovieScheduleRepository movieScheduleRepository,
            IMovieRepository movieRepository,
            IStudioRepository studioRepository,
            ILogger<MovieSchedulesController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _configuration = configuration;
            _movieScheduleRepository = movieScheduleRepository;
            _movieRepository = movieRepository;
            _studioRepository = studioRepository;
        }

        [Authorize]
        [HttpGet("/api/movie-schedules")]
        public async Task<IActionResult> GetMovieSchedules()
        {
            IList<MovieSchedules> movieSchedules = await _movieScheduleRepository.GetListAsync();
            _logger.LogInformation("[gGET: /api/movie-schedules] All MovieSchedules data accessed");
            return Ok(movieSchedules);
        }

        [Authorize]
        [HttpGet("/api/movie-schedules/{id}")]
        public async Task<IActionResult> GetMovieSchedule(int id)
        {
            MovieSchedules movieSchedule = await _movieScheduleRepository.GetAsync(id);
            if (movieSchedule == null) {
                _logger.LogInformation("[GET: /api/movie-schedules/{id}] movieSchedule data not found ( id : " + id + ")");
                return NotFound("Schedule not found");
            }

            _logger.LogInformation("[GET: /api/movie-schedules/{id}] movieSchedule data accessed ( id : " + id + ")");
            return Ok(movieSchedule);
        }

        [Authorize]
        [HttpPost("/api/movie-schedules")]
        public async Task<IActionResult> AddMovieSchedule(MovieSchedules movieSchedule)
        {
            MovieSchedulesValidation Obj = new MovieSchedulesValidation();
            ValidationResult Result = Obj.Validate(movieSchedule);

            if (Result.IsValid == false) {
                return BadRequest(Result);
            }

            Movies movie = await _movieRepository.GetAsync(movieSchedule.movie_id);
            if (movie == null)
                return BadRequest("Movie id not exists");

            Studios studio = await _studioRepository.GetAsync(movieSchedule.studio_id);
            if (studio == null)
                return BadRequest("Studio id not exists");
            
            movieSchedule.movie = movie;
            movieSchedule.studio = studio;
            _logger.LogInformation($"[POST: /api/movie-schedules] movieSchedule creation requested => '{JsonConvert.SerializeObject(movieSchedule)}'");
            MovieSchedules createSchedule = await _movieScheduleRepository.CreateAsync(movieSchedule);

            return Ok(createSchedule);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/movie-schedules/delete/{id}")]
        public async Task<IActionResult> DeleteMovieSchedule(int id)
        {
            MovieSchedules movieSchedule = await _movieScheduleRepository.DeleteAsync(id);
           
            if (movieSchedule == null) 
            {
                _logger.LogInformation("[DELETE: /api/movie-schedules/delete/{id}] movieSchedule not exist ( id : " + id + ")");
                return NotFound("movieSchedule not found");
            }
            
            _logger.LogInformation("[DELETE: /api/movie-schedules/delete/{id}] Deleting movie Schedule ( id : "+id+")");
            return Ok(movieSchedule);

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movie-schedules/delete/{id}")]
        public async Task<IActionResult> SoftDeleteMovieSchedule(int id)
        {
            MovieSchedules movieSchedule = await _movieScheduleRepository.SoftDeleteAsync(id);
           
            if (movieSchedule == null) 
            {
                _logger.LogInformation("[PATCH: /api/movie-schedules/delete/{id}] not exist movieSchedule ( id : " + id + ")");
                return NotFound("Schedule not found");
            }
            
            _logger.LogInformation("[PATCH: /api/movie-schedules/delete/{id}] Deleting movieSchedule ( id : "+id+")");
            return Ok(movieSchedule);
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movie-schedules/{id}")]
        public async Task<IActionResult> UpdateMovieSchedule(int id, MovieSchedules movieSchedule)
        {
            MovieSchedules updateSchedule = await _movieScheduleRepository.UpdateAsync(id, movieSchedule);
             
            if (updateSchedule == null) 
            {
                _logger.LogInformation("[PATCH: /api/movie-schedules/{id}] not exist movieSchedule ( id : " + id + ")");
                return NotFound("Schedule not found");
            }
            
            _logger.LogInformation("[PATCH: /api/movie-schedules/{id}] Updating movieSchedule ( id : "+ id +")");
            return Ok(updateSchedule);
        }
    }
}