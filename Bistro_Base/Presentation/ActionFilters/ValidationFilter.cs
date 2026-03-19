using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Common.Interfaces;
using Common.Models;

namespace Presentation.ActionFilters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly IItemsRepository _repo;

        public ValidationFilter([FromKeyedServices("db")] IItemsRepository repo)
        {
            _repo = repo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var currentUser = user.Identity?.Name;
            var isAdmin = currentUser == "admin1@site.com";

            var action = context.ActionDescriptor.RouteValues["action"];
            var controller = context.ActionDescriptor.RouteValues["controller"];

            // BulkImport → admin only
            if (controller == "BulkImport" &&
                (action == "Import" || action == "UploadZip"))
            {
                if (!isAdmin)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            // Must be authenticated
            if (!(user.Identity?.IsAuthenticated ?? false))
            {
                context.Result = new ForbidResult();
                return;
            }

            // No selectedIds → skip
            if (!context.ActionArguments.TryGetValue("selectedIds", out var idsObj))
                return;

            var selectedIds = idsObj as Guid[];
            if (selectedIds == null || !selectedIds.Any())
                return;

            var items = _repo.Get();

            foreach (var item in items)
            {
                if (item is Restaurant r && selectedIds.Contains(r.Id) && !isAdmin)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (item is MenuItem m && selectedIds.Contains(m.Id) &&
                    m.Restaurant.OwnerEmailAddress != currentUser)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}