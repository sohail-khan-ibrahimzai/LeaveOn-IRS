using InventoryRepo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LeaveON.Dtos
{
    public class CreateTripDto
    {
        public int? Id { get; set; }
        public int? DriverId { get; set; }
        public int? PassengerId { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PlaceId { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public decimal? TotalHours { get; set; }
        public string Status { get; set; } = "In progress";
        public string PlaceName { get; set; }
        public bool? IsBlackListed { get; set; }
        public string Floor { get; set; }
        public string Bell { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Passenger Passenger { get; set; }
        public virtual Place Place { get; set; }
    }
}
