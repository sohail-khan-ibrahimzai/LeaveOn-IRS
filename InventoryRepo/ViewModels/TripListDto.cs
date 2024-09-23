using InventoryRepo.Models;

namespace InventoryRepo.ViewModels
{
    public class TripListDto
    {
        //public Trip Trip { get; set; }
        //public Driver Driver { get; set; }
        //public AspNetUser Manager { get; set; }
        //public Passenger Passenger { get; set; }
        //public Place Place { get; set; }
        //public decimal TripCost { get; set; }
        //public decimal ManagerComission { get; set; }
        public Trip Trip { get; set; }
        public Driver Driver { get; set; }
        public AspNetUser Manager { get; set; } // The manager associated with the trip (from AspNetUser)
        public Passenger Passenger { get; set; } // The passenger associated with the trip
        public Place Place { get; set; } // The place associated with the trip
        public decimal TripCost { get; set; } // Calculated trip cost
        public decimal ManagerComission { get; set; } // Calculated manager commission
    }
}
