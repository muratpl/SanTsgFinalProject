using FinalProject.Application.Services.Interfaces;
using FinalProject.Domain.Reservations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinalProject.Web.Controllers
{
    public class ListController : Controller
    {
        private readonly IPaximumService _paximumService;
        private readonly IReservationService _reservationService;

        public ListController(IPaximumService paximumService, IReservationService reservationService)
        {
            _paximumService = paximumService;
            _reservationService = reservationService;
        }

        public IActionResult Cities(string city)
        {
            Dictionary<int, string> cities = _paximumService.GetArrivalAutoCompleteAsync(city).Result;
            return View(cities);
        }

        public IActionResult Hotels(int cityId)
        {
            var hotels = _paximumService.PriceSearchAsync(cityId).Result;
            return View(hotels);
        }

        public IActionResult HotelDetails(string itemId, string ownerProvider, string offerId, string description, string boardName, string roomName, string price, string priceCurrency)
        {
            var hotelDetails = _paximumService.GetProductInfoAsync(itemId, ownerProvider).Result;
            ViewBag.HotelDetails = hotelDetails;
            ViewBag.OfferId = offerId;
            ViewBag.Description = description;
            ViewBag.BoardName = boardName;
            ViewBag.RoomName = roomName;
            ViewBag.Price = price;
            ViewBag.PriceCurrency = priceCurrency;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult HotelDetails(Reservation reservation)
        {
            reservation.ReservationNumber = string.Empty;
  
            if (ModelState.IsValid)
            {
                var offerId = TempData["OfferId"].ToString();
                string transactionId = _paximumService.BeginTransactionAsync(offerId).Result;
                string reservationNumber = _paximumService.SendReservationAsync(transactionId, reservation).Result;
                
                if(reservationNumber != "")
                {
                    reservation.ReservationNumber = reservationNumber;
                    reservation.Hotel = TempData["HotelName"].ToString();
                    _reservationService.Create(reservation);
                    TempData["Reservation"] = "Your reservation has been successfully saved. We wish you pleasant holidays.";
                }
                else
                {
                    TempData["ReservationFailed"] = "There was a problem creating your reservation. Please check your information and try again.";
                }
                return RedirectToAction("Index", "Home");
            }
            return View(reservation);
        }
    }
}
