using Microsoft.AspNetCore.Mvc;
using Prepaid.Api.Requests;
using Prepaid.Api.Responses;
using Prepaid.Application.Contracts;
using Prepaid.Application.Requests;

namespace Prepaid.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreteBookingApiRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingService.Create(new CreateBookingApplicationRequest()
        {
            UserId = request.UserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime
        }, cancellationToken);

        return Ok(new CreateBookingApiResponse()
        {
            UniqueId = booking.UniqueId,
            PaymentUrl = booking.PaymentUrl!
        });
    }
}