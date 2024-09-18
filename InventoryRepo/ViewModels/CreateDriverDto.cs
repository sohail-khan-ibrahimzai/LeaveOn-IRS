using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InventoryRepo.ViewModels
{
    public class CreateDriverDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal CostPerHour { get; set; }
        [AllowHtml]
        public string Remarks { get; set; }
    }
}
