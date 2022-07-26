using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class OrdersValidation: AbstractValidator<Orders>
    {
        public OrdersValidation()
        {
            RuleFor(x => x.user_id).NotEmpty().WithMessage("User id tidak boleh kosong");
            RuleFor(x => x.payment_method).NotEmpty().WithMessage("Metode pembayaran tidak boleh kosong");
            RuleFor(x => x.total_item_price).NotEmpty().WithMessage("harga total item tidak boleh kosong");
        }
    }
}