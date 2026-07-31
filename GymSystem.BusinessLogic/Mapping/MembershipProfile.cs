using AutoMapper;
using GymSystem.BusinessLogic.ViewModels.Memberships;

namespace GymSystem.BusinessLogic.Mapping;

public class MembershipProfile : Profile
{
    public MembershipProfile()
    {

        CreateMap<Membership, MembershipViewModel>()
            .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member != null ? s.Member.Name : "Unknown"))
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan != null ? s.Plan.Name : "Unknown"))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Plan != null ? s.Plan.Price : 0))
            .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreatedAt.ToString("dd MMM yyyy")))
            .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate.ToString("dd MMM yyyy")))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
            .ForMember(d => d.DaysRemaining, o => o.MapFrom(s => DaysRemaining(s.EndDate)));

        CreateMap<Membership, MembershipDetailsViewModel>()
            .ForMember(d => d.MemberId, o => o.MapFrom(s => s.MemberId))
            .ForMember(d => d.MemberName, o => o.MapFrom(s => s.Member != null ? s.Member.Name : "Unknown"))
            .ForMember(d => d.MemberEmail, o => o.MapFrom(s => s.Member != null ? s.Member.Email : string.Empty))
            .ForMember(d => d.MemberPhone, o => o.MapFrom(s => s.Member != null ? s.Member.Phone : string.Empty))
            .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan != null ? s.Plan.Name : "Unknown"))
            .ForMember(d => d.PlanDescription, o => o.MapFrom(s => s.Plan != null ? s.Plan.Description : string.Empty))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Plan != null ? s.Plan.Price : 0))
            .ForMember(d => d.DurationDays, o => o.MapFrom(s => s.Plan != null ? s.Plan.DurationDays : 0))
            .ForMember(d => d.StartDate, o => o.MapFrom(s => s.CreatedAt.ToString("dd MMM yyyy")))
            .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate.ToString("dd MMM yyyy")))
            .ForMember(d => d.IsActive, o => o.MapFrom(s => s.IsActive))
            .ForMember(d => d.DaysRemaining, o => o.MapFrom(s => DaysRemaining(s.EndDate)));
    }

    private static int DaysRemaining(DateOnly endDate)
    {
        var days = endDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;

        return days > 0 ? days : 0;
    }
}
