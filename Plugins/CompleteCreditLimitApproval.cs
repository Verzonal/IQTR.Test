using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;

namespace Plugins
{
    public class CompleteCreditLimitApproval : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = null;
            IOrganizationService service = null;
            try
            {
                IExecutionContext context = (IExecutionContext)serviceProvider.GetService(typeof(IExecutionContext));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                service = serviceFactory.CreateOrganizationService(context.UserId);
                tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity targetRecord = (Entity)context.InputParameters["Target"];
                    // TODO: Core Logic
                    // Read Target Entity's other fields
                    string[] columns = new string[] { "iqtr_account" };
                    Entity approvalRequest = service.Retrieve("iqtr_approvalrequest", targetRecord.Id, new ColumnSet(columns));
                    // Update Account
                    int approvalStatus = ((OptionSetValue)targetRecord["iqtr_approvalstatus"]).Value;
                    if (approvalStatus == 1)
                    {
                        Entity account = new Entity("account", ((EntityReference)approvalRequest["iqtr_account"]).Id);
                        account["iqtr_creditlimit"] = targetRecord["iqtr_approvedcreditlimit"];
                        service.Update(account);
                    }
                    else if (approvalStatus == 2)
                    {
                        Entity account = new Entity("account", ((EntityReference)approvalRequest["iqtr_account"]).Id);
                        account["iqtr_creditlimit"] = new Money(0);
                        service.Update(account);
                    }
                }
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
            }
            catch (Exception ex)
            {
                tracingService.Trace("FollowUpPlugin: {0}", ex.ToString());
                throw;
            }
        }
    }
}
