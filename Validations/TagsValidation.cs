using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class TagsValidation: AbstractValidator<Tags>
    {
        public TagsValidation()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Nama tag tidak boleh kosong");
        }
    }
}