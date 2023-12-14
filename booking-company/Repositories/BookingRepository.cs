using booking_company.Model; // Adjust to your project's namespace
using System;
using System.Linq;

namespace booking_company.Repositories // Adjust to your project's namespace
{
    public class BookingRepository 
    {
        // Method to check if a house is available for a specific period
        public bool IsHouseAvailable(int houseId, DateTime fromDate, DateTime toDate)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            try
            {
                // Check if there are any bookings that overlap the requested period for the given house
                var overlappingBooking = context.Bookings
                    .Any(b => b.HouseId == houseId && !(b.ToDate <= fromDate || b.FromDate >= toDate));

                return !overlappingBooking; // If there's no overlapping booking, the house is available
            }
            catch (Exception)
            {
                return false; // In case of an exception, return false indicating the house is not available
            }
        }

        // Additional methods as needed for your application
    }
}
