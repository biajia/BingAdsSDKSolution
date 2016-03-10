using Microsoft.BingAds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if(!GlobalConfig.Initialize())
                {
                    CommonHelper.OutputErrorMessage("Error Reading Config!");
                    return;
                }

                var authorizationData = OAuthAuthentication.GetAuthorizationData();
                if(authorizationData == null)
                {
                    CommonHelper.OutputErrorMessage("Error Getting AuthorizationData");
                    return;
                }

                //CampaignCRUD campaignOperator = new CampaignCRUD();
                //campaignOperator.RunAsync(authorizationData).Wait();
                AdExtensionCRUD extensionOperator = new AdExtensionCRUD();
                extensionOperator.RunAsync(authorizationData).Wait();
            }
            catch (Exception ex)
            {
                CommonHelper.OutputErrorMessage(ex.Message);
                CommonHelper.OutputErrorMessage(ex.StackTrace);
            }
        }

        private static void TestRefreshToken()
        {
            Dictionary<string, int> refreshTokenDic = new Dictionary<string, int>();
            Dictionary<string, int> accessTokenDic = new Dictionary<string, int>();
            for (int i = 0; i < 100; i++)
            {
                CommonHelper.OutputSuccessMessage(string.Format("Iteration {0}:", i));

                var authorizationData = OAuthAuthentication.GetAuthorizationData();
                OAuthTokens token = (authorizationData.Authentication as OAuthDesktopMobileAuthCodeGrant).OAuthTokens;
                if(refreshTokenDic.ContainsKey(token.RefreshToken))
                {
                    CommonHelper.OutputErrorMessage(string.Format("RefreshToken Already Exist! Index = {0}", refreshTokenDic[token.RefreshToken]));
                }
                else
                {
                    refreshTokenDic[token.RefreshToken] = refreshTokenDic.Count;
                }

                if (accessTokenDic.ContainsKey(token.AccessToken))
                {
                    CommonHelper.OutputErrorMessage(string.Format("AccessToken Already Exist! Index = {0}", accessTokenDic[token.AccessToken]));
                }
                else
                {
                    accessTokenDic[token.AccessToken] = accessTokenDic.Count;
                }

                System.Threading.Thread.Sleep(1000);
            }

            CommonHelper.OutputMessage(string.Format("RefreshTokenDic.Count = {0}", refreshTokenDic.Count));
            CommonHelper.OutputMessage(string.Format("AccessTokenDic.Count = {0}", accessTokenDic.Count));
        }
    }
}
