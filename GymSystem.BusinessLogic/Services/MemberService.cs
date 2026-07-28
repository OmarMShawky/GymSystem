namespace GymSystem.BusinessLogic.Services;

public class MemberService : IMemberService
{
    private readonly IGenericRepository<Member> _memberRepo;
    private readonly IGenericRepository<Membership> _membershipRepo;
    private readonly IGenericRepository<Plan> _planRepo;
    private readonly IGenericRepository<HealthRecord> _healthRecordRepo;
    public MemberService(
        IGenericRepository<Member> memberRepo,
        IGenericRepository<Membership> membershipRepo,
        IGenericRepository<Plan> planRepo,
        IGenericRepository<HealthRecord> healthRecordRepo)
    {
        _memberRepo = memberRepo;
        _membershipRepo = membershipRepo;
        _planRepo = planRepo;
        _healthRecordRepo = healthRecordRepo;
    }

    public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(
        CancellationToken cancellationToken = default)
    {
        var members = await _memberRepo
            .GetAllAsync(cancellationToken: cancellationToken);

        return members.Select(m => new MemberViewModel
        {
            Id = m.Id,
            Name = m.Name,
            Email = m.Email,
            Phone = m.Phone,
            Gender = m.Gender.ToString(),
            Photo = m.Photo
        });
    }

    public async Task<bool> CreateMemberAsync(
        CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default)
    {
        // validate the input model

        //if (createMemberViewModel is null || createMemberViewModel.HealthRecord is null)
        //    return CreateMemberResult.ValidationFailed;

        var emailExists = await _memberRepo
            .AnyAsync(m => m.Email == createMemberViewModel.Email, cancellationToken);

        var phoneExists = await _memberRepo
            .AnyAsync(m => m.Phone == createMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        // mapping ==> create CreateMemberViewModel ==> Member entity

        var newMember = new Member
        {
            Name = createMemberViewModel.Name,
            Email = createMemberViewModel.Email,
            Phone = createMemberViewModel.Phone,
            DateOfBirth = createMemberViewModel.DateOfBirth,
            Gender = createMemberViewModel.Gender,
            Photo = createMemberViewModel.Photo,
            Address = new Address
            {
                BuildingNumber = createMemberViewModel.BuildingNumber,
                Street = createMemberViewModel.Street,
                City = createMemberViewModel.City
            },
            HealthRecord = new HealthRecord
            {
                Height = createMemberViewModel.HealthRecord.Height,
                Weight = createMemberViewModel.HealthRecord.Weight,
                BloodType = createMemberViewModel.HealthRecord.BloodType,
                Notes = createMemberViewModel.HealthRecord.Notes
            }

        };

        // save the new member to the database

        return (await _memberRepo.AddAsync(newMember, cancellationToken)) > 0;
    }

    public async Task<MemberDetailsViewModel?> GetMemberDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return null;

        //var plan = await _planRepo.FirstOrDefault(p => p.Id == member.PlanId, cancellationToken);
        var memberDetailsViewModel = new MemberDetailsViewModel
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            DateOfBirth = member.DateOfBirth.ToShortDateString(),
            Gender = member.Gender.ToString(),
            Photo = member.Photo,

        };
        // IsActive is a [NotMapped] computed property, so EF can't translate it.
        // Filter on the mapped EndDate column instead (mirrors Membership.IsActive).
        var today = DateOnly.FromDateTime(DateTime.Now);
        var membership = await _membershipRepo
            .FirstOrDefault(m => m.MemberId == member.Id && m.EndDate > today, cancellationToken);

        if (membership is not null)
        {
            var plan = await _planRepo.GetByIdAsync(membership.PlanId, cancellationToken);
            memberDetailsViewModel.MembershipStartDate = membership.CreatedAt.ToShortDateString();
            memberDetailsViewModel.MembershipEndDate = membership.EndDate.ToShortDateString();
            memberDetailsViewModel.PlanName = plan?.Name ?? "No Plan";
        }
        return memberDetailsViewModel;
    }

    public async Task<EditMemberViewModel?> GetMemberDetailsForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return null;

        return new EditMemberViewModel
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email,
            Phone = member.Phone,
            DateOfBirth = member.DateOfBirth,
            BuildingNumber = member.Address.BuildingNumber,
            Street = member.Address.Street,
            City = member.Address.City
        };
    }
    public async Task<HealthRecordViewModel?> GetHealthRecordDetailsAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var record = await _healthRecordRepo.FirstOrDefault(h => h.MemberId == memberId, cancellationToken);

        if (record is null) return null;

        // Age is not stored; derive it at runtime from the member's DateOfBirth.
        var member = await _memberRepo.GetByIdAsync(memberId, cancellationToken);

        return new HealthRecordViewModel
        {
            Height = record.Height,
            Weight = record.Weight,
            Age = member?.Age ?? 0,
            BloodType = record.BloodType,
            Notes = record.Notes
        };

    }

    public async Task<bool> UpdateMemberAsync(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return false;

        var emailExists = await _memberRepo
            .AnyAsync(m => m.Id != id && m.Email == editMemberViewModel.Email, cancellationToken);

        var phoneExists = await _memberRepo
            .AnyAsync(m => m.Id != id && m.Phone == editMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return false;

        member.Name = editMemberViewModel.Name;
        member.Email = editMemberViewModel.Email;
        member.Phone = editMemberViewModel.Phone;
        member.DateOfBirth = editMemberViewModel.DateOfBirth;
        member.Address.BuildingNumber = editMemberViewModel.BuildingNumber;
        member.Address.Street = editMemberViewModel.Street;
        member.Address.City = editMemberViewModel.City;

        return (await _memberRepo.UpdateAsync(member, cancellationToken)) > 0;
    }

    public async Task<bool> DeleteMemberAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return false;

        // Soft delete: flag the row. The AuditColumnInterceptor stamps DeletedAt,
        // and the global query filter (!IsDeleted) hides it from future queries.
        member.IsDeleted = true;

        return (await _memberRepo.UpdateAsync(member, cancellationToken)) > 0;
    }

}
