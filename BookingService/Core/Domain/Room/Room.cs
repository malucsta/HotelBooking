using Domain.DomainExceptions;
using Domain.Ports;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMantainance { get; set; }

        public Price Price { get; set; }

        public List<Booking> Bookings { get; set; }

        public bool IsAvailable
        {
            get { return !InMantainance && !HasGuests; }
        }

        public bool HasGuests
        {
            //TODO: check if this room has open bookings 
            get
            {
                var notAvailableStatuses = new List<Enums.Status>
                {
                    Enums.Status.Created,
                    Enums.Status.Paid,
                };

                if(this.Bookings != null)
                {
                    return this.Bookings.Where(
                    //where booking roomID == roomID
                    x => x.Room.Id == this.Id
                    //booking current status != notAvailableStatuses
                    ).Count() > 0;
                } 
                else
                {
                    return false;
                }
            }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new MissingFieldsException(); 
            };

            //supposing that does not have underground levels
            if (Level < 0)
            {
                throw new InvalidFieldException("level");
            };

            if(Price.Value <= 0)
            {
                throw new InvalidFieldException("price");
            }
        }

        public bool IsValid()
        {
            try
            {
                this.ValidateState();

                if(!this.IsAvailable) return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Save(IRoomRepository repository)
        {
            this.ValidateState();
            
            if(this.Id == 0)
            {
                this.Id = await repository.CreateRoom(this);
            } 
            else
            {
                //updates existing room
            }
        }
    }
}
