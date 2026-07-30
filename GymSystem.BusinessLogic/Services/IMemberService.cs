namespace GymSystem.BusinessLogic.Services;

public interface IMemberService
{
    // No failure mode - an empty list is a valid result.
    Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken cancellationToken = default);

    Task<Result> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default);
    Task<Result<MemberDetailsViewModel>> GetMemberDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<EditMemberViewModel>> GetMemberDetailsForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<HealthRecordViewModel>> GetHealthRecordDetailsAsync(int memberId, CancellationToken cancellationToken = default);
    Task<Result> UpdateMemberAsync(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken = default);
    Task<Result> DeleteMemberAsync(int id, CancellationToken cancellationToken = default);
}
