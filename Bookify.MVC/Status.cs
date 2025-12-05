namespace Bookify.MVC
{
    public enum Status
    {
        Available,
        Occupied,
        Reserved,
        Maintenance
    }
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public enum ErrorType
    {
        NotFound,
        Validation,
        Unauthorized,
        Forbidden,
        Conflict,
        Internal
    }
}
