using AutoMapper;

namespace GymSystem.BusinessLogic.Services;

public class MemberService(IUnitOfWork unitOfWork, IMapper mapper) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(
        CancellationToken cancellationToken = default)
    {
        var members = await _unitOfWork.GetRepository<Member>()
            .GetAllAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<MemberViewModel>>(members);
    }

    public async Task<bool> CreateMemberAsync(
        CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var emailExists = await memberRepo
            .AnyAsync(m => m.Email == createMemberViewModel.Email, cancellationToken);

        var phoneExists = await memberRepo
            .AnyAsync(m => m.Phone == createMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        // mapping ==> create CreateMemberViewModel ==> Member entity

        var newMember = _mapper.Map<Member>(createMemberViewModel);

        // save the new member to the database

        memberRepo.Add(newMember, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0;
    }

    public async Task<MemberDetailsViewModel?> GetMemberDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(id, cancellationToken);

        if (member is null)
            return null;

        var memberDetailsViewModel = _mapper.Map<MemberDetailsViewModel>(member);

        // IsActive is a [NotMapped] computed property, so EF can't translate it.
        // Filter on the mapped EndDate column instead (mirrors Membership.IsActive).
        var today = DateOnly.FromDateTime(DateTime.Now);
        var membership = await _unitOfWork.GetRepository<Membership>()
            .FirstOrDefault(m => m.MemberId == member.Id && m.EndDate > today, cancellationToken);

        if (membership is not null)
        {
            var plan = await _unitOfWork.GetRepository<Plan>()
                .GetByIdAsync(membership.PlanId, cancellationToken);

            memberDetailsViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
            memberDetailsViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();
            memberDetailsViewModel.PlanName = plan?.Name ?? "No Plan";
        }

        return memberDetailsViewModel;
    }

    public async Task<EditMemberViewModel?> GetMemberDetailsForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(id, cancellationToken);

        if (member is null)
            return null;

        return _mapper.Map<EditMemberViewModel>(member);
    }

    public async Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.GetRepository<HealthRecord>()
            .FirstOrDefault(h => h.MemberId == memberId, cancellationToken);

        if (record is null) return null;

        // Age is not stored; derive it at runtime from the member's DateOfBirth.
        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(memberId, cancellationToken);

        var healthRecordViewModel = _mapper.Map<HealthRecordViewModel>(record);
        healthRecordViewModel.Age = member?.Age ?? 0;

        return healthRecordViewModel;
    }

    public async Task<bool> UpdateMemberAsync(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var member = await memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return false;

        // Email/Phone must stay unique across OTHER members.
        var emailExists = await memberRepo
            .AnyAsync(m => m.Id != id && m.Email == editMemberViewModel.Email, cancellationToken);

        var phoneExists = await memberRepo
            .AnyAsync(m => m.Id != id && m.Phone == editMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        // Maps onto the tracked entity in place, so EF sees the changes.
        _mapper.Map(editMemberViewModel, member);

        memberRepo.Update(member, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0;
    }

    public async Task<bool> DeleteMemberAsync(int id, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var member = await memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return false;

        // Soft delete: flag the row. The AuditColumnInterceptor stamps DeletedAt,
        // and the global query filter (!IsDeleted) hides it from future queries.
        member.IsDeleted = true;

        memberRepo.Update(member, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0;
    }
}
