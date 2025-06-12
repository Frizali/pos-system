using Microsoft.AspNetCore.Mvc;
using pos_system.Models;

namespace pos_system.Controllers
{
    public class PoController : Controller
    {
        public ActionResult OrderPo()
        {
            var menuList = new List<Po>
{
                new Po { Id = 1, Name = "Nasi Kucing", Price = 5000, ImageUrl = "~/Images/nasi kucing.jpg",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = true },
                new Po { Id = 2, Name = "Sate Usus", Price = 3000, ImageUrl = "~/Images/sate usus.jpeg",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = true },
                new Po { Id = 3, Name = "Sate Telur Puyuh", Price = 4000, ImageUrl = "~/Images/foto 3.png",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = true },
                new Po { Id = 4, Name = "Gorengan", Price = 2000, ImageUrl = "~/Images/foto 1.png",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = false },
                new Po { Id = 5, Name = "Wedang Jahe", Price = 6000, ImageUrl = "~/Images/foto 2.png",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = false },
                new Po { Id = 6, Name = "Teh Manis Hangat", Price = 3000, ImageUrl = "~/Images/teh.jpg",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = false },
                new Po { Id = 7, Name = "Kopi Hitam", Price = 5000, ImageUrl = "~/Images/foto 1.png",Description = "Nasi, telur ddr, ayam kruk, saus sam yang, cabe", IsFavorite = false },
            };


            return View(menuList);
        }
    }
}
