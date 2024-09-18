using System.Web.Mvc;

namespace InventoryRepo.ViewModels
{
    public class CreatePassengerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public string PickupAddress { get; set; }
        public decimal ManagerDeal { get; set; }
        public decimal ManagerComission { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
    }
}
