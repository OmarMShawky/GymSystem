using Microsoft.EntityFrameworkCore;

namespace GymSystem.BusinessLogic.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepo;

    public MemberService(IMemberRepository memberRepo)
    {
        _memberRepo = memberRepo;
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
            .AnyAsync(m => m.Phone == createMemberViewModel.PhoneNumber, cancellationToken);

        if (emailExists || phoneExists)
            return false;
        
        // mapping ==> create CreateMemberViewModel ==> Member entity

        var newMember = new Member
        {
            Name = createMemberViewModel.Name,
            Email = createMemberViewModel.Email,
            Phone = createMemberViewModel.PhoneNumber,
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
                Age = createMemberViewModel.HealthRecord.Age,
                BloodType = createMemberViewModel.HealthRecord.BloodType,
                Notes = createMemberViewModel.HealthRecord.Notes
            }

        };

        // save the new member to the database

        return (await _memberRepo.AddAsync(newMember, cancellationToken)) > 0;
    }
}
