# 一个可以利用微信测试号搭建的类似Server酱的消息服务

## What can I do

利用微信测试号，通过web请求发送消息到手机上。

## Demo

1. 微信扫码测试号关注：

![](http://mmbiz.qpic.cn/mmbiz_jpg/qt4Br1q5Yy5vF8tjhFmNKPJV6JESuAXGlZRboJlVibUjLiaNGqjjc5nuFxMqh1thlndjiaM5xGwJrztnsrNybUVww/0)

2. 接收测试号消息

```
感谢订阅，您的专属消息地址为： http://wechat.supperxin.com/message/马赛克

您可以访问如下链接来测试：

http://wechat.supperxin.com/message/马赛克?title=MessageTitle&content=MessageContent
```

3. 发送消息

给如上地址发送get请求，可选title和content参数

4. 效果


![36WmNV.jpg](https://s2.ax1x.com/2020/03/01/36WmNV.jpg)

![36WK9U.jpg](https://s2.ax1x.com/2020/03/01/36WK9U.jpg)

![36WnhT.jpg](https://s2.ax1x.com/2020/03/01/36WnhT.jpg)

## How to use

1. 申请微信测试号及相关设定

[申请微信测试号](https://mp.weixin.qq.com/debug/cgi-bin/sandbox?t=sandbox/login)

扫码登录以后就可以拿到测试号的appID和appsecret：

![3a1wMF.png](https://s2.ax1x.com/2020/02/26/3a1wMF.png)

扫码关注测试号，可以得到关注的微信号userID：

![3a1gG6.png](https://s2.ax1x.com/2020/02/26/3a1gG6.png)

建立消息模板，得到模板ID，内容固定为

    {{title.DATA}}

    {{content.DATA}}

![3a1Hit.png](https://s2.ax1x.com/2020/02/26/3a1Hit.png)

2. 搭建服务器环境

执行如下命令，下载代码和配置微信测试号信息

```bash
git clone https://github.com/xiaoxin01/Supperxin.WechatMessager.git
cd Supperxin.WechatMessager/Supperxin.WechatMessager
cp appsettings.json appsettings.Production.json
vi appsettings.Production.json
```

在appsettings.Production.json修改Wechat节点：

```json
  "Wechat": {
    "AppID": "",
    "AppSecret": "",
    "TemplateMessageID": ""
  },
```

构建和运行服务：

```bash
cd ..
docker build . -f ./Supperxin.WechatMessager/Dockerfile -t wechat-messager
docker run --name wechat_messager_1 -p 5008:80 --rm wechat-messager
```

3. 发送消息

向服务器发送一条get消息即可：

    http://[ip]:5008/message/[userID]?title=this is title&content=this is content
