using booking_company.Model;
using booking_company.Services;


namespace booking_company.Repositories 
{
    public class HouseRepository
    {
        // Method to get available houses based on a query
        public List<House> GetAvailableHouses(QueryHouse model)
        {
            using var context = new BookingDbContext();
            var filter = new PagingService(model.PageNumber, model.PageSize);

            var pagedData = context.Houses
                .Where(house => !house.Bookings.Any(b => b.FromDate < model.ToDate && b.ToDate > model.FromDate)
                                && house.MaxGuests >= model.NumberOfPeople)
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return pagedData;
        }

        // Method to create a new house listing
        public House CreateHouse(House houseModel)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            context.Houses.Add(houseModel);
            context.SaveChanges();
            return houseModel;
        }

        // Method to book a house, similar to buying a ticket
        public string BookHouse(BookHouse bookingDetails, Client client)
        {
            using var context = new BookingDbContext(); // Replace with your DbContext
            var status = "";

            var house = context.Houses.FirstOrDefault(x => x.HouseId == bookingDetails.HouseId);
            if (house != null)
            {
                var houseBooking = context.Bookings.FirstOrDefault(x => x.HouseId == house.HouseId &&
                                                                        x.FromDate < bookingDetails.ToDate &&
                                                                        x.ToDate > bookingDetails.FromDate);

                if (houseBooking != null)
                {
                    status = "House is not available for the selected dates.";
                }
                else
                {
                    var newBooking = new Booking
                    {
                        ClientId = client.ClientId,
                        HouseId = house.HouseId,
                        FromDate = bookingDetails.FromDate,
                        ToDate = bookingDetails.ToDate
                        // Other booking details
                    };
                    context.Bookings.Add(newBooking);
                    // Update house availability if needed
                    // ...
                    context.SaveChanges();
                    status = "House booked successfully.";
                }
            }
            else
            {
                status = "House not found.";
            }
            return status;
        }

        // Additional methods as needed for your application
    }
}
