using System.Web.Mvc;

namespace InventoryRepo.ViewModels
{
    public class CreatePlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool? IsBlackListed { get; set; }
        public string Floor { get; set; }
        public string Bell { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
    }
}
