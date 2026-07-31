using AutoMapper;

namespace GymSystem.BusinessLogic.Mapping;

public class TrainerProfile : Profile
{
    public TrainerProfile()
    {

        CreateMap<Trainer, TrainerViewModel>()
            .ForMember(d => d.Specialization, o => o.MapFrom(s =>
                s.Category != null ? s.Category.Name : "Unassigned"));

        CreateMap<Trainer, TrainerDetailsViewModel>()
            .ForMember(d => d.Specialization, o => o.MapFrom(s =>
                (s.Category != null ? s.Category.Name : "Unassigned") + " Trainer"))
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City));

        CreateMap<Trainer, EditTrainerViewModel>()
            .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
            .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.DateOfBirth.ToShortDateString()))
            .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.Address.BuildingNumber))
            .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street))
            .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
            .ForMember(d => d.Categories, o => o.Ignore());

        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(d => d.HireDate, o => o.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.Address, o => o.MapFrom(s => new Address
            {
                BuildingNumber = s.BuildingNumber,
                Street = s.Street,
                City = s.City
            }));

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

        CreateMap<Category, LookupItemViewModel>();
    }
}
