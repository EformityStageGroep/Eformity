using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace Organizer.Services
{
    public interface IGraphClientService
    {
        GraphServiceClient GetGraphServiceClient();
        User GetUserProfile();
    }

    public class GraphClientService : IGraphClientService
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _configuration;
        public GraphClientService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            _tokenAcquisition = tokenAcquisition;
            _configuration = configuration;
        }

        public GraphServiceClient GetGraphServiceClient()
        {
            // Get scopes from appsettings
            var token = _tokenAcquisition.GetAccessTokenForUserAsync(
                scopes: new string[] { "User.Read",}
                //scopes: (IEnumerable<string>)_configuration.GetSection("MicrosoftGraph:Scopes")
            ).GetAwaiter().GetResult();

            return new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
               {
                   // Add the access token in the Authorization header of the API request.
                   requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
               })
           );
        }

        public User GetUserProfile()
        {
            return GetGraphServiceClient().Me.Request().GetAsync().GetAwaiter().GetResult();
        }
    }
}