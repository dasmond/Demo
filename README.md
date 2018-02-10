# Demo

## Asp.Net WebAPI - 使用多个Xml文件显示帮助文档[HelpPage]
*在项目属性->生成->输出->勾选`XML 文档文件`
*在目录 ~/Areas/HelpPage/ 下添加 MultiXmlDocumentationProvider.cs
''''''
//// 替换 ~/Areas/HelpPage/App_Start/HelpPageConfig.cs 中的配置
//// Uncomment the following to use the documentation from XML documentation file.
//config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/XmlDocument.xml")));
config.SetDocumentationProvider(new MultiXmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/bin/WebAPI.xml"), HttpContext.Current.Server.MapPath("~/bin/DsLib.xml")));
''''''