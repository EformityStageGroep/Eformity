namespace Organizer.Services
{
    public interface ICurrentUserService
    {
        string? userid { get; set; }
        public Task<bool> SetUser(string? User);
    }
}