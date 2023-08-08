using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class BaseController : Microsoft.AspNetCore.Mvc.ControllerBase
{
	public string SteamId => HttpContext.Request.Headers["X-Auth-Id"];
}
