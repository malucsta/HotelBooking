using Application.Guest.DTOs;
using Application.Guest.Ports;
using Application.Guest.Requests;
using Application.Guest.Responses;
using Domain.DomainExceptions;
using Domain.Ports;

namespace Application
{
    public class GuestManager : IGuestManager
    {
        private readonly IGuestRepository _repository;
        public GuestManager(IGuestRepository repository)
        {
            _repository = repository;
        }
        public async Task<GuestResponse> CreateGuest(CreateGuestRequest request)
        {
            try
            {
                var guest = GuestDTO.MapToEntity(request.Data);
                await guest.Save(_repository);
                request.Data.Id = guest.Id;

                return new GuestResponse
                {
                    Data = request.Data,
                    Sucess = true,
                };
            }
            catch (InvalidDocumentException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.INVALID_PERSON_ID,
                    Message = "Document Id is invalid",
                };
            }
            catch (MissingFieldsException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information",
                };
            }
            catch (InvalidFieldException e)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.INVALID_FIELD,
                    Message = $"Invalid information: {e.Message}",
                };
            }
            catch (Exception)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.COULD_NOT_STORE_DATA,
                    Message = "Something went wrong while trying to save guest to database",
                };
            }
        }

        public async Task<GuestResponse> GetGuest(int guestID)
        {
            var guest = await _repository.Get(guestID);

            if(guest == null)
            {
                return new GuestResponse
                {
                    Sucess = false,
                    ErrorCode = ErrorCode.NOT_FOUND,
                    Message = "This guest doesn't exist",
                };
            };

            return new GuestResponse
            {
                Data = GuestDTO.MapToDTO(guest),
                Sucess = true,
            };
        }
    }
}
