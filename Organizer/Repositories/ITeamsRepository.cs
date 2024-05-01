namespace Organizer.Repositories
{
    public interface ITeamsRepository
    {
        Task<int> SaveChangesAsync();
        
        Task CreateTeam(Entities.Team team);
        Task CreateUserTeam(Entities.UserTeam userteam);
        Task Delete(string? id);
        Task DeleteUserFromTeam(string user_id, Guid team_id);
        Task EditTeam(Entities.Team team);
        Task<List<Entities.Team>> GetTeamsByUser();
        
    }
}
