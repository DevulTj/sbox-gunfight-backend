using Api.Services;
using Base;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
	public class MatchHistoryModel : PageModel
	{
		public IMatchService Service;
		
		public MatchHistoryModel( IMatchService _service )
		{
			Service = _service;
		}

		public void OnGet()
		{

		}

		public string GetIcon( string key )
		{
			return key switch
			{
				"kills" => "target",
				"deaths" => "skull",
				_ => "shield"
			};
		}
	}
}
