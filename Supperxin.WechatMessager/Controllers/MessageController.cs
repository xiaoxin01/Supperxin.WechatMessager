using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Supperxin.WechatMessager.Model;
using Supperxin.WechatMessager.Services;

namespace Supperxin.WechatMessager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly WechatSetting _wechatSetting;
        private readonly IAccessTokenService _accessTokenService;

        public MessageController(WechatSetting wechatSetting, IAccessTokenService accessTokenService)
        {
            _wechatSetting = wechatSetting;
            _accessTokenService = accessTokenService;
        }
        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> GetAsync(string id, [FromQuery] SendMessageModel messageModel)
        {
            var result = await SendTemplateMessageAsync(messageModel.Title, messageModel.Content, id);

            return new JsonResult(result);
        }

        [HttpPost("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> PostAsync(string id,
            [FromBody] GrafanaWebhookModel webhookModel)
        {
            var result = await SendTemplateMessageAsync(webhookModel.title, webhookModel.message, id);

            return new JsonResult(result);
        }

        private async System.Threading.Tasks.Task<SendTemplateMessageResult> SendTemplateMessageAsync(
            string title, string content, string openID
        )
        {

            // Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage
            var accesstoken = await _accessTokenService.GetAccessToken(_wechatSetting.AppID, _wechatSetting.AppSecret);
            // get error, see https://www.cnblogs.com/szw/p/9265828.html
            // var accesstoken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(_wechatSetting.AppID, _wechatSetting.AppSecret);

            var data = new
            {
                title = new
                {
                    value = title,
                    color = "#cc2900"
                },
                content = new
                {
                    value = content,
                    color = "#173177"
                }
            };
            var result = await Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(accesstoken, openID,
            _wechatSetting.TemplateMessageID, "", data);

            return result;
        }
    }
}
