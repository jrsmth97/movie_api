using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Helpers;
using movie_api.Attributes;
using movie_api.Utils;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieTagsController : ControllerBase
    {
        private readonly IMovieTagRepository _movieTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieTagsController> _logger;
        private readonly IConfiguration _configuration;

        public MovieTagsController(
            IMovieTagRepository movieTagRepository,
            ITagRepository tagRepository,
            IMovieRepository movieRepository,
            ILogger<MovieTagsController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _movieTagRepository = movieTagRepository;
            _tagRepository      = tagRepository;
            _movieRepository    = movieRepository;
            _configuration      = configuration;
        }

        [Authorize]
        [HttpGet("/api/movie-tags")]
        public async Task<IActionResult> GetMovieTags()
        {
            IList<MovieTags> movieTags = await _movieTagRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/movie-tags] All MovieTags data accessed");
            return Ok(movieTags);
        }

        [Authorize]
        [HttpGet("/api/movie-tags/{id}")]
        public async Task<IActionResult> GetMovieTag(int id)
        {
            MovieTags movieTag = await _movieTagRepository.GetAsync(id);
            if (movieTag == null) {
                _logger.LogInformation("[GET: /api/movie-tags/{id}] movieTag data not found ( id : " + id + ")");
                return NotFound("Tag not found");
            }

            _logger.LogInformation("[GET: /api/movie-tags/{id}] movieTag data accessed ( id : " + id + ")");
            return Ok(movieTag);
        }

        [Authorize]
        [HttpPost("/api/movie-tags")]
        public async Task<IActionResult> AddMovieTag(MovieTags movieTag)
        {
            MovieTagsValidation Obj = new MovieTagsValidation();
            ValidationResult Result = Obj.Validate(movieTag);

            if (Result.IsValid == false)
                return BadRequest(Result);

            Movies movie = await _movieRepository.GetAsync(movieTag.movie_id);
            if (movie == null)
                return BadRequest("Movie id not exists");

            Tags tag = await _tagRepository.GetAsync(movieTag.tag_id);
            if (tag == null)
                return BadRequest("Tag id not exists");
            
            movieTag.movie = movie;
            movieTag.tag = tag;
            _logger.LogInformation($"[POST: /api/movie-tags] movieTag creation requested => '{JsonConvert.SerializeObject(movieTag)}'");
            MovieTags createTag = await _movieTagRepository.CreateAsync(movieTag);

            return Ok(createTag);
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/movie-tags/delete/{id}")]
        public async Task<IActionResult> DeleteMovieTag(int id)
        {
            MovieTags movieTag = await _movieTagRepository.DeleteAsync(id);
           
            if (movieTag == null) 
            {
                _logger.LogInformation("[DELETE: /api/movie-tags/delete/{id}] movieTag not exist ( id : " + id + ")");
                return NotFound("movieTag not found");
            }
            
            _logger.LogInformation("[DELETE: /api/movie-tags/delete/{id}] Deleting movie Tag ( id : "+id+")");
            return Ok(movieTag);

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movie-tags/delete/{id}")]
        public async Task<IActionResult> SoftDeleteMovieTag(int id)
        {
            MovieTags movieTag = await _movieTagRepository.SoftDeleteAsync(id);
           
            if (movieTag == null) 
            {
                _logger.LogInformation("[PATCH: /api/movie-tags/delete/{id}] not exist movieTag ( id : " + id + ")");
                return NotFound("Tag not found");
            }
            
            _logger.LogInformation("[PATCH: /api/movie-tags/delete/{id}] Deleting movieTag ( id : "+id+")");
            return Ok(movieTag);
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/movie-tags/{id}")]
        public async Task<IActionResult> UpdateMovieTag(int id, MovieTags movieTag)
        {
            MovieTags updateTag = await _movieTagRepository.UpdateAsync(id, movieTag);
             
            if (updateTag == null) 
            {
                _logger.LogInformation("[PATCH: /api/movie-tags/{id}] not exist movieTag ( id : " + id + ")");
                return NotFound("Tag not found");
            }
            
            _logger.LogInformation("[PATCH: /api/movie-tags/{id}] Updating movieTag ( id : "+ id +")");
            return Ok(updateTag);
        }
    }
}