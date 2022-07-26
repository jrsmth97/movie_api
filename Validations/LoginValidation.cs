using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class LoginsValidation: AbstractValidator<Login>
    {
        public LoginsValidation()
        {
            RuleFor(x => x.email).NotEmpty().WithMessage("Email tidak boleh kosong");
            RuleFor(x => x.password).NotEmpty().WithMessage("Password tidak boleh kosong");
        }
    }
}