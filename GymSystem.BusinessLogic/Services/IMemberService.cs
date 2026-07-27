namespace GymSystem.BusinessLogic.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken cancellationToken = default);
    Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default);
    Task<MemberDetailsViewModel?> GetMemberDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<EditMemberViewModel?> GetMemberDetailsForEditAsync(int id, CancellationToken cancellationToken = default);
    Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int memberId, CancellationToken cancellationToken = default);
    Task<bool> UpdateMemberAsync(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken = default);
    Task<bool> DeleteMemberAsync(int id, CancellationToken cancellationToken = default);
}