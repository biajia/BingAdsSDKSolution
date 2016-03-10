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
    class AdExtensionCRUD
    {
        private static ServiceClient<ICampaignManagementService> Service;
        public string Description
        {
            get { return "AdExtension CRUD | Campaign Management V10"; }
        }

        public async Task RunAsync(AuthorizationData authorizationData)
        {
            try
            {
                CommonHelper.OutputSuccessMessage(Description);
                Service = new ServiceClient<ICampaignManagementService>(authorizationData);

                var extensions = new[]
                {
                    new LocationAdExtension
                    {
                        Address = new Address { CityName = "bellevue", CountryCode = "us", StreetAddress = "bellevue2", ProvinceName = "Washington"},
                        CompanyName = "first_add_then_update",
                    },
                };

                // Add the extension
                //CommonHelper.OutputMessage("Start Add AdExtensions ----------");
                //AddAdExtensionsResponse addAdExtensionsResponse = await AddAdExtensionsAsync(authorizationData.AccountId, extensions);
                //AdExtensionIdentity[] extensionIds = addAdExtensionsResponse.AdExtensionIdentities.ToArray();
                //if (extensionIds.Length != extensions.Length)
                //{
                //    CommonHelper.OutputErrorMessage(string.Format("Expect {0} AdExtensions, But Returned {1} AdExtensions", extensions.Length, extensionIds.Length));
                //    return;
                //}
                //for (int i = 0; i < extensionIds.Length; i++)
                //{
                //    CommonHelper.OutputMessage(string.Format("Extension Index = {0}, Id = {1}", i, extensionIds[i].Id));
                //}

                //Update the extension
                CommonHelper.OutputMessage("Start Update AdExtensions ----------");
                var updateExtensions = new[]
                {
                    new LocationAdExtension
                    {
                        Address = new Address { CityName = "bellevue2", CountryCode = "us", StreetAddress = "bellevue3", ProvinceName = "Washington"},
                        CompanyName = "first_add_then_update2",
                        Id = (long)6601371170,
                    },
                };
                UpdateAdExtensionsResponse updateAdExtensionsResponse = await UpdateAdExtensionsAsync(authorizationData.AccountId, updateExtensions);
                CommonHelper.OutputMessage("End Update AdExtensions ----------");

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

        private async Task<UpdateAdExtensionsResponse> UpdateAdExtensionsAsync(long accountId, LocationAdExtension[] extensions)
        {
            var request = new UpdateAdExtensionsRequest
            {
                AccountId = accountId,
                AdExtensions = extensions
            };

            return (await Service.CallAsync((s, r) => s.UpdateAdExtensionsAsync(r), request));
        }

        private async Task<AddAdExtensionsResponse> AddAdExtensionsAsync(long accountId, IList<AdExtension> extensions)
        {
            var request = new AddAdExtensionsRequest
            {
                AccountId = accountId,
                AdExtensions = extensions
            };

            return (await Service.CallAsync((s, r) => s.AddAdExtensionsAsync(r), request));
        }
    }
}
