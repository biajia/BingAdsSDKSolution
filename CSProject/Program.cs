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

                CampaignCRUD campaignOperator = new CampaignCRUD();
                campaignOperator.RunAsync(authorizationData).Wait();
            }
            catch (Exception ex)
            {
                CommonHelper.OutputErrorMessage(ex.Message);
                CommonHelper.OutputErrorMessage(ex.StackTrace);
            }
        }
    }
}
