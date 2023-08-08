using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;
using Base.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route( "api/[controller]") ]
    public class AuthController : BaseController
    {
        private static System.Net.Http.HttpClient Http;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;

            Http = new();
            Http.Timeout = TimeSpan.FromSeconds( 5 );
        }

        private class ValidateAuthTokenResponse
        {
            public long SteamId { get; set; }
            public string Status { get; set; }
        }

        public static async Task<long> ValidateToken( long steamId, string token )
        {
            try
            {
                var data = new Dictionary<string, object>
            {
                { "steamid", steamId },
                { "token", token }
            };
                var content = new StringContent( JsonSerializer.Serialize( data ), Encoding.UTF8, "application/json" );
                var result = await Http.PostAsync( "https://services.facepunch.com/sbox/auth/token", content );

                if ( result.StatusCode != HttpStatusCode.OK ) return 0;

                var response = await result.Content.ReadFromJsonAsync<ValidateAuthTokenResponse>();
                if ( response is null || response.Status != "ok" ) return 0;

                return response.SteamId;
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
                return 0;
            }
        }
	}
}
