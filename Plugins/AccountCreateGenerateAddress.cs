using Microsoft.Xrm.Sdk;
using System;

namespace Plugins
{
    public class AccountCreateGenerateAddress : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                IExecutionContext context = (IExecutionContext)serviceProvider.GetService(typeof(IExecutionContext));
                ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                Entity targetRecord = (Entity)context.InputParameters["Target"];
                tracingService.Trace("Running line after getting tarte record.");
                Entity account = new Entity("account");
                account["address1_line1"] = "C/103, Mumbra, Thane - 400612.";
                account.Id = targetRecord.Id;
                service.Update(account);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
