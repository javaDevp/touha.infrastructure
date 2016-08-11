using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace touha.infrastructure.mvc
{
    public class ViewEngineUtil
    {
        /// <summary>
        /// 将视图引擎设置为只支持Razor，格式仅支持cshtml
        /// </summary>
        public static void SetViewEngineRazorWithCshtml()
        {
            //清除WebFormViewEngine
            ViewEngines.Engines.RemoveAt(0);

            RazorViewEngine razor = ViewEngines.Engines[0] as RazorViewEngine;
            razor.AreaViewLocationFormats = new string[] 
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                //"~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
                //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            razor.AreaMasterLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
               // "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
                //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            razor.AreaPartialViewLocationFormats = new string[]
            {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                //"~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml"
                //"~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            razor.ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                //"~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml"
                //"~/Views/Shared/{0}.vbhtml"
            };
            razor.MasterLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                //"~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml"
                //"~/Views/Shared/{0}.vbhtml"
            };
            razor.PartialViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml",
                //"~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml"
                //"~/Views/Shared/{0}.vbhtml"
            };
            razor.FileExtensions = new[]
            {
                "cshtml"
                //"vbhtml"
            };
        }
    }
}
