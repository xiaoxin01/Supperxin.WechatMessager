using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Supperxin.WechatMessager.Model;

namespace Supperxin.WechatMessager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WechatController : ControllerBase
    {
        private readonly WechatSetting _wechatSetting;

        public WechatController(WechatSetting wechatSetting)
        {
            _wechatSetting = wechatSetting;
        }
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public ActionResult Get([FromQuery] PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _wechatSetting.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + ","
                    + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, _wechatSetting.Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        // [HttpPost]
        // [ActionName("Index")]
        // public ActionResult Post(PostModel postModel)
        // {
        //     if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
        //     {
        //         return Content("参数错误！");
        //     }

        //     postModel.Token = Token;
        //     postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
        //     postModel.AppId = AppId;//根据自己后台的设置保持一致

        //     var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);//接收消息（第一步）

        //     messageHandler.Execute();//执行微信处理过程（第二步）

        //     return new FixWeixinBugWeixinResult(messageHandler);//返回（第三步）
        // }
    }
}
