using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class MovieSchedulesValidation : AbstractValidator<MovieSchedules>
    {
        public MovieSchedulesValidation()
        {
            RuleFor(x => x.movie_id).NotEmpty().WithMessage("Movie akun tidak boleh kosong");
            RuleFor(x => x.studio_id).NotEmpty().WithMessage("Studio tidak boleh kosong");
            RuleFor(x => x.start_time).NotEmpty().WithMessage("Waktu mulai tidak boleh kosong");
            RuleFor(x => x.price).NotEmpty().WithMessage("Harga tidak boleh kosong");
            RuleFor(x => x.date).NotEmpty().WithMessage("Tanggaltidak boleh kosong"); 
        }
    }
}