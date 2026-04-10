public class LeadManager
{
    private readonly IContactAccess _contactAccess;

    public LeadManager(IContactAccess contactAccess)
    {
        _contactAccess = contactAccess;
    }

    public async Task PromotedToLeadAsync(int contactId)
    {
        var contact = await _contactAccess.GetContactAsync(contactId);
        if (contact == null)
            throw new InvalidOperationException("Contact not found");
        
        contact.IsLead = true;
        await _contactAccess.SaveContactAsync(contact);
    }

    public async Task<bool> IsLeadAsync(int contactId)
    {
        var contact = await _contactAccess.GetContactAsync(contactId);
        return contact?.IsLead ?? false;
    }
}
