using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class MovieTagsValidation: AbstractValidator<MovieTags>
    {
        public MovieTagsValidation()
        {
            RuleFor(x => x.movie_id).NotEmpty().WithMessage("Movie tidak boleh kosong");
            RuleFor(x => x.tag_id).NotEmpty().WithMessage("Tag tidak boleh kosong");
        }
    }
}