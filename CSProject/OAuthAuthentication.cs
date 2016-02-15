using Microsoft.BingAds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSProject
{
    class OAuthAuthentication
    {
        public static AuthorizationData GetAuthorizationData()
        {
            var authentication = GetOAuthDesktopMobileImplicitGrant();

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
            var authentication = new OAuthDesktopMobileImplicitGrant(GlobalConfig.ClientId);

            //WebBrowser browser = new WebBrowser();
            //browser.Navigate(authentication.GetAuthorizationEndpoint());

            authentication.ExtractAccessTokenFromUri(new Uri("https://login.live.com/oauth20_desktop.srf?lc=1033#access_token=EwBwAnhlBAAUxT83/QvqiAZEx5SuwyhZqHzk21oAAQByeu4TZtrHQXh6uMcQ4JXDbWzcJxVuVwl1RydCm19GuyJM7eAIYzuSgteCLEW%2bkQyFsM79mVPUiDKOLboD0IPEXvrAKRYFbJ7/gcNIZHWxWSfE5ua8Gn5/p%2bRB76rY6KTvxfRM3pKmQ5E698qZ%2b0c7iIE9mBZ%2b99kNEZGf%2bAZpUwwt/b60PEg8u9w9PyMFgCmZ867ls7xNRKswHuED5B6nBAtbHYaIGp0dXWsAF791CVTXlUuqSd34EqPoqzLfwWtF5sVmPOD0FzBBKZ8WGNSlouJH0kdHCLYLB35oxOD3A%2bEbe/eTU8OIR69s3hnjlluE1m0WJBdTlheobg8ZDTkDZgAACJBHdLDmP1gjQAGKO5duubr8mq2gXSZfhXgDdal3yBBfkHk9ASrEacXf59Y9YmMBOT7uJIEpSyB8T5R4evs3Mhew069aX6B3n5%2bK7MCfK4RHyjCLzDlCW/WKYfNlU2jWW6u4CZoahSbGd03uI5UtmmxHl/kMLWhe6QSxr26wrzmXDQoRRQC5TnGoF3R0gy7I2Cmh4Ucxqie/1eW0aPyJHA8nAWiv/dwaoSZc/bzjoIeI5nfizyy9zq3jo7xaVOLUhHVTRPXLRkV6H7XnKxnFVN6vkiwNRsnBl0nL480KqZ8kokMKE8/QqiyKmTW3CBWLJJmlLhmVLxKtMlDoQqST%2bQvv1igLaCsb/5VU7IQ/NFYcpg24O2C77hq6/8bpjdAnxw%2b2dEQzIkzqbc7BPMFAAIg13QhVZGBGFG8bF5S6vjmmrrPqvx/diWYiMlsB&token_type=bearer&expires_in=3600&scope=bingads.manage&user_id=38097d39a8aba1094d0e0fc7996e3bf5"));

            return authentication;
        }
    }
}
