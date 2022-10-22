using dataentry.AutoGraph.Attributes;

namespace dataentry.ViewModels.GraphQL
{
    [AutoObjectGraphType("Contacts")]
    [AutoInputObjectGraphType("ContactsInput")]
    public class ContactsViewModel
    {
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; } 
        public string Avatar { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Order { get; set; }
        public AdditionalFieldsViewModel AdditionalFields { get; set; }
        public bool PreventOverwrite { get; set; }
    }
}
