using NSubstitute;
using Prepaid.Application.Contracts;
using Prepaid.Application.Responses;
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
        // Arrange
        _bookingRepository.Get(Arg.Any<Guid>())!.Returns(default(Booking));
        
        //Act
        var booking = await _bookingService.Get(Arg.Any<Guid>())!;
        
        // Assert
        
        Assert.Null(booking);

    }
}