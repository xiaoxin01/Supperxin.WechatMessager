using Microsoft.AspNetCore.Mvc;
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

            // Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage
            var accesstoken = await _accessTokenService.GetAccessToken(_wechatSetting.AppID, _wechatSetting.AppSecret);
            // get error, see https://www.cnblogs.com/szw/p/9265828.html
            // var accesstoken = Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessToken(_wechatSetting.AppID, _wechatSetting.AppSecret);

            var data = new
            {
                title = new
                {
                    value = messageModel.Title,
                    color = "#cc2900"
                },
                content = new
                {
                    value = messageModel.Content,
                    color = "#173177"
                }
            };
            var result = await Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(accesstoken, id,
            _wechatSetting.TemplateMessageID, "", data);

            return new JsonResult(result);
        }
    }
}
