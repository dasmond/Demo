﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MyCube.Precompiled
{
    /// <summary>视图缓存依赖</summary>
    public class ViewCacheDependency : CacheDependency
    {
        /// <summary>实例化缓存依赖</summary>
        /// <param name="virtualPath"></param>
        public ViewCacheDependency(String virtualPath)
        {
            base.SetUtcLastModified(DateTime.UtcNow);
        }

        /// <summary>验证</summary>
        public void Invalidate()
        {
            base.NotifyDependencyChanged(this, EventArgs.Empty);
        }
    }
}