using Domain.DomainExceptions;
using Domain.Ports;
using Domain.ValueObjects;
using Utils = Domain.UtilsTools.Utils;

namespace Domain.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public PersonId DocumentId { get; set; }

        private void ValidateState()
        {
            if(
                DocumentId == null || 
                DocumentId.IdNumber.Length <= 3 ||
                DocumentId.DocumentType == 0
             )
            {
                throw new InvalidDocumentException(); 
            }

            if(Name == null || Surname == null || Email == null)
            {
                throw new MissingFieldsException();
            }

            if(Utils.ValidateEmail(Email) == false)
            {
                throw new InvalidFieldException("email");
            }
        }

        public async Task Save(IGuestRepository repository)
        {
            this.ValidateState(); 
            if(this.Id == 0)
            {
               this.Id = await repository.Create(this);
            }
            else
            {
                // updates existing guest
            }
        }

    }
}
