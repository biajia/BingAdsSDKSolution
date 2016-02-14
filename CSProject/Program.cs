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
                var authentication = new OAuthDesktopMobileImplicitGrant("000000004C181609");

                //https://login.live.com/oauth20_authorize.srf?client_id=000000004C181609&scope=bingads.manage&response_type=token&redirect_uri=https://login.live.com/oauth20_desktop.srf
                authentication.ExtractAccessTokenFromUri(new Uri("https://login.live.com/oauth20_desktop.srf?lc=2052#access_token=EwBwAnhlBAAUxT83/QvqiAZEx5SuwyhZqHzk21oAAU0TMvl3p6p3mXU7NM%2bFRcuqK/pOA9I2UXoOfwH43uK9XAa4QwU6Oa6HVV7vr9l6Y4wn7kefr4nSEcUKxOj5fR5/E4HAv%2b4im1qPgaWFwQsYqtu4eky13Gt5MDvszXLng64srzKNJoHny36BYQPGPrnwppWfWGxPh2YRj13qSuEV/GuwLVIUtceLksNXaZKLxT6oWFRQAdUqLJmAUJWGWWhdWSYPfMl52Inm4b%2b9i8kH7N7A8T5DGcfQHR0NgDduRzKEkD4o7DbI8mUjn1nsyUCbNQuXu2hx4QLuiCZNEHBI2GTDCi7q9B/VJ9h86spkHJZdA3hKAYuYK/sowY78D40DZgAACK3NzvKPHzQ0QAFGWyMBqp3a4v1hOpF3Ob7oRowg8c1RYiUcegAJP8u4cz/N2RWcv514skoy/kXVjQ6NOF5EEut5XUmFmQZsZMyR00vok5JgnMAz94yx3L1TN0LrxU2sLPY/URr1wdT%2b4arvvDxZoxiEEQdUFCd1Fd93yaxTODz74EtpsOwfBKteXsAp248%2bHPZskhI/wJEE/ZcqeKEA8xqFPyJL03LsklcpuTnA9DTL8SXjbDLd84QxMQP45d%2bhNncF0nR%2bFVMhSc7i0scz61fVgWf30wapewxwYp4O%2buqlAm6qP5wn14WPtjxfzO76DWYDLRuGBENYhr%2b0D0Wr4ppLps4WMfmWMLiGrJVtRnXctHGgeXrFRhBP2xLKr63R5guC6wao7zNv%2b3yy7uPC3s61eSqkG1uu9HC%2bwGSjsM1v/ZSn7tFXeuY%2bpFsB&token_type=bearer&expires_in=3600&scope=bingads.manage&user_id=38097d39a8aba1094d0e0fc7996e3bf5"));

                Console.WriteLine("authentication.OAuthTokens.AccessToken = {0}, AccessTokenExpiresInSeconds = {1}, RefreshToken = {2}", authentication.OAuthTokens.AccessToken, authentication.OAuthTokens.AccessTokenExpiresInSeconds, authentication.OAuthTokens.RefreshToken);

                var authorizationData = new AuthorizationData
                {
                    Authentication = authentication,
                    CustomerId = 15310009,
                    AccountId = 44131149,
                    DeveloperToken = "014BT1U389334786"
                };

                CampaignCRUD campaignOperator = new CampaignCRUD();
                campaignOperator.RunAsync(authorizationData).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
