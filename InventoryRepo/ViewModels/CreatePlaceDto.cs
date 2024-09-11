using System.Web.Mvc;

namespace InventoryRepo.ViewModels
{
    public class CreatePlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
    }
}
