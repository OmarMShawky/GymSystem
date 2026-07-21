namespace GymSystem.BusinessLogic.Services;

public interface IMemberService
{
    Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken cancellationToken = default);
    Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default);
}