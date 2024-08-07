﻿namespace Prepaid.Domain.Models.States;

public interface IBookingState
{
    void SetPaid();
    void SetPending();
    void SetRefunded();
    void SetExpired();
    void SetCancelled();
    BookingState BookingState { get; }
}