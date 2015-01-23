using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DGII_PFD.Controllers;
using DGII_PFD.Models;
using DGII_PFD.Helpers;
using System.Diagnostics;
using System.Web.Configuration;

namespace DGII_PFD
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
            AsyncWorker.Instance.LoadADUsers();
		}
		
		protected void Application_EndRequest()
		{
			if (Context.Response.StatusCode == 404)
			{
				Response.Clear();

				var rd = new RouteData();
				rd.Values["controller"] = "CustomErrors";
				rd.Values["action"] = "NotFound";

				IController c = new CustomErrorsController();
				c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
			}
            
		}
	}
}