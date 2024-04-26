namespace Organizer.Services
{
    public interface ICurrentUserService
    {
        string? user_id { get; set; }
        public Task<bool> SetUser(string? User);
    }
}