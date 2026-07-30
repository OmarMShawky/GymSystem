using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        //----- Plan -> edit form -----
        CreateMap<Plan, EditPlanViewModel>();

        //----- edit form -> existing Plan -----
        // IsActive is owned by the dedicated ToggleStatus action, so it is never mapped back.
        CreateMap<EditPlanViewModel, Plan>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.PlanMembers, o => o.Ignore());
    }
}
