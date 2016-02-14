using Microsoft.BingAds;
using Microsoft.BingAds.V10.CampaignManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CSProject
{
    class CampaignCRUD
    {
        private static ServiceClient<ICampaignManagementService> Service;
        public string Description
        {
            get { return "Campaign CRUD | Campaign Management V10"; }
        }

        /// <summary>
        /// Write to the console by default.
        /// </summary>
        /// <param name="msg">The message to send as output.</param>
        private void OutputStatusMessage(String msg)
        {
            Console.WriteLine(msg);
        }

        public async Task RunAsync(AuthorizationData authorizationData)
        {
            try
            {
                Service = new ServiceClient<ICampaignManagementService>(authorizationData);

                var campaigns = new[]{
                    new Campaign
                    {
                        Name = "Women's Shoes - " + DateTime.UtcNow,
                        Description = "Red shoes line.",
                        BudgetType = BudgetLimitType.MonthlyBudgetSpendUntilDepleted,
                        MonthlyBudget = 1000.00,
                        TimeZone = "PacificTimeUSCanadaTijuana",
                        DaylightSaving = true,

                        // Used with FinalUrls shown in the text ads that we will add below.
                        //TrackingUrlTemplate = 
                            //"http://tracker.example.com/?season={_season}&promocode={_promocode}&u={lpurl}"
                    },
                };


                // Add the campaign
                AddCampaignsResponse addCampaignsResponse = await AddCampaignsAsync(authorizationData.AccountId, campaigns);
                long?[] campaignIds = addCampaignsResponse.CampaignIds.ToArray();
                BatchError[] campaignErrors = addCampaignsResponse.PartialErrors.ToArray();
                OutputCampaignsWithPartialErrors(campaigns, campaignIds, campaignErrors);

                // Update the campaign
                var updateCampaign = new Campaign
                {
                    Id = campaignIds[0],
                    MonthlyBudget = 500,
                };
                Console.WriteLine("Start Get Campaigns");
                GetCampaignsByIdsResponse res = await GetCampaignsByIdsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                Console.WriteLine("get Campaign Count = {0}", res.Campaigns.Count);
                Console.WriteLine("{0} Start Update Campaigns", DateTime.Now);
                await UpdateCampaignsAsync(authorizationData.AccountId, new[] { updateCampaign });
                Console.WriteLine("{0} End Update Campaigns", DateTime.Now);

                // Delete the campaign
                Console.WriteLine("{0} Start Delete Campaign", DateTime.Now);
                await DeleteCampaignsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                Console.WriteLine("{0} End Delete Campaign", DateTime.Now);
                GetCampaignsByIdsResponse res2 = await GetCampaignsByIdsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                if (res2 == null)
                {
                    Console.WriteLine("Res2 = null");
                    return;
                }
                Console.WriteLine("get Campaign Count = {0}, CampaignId = {1}", res2.Campaigns == null ? 0 : res2.Campaigns.Count, res2.Campaigns[0] == null ? "null" : res2.Campaigns[0].ToString());
            }
            // Catch authentication exceptions
            catch (OAuthTokenRequestException ex)
            {
                OutputStatusMessage(string.Format("Couldn't get OAuth tokens. Error: {0}. Description: {1}", ex.Details.Error, ex.Details.Description));
            }
            // Catch Campaign Management service exceptions
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.AdApiFaultDetail> ex)
            {
                OutputStatusMessage(string.Join("; ", ex.Detail.Errors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.ApiFaultDetail> ex)
            {
                OutputStatusMessage(string.Join("; ", ex.Detail.OperationErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
                OutputStatusMessage(string.Join("; ", ex.Detail.BatchErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.EditorialApiFaultDetail> ex)
            {
                OutputStatusMessage(string.Join("; ", ex.Detail.OperationErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
                OutputStatusMessage(string.Join("; ", ex.Detail.BatchErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (Exception ex)
            {
                OutputStatusMessage(ex.Message);
            }
        }

        private async Task<AddCampaignsResponse> AddCampaignsAsync(long accountId, IList<Campaign> campaigns)
        {
            var request = new AddCampaignsRequest
            {
                AccountId = accountId,
                Campaigns = campaigns
            };

            return (await Service.CallAsync((s, r) => s.AddCampaignsAsync(r), request));
        }

        private async Task<UpdateCampaignsResponse> UpdateCampaignsAsync(long accountId, IList<Campaign> campaigns)
        {
            var request = new UpdateCampaignsRequest
            {
                AccountId = accountId,
                Campaigns = campaigns
            };

            return (await Service.CallAsync((s, r) => s.UpdateCampaignsAsync(r), request));
        }

        private async Task<DeleteCampaignsResponse> DeleteCampaignsAsync(long accountId, IList<long> campaignIds)
        {
            var request = new DeleteCampaignsRequest
            {
                AccountId = accountId,
                CampaignIds = campaignIds
            };

            return (await Service.CallAsync((s, r) => s.DeleteCampaignsAsync(r), request));
        }

        private async Task<GetCampaignsByIdsResponse> GetCampaignsByIdsAsync(long accountId, IList<long> campaignIds)
        {
            var request = new GetCampaignsByIdsRequest
            {
                AccountId = accountId,
                CampaignIds = campaignIds,
                CampaignType = CampaignType.SearchAndContent
            };

            return (await Service.CallAsync((s, r) => s.GetCampaignsByIdsAsync(r), request));
        }

        protected void OutputCampaignsWithPartialErrors(
            Campaign[] campaigns,
            long?[] campaignIds,
            IEnumerable<BatchError> partialErrors)
        {
            if (campaignIds == null)
            {
                return;
            }

            // Output the identifier of each successfully added campaign.

            for (var index = 0; index < campaigns.Length; index++)
            {
                // The array of campaign identifiers equals the size of the attempted campaigns. If the element 
                // is not null, the campaign at that index was added successfully and has a campaign identifer. 

                if (campaignIds[index] != null)
                {
                    OutputStatusMessage(String.Format("Campaign[{0}] (Name:{1}) successfully added and assigned CampaignId {2}",
                        index,
                        campaigns[index].Name,
                        campaignIds[index]));
                }
            }

            // Output the error details for any campaign not successfully added.
            // Note also that multiple error reasons may exist for the same attempted campaign.

            foreach (BatchError error in partialErrors)
            {
                // The index of the partial errors is equal to the index of the list
                // specified in the call to AddCampaigns.

                OutputStatusMessage(String.Format("\nCampaign[{0}] (Name:{1}) not added due to the following error:",
                    error.Index, campaigns[error.Index].Name));

                OutputStatusMessage(String.Format("\tIndex: {0}", error.Index));
                OutputStatusMessage(String.Format("\tCode: {0}", error.Code));
                OutputStatusMessage(String.Format("\tErrorCode: {0}", error.ErrorCode));
                OutputStatusMessage(String.Format("\tMessage: {0}", error.Message));

                // In the case of an EditorialError, more details are available
                if (error.Type == "EditorialError" && error.ErrorCode == "CampaignServiceEditorialValidationError")
                {
                    OutputStatusMessage(String.Format("\tDisapprovedText: {0}", ((EditorialError)(error)).DisapprovedText));
                    OutputStatusMessage(String.Format("\tLocation: {0}", ((EditorialError)(error)).Location));
                    OutputStatusMessage(String.Format("\tPublisherCountry: {0}", ((EditorialError)(error)).PublisherCountry));
                    OutputStatusMessage(String.Format("\tReasonCode: {0}\n", ((EditorialError)(error)).ReasonCode));
                }
            }

            OutputStatusMessage("\n");
        }
    }
}
