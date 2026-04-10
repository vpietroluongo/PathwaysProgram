public interface IContactAccess
{
    Task<Contact?> GetContactAsync(int contactId);
    Task SaveContactAsync(Contact contact);
}
