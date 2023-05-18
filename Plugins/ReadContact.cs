using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;

namespace Plugins
{
    public class ReadContact : IPlugin
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
                    string[] columns = new string[] { "firstname", "lastname" };
                    Entity contact = service.Retrieve("contact", new Guid("14053314-bcf3-ed11-8847-000d3a9d02b8"), new ColumnSet(columns));
                    string firstName = contact["firstname"].ToString();
                    string lastName = contact["lastname"].ToString();
                    throw new InvalidPluginExecutionException(firstName + " " + lastName);
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
