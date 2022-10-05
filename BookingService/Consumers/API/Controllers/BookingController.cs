using Application.Booking.DTOs;
using Application.Booking.Ports;
using Application.Booking.Requests;
using Application.Booking.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingManager _bookingManager;

        public BookingController(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [HttpPost]
        public async Task<ActionResult<BookingResponse>> CreateBooking(BookingDTO bookingDTO)
        {
            var request = new CreateBookingRequest
            {
                Data = bookingDTO,
            };

            var response = await _bookingManager.CreateBooking(request);
            if(response.Sucess == true) return Created("", response.Data);

            if(response.ErrorCode == Application.ErrorCode.BOOKING_INVALID_FIELD ||
                response.ErrorCode == Application.ErrorCode.ROOM_NOT_FOUND ||
                response.ErrorCode == Application.ErrorCode.NOT_FOUND)
            {
                return BadRequest(response);
            }

            _logger.LogError("Unknown error occurred", response);
            return StatusCode(500, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingResponse>> GetBooking(int id)
        {
            var response = await _bookingManager.GetBooking(id);
            if (response.Sucess == true) return Ok(response.Data);

            if (response.ErrorCode == Application.ErrorCode.BOOKING_NOT_FOUND)
            {
                return NotFound(response);
            }

            _logger.LogError("Unknown error occurred", response);
            return StatusCode(500, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookingResponse>> DeleteBooking(int id)
        {
            var response = await _bookingManager.DeleteBooking(id);
            if (response.Sucess == true) return Ok(response.Data);

            if (response.ErrorCode == Application.ErrorCode.BOOKING_NOT_FOUND)
            {
                return NotFound(response);
            }

            _logger.LogError("Unknown error occurred", response);
            return StatusCode(500, response);
        }
    }
}
