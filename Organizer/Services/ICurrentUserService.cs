namespace Organizer.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; set; }
        public Task<bool> SetUser(Guid? User);
    }
}