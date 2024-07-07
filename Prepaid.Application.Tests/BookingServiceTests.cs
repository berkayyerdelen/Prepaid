using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Prepaid.Application.Services;
using Prepaid.Domain.Models;
using Prepaid.Domain.Policies.Contracts;
using Prepaid.Domain.Repositories;

namespace Prepaid.Application.Tests;

public class BookingServiceTests
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingRefundPolicy _bookingRefundPolicy;
    private readonly IMockPaymentService _mockPaymentService;
    private readonly IMockPricingService _mockPricingService;
    private readonly BookingService _bookingService;

    public BookingServiceTests()
    {
        _bookingRepository = Substitute.For<IBookingRepository>();
        _bookingRefundPolicy = Substitute.For<IBookingRefundPolicy>();
        _mockPaymentService = Substitute.For<IMockPaymentService>();
        _mockPricingService = Substitute.For<IMockPricingService>();
        _bookingService = new BookingService(_bookingRepository, _bookingRefundPolicy, _mockPricingService,
            _mockPaymentService);
    }

    [Fact]
    public async Task Should_ReturnEmptyObject_WhenBookingNotFound()
    {
        var bookingId = Arg.Any<Guid>();
        // Arrange
        _bookingRepository.Get(bookingId).Returns(default(Booking));
        
        //Act
        var booking = await _bookingService.Get(bookingId)!;
        
        // Assert
        
        // Assert.Null(booking);
        await _bookingRepository.Received(1).Get(bookingId);
    }
}