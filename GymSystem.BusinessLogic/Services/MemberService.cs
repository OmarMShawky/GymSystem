using AutoMapper;

namespace GymSystem.BusinessLogic.Services;

public class MemberService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IFileService _fileService = fileService;

    public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(
        CancellationToken cancellationToken = default)
    {
        var members = await _unitOfWork.GetRepository<Member>()
            .GetAllAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<MemberViewModel>>(members);
    }

    public async Task<Result> CreateMemberAsync(
        CreateMemberViewModel createMemberViewModel, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var emailExists = await memberRepo
            .AnyAsync(m => m.Email == createMemberViewModel.Email, cancellationToken);

        var phoneExists = await memberRepo
            .AnyAsync(m => m.Phone == createMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return Result.Conflict("Email or phone already exists.");

        var upload = await _fileService.UploadAsync(
            createMemberViewModel.Photo, FileSettings.MemberPhotosFolder, cancellationToken);

        if (upload.IsFailure)
            return Result.Fail(upload.Error!);

        var newMember = _mapper.Map<Member>(createMemberViewModel);

        newMember.Photo = upload.Value;

        memberRepo.Add(newMember, cancellationToken);

        try
        {
            if ((await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0)
                return Result.Ok();
        }
        catch
        {
            _fileService.DeleteFile(FileSettings.MemberPhotosFolder, upload.Value);
            throw;
        }

        _fileService.DeleteFile(FileSettings.MemberPhotosFolder, upload.Value);

        return Result.Fail("The member could not be saved.");
    }

    public async Task<Result<MemberDetailsViewModel>> GetMemberDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(id, cancellationToken);

        if (member is null)
            return Result.NotFound<MemberDetailsViewModel>("Member not found.");

        var memberDetailsViewModel = _mapper.Map<MemberDetailsViewModel>(member);

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

    public async Task<Result<EditMemberViewModel>> GetMemberDetailsForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(id, cancellationToken);

        if (member is null)
            return Result.NotFound<EditMemberViewModel>("Member not found.");

        return _mapper.Map<EditMemberViewModel>(member);
    }

    public async Task<Result<HealthRecordViewModel>> GetHealthRecordDetailsAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var record = await _unitOfWork.GetRepository<HealthRecord>()
            .FirstOrDefault(h => h.MemberId == memberId, cancellationToken);

        if (record is null)
            return Result.NotFound<HealthRecordViewModel>("This member has no health record.");

        var member = await _unitOfWork.GetRepository<Member>()
            .GetByIdAsync(memberId, cancellationToken);

        var healthRecordViewModel = _mapper.Map<HealthRecordViewModel>(record);
        healthRecordViewModel.Age = member?.Age ?? 0;

        return healthRecordViewModel;
    }

    public async Task<Result> UpdateMemberAsync(int id, EditMemberViewModel editMemberViewModel, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var member = await memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return Result.NotFound("Member not found.");

        var emailExists = await memberRepo
            .AnyAsync(m => m.Id != id && m.Email == editMemberViewModel.Email, cancellationToken);

        var phoneExists = await memberRepo
            .AnyAsync(m => m.Id != id && m.Phone == editMemberViewModel.Phone, cancellationToken);

        if (emailExists || phoneExists)
            return Result.Conflict("Email or phone is already in use by another member.");

        var previousPhoto = member.Photo;
        string? uploadedPhoto = null;

        if (editMemberViewModel.Photo is { Length: > 0 })
        {
            var upload = await _fileService.UploadAsync(
                editMemberViewModel.Photo, FileSettings.MemberPhotosFolder, cancellationToken);

            if (upload.IsFailure)
                return Result.Fail(upload.Error!);

            uploadedPhoto = upload.Value;
        }

        _mapper.Map(editMemberViewModel, member);

        if (uploadedPhoto is not null)
            member.Photo = uploadedPhoto;

        memberRepo.Update(member, cancellationToken);

        int saved;

        try
        {
            saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {

            DeletePhotoIfPresent(uploadedPhoto);
            throw;
        }

        if (saved <= 0)
        {
            DeletePhotoIfPresent(uploadedPhoto);
            return Result.Fail("The member could not be updated.");
        }

        if (uploadedPhoto is not null && previousPhoto != uploadedPhoto)
            DeletePhotoIfPresent(previousPhoto);

        return Result.Ok();
    }

    private void DeletePhotoIfPresent(string? fileName)
    {
        if (!string.IsNullOrWhiteSpace(fileName))
            _fileService.DeleteFile(FileSettings.MemberPhotosFolder, fileName);
    }

    public async Task<Result> DeleteMemberAsync(int id, CancellationToken cancellationToken = default)
    {
        var memberRepo = _unitOfWork.GetRepository<Member>();

        var member = await memberRepo.GetByIdAsync(id, cancellationToken);

        if (member is null)
            return Result.NotFound("Member not found.");

        member.IsDeleted = true;

        var photoToRemove = member.Photo;
        member.Photo = null;

        memberRepo.Update(member, cancellationToken);

        if ((await _unitOfWork.SaveChangesAsync(cancellationToken)) <= 0)
            return Result.Fail("The member could not be deleted.");

        DeletePhotoIfPresent(photoToRemove);

        return Result.Ok();
    }
}
