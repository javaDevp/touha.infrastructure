using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace touha.infrastructure.webapi
{
    public class WebApiFormatter
    {
        /// <summary>
        /// 移除WebApi默认的xml格式
        /// 用法：WebApiFormatter.RemoveXmlFormat(GlobalConfiguration.Configuration);
        /// </summary>
        /// <param name="config"></param>
        public static void RemoveXmlFormat(HttpConfiguration config)
        {
            #region 方式1（推荐）：可去除自定义Formatter中包含的xml格式
            var matches = config.Formatters.
                Where(f => f.SupportedMediaTypes.
                Where(m => m.MediaType == "application/xml" || m.MediaType == "text/xml").
                Any()).ToList();
            foreach(var match in matches)
            {
                config.Formatters.Remove(match);
            }
            #endregion

            #region 方式2（不推荐）
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            #endregion
        }
    }
}
