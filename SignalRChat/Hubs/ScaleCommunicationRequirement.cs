using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
  
    public class ScaleCommunicationRequirement :
    AuthorizationHandler<ScaleCommunicationRequirement, HubInvocationContext>,
    IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ScaleCommunicationRequirement requirement,
        HubInvocationContext resource)
    {
        if (context.User.Identity != null &&
          IsUserAllowedToDoThis(resource.HubMethodName, context.User.Identity.Name))
        {
                context.Succeed(requirement);
            
        }
        return Task.CompletedTask;
    }

    private bool IsUserAllowedToDoThis(string hubMethodName,
        string currentUsername)
    {
        return !(currentUsername.Equals("asdf42@microsoft.com") &&
            hubMethodName.Equals("banUser", StringComparison.OrdinalIgnoreCase));
    }
}
}
