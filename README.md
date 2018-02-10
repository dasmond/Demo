# Demo


//多个XML
config.SetDocumentationProvider(new MultiXmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/bin/DJW.WebAPI.xml"), HttpContext.Current.Server.MapPath("~/bin/DJW.Core.xml")));
