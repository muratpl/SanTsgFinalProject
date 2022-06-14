using FinalProject.Application.Models;
using FinalProject.Application.Services.Interfaces;
using FinalProject.Data;
using FinalProject.Domain.Reservations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Services.Concretes
{
    //If the reservation is successful, it adds the reservation information to the reservations table in the database.
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReservationService> _logger;
        private readonly IEmailService _emailService;

        public ReservationService(IUnitOfWork unitOfWork, ILogger<ReservationService> logger, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailService = emailService;
        }


        public async Task Create(Reservation reservation)
        {
            _unitOfWork.Reservations.Add(reservation);
            _unitOfWork.Complete();

            MailRequest mail = new MailRequest()
            {
                Body = "Your reservation has been successfully saved. We wish you pleasant holidays.",
                Subject = "Reservation Registration Information",
                ToEmail = reservation.Email
            };

            await _emailService.SendEmailAsync(mail);

            _logger.LogInformation($"New reservation registration added. Added record: {reservation}", reservation);
        }

        public IEnumerable<Reservation> List()
        {
            var reservations = _unitOfWork.Reservations.GetAll();
            return reservations;


        }

    }
}