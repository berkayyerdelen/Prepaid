using Prepaid.Domain.Exceptions;
using Prepaid.Domain.Models;
using Prepaid.Domain.Models.States;

namespace Prepaid.Domain.Tests;

public class BookingTests
{
    private Booking _booking;

    public BookingTests()
    {
    }

    [Fact]
    public void InitialBookingStatus_ShouldBe_Pending()
    {
        // Arrange && Act
        _booking = new Booking();
        
        // Assert
        Assert.Equal(BookingState.Pending, _booking.BookingSate);
    }

    [Fact]
    public void BookingWithPendingState_CanTransition_PaidState()
    {
        
        // Arrange
        _booking = new Booking();
        
        // Act
        _booking.SetPaidState();
        
        // Assert
        Assert.Equal(BookingState.Paid, _booking.BookingSate);
    }
    
    
    [Fact]
    public void BookingWithPendingState_CanTransition_PendingState()
    {
        // Arrange 
        _booking = new Booking();
        
        //Act && Assert
        Assert.Throws<StateTransitionNotAllowedDomainException>(() =>
        {
            _booking.SetPendingState();
        });
    }
}