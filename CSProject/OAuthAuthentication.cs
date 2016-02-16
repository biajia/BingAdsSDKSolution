using Microsoft.BingAds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSProject
{
    class OAuthAuthentication
    {
        public static AuthorizationData GetAuthorizationData()
        {
            //var authentication = GetOAuthDesktopMobileImplicitGrant();
            //var authentication = GetOAuthDesktopMobileAuthCodeGrantUsingCode();
            var authentication = GetOAuthDesktopMobileAuthCodeGrantUsingToken();

            var authorizationData = new AuthorizationData
            {
                Authentication = authentication,
                CustomerId = GlobalConfig.CustomerId,
                AccountId = GlobalConfig.AccountId,
                DeveloperToken = GlobalConfig.DevToken
            };

            return authorizationData;
        }

        //authorization endpoint
        //https://login.live.com/oauth20_authorize.srf?client_id=000000004C181609&scope=bingads.manage&response_type=token&redirect_uri=https://login.live.com/oauth20_desktop.srf
        private static Authentication GetOAuthDesktopMobileImplicitGrant()
        {
            Uri urlContainsAccessToken = new Uri("https://login.live.com/oauth20_desktop.srf?lc=1033#access_token=EwBwAnhlBAAUxT83/QvqiAZEx5SuwyhZqHzk21oAAQByeu4TZtrHQXh6uMcQ4JXDbWzcJxVuVwl1RydCm19GuyJM7eAIYzuSgteCLEW%2bkQyFsM79mVPUiDKOLboD0IPEXvrAKRYFbJ7/gcNIZHWxWSfE5ua8Gn5/p%2bRB76rY6KTvxfRM3pKmQ5E698qZ%2b0c7iIE9mBZ%2b99kNEZGf%2bAZpUwwt/b60PEg8u9w9PyMFgCmZ867ls7xNRKswHuED5B6nBAtbHYaIGp0dXWsAF791CVTXlUuqSd34EqPoqzLfwWtF5sVmPOD0FzBBKZ8WGNSlouJH0kdHCLYLB35oxOD3A%2bEbe/eTU8OIR69s3hnjlluE1m0WJBdTlheobg8ZDTkDZgAACJBHdLDmP1gjQAGKO5duubr8mq2gXSZfhXgDdal3yBBfkHk9ASrEacXf59Y9YmMBOT7uJIEpSyB8T5R4evs3Mhew069aX6B3n5%2bK7MCfK4RHyjCLzDlCW/WKYfNlU2jWW6u4CZoahSbGd03uI5UtmmxHl/kMLWhe6QSxr26wrzmXDQoRRQC5TnGoF3R0gy7I2Cmh4Ucxqie/1eW0aPyJHA8nAWiv/dwaoSZc/bzjoIeI5nfizyy9zq3jo7xaVOLUhHVTRPXLRkV6H7XnKxnFVN6vkiwNRsnBl0nL480KqZ8kokMKE8/QqiyKmTW3CBWLJJmlLhmVLxKtMlDoQqST%2bQvv1igLaCsb/5VU7IQ/NFYcpg24O2C77hq6/8bpjdAnxw%2b2dEQzIkzqbc7BPMFAAIg13QhVZGBGFG8bF5S6vjmmrrPqvx/diWYiMlsB&token_type=bearer&expires_in=3600&scope=bingads.manage&user_id=38097d39a8aba1094d0e0fc7996e3bf5");
            var authentication = new OAuthDesktopMobileImplicitGrant(GlobalConfig.ClientId);
            authentication.ExtractAccessTokenFromUri(urlContainsAccessToken);
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessToken = {0}", authentication.OAuthTokens.AccessToken));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessTokenExpiresInSeconds = {0}", authentication.OAuthTokens.AccessTokenExpiresInSeconds));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.RefreshToken = {0}", authentication.OAuthTokens.RefreshToken));

            return authentication;
        }

        //authorization endpoint
        //https://login.live.com/oauth20_authorize.srf?client_id=000000004C181609&scope=bingads.manage&response_type=code&redirect_uri=https://login.live.com/oauth20_desktop.srf
        private static Authentication GetOAuthDesktopMobileAuthCodeGrantUsingCode()
        {
            var authentication = new OAuthDesktopMobileAuthCodeGrant(GlobalConfig.ClientId);

            Uri urlContainsCode = new Uri("https://login.live.com/oauth20_desktop.srf?code=M660f6d05-b20f-88b6-106f-deb7707b0072&lc=1033");
            CommonHelper.OutputMessage(string.Format("urlContainsCode = {0}", urlContainsCode));
            authentication.RequestAccessAndRefreshTokensAsync(urlContainsCode).Wait();
            WriteToFile(authentication.OAuthTokens);
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessToken = {0}", authentication.OAuthTokens.AccessToken));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessTokenExpiresInSeconds = {0}", authentication.OAuthTokens.AccessTokenExpiresInSeconds));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.RefreshToken = {0}", authentication.OAuthTokens.RefreshToken));

            return authentication;
        }

        private static Authentication GetOAuthDesktopMobileAuthCodeGrantUsingToken()
        {
            string accessToken = "", refreshToken = "";
            DateTime generationTime = DateTime.MinValue;
            int accessTokenExpiresInSeconds = 0;
            //Hardcode file name
            string iniFile = "MyCredential.ini";
            ParseOAuthFile(iniFile, out generationTime, out accessToken, out accessTokenExpiresInSeconds, out refreshToken);
            CommonHelper.OutputSuccessMessage("Read MyCredential.ini");
            CommonHelper.OutputMessage(string.Format("OAuthTokens.GenerationTime = {0}", CommonHelper.GetDateTimeInMillisecond(generationTime)));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessToken = {0}", accessToken));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessTokenExpiresInSeconds = {0}", accessTokenExpiresInSeconds));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.RefreshToken = {0}", refreshToken));

            var authentication = new OAuthDesktopMobileAuthCodeGrant(GlobalConfig.ClientId);
            if (NeedRequestAccessToken(generationTime, accessTokenExpiresInSeconds))
            {
                authentication.RequestAccessAndRefreshTokensAsync(refreshToken).Wait();
                WriteToFile(authentication.OAuthTokens);
            }
            else
            {
                //How to generate OAuthTokens
            }

            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessToken = {0}", authentication.OAuthTokens.AccessToken));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.AccessTokenExpiresInSeconds = {0}", authentication.OAuthTokens.AccessTokenExpiresInSeconds));
            CommonHelper.OutputMessage(string.Format("OAuthTokens.RefreshToken = {0}", authentication.OAuthTokens.RefreshToken));

            return authentication;
        }

        private static void WriteToFile(OAuthTokens token)
        {
            string res = "#Generation Time\r\n#AccessToken\r\n#AccessTokenExpiresInSeconds\r\n#RefreshToken\r\n";
            res += string.Format("{0}\r\n", CommonHelper.GetDateTimeInMillisecond());
            res += string.Format("{0}\r\n", token.AccessToken);
            res += string.Format("{0}\r\n", token.AccessTokenExpiresInSeconds);
            res += string.Format("{0}\r\n", token.RefreshToken);

            DateTime currentTime = DateTime.Now;
            string fileName = string.Format("MyCredential_{0}.ini", currentTime.ToString("yyyy_MM_dd_hh_mm_ss"));
            StreamWriter writer = new StreamWriter(fileName);
            writer.Write(res);
            writer.Close();
            fileName = string.Format("MyCredential.ini");
            writer = new StreamWriter(fileName);
            writer.Write(res);
            writer.Close();
        }

        private static bool NeedRequestAccessToken(DateTime generationTime, int accessTokenExpiresInSeconds)
        {
            return true;
            DateTime currentTime = DateTime.Now;

            //CommonHelper.OutputMessage(string.Format("CurrentTime = {0}, GenerationTime = {1}, Diff = {2}", CommonHelper.GetDateTimeInMillisecond(currentTime), CommonHelper.GetDateTimeInMillisecond(generationTime), currentTime.Subtract(generationTime).TotalSeconds));

            //add 60 seconds for buffer
            if(currentTime.Subtract(generationTime).TotalSeconds + 60 >= accessTokenExpiresInSeconds)
            {
                return true;
            }

            return false;
        }

        private static void ParseOAuthFile(string iniFile, out DateTime generationTime, out string accessToken, out int accessTokenExpiresInSeconds, out string refreshToken)
        {
            StreamReader reader = new StreamReader(iniFile);
            string line = "";
            //skip comments line at the beginning
            while ((line = reader.ReadLine()) != null)
            {
                if (!line.StartsWith("#")) break;
            }
            generationTime = DateTime.Parse(line);
            accessToken = reader.ReadLine();
            accessTokenExpiresInSeconds = int.Parse(reader.ReadLine());
            refreshToken = reader.ReadLine();

            reader.Close();
        }
    }
}
