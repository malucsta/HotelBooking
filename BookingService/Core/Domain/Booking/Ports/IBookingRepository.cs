using Entities = Domain.Entities;

namespace Domain.Ports
{
    public interface IBookingRepository
    {
        Task<int> CreateBooking(Entities.Booking booking);
        Task<Entities.Booking?> GetBooking(int id);
        Task<List<Entities.Booking>?> GetBookingsByRoom(int roomID);
        Task<bool> CheckBookingsForRoomByPeriod(int roomID, DateTime start, DateTime end);
        Task<bool> CheckBookingsForRoom(int roomID);
        Task DeleteBooking(Entities.Booking booking);
    }
}
