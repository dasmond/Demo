﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using MyCube;
    using NewLife;
    
    #line 1 "..\..\Views\Shared\_Layout_Footer.cshtml"
    using NewLife.Common;
    
    #line default
    #line hidden
    using NewLife.Reflection;
    using NewLife.Web;
    using XCode;
    using XCode.Membership;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout_Footer.cshtml")]
    public partial class _Views_Shared__Layout_Footer_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Views_Shared__Layout_Footer_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\_Layout_Footer.cshtml"
  
    var cfg = NewLife.Common.SysConfig.Current;
    var user = ManageProvider.User;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"container footer\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"page-divider\"");

WriteLiteral("></div>\r\n    <div");

WriteLiteral(" class=\"clearfix\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"pull-right btn-group dropup\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" class=\"dropdown-toggle\"");

WriteLiteral(" data-toggle=\"dropdown\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">友情链接 <span");

WriteLiteral(" class=\"caret\"");

WriteLiteral("></span></a>\r\n            <ul");

WriteLiteral(" class=\"dropdown-menu\"");

WriteLiteral(">\r\n                <li>友情链接1</li>\r\n                <li>友情链接2</li>\r\n              " +
"  <li>友情链接3</li>\r\n            </ul>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral("><p");

WriteLiteral(" class=\"muted\"");

WriteLiteral(" title=\"\"");

WriteLiteral(">&copy; 2002-");

            
            #line 17 "..\..\Views\Shared\_Layout_Footer.cshtml"
                                                                Write(DateTime.Now.Year);

            
            #line default
            #line hidden
WriteLiteral(" ");

            
            #line 17 "..\..\Views\Shared\_Layout_Footer.cshtml"
                                                                                   Write(cfg.Company);

            
            #line default
            #line hidden
WriteLiteral("</p></div>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
