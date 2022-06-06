using AutoMapper;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.PetBreeds;
using FuzzyPaws2.ViewModels.Pets;
using FuzzyPaws2.ViewModels.PetTypes;
using FuzzyPaws2.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace FuzzyPaws2
{
    public class PetProfile : Profile
    {
        public PetProfile()
        {
            CreateMap<Pet, PetDetailsViewModel>()
                .ForMember(dest => dest.PetBreedName, src => src.MapFrom(x => x.PetBreed.Name))
                .ForMember(dest => dest.PetTypeName, src => src.MapFrom(x => x.PetType.Name))
                ;
            CreateMap<PetCreateViewModel, Pet>();
            CreateMap<Pet, PetCreateViewModel>();

            CreateMap<CreateBreedViewModel, PetBreed>();
            CreateMap<PetBreed, CreateBreedViewModel>();

            CreateMap<CreateTypeViewModel, PetType>();
            CreateMap<PetType, CreateTypeViewModel>();
            
            CreateMap<UserCreateViewModel, IdentityUser>();
            CreateMap<IdentityUser, UserCreateViewModel>();

        }
    }
}
