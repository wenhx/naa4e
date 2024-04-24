using Microsoft.AspNetCore.Mvc.Razor;

namespace IBuyStuff.Server.Common
{
    public class MultiDeviceViewLocationExpander : IViewLocationExpander
    {
        private readonly string[] newViewLocations = new[]
            {
                "~/Views/{1}/{0}/{0}.cshtml",
                "~/Views/{1}/{0}/Partials/{0}.cshtml",
                "~/Views/{1}/Partials/{0}.cshtml",
                "~/Views/{1}/Partials/{0}/{0}.cshtml",
                "~/Views/Shared/{0}/{0}.cshtml",
                "~/Views/Shared/Partials/{0}.cshtml",
                "~/Views/Shared/Partials/{0}/{0}.cshtml"
            };

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return viewLocations.Union(newViewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            
        }
    }
}