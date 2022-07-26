using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class UsersValidation: AbstractValidator<Users>
    {
        public UsersValidation()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Nama akun tidak boleh kosong");
            RuleFor(x => x.email).NotEmpty().WithMessage("Email tidak boleh kosong");
            RuleFor(x => x.password).NotEmpty().WithMessage("Password tidak boleh kosong");
            // RuleFor(x => x.avatar).NotEmpty().WithMessage("Avatar tidak boleh kosong");
            // RuleFor(x => x.is_admin).NotEmpty().WithMessage("Jenis akun tidak boleh kosong"); 
        }
    }
}