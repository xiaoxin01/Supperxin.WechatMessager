using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Supperxin.WechatMessager.MessageHandler;
using Supperxin.WechatMessager.Model;

namespace Supperxin.WechatMessager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WechatController : ControllerBase
    {
        private readonly WechatSetting _wechatSetting;
        private readonly ILogger<WechatController> _logger;

        public WechatController(WechatSetting wechatSetting, ILogger<WechatController> logger)
        {
            _wechatSetting = wechatSetting;
            _logger = logger;
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

        [HttpPost]
        [ActionName("Index")]
        public async System.Threading.Tasks.Task<ActionResult> PostAsync([FromQuery] PostModel postModel)
        {
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, _wechatSetting.Token))
            {
                _logger.LogError("参数错误！");
                return Content("参数错误！");
            }

            postModel.Token = _wechatSetting.Token;
            // postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
            postModel.AppId = _wechatSetting.AppID;//根据自己后台的设置保持一致

            #region // Add for read Request.Body multiple times
            {
                Request.EnableBuffering();
                var reader = new System.IO.StreamReader(Request.Body, encoding: System.Text.Encoding.UTF8);
                {
                    var body = await reader.ReadToEndAsync();
                    _logger.LogInformation(body);
                    // Do some processing with body…
                    // Reset the request body stream position so the next middleware can read it
                    Request.Body.Position = 0;
                }
            }
            #endregion

            var messageHandler = new CustomMessageHandler(Request.Body, postModel);//接收消息（第一步）
            Request.Body.Position = 0;

            messageHandler.Execute();//执行微信处理过程（第二步）

            return Content(messageHandler.ResponseDocument.ToString()); ;//返回（第三步）
        }
    }
}
