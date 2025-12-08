using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using PeopleManager.Models;

namespace PeopleManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // If the model changes, drop and recreate the database (data will be lost)
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PeopleDbContext>());
        }

        protected void Application_BeginRequest()
        {
            // Ensure responses use UTF-8 charset
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.ContentType = "text/html; charset=utf-8";
        }
    }
}
