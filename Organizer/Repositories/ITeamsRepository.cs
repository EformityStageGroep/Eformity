namespace Organizer.Repositories
{
    public interface ITeamsRepository
    {
        Task<int> SaveChangesAsync();
        
        Task CreateTeam(Entities.Team team);
        Task CreateUserTeam(Entities.UserTeam userteam);
        Task Delete(string? id);
        Task<bool> DeleteUserFromTeam(string user_id, Guid team_id);
        Task EditTeam(Entities.Team team);
        Task DeleteTeam(Guid team_id);
        Task DeleteAllTasks(Guid teamid);
        Task DeleteTasksFromUser(string user_id, Guid team_id);
        Task<List<Entities.Team>> GetTeamsByUser();
        Task<List<Entities.Team>> GetUsersByTeam();
        Task UpdateTeam(Guid teamId, string userIdsString);


    }
}
