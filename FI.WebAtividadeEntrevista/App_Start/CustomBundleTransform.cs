using System;
using System.Web;
using System.Web.Optimization;

namespace FI.WebAtividadeEntrevista.App_Start
{
    public class CustomBundleTransform : IBundleTransform
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            string version = DateTime.Now.Ticks.ToString();
            response.Content = response.Content + "\n/* v=" + version + " */";
            response.Cacheability = HttpCacheability.Public;
        }
    }
}