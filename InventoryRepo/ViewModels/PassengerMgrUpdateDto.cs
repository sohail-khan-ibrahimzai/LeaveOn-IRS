using InventoryRepo.Models;
using System.Collections.Generic;

namespace InventoryRepo.ViewModels
{
    public class PassengerMgrUpdateDto
    {
        public List<CreatePassengerDto> Passengers { get; set; }
        public List<ManagerDto> Managers { get; set; }
    }
}
