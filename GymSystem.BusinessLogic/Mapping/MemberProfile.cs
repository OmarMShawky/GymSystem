using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class MemberProfile : Profile
{
    public MemberProfile()
    {
        //----- Member -> list row -----
        CreateMap<Member, MemberViewModel>()
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()));

        //----- Member -> details card -----
        // Membership/plan fields are filled in by the service after a second query.
        CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.Address, o => o.MapFrom(s =>
                $"{s.Address.BuildingNumber} - {s.Address.Street} - {s.Address.City}"))
            .ForMember(d => d.MembershipStartDate, o => o.Ignore())
            .ForMember(d => d.MembershipEndDate, o => o.Ignore())
            .ForMember(d => d.PlanName, o => o.Ignore());

        //----- Member -> edit form (owned Address flattened) -----
        CreateMap<Member, EditMemberViewModel>()
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

        //----- create form -> new Member -----
        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(d => d.Address, o => o.MapFrom(s => new Address
            {
                BuildingNumber = s.BuildingNumber,
                Street = s.Street,
                City = s.City
            }));

        //----- edit form -> existing Member (Address updated in place) -----
        CreateMap<EditMemberViewModel, Member>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Address, o => o.Ignore())
            .ForMember(d => d.HealthRecord, o => o.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.Street = src.Street;
                dest.Address.City = src.City;
            });

        //----- health record -----
        // Age is derived from the member's DateOfBirth, so the service supplies it.
        CreateMap<HealthRecordViewModel, HealthRecord>();
        CreateMap<HealthRecord, HealthRecordViewModel>()
            .ForMember(d => d.Age, o => o.Ignore());
    }
}
