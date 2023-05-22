using System;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;

namespace Plugins
{
    public class ReadMultipleContacts : IPlugin
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
                    // SELECT NAME, EMAIL, PHONE FROM CONTACT WHERE EMAIL IS NOT NULL
                    // SELECT * FROM CONTACT WHERE EMAIL IS NOT NULL
                    QueryExpression query = new QueryExpression("contact");
                    query.ColumnSet = new ColumnSet(new string[] { "firstname", "lastname", "mobilephone", "emailaddress1" });
                    //query.ColumnSet = new ColumnSet(true);
                    query.Criteria.AddCondition("emailaddress1", ConditionOperator.NotNull);
                    EntityCollection ec = service.RetrieveMultiple(query);
                    if (ec != null && ec.Entities.Count > 0)
                    {
                        string emails = "";
                        foreach (Entity e in ec.Entities)
                        {
                            emails += e["emailaddress1"].ToString();
                        }
                        throw new InvalidPluginExecutionException(emails);
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
