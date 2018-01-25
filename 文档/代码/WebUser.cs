using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSNN.Core.Common
{
    public class WebUser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebUser() { }

        /// <summary>
        /// Cookie名称
        /// </summary>
        private static string CookieName = (Config.CookieName.Length == 0 ? "DQRD" : Config.CookieName);
        /// <summary>
        /// 游客用户组ID
        /// </summary>
        private static int GuestGroupID = Config.GuestGroupID;
        /// <summary>
        /// 内网IP段
        /// </summary>
        private static string NetAddrIn = Config.NetAddrIn;
        /// <summary>
        /// 外网IP段
        /// </summary>
        private static string NetAddrOut = Config.NetAddrOut;
        

        //设置验证码，比对验证码
        #region - SetVerifyCode(),CheckVerifyCode(string _verifycode) -
        /// <summary>
        /// 设置验证码，写入数据库在线信息表
        /// </summary>
        /// <returns></returns>
        public static string SetVerifyCode()
        {
            //大小写敏感
            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0
            string verifycode = Utils.CreateAuthStr(4, true).ToLower().ToString();
            BLL.ActOnline.SetVerifyCode(olid, Utils.MD5(verifycode));

            return verifycode.ToLower();
        }
        /// <summary>
        /// 比对验证码,和数据库中的在线信息表比对
        /// </summary>
        /// <returns></returns>
        public static bool CheckVerifyCode(string _verifycode)
        {
            //大小写敏感
            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0

            Model.ActOnlineInfo m = BLL.ActOnline.GetModel(olid);
            if (m != null)
            {
                string verifycode = m.VerifyCode.ToLower().ToString();

                if (Utils.MD5(_verifycode.Trim()) == verifycode.Trim())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        //用户登录:Cookie([uid],[pcode])写入
        #region - Login(string _username, string _password) -
        /// <summary>
        /// 用户登录:Cookie([uid],[pcode])写入
        /// </summary>
        /// <param name="_username">用户名</param>
        /// <param name="_password">密码（MD5加密）</param>
        /// <param name="_cookietime">时效（分钟）</param>
        /// <param name="_errmsg">错误信息</param>
        /// <returns></returns>
        public static bool Login(string _username, string _password, int _cookietime, ref string _errmsg)
        {
            //_password = Utils.MD5(_password);
            Model.ActUserInfo userinfo = BLL.ActUser.GetInfoByUserName(_username, _password);
            if (userinfo != null && userinfo.UserID > 0)
            {
                if (userinfo.CheckState.ToString() != Model.ActUserInfo.EnumCheckState.通过.ToString())
                {
                    _errmsg = "用户账户未审核，不能登录，请联系管理员！";
                    return false;
                }
                try
                {
                    int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0
                    int uid = userinfo.UserID;
                    string pcode = Utils.UrlEncode(Utils.Encrypt(BLL.ActUser.GetPassword(userinfo.UserID)));

                    Model.ActOnlineInfo onlineinfo = BLL.ActOnline.GetModel(olid);
                    onlineinfo.PasswordCode = pcode.ToString();
                    onlineinfo.UserID = uid;
                    BLL.ActOnline.Update(onlineinfo);

                    userinfo.OnlineState = Model.ActUserInfo.EnumOnlineState.在线.ToString();
                    userinfo.Logcount += 1;
                    userinfo.Lasttime = DateTime.Now;
                    userinfo.Lastip = WebHelper.GetIP();
                    userinfo.Activetime = DateTime.Now;
                    BLL.ActUser.Update(userinfo);

                    WebHelper.WriteCookie(CookieName, "uid", uid.ToString());
                    WebHelper.WriteCookie(CookieName, "pcode", pcode.ToString());
                    WebHelper.WriteCookie(CookieName, "ctime", _cookietime.ToString());
                    WebHelper.WriteCookie(CookieName, _cookietime);//重置Cookie过期时间

                    //操作日志，登陆
                    BLL.ActOperate.Record(userinfo, Model.ActOperateInfo.EnumOperateType.登录, "用户登陆");

                    return true;
                }
                catch (Exception ex)
                {
                    _errmsg = "用户登录:执行错误！" + ex.ToString(); 
                    return false;
                }
            }
            else
            {
                _errmsg = "用户名密码错误！";
                return false;
            }
        }
        #endregion

        //用户退出:Cookies设置过期并清空
        #region - Logout() -
        /// <summary>
        /// 用户退出:Cookie设置过期并清空
        /// </summary>
        public static void Logout()
        {


            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0
            int uid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "uid"), 0);    //用户ID，游客为0

            Model.ActUserInfo userinfo = BLL.ActUser.GetModel(uid);
            if (userinfo != null)
            {
                //操作日志，登陆
                BLL.ActOperate.Record(userinfo, Model.ActOperateInfo.EnumOperateType.退出, "用户退出");
            }
            
            BLL.ActOnline.Delete(olid);
            BLL.ActUser.SetOffline(uid);//设置用户为离线

            WebHelper.WriteCookie(CookieName, "", -1);

            //---------------------------------------------------------------------------------------

            //Cookie被清除后，注意初始化，无游客信息，验证码会出错
            WebUser.InitialOnline(); // 初始化在线信息 
            WebUser.CleanOnline();   // 清理用户在线信息
        }
        #endregion

        //公共：检查访问者权限
        #region - CheckPermissions(params string[] _codes) -
        /// <summary>
        /// 公共：检查访问者权限
        /// </summary>
        /// <param name="_codes"></param>
        /// <returns></returns>
        public static bool CheckPermission(params string[] _codes)
        {
            // 网域权限检查
            bool reA = false;
            string domain = GetIpDomain(WebHelper.GetIP());
            switch (domain)
            {
                case "本地":
                    reA = true;
                    break;
                case "内网":
                    reA = CheckPermission("内网访问");
                    break;
                case "外网":
                    reA = CheckPermission("外网访问");
                    break;
                case "未知":
                    reA = CheckPermission("外网访问");
                    break;
                default:
                    reA = CheckPermission("外网访问");
                    break;
            }
            if (reA == false) { return false; }

            // 用户权限检查
            bool reB = true;
            foreach (string _code in _codes)
            {
                if (_code != "内网访问" && _code != "外网访问")
                {
                    reB = CheckPermission(_code);
                    if (reB == false) { return false; }
                }
            }
            return reB;
        }
        #endregion

        //私有：检查访问者权限
        #region - CheckPermission(string _code) -
        /// <summary>
        /// 私有：检查访问者权限
        /// </summary>
        /// <param name="_code">权限标识码</param>
        /// <returns></returns>
        private static bool CheckPermission(string _code)
        {
            //--------- 用户权限检查 ---------
            int ctime = Utils.StrToInt(WebHelper.GetCookie(CookieName, "ctime"), 15);//cookie时效
            int uid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "uid"), 0);    //用户ID，游客为0
            string pcode = WebHelper.GetCookie(CookieName, "pcode");                //用户密码 二次加密
            pcode = Utils.Decrypt(Utils.UrlDecode(pcode));
            if (uid != 0 && pcode.Length > 0)
            {
                Model.ActUserInfo userinfo = BLL.ActUser.GetInfoByUserID(uid, pcode);
                if (userinfo != null && userinfo.UserID > 0)
                {
                    Model.ActGroupInfo gm = BLL.ActGroup.GetModel(userinfo.GroupID);  //获取用户组
                    List<Model.ActPermissionInfo> pml = gm.ActPermissionInfoList;           //获取权限信息组
                    if (pml != null && pml.Count > 0)
                    {
                        foreach (Model.ActPermissionInfo pm in pml)
                        {
                            if (_code.Trim() == pm.Code.Trim())
                            {
                                WebHelper.WriteCookie(CookieName, ctime);//重置Cookie过期时间
                                return true;
                            }
                        }
                    }
                }
            }


            //--------- 游客权限检查 ---------
            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0
            string ccode = WebHelper.GetCookie(CookieName, "ccode");                //CookieCode Cookie值（加密，连接即赋值唯一）
            Model.ActOnlineInfo onlineinfo = BLL.ActOnline.GetInfoByCookieCode(olid, ccode);
            if (onlineinfo != null && onlineinfo.OnlineID > 0)
            {
                Model.ActGroupInfo gm = BLL.ActGroup.GetModel(GuestGroupID);  //获取用户组
                if (gm.ActPermissionInfoList != null)
                {
                    List<Model.ActPermissionInfo> pml = gm.ActPermissionInfoList;       //获取权限信息组
                    if (pml != null && pml.Count > 0)
                    {
                        foreach (Model.ActPermissionInfo pm in pml)
                        {
                            if (_code.Trim() == pm.Code.Trim())
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        #endregion


        //获取用户ID
        #region - GetUserIDByCookie() -
        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public static int GetUserIDByCookie()
        {
            Model.ActUserInfo m = GetUserInfoByCookie();
            if (m != null)
            {
                return m.UserID;
            }

            return 0;
        }
        #endregion

        //获取用户信息
        #region - GetUserInfoByCookie() -
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static Model.ActUserInfo GetUserInfoByCookie()
        {
            int uid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "uid"), 0);    //用户ID，游客为0
            string pcode = WebHelper.GetCookie(CookieName, "pcode");                //用户密码 二次加密
            pcode = Utils.Decrypt(Utils.UrlDecode(pcode));

            if (uid != 0 && pcode.Length > 0)
            {
                Model.ActUserInfo userinfo = BLL.ActUser.GetInfoByUserID(uid, pcode);
                if (userinfo != null && userinfo.UserID > 0)
                {
                    return userinfo;
                }
            }

            return null;
        }
        #endregion

        //获取在线信息
        #region - GetOnlineInfoByCookie() -
        /// <summary>
        /// 获取在线信息
        /// </summary>
        /// <returns></returns>
        public static Model.ActOnlineInfo GetOnlineInfoByCookie()
        {
            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);  //在线ID，游客为0
            string ccode = Utils.FilterSqlString(WebHelper.GetCookie(CookieName, "ccode")); //CookieCode Cookie值（加密，连接即赋值唯一）

            Model.ActOnlineInfo onlineinfo = BLL.ActOnline.GetInfoByCookieCode(olid, ccode);
            if (onlineinfo != null && onlineinfo.OnlineID > 0)
            {
                return onlineinfo;
            }

            return null;
        }
        #endregion

        //获取菜单可视数据
        #region - GetMenuListByNode(int _node) -
        /// <summary>
        /// 获取菜单可视数据
        /// </summary>
        /// <param name="_node">纵深节点ID</param>
        /// <returns></returns>
        public static List<Model.ActMenuInfo> GetMenuListByNode(int _node)
        {
            List<Model.ActMenuInfo> menulist = new List<Core.Model.ActMenuInfo>();

            //依据用户组获取菜单
            int gid = 0;
            if (GetUserInfoByCookie() != null)
            {
                gid = GetUserInfoByCookie().GroupID;
            }
            else
            {
                gid = Config.GuestGroupID;
            }

            Model.ActGroupInfo gm = BLL.ActGroup.GetModel(gid);
            List<Model.ActMenuInfo> mlgroup = gm.ActMenuInfoList;
            
            //所有菜单
            List<Model.ActMenuInfo> mlall = BLL.ActMenu.ListNodeFirst(_node);

            foreach (Model.ActMenuInfo m in mlgroup)
            {
                Model.ActMenuInfo minfo = mlall.Find(delegate(Model.ActMenuInfo model) { return model.MenuID == m.MenuID; });
                if (minfo != null && minfo.MenuID > 0)
                {
                    menulist.Add(minfo);
                }
            }

            return menulist;
        }
        #endregion

        //根据IP获得网域（本地，内网，外网，未知）
        #region - GetIpDomain(string _userip) -
        /// <summary>
        /// 根据IP获得网域（本地，内网，外网，未知）
        /// </summary>
        /// <param name="_userip"></param>
        /// <returns></returns>
        public static string GetIpDomain(string _ip)
        {
            string unitNetAddr = NetAddrIn;  //内网IP段
            string unitExtAddr = NetAddrOut; //外网IP段

            string[] arrNetAddr = Utils.SplitString(unitNetAddr, "\n");
            string[] arrExtAddr = Utils.SplitString(unitExtAddr, "\n");

            if (_ip == "127.0.0.1")
            {
                return "本地";
            }
            else if (Utils.InIPArray(_ip, arrNetAddr))
            {
                return "内网";
            }
            else if (Utils.InIPArray(_ip, arrExtAddr))
            {
                return "外网";
            }
            else
            {
                return "未知";
            }
        }
        #endregion

        
        //---------------------------------------------------------------------

        //初始化在线信息:Cookie([olid],[ccode])写入
        #region - InitialOnline() -
        /// <summary>
        /// 初始化在线信息:Cookie([olid],[ccode])写入
        /// </summary>
        public static void InitialOnline()
        {
            int ctime = Utils.StrToInt(WebHelper.GetCookie(CookieName, "ctime"), 15);    //cookie 过期时间
            //-----------------------------------------------------------------------------------------------------------------
            int uid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "uid"), 0);    //用户ID，游客为0
            string pcode = WebHelper.GetCookie(CookieName, "pcode");                //用户密码 二次加密
            Model.ActUserInfo userinfo = new Model.ActUserInfo();
            if (pcode.Length > 0)
            {
                userinfo = BLL.ActUser.GetInfoByUserID(uid, Utils.Decrypt(Utils.UrlDecode(pcode)));
            }
            else
            {
                userinfo = null;
            }
            //-----------------------------------------------------------------------------------------------------------------
            int olid = Utils.StrToInt(WebHelper.GetCookie(CookieName, "olid"), 0);          //在线ID，游客为0
            string ccode = Utils.FilterSqlString(WebHelper.GetCookie(CookieName, "ccode")); //CookieCode Cookie值（加密，连接即赋值唯一）
            Model.ActOnlineInfo onlineinfo = BLL.ActOnline.GetInfoByCookieCode(olid, ccode);
            //-----------------------------------------------------------------------------------------------------------------

            if (userinfo != null && userinfo.UserID > 0)
            {
                BLL.ActUser.SetOnline(userinfo.UserID);//设置用户为在线
                // 注册用户
                if (onlineinfo != null && onlineinfo.OnlineID > 0)
                {
                    // 注册用户有在线信息
                    onlineinfo.ActiveIp = WebHelper.GetIP();
                    onlineinfo.ActivePage = WebHelper.GetUrlRaw();
                    onlineinfo.ActiveTime = DateTime.Now;
                    BLL.ActOnline.Update(onlineinfo);

                    WebHelper.WriteCookie(CookieName, ctime);
                }
                else
                {
                    // 注册用户无在线信息
                    ccode = Utils.MD5(Utils.CreateAuthStr(6));

                    Model.ActOnlineInfo m = new Model.ActOnlineInfo();
                    m.ActiveIp = WebHelper.GetIP();
                    m.ActivePage = WebHelper.GetUrlRaw();
                    m.ActiveTime = DateTime.Now;
                    m.CookieCode = ccode;
                    m.PasswordCode = pcode;
                    m.UserID = userinfo.UserID;
                    m.VerifyCode = "";
                    m.LaunchTime = DateTime.Now;
                    olid = BLL.ActOnline.Add(m);

                    WebHelper.WriteCookie(CookieName, ctime);
                    WebHelper.WriteCookie(CookieName, "olid", olid.ToString());
                    WebHelper.WriteCookie(CookieName, "ccode", ccode);
                }
            }
            else
            {
                // 游客访问
                if (onlineinfo != null && onlineinfo.OnlineID > 0)
                {
                    //游客有在线信息
                    onlineinfo.ActiveIp = WebHelper.GetIP();
                    onlineinfo.ActivePage = WebHelper.GetUrlRaw();
                    onlineinfo.ActiveTime = DateTime.Now;
                    BLL.ActOnline.Update(onlineinfo);

                    WebHelper.WriteCookie(CookieName, ctime);
                }
                else
                {
                    //游客无在线信息
                    ccode = Utils.MD5(Utils.CreateAuthStr(6));

                    Model.ActOnlineInfo m = new Model.ActOnlineInfo();
                    m.ActiveIp = WebHelper.GetIP();
                    m.ActivePage = WebHelper.GetUrlRaw();
                    m.ActiveTime = DateTime.Now;
                    m.CookieCode = ccode;
                    m.PasswordCode = "";
                    m.UserID = 0;
                    m.VerifyCode = "";
                    m.LaunchTime = DateTime.Now;
                    olid = BLL.ActOnline.Add(m);

                    WebHelper.WriteCookie(CookieName, "", ctime);
                    WebHelper.WriteCookie(CookieName, "olid", olid.ToString());
                    WebHelper.WriteCookie(CookieName, "ccode", ccode);

                }
            }
        }
        #endregion

        //清理用户在线信息
        #region - CleanOnline() -
        /// <summary>
        /// 清理用户在线信息
        /// </summary>
        public static void CleanOnline()
        {
            //---  清理用户在线信息  ---------------------------------------------------------------
            int _minute = 3; //清理空闲时间大于3分钟的在线信息
            List<Model.ActOnlineInfo> ml = BLL.ActOnline.GetModelList("[ActiveTime]<'" + DateTime.Now.AddMinutes(-_minute) + "'");
            if (ml.Count > 0)
            {
                for (int i = 0; i < ml.Count; i++)
                {
                    BLL.ActOnline.Delete(ml[i].OnlineID);
                    BLL.ActUser.SetOffline(ml[i].UserID);//设置用户为离线
                }
            }
        }
        #endregion

        //---------------------------------------------------------------------

    }
}
