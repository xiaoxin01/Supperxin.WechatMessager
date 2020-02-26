using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Supperxin.WechatMessager.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IMemoryCache _cache;

        public AccessTokenService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public async Task<string> GetAccessToken(string appID, string secret)
        {
            if (string.IsNullOrEmpty(appID) || string.IsNullOrEmpty(secret))
            {
                throw new System.ArgumentNullException("appID or secrete can't be null");
            }
            var access_token = await _cache.GetOrCreateAsync<string>(appID, async entry =>
            {
                var result = await Senparc.Weixin.MP.CommonAPIs.CommonApi.GetTokenAsync(appID, secret);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                {
                    return null;
                }

                entry.Value = result.access_token;
                entry.SlidingExpiration = System.TimeSpan.FromSeconds(result.expires_in);

                return result.access_token;
            });

            return access_token;
        }
    }
}
