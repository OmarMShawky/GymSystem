using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {
        //----- Trainer -> list row -----
        CreateMap<Trainer, TrainerViewModel>()
            .ForMember(d => d.Specialization, o => o.MapFrom(s =>
                s.Category != null ? s.Category.Name : "Unassigned"));

        //----- Trainer -> details card ("{Category} Trainer") -----
        CreateMap<Trainer, TrainerDetailsViewModel>()
            .ForMember(d => d.Specialization, o => o.MapFrom(s =>
                (s.Category != null ? s.Category.Name : "Unassigned") + " Trainer"))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

        //----- Trainer -> edit form -----
        CreateMap<Trainer, EditTrainerViewModel>()
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
            .ForMember(d => d.Categories, o => o.Ignore());

        //----- create form -> new Trainer -----
        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(d => d.HireDate, o => o.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.Address, o => o.MapFrom(s => new Address
            {
                BuildingNumber = s.BuildingNumber,
                Street = s.Street,
                City = s.City
            }));

        //----- edit form -> existing Trainer -----
        // Name, DateOfBirth and Gender are locked on the form, so they are never mapped back.
        CreateMap<EditTrainerViewModel, Trainer>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Name, o => o.Ignore())
            .ForMember(d => d.DateOfBirth, o => o.Ignore())
            .ForMember(d => d.Gender, o => o.Ignore())
            .ForMember(d => d.HireDate, o => o.Ignore())
            .ForMember(d => d.Category, o => o.Ignore())
            .ForMember(d => d.Address, o => o.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.Street = src.Street;
                dest.Address.City = src.City;
            });

        //----- Category -> dropdown option -----
        CreateMap<Category, LookupItemViewModel>();
    }
}
