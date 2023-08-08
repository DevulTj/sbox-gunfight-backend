using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class BaseController : Microsoft.AspNetCore.Mvc.ControllerBase
{
	public long SteamId => long.Parse( HttpContext.Request.Headers["X-Auth-Id"] );
}
