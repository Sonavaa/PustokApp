﻿using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Areas.Admin.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
