using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Api.Auth;

[AttributeUsage( validOn: AttributeTargets.Class | AttributeTargets.Method )]
public class RequireTokenAttribute : Attribute, IAsyncActionFilter
{
	private const string APIKEYNAME = "X-Auth-Token";
	private const string STEAMIDNAME = "X-Auth-Id";

	private static System.Net.Http.HttpClient Http;
	private class ValidateAuthTokenResponse
	{
		public long SteamId { get; set; }
		public string Status { get; set; }
	}

	public async Task OnActionExecutionAsync( ActionExecutingContext context, ActionExecutionDelegate next )
    {
		if ( Global.IsDevelopment )
		{
			next();
			return;
		}

		if ( !context.HttpContext.Request.Headers.TryGetValue( APIKEYNAME, out var extractedApiKey ) )
		{
			context.Result = new ContentResult()
			{
				StatusCode = 401,
				Content = "Invalid client authorization key."
			};

			return;
		}

		if ( !context.HttpContext.Request.Headers.TryGetValue( STEAMIDNAME, out var extractedSteamId ) )
		{
			context.Result = new ContentResult()
			{
				StatusCode = 401,
				Content = "No SteamID specified for auth."
			};

			return;
		}

		long steamId = await ValidateToken( long.Parse( extractedSteamId ), extractedApiKey );

		if ( steamId == 0 )
		{
			context.Result = new ContentResult()
			{
				StatusCode = 401,
				Content = "Auth token incorrect for client."
			};

			return;
		}

		await next();
	}

	public static async Task<long> ValidateToken( long steamId, string token )
	{
		try
		{
			Http ??= new()
			{
				Timeout = TimeSpan.FromSeconds( 5 )
			};

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

			Console.WriteLine( "Authorized." );
			return response.SteamId;
		}
		catch ( Exception e )
		{
			Console.WriteLine( e.Message );
			return 0;
		}
	}
}
