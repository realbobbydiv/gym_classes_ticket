using FluentValidation;
using GymClassBooking.Host.Dtos;

namespace GymClassBooking.Host.Validation;

public class BookingRequestValidator : AbstractValidator<BookingRequestDto>
{
    public BookingRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ClassSessionId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
