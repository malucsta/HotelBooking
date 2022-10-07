using Application.Guest.DTOs;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly ILogger<GuestController> _logger;
        private readonly IGuestManager _guestManager;

        public GuestController(IGuestManager guestManager, ILogger<GuestController> logger)
        {
            _guestManager = guestManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<GuestDTO>> CreateGuest(GuestDTO guest)
        {
            var request = new CreateGuestRequest
            {
                Data = guest,
            };

            var res = await _guestManager.CreateGuest(request);

            if (res.Sucess) return Created("", res.Data);

            if (res.ErrorCode == Application.ErrorCode.MISSING_REQUIRED_INFORMATION ||
                res.ErrorCode == Application.ErrorCode.INVALID_FIELD ||
                res.ErrorCode == Application.ErrorCode.INVALID_PERSON_ID
                )
            {
                return BadRequest(res);
            }

            if(res.ErrorCode == Application.ErrorCode.COULD_NOT_STORE_DATA)
            {
                return StatusCode(500, res);
            }

            _logger.LogError("Unknown error occurred", res);
            return StatusCode(500, res);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestDTO>> GetGuest(int id)
        {
            var res = await _guestManager.GetGuest(id);

            if (res.Sucess) return Ok(res.Data);

            if (res.ErrorCode == Application.ErrorCode.NOT_FOUND)
            {
                return NotFound(res);
            }

            _logger.LogError("Unknown error occurred", res);
            return StatusCode(500, res);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GuestDTO>> DeleteGuest(int id)
        {
            var res = await _guestManager.DeleteGuest(id);

            if (res.Sucess) return Ok(res.Data);

            if (res.ErrorCode == Application.ErrorCode.NOT_FOUND)
            {
                return NotFound(res);
            }

            if (res.ErrorCode == Application.ErrorCode.INVALID_OPERATION)
            {
                return BadRequest(res);
            }

            _logger.LogError("Unknown error occurred", res);
            return StatusCode(500, res);

        }


    }
}
