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

        public bool IsAvailable
        {
            get { return !InMantainance && !HasGuests; }
        }

        public bool HasGuests
        {
            //TODO: check if this room has open bookings 
            get { return true; }
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

        public async Task Save(IRoomRepository repository)
        {
            this.ValidateState();
            
            if(this.Id == 0)
            {
                this.Id = await repository.Create(this);
            } 
            else
            {
                //updates existing room
            }
        }
    }
}
