using System.Collections.Generic;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        bool SaveAll();
        void AddTrip(Trip obj);
        Trip GetTripByName(string tripName, string username);
        void AddStop(string tripName, string username, Stop newStop);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}