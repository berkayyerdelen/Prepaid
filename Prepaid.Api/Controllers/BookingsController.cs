using Microsoft.AspNetCore.Mvc;
using Prepaid.Api.Requests;
using Prepaid.Api.Responses;
using Prepaid.Application.Contracts;
using Prepaid.Application.Requests;
using Prepaid.Application.Responses;

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

    [HttpPost("{uniqueId}/confirm")]
    public async Task<ActionResult> Confirm(Guid uniqueId, PaymentInformationApiRequest request, CancellationToken cancellationToken =default)
    {
       await _bookingService.Confirm(uniqueId, new PaymentInformationApplicationRequest()
        {
            PaymentDate = request.PaymentDate,
            PaymentId = request.PaymentId,
            Amount = request.Amount,
            PaymentToken = request.PaymentToken,
            ServiceFee = request.ServiceFee
        }, string.Empty,cancellationToken);

        return Ok();
    }

    [HttpPut("{bookingId}")]
    public async Task<ActionResult> Update(Guid bookingId, UpdateBookingApiRequest request,
        CancellationToken cancellationToken = default)
    {
        var updateBookingApplicationResponse = await _bookingService.Update(bookingId, new UpdateBookingApplicationRequest()
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            UserId = request.UserId
        }, cancellationToken);

        return Ok(new UpdateBookingApiResponse()
        {
            UniqueId = updateBookingApplicationResponse.UniqueId,
            PaymentUrl = updateBookingApplicationResponse.PaymentUrl
        });
    }

    [HttpDelete]
    public async Task<ActionResult> Refund(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        await _bookingService.Refund(uniqueId, cancellationToken);
        
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult> Get(Guid uniqueId, CancellationToken cancellationToken = default)
    {
       var booking = await _bookingService.Get(uniqueId, cancellationToken);
       
       return Ok(new BookingApiResponse()
       {
           UniqueId = booking.UniqueId,
           BookingState = booking.BookingState,
           StartTime = booking.StartTime,
           EndTime = booking.EndTime,
           UserId = booking.UserId
       });
    }
}