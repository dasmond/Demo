
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac;
using Autofac.Extras.DynamicProxy;

namespace CoreMVC.IOC
{
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注入AOP拦截类
            builder.Register(c => new AOPTest());

            //注入测试服务
            //builder.RegisterType<TestService>().As<ITestService>();

            //注入测试服务，并开启服务的拦截状态
            builder.RegisterType<TestService>().As<ITestService>().PropertiesAutowired().EnableInterfaceInterceptors();

        }
    }
}
