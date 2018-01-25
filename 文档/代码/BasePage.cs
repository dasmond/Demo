using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Text;


using DSNN.Core.BLL;
using DSNN.Core.Model;
using DSNN.Core.API.Weixin;

namespace DSNN.Core.Common
{
    public class BasePage : Page
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BasePage()
        {
            BLL.ActOnline.CheckTable(); // 检查[Act_Online]表ID最大值，若超出1亿，清空表
            WebUser.InitialOnline();    // 初始化在线信息:Cookie([olid],[ccode])写入
            WebUser.CleanOnline();      // 清理用户在线信息
        }

        #region - 属性：配置参数 -
        /// <summary>
        /// 页面唯一散列值32位，带分隔符，随刷新
        /// </summary>
        public string PageHash
        {
            get
            {
                return System.Guid.NewGuid().ToString();
            }
        }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SysName 
        {
            get
            {
                return BLL.ActConfig.GetValueByName("SysName");
            }
        }
        /// <summary>
        /// 站点根目录 无"/"
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return (BLL.ActConfig.GetValueByName("BaseUrl") == "") ? WebHelper.GetUrlBase() : BLL.ActConfig.GetValueByName("BaseUrl");
            }
        }
        /// <summary>
        /// 主题样式目录 无"/"
        /// </summary>
        public string CssUrl
        {
            get
            {
                return this.BaseUrl + "/style/default";
            }
        }
        /// <summary>
        /// 游客用户组ID
        /// </summary>
        public int GuestGroupID
        {
            get
            {
                return Utils.StrToInt(BLL.ActConfig.GetValueByName("GuestGroupID"), 0);
            }
        }
        /// <summary>
        /// 微信关注用户组ID
        /// </summary>
        public int WeixinGroupID
        {
            get
            {
                return Utils.StrToInt(BLL.ActConfig.GetValueByName("WeixinGroupID"), 0);
            }
        }
        /// <summary>
        /// 上传文件根目录 后缀无"/"
        /// </summary>
        public string FilesPath 
        {
            get
            {
                return BLL.ActConfig.GetValueByName("FilesPath");
            }
        }
        /// <summary>
        /// 分类图标存储目录 前后无"/"
        /// </summary>
        public string CategoryPath
        {
            get
            {
                return BLL.ActConfig.GetValueByName("CategoryPath");
            }
        }
        /// <summary>
        /// 用户头像存储目录 前后无"/"
        /// </summary>
        public string AvatarPath
        {
            get
            {
                return BLL.ActConfig.GetValueByName("AvatarPath");
            }
        }
        /// <summary>
        /// 用户默认头像文件
        /// </summary>
        public string AvatarDefault 
        {
            get
            {
                return BLL.ActConfig.GetValueByName("AvatarDefault");
            }
        }
        #endregion

        #region - 属性：当前信息 CrtUser、CrtOnline -
        /// <summary>
        /// 当前登录用户 CurrentUser
        /// </summary>
        public ActUserInfo CrtUser 
        {
            get
            {
                ActUserInfo model = WebUser.GetUserInfoByCookie();
                if (model == null)
                {
                    model = new ActUserInfo();
                    model.UserID = 0;    //用户ID
                    model.GroupID = GuestGroupID;    //用户组ID
                    model.Username = "游客";    //用户名
                    model.Password = "";    //密码
                    model.Email = "";    //电子邮件
                    model.Mobile = "";    //手机号码
                    model.Realname = "游客";    //真实姓名
                    model.Gender = "男";    //性别
                    model.NewMessage = 0;    //新短消息数
                    model.Regip = WebHelper.GetIP();    //注册时的IP
                    model.Regtime = DateTime.Now;    //注册时的时间
                    model.Lastip = WebHelper.GetIP();    //最后一次登录时的IP
                    model.Lasttime = DateTime.Now;    //最后一次登录的时间
                    model.Activetime = DateTime.Now;    //最后一次活动的时间
                    model.Logcount = 0;    //累积登录次数
                    model.Avatar = BaseUrl + "/" + AvatarPath + "/" + AvatarDefault; //用户头像（不包含路径）
                    model.OnlineState = "未知";    //在线状态（未知,离线,在线）
                    model.CheckState = "未知";    //信息状态（未知,待审,通过,驳回）
                    model.Created = DateTime.Now;    //添加时间
                    model.Updated = DateTime.Now;    //更新时间
                    model.Deleted = 0;   //是否已删（0,1）
                    //model.ActGroupName //通过 this.GroupID -> ActGroupInfo 获取
                    //model.ActGroupInfo (外键实体 this.GroupID)
                    //model.ActAttachmentInfoList (主键实体 this.UserID)
                    //model.ActOperateInfoList (主键实体 this.UserID)
                }
                else
                {
                    if (model.Avatar.Length > 0)
                    {
                        model.Avatar = BaseUrl + "/" + AvatarPath + "/" + model.Avatar; //用户头像（不包含路径）
                    }
                    else
                    {
                        model.Avatar = BaseUrl + "/" + AvatarPath + "/" + AvatarDefault; //用户头像（不包含路径）
                    }
                    if (model.Realname.Trim().Length == 0)
                    {
                        model.Realname = model.Username; //用户：真实姓名
                    }
                }
                return model;
            }
        }
        /// <summary>
        /// 当前在线信息 CurrentOnline
        /// </summary>
        public ActOnlineInfo CrtOnline
        { 
            get
            {
                ActOnlineInfo model = WebUser.GetOnlineInfoByCookie();
                return model;
            }
        }
        #endregion
        
        





    }
}
