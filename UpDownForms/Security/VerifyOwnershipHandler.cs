using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using System.Security.Claims;
using UpDownForms.Models;

namespace UpDownForms.Security
{
    public class VerifyOwnershipHandler : AuthorizationHandler<SameAuthorRequirement, IVerifyOwnership>
    {
        public override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameAuthorRequirement requirement, IVerifyOwnership resource)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (context.User.Identity.Name == resource.UserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

            // TODO: verify if it's a good idea implement all the null checks and stuff like that
        }
    }


}
