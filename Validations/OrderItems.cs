using System;
using FluentValidation;
using movie_api.Models;

namespace movie_api.Validations
{
    public class OrderItemsValidation: AbstractValidator<OrderItems>
    {
        public OrderItemsValidation()
        {
            RuleFor(x => x.order_id).NotEmpty().WithMessage("Order tidak boleh kosong");
            RuleFor(x => x.movie_schedule_id).NotEmpty().WithMessage("Jadwal movie tidak boleh kosong");
            RuleFor(x => x.qty).NotEmpty().WithMessage("Kuantitas tidak boleh kosong");
            RuleFor(x => x.price).NotEmpty().WithMessage("Harga tidak boleh kosong");
            RuleFor(x => x.sub_total_price).NotEmpty().WithMessage("Sub total harga tidak boleh kosong"); 
        }
    }
}