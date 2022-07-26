using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class MoviesValidation: AbstractValidator<Movies>
    {
        public MoviesValidation()
        {
            RuleFor(x => x.title).NotEmpty().WithMessage("Judul tidak boleh kosong");
            RuleFor(x => x.overview).NotEmpty().WithMessage("Sinopsis tidak boleh kosong");
            RuleFor(x => x.poster).NotEmpty().WithMessage("Poster tidak boleh kosong");
            RuleFor(x => x.play_until).NotEmpty().WithMessage("Waktu berhenti tidak boleh kosong");
        }
    }
}