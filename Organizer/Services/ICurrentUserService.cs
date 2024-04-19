namespace Organizer.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; set; }
        public Task<bool> SetUser(string User);
    }
}