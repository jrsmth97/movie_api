using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class StudiosValidation: AbstractValidator<Studios>
    {
        public StudiosValidation()
        {
            RuleFor(x => x.studio_number).NotEmpty().WithMessage("Nomor studio tidak boleh kosong");
            RuleFor(x => x.seat_capacity).NotEmpty().WithMessage("Kapasitas tempat duduk tidak boleh kosong");
        }
    }
}