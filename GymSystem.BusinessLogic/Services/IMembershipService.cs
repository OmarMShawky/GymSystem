using GymSystem.BusinessLogic.ViewModels.Memberships;

namespace GymSystem.BusinessLogic.Services;

public interface IMembershipService
{

    Task<IEnumerable<MembershipViewModel>> GetMembershipsAsync(CancellationToken cancellationToken = default);

    Task<CreateMembershipViewModel> LoadLookupsAsync(
        CreateMembershipViewModel createMembershipViewModel, CancellationToken cancellationToken = default);

    Task<Result> CreateMembershipAsync(
        CreateMembershipViewModel createMembershipViewModel, CancellationToken cancellationToken = default);

    Task<Result<MembershipDetailsViewModel>> GetMembershipDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task<Result> DeleteMembershipAsync(int id, CancellationToken cancellationToken = default);
}
