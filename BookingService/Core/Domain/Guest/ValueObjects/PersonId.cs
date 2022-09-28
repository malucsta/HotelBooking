using Domain.Enums;

namespace Domain.ValueObjects
{
    public class PersonId
    {
        public string IdNumber { get; set; }
        public DocumentType DocumentType { get; set; }

        //could also have a date and other fields
        
    }
}
