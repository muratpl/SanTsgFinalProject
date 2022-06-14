using FinalProject.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Web.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        public IActionResult List()
        {
            var reservations = _reservationService.List();
            return View(reservations);
        }
    }
}
