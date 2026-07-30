namespace GymSystem.BusinessLogic.Common;

/// <summary>
/// Why an operation failed. Maps cleanly onto HTTP status codes at the controller edge.
/// </summary>
public enum ResultStatus
{
    /// <summary>The operation succeeded (200).</summary>
    Success,

    /// <summary>The target does not exist (404).</summary>
    NotFound,

    /// <summary>The request was rejected by a business rule (400).</summary>
    Invalid,

    /// <summary>The request clashes with existing data, e.g. a duplicate or overlap (409).</summary>
    Conflict
}
