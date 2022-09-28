using Application.Room.DTOs;
using Application.Room.Ports;
using Application.Room.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomManager _roomManager;

        public RoomController(IRoomManager roomManager, ILogger<RoomController> logger)
        {
            _logger = logger;
            _roomManager = roomManager;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDTO>> CreateRoom(RoomDTO roomDTO)
        {
            var request = new CreateRoomRequest
            {
                Data = roomDTO
            };

            var res = await _roomManager.CreateRoom(request);

            if (res.Sucess) return Created("", res);

            if(res.ErrorCode == Application.ErrorCode.ROOM_MISSING_REQUIRED_INFORMATION ||
                res.ErrorCode == Application.ErrorCode.ROOM_INVALID_FIELD)
            {
                return BadRequest(res);
            }

            if(res.ErrorCode != Application.ErrorCode.ROOM_COULD_NOT_STORE_DATA)
            {
                return StatusCode(500, res);
            }

            _logger.LogError("Unknown error occurred", res);
            return StatusCode(500, res);
        }
    }
}
