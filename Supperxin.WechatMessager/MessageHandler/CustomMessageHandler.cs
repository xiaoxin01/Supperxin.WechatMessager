using System;
using System.IO;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.MessageContexts;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.NeuChar.App.AppStore;

namespace Supperxin.WechatMessager.MessageHandler
{
    public class CustomMessageHandler : MessageHandler<DefaultMpMessageContext>
    {
        public CustomMessageHandler(Stream inputStream, PostModel postModel)
            : base(inputStream, postModel)
        {
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
    }
}
