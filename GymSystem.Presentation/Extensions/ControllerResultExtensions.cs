using GymSystem.BusinessLogic.Common;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Presentation.Controllers;

public static class ControllerResultExtensions
{
    public static IActionResult FromFailure(this Controller controller, Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("FromFailure was called with a successful result.");

        return result.Status switch
        {
            ResultStatus.NotFound => controller.NotFound(result.Error),
            ResultStatus.Conflict => controller.Conflict(result.Error),
            _ => controller.BadRequest(result.Error)
        };
    }
}
