using System;
using System.IO;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.NeuChar.App.AppStore;
using Supperxin.WechatMessager.Model;

namespace Supperxin.WechatMessager.MessageHandler
{
    public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        private readonly WechatSetting _wechatSetting;

        public CustomMessageHandler(Stream inputStream, PostModel postModel, WechatSetting wechatSetting)
            : base(inputStream, postModel)
        {
            _wechatSetting = wechatSetting;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = "这条消息来自DefaultResponseMessage。";
            return responseMessage;
        }

        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = $"您发了：{requestMessage.Content}";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = $"感谢订阅，您的专属消息地址为： {_wechatSetting.WechatMessageUrl}{requestMessage.FromUserName}\n\n";
            responseMessage.Content += $"您可以访问如下链接来测试：\n\n{_wechatSetting.WechatMessageUrl}{requestMessage.FromUserName}?title=MessageTitle&content=MessageContent";

            return responseMessage;
        }
    }
}
