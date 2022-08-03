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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MoviesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppUtils _utils;

        public MoviesController(
            IMovieRepository movieRepository,
            ILogger<MoviesController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _configuration = configuration;
            _utils = new AppUtils(_configuration);
        }

        [Authorize]
        [HttpGet("/api/movies")]
        public async Task<IActionResult> GetMovies([FromQuery] QueryParamsMovies qParams)
        {
            IList<Movies> movies = await _movieRepository.GetListAsync(qParams);
            int totalData = await _movieRepository.GetCountListAsync(qParams);
            
            MovieListData listData = new MovieListData{
                page = qParams.page,
                limit = qParams.limit,
                total_data = totalData,
                search_query = new { title = qParams.title },
                movies = movies
            };

            _logger.LogInformation("[GET: /api/movies] All movies data accessed");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                listData
            ));
        }

        [Authorize]
        [HttpGet("/api/movies/{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            Movies movie = await _movieRepository.GetAsync(id);

            if (movie == null) {
                _logger.LogInformation("[GET: /api/movies/{id}] movie data not found ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Movie not found" },
                    StatusCodes.Status404NotFound
                ));
            }

            _logger.LogInformation("[GET: /api/movies/{id}] movie data accessed ( id : " + id + ")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.GET_OK,
                movie
            ));
        }

        [Authorize]
        [HttpPost("/api/movies")]
        public async Task<IActionResult> AddMovies(Movies movie)
        {
            MoviesValidation Obj = new MoviesValidation();
            ValidationResult Result = Obj.Validate(movie);

            if (Result.IsValid == false) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.CREATE_FAILED,
                    Result,
                    StatusCodes.Status400BadRequest
                ));
            }
            
            _logger.LogInformation($"[POST: /api/movies] Moviecreation requested => '{JsonConvert.SerializeObject(movie)}'");
            Movies createMovie = await _movieRepository.CreateAsync(movie);

            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.CREATE_OK,
                createMovie
            ));
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/movies/delete/{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            Movies movie = await _movieRepository.DeleteAsync(id);
           
            if (movie == null) 
            {
                _logger.LogInformation("[DELETE: /api/movies/delete/{id}] not exist Movie ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Movie not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[DELETE: /api/movies/delete/{id}] Deleting Movie ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                movie
            ));
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movies/delete/{id}")]
        public async Task<IActionResult> SoftDeleteMovie(int id)
        {
            Movies movie = await _movieRepository.SoftDeleteAsync(id);
           
            if (movie == null) 
            {
                _logger.LogInformation("[PATCH: /api/movies/delete/{id}] not exist Movie ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Movie not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/movies/delete/{id}] Deleting Movie ( id : "+id+")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.DELETE_OK,
                movie
            ));
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movies/{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movies movie)
        {
            Movies updateMovie = await _movieRepository.UpdateAsync(id, movie);
             
            if (updateMovie == null) 
            {
                _logger.LogInformation("[PATCH: /api/movies/{id}] not exist Movie ( id : " + id + ")");
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.NOT_FOUND,
                    new string[] { "Movie not found" },
                    StatusCodes.Status404NotFound
                ));
            }
            
            _logger.LogInformation("[PATCH: /api/movies/{id}] Updating Movie ( id : "+ id +")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.UPDATE_OK,
                updateMovie
            ));
        }
    }
}