namespace Supperxin.WechatMessager.Services
{
    public interface IAccessTokenService
    {
        System.Threading.Tasks.Task<string> GetAccessToken(string appID, string secret);
    }
}
