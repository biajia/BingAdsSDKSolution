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

        public async Task RunAsync(AuthorizationData authorizationData)
        {
            try
            {
                CommonHelper.OutputSuccessMessage(Description);
                Service = new ServiceClient<ICampaignManagementService>(authorizationData);

                var campaigns = new[]
                {
                    new Campaign
                    {
                        Name = "Women's Shoes - " + DateTime.UtcNow,
                        Description = "Red shoes line.",
                        BudgetType = BudgetLimitType.MonthlyBudgetSpendUntilDepleted,
                        MonthlyBudget = 1000.00,
                        TimeZone = "PacificTimeUSCanadaTijuana",
                        DaylightSaving = true
                    },
                };
                CommonHelper.OutputMessage("Campaigns To Be Added ----------");
                OutputCampaigns(campaigns);

                // Add the campaign
                CommonHelper.OutputMessage("Start Add Campaigns ----------");
                AddCampaignsResponse addCampaignsResponse = await AddCampaignsAsync(authorizationData.AccountId, campaigns);
                long?[] campaignIds = addCampaignsResponse.CampaignIds.ToArray();
                BatchError[] campaignErrors = addCampaignsResponse.PartialErrors.ToArray();
                if(campaignIds.Length != campaigns.Length)
                {
                    CommonHelper.OutputErrorMessage(string.Format("Expect {0} Campaigns, But Returned {1} Campaigns", campaigns.Length, campaignIds.Length));
                    return;
                }
                for (int i = 0; i < campaignIds.Length; i++)
                {
                    CommonHelper.OutputMessage(string.Format("Campaign Index = {0}, Id = {1}", i, campaignIds[i]));
                }


                CommonHelper.OutputMessage("Start Get Campaigns ----------");
                GetCampaignsByIdsResponse res = await GetCampaignsByIdsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                OutputCampaigns(res.Campaigns.ToArray());

                // Update the campaign
                var updateCampaign = new Campaign
                {
                    Id = campaignIds[0],
                    MonthlyBudget = 500,
                };
                CommonHelper.OutputMessage("Start Update Campaigns ----------");
                await UpdateCampaignsAsync(authorizationData.AccountId, new[] { updateCampaign });
                res = await GetCampaignsByIdsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                OutputCampaigns(res.Campaigns.ToArray());

                // Delete the campaign
                CommonHelper.OutputMessage("Start Delete Campaign ----------");
                await DeleteCampaignsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                GetCampaignsByIdsResponse res2 = await GetCampaignsByIdsAsync(authorizationData.AccountId, new[] { (long)campaignIds[0] });
                if (res2 == null)
                {
                    CommonHelper.OutputErrorMessage(string.Format("Get Campaign Id = {0} Failed", (long)campaignIds[0]));
                    return;
                }
                if(res2.Campaigns.Count == 1 && res2.Campaigns[0] == null)
                {
                    CommonHelper.OutputMessage(string.Format("Delete Campaign Id = {0} Successfully", (long)campaignIds[0]));
                }
                else
                {
                    CommonHelper.OutputErrorMessage(string.Format("Delete Campaign Id = {0} Failed", (long)campaignIds[0]));
                }
            }
            // Catch authentication exceptions
            catch (OAuthTokenRequestException ex)
            {
                CommonHelper.OutputErrorMessage(string.Format("Couldn't get OAuth tokens. Error: {0}. Description: {1}", ex.Details.Error, ex.Details.Description));
            }
            // Catch Campaign Management service exceptions
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.AdApiFaultDetail> ex)
            {
                CommonHelper.OutputErrorMessage(string.Join("; ", ex.Detail.Errors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.ApiFaultDetail> ex)
            {
                CommonHelper.OutputErrorMessage(string.Join("; ", ex.Detail.OperationErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
                CommonHelper.OutputErrorMessage(string.Join("; ", ex.Detail.BatchErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (FaultException<Microsoft.BingAds.V10.CampaignManagement.EditorialApiFaultDetail> ex)
            {
                CommonHelper.OutputErrorMessage(string.Join("; ", ex.Detail.OperationErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
                CommonHelper.OutputErrorMessage(string.Join("; ", ex.Detail.BatchErrors.Select(error => string.Format("{0}: {1}", error.Code, error.Message))));
            }
            catch (Exception ex)
            {
                CommonHelper.OutputErrorMessage(ex.Message);
                CommonHelper.OutputErrorMessage(ex.StackTrace);
            }
        }

        private void OutputCampaigns(Campaign[] campaigns)
        {
            CommonHelper.OutputMessage(string.Format("Output {0} Campaigns", campaigns.Length));
            for (int i = 0; i < campaigns.Length; i++)
            {
                CommonHelper.OutputMessage(string.Format("Campaigns_{0}", i));
                CommonHelper.OutputMessage(string.Format("Campaign Name = {0}", campaigns[i].Name));
                CommonHelper.OutputMessage(string.Format("Campaign Description = {0}", campaigns[i].Description));
                CommonHelper.OutputMessage(string.Format("Campaign BudgetType = {0}", campaigns[i].BudgetType));
                CommonHelper.OutputMessage(string.Format("Campaign MonthlyBudget = {0}", campaigns[i].MonthlyBudget));
                CommonHelper.OutputMessage(string.Format("Campaign TimeZone = {0}", campaigns[i].TimeZone));
                CommonHelper.OutputMessage(string.Format("Campaign DaylightSaving = {0}", campaigns[i].DaylightSaving));
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
                    CommonHelper.OutputMessage(String.Format("Campaign[{0}] (Name:{1}) successfully added and assigned CampaignId {2}",
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

                CommonHelper.OutputMessage(String.Format("\nCampaign[{0}] (Name:{1}) not added due to the following error:",
                    error.Index, campaigns[error.Index].Name));

                CommonHelper.OutputMessage(String.Format("\tIndex: {0}", error.Index));
                CommonHelper.OutputMessage(String.Format("\tCode: {0}", error.Code));
                CommonHelper.OutputMessage(String.Format("\tErrorCode: {0}", error.ErrorCode));
                CommonHelper.OutputMessage(String.Format("\tMessage: {0}", error.Message));

                // In the case of an EditorialError, more details are available
                if (error.Type == "EditorialError" && error.ErrorCode == "CampaignServiceEditorialValidationError")
                {
                    CommonHelper.OutputMessage(String.Format("\tDisapprovedText: {0}", ((EditorialError)(error)).DisapprovedText));
                    CommonHelper.OutputMessage(String.Format("\tLocation: {0}", ((EditorialError)(error)).Location));
                    CommonHelper.OutputMessage(String.Format("\tPublisherCountry: {0}", ((EditorialError)(error)).PublisherCountry));
                    CommonHelper.OutputMessage(String.Format("\tReasonCode: {0}\n", ((EditorialError)(error)).ReasonCode));
                }
            }

            CommonHelper.OutputMessage("\n");
        }
    }
}
