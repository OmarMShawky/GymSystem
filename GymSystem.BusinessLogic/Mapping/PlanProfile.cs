using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class PlanProfile : Profile
{
    public PlanProfile()
    {

        CreateMap<Plan, EditPlanViewModel>();

        CreateMap<EditPlanViewModel, Plan>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.PlanMembers, o => o.Ignore());
    }
}
