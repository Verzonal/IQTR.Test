using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Plugins
{
    public class InitiateCreditLimitApproval : IPlugin
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
                    Entity approvalRequest = new Entity("iqtr_approvalrequest");
                    approvalRequest["iqtr_topic"] = "Credit Limi Approval Request";
                    approvalRequest["iqtr_account"] = new EntityReference("account", targetRecord.Id);
                    approvalRequest["ownerid"] = (EntityReference)targetRecord["iqtr_approver"];
                    service.Create(approvalRequest);
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
