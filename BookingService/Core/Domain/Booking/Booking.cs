using Domain.Booking.Exceptions;
using Domain.DomainExceptions;
using Domain.Enums;
using Domain.Ports;
using Action = Domain.Enums.Action;

namespace Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        public Status Status { get; private set; }
        
        public void ChangeState(Action action)
        {
            Status = (this.Status, action) switch
            {
                (Status.Created,  Action.Pay)     => Status.Paid,
                (Status.Created,  Action.Cancel)  => Status.Canceled,
                (Status.Paid,     Action.Finish)  => Status.Finished,
                (Status.Paid,     Action.Refound) => Status.Refounded,
                (Status.Canceled, Action.Reopen)  => Status.Created,
                (_, Action.Mantain) => Status,
                _ => Status
            };

        }

        public Booking()
        {
            this.Status = Status.Created;
            this.PlacedAt = DateTime.UtcNow;
        }

        public async Task Save(IBookingRepository repository)
        {
            this.ValidateState();

            if (!this.Guest.IsValid())
            {
                throw new InvalidGuestException();
            }

            if (!this.Room.IsValid())
            {
                throw new RoomCannotBeBookedException();
            }

            if (this.Id == 0)
            {
                this.Id = await repository.CreateBooking(this);
                this.Room.Bookings.Add(this);
            }
            else
            {
                //updates existing booking
            }
        }

        private void ValidateState()
        {
            if (this.Room == null || this.Guest == null)
            {
                throw new MissingFieldsException();
            }

            if(this.Start <= DateTime.UtcNow || this.End <= DateTime.UtcNow)
            {
                throw new InvalidDateException("Date cannot be past");
            }

            if(this.End <= this.Start)
            {
                throw new InvalidPeriodException("End date should come after start date");
            }
        }
    }
}
