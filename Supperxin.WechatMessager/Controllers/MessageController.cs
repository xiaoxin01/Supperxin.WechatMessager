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
        public async System.Threading.Tasks.Task<ActionResult> GetAsync(string id, [FromQuery] SendMessageModel postModel)
        {
            //Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessage
            var accesstoken = await _accessTokenService.GetAccessToken(_wechatSetting.AppID, _wechatSetting.AppSecret);
            return new JsonResult(new { aa = postModel, b = id, s = _wechatSetting, token = accesstoken });

            //return new JsonResult(new { aa = postModel, b = id, s = _wechatSetting });
        }
    }
}
