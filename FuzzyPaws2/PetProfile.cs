using AutoMapper;
using FuzzyPaws2.Models;
using FuzzyPaws2.ViewModels.MyPets;
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
                .ForMember(dest => dest.PetBreedId, src => src.MapFrom(x => x.PetBreed.Id))
                .ForMember(dest => dest.PetTypeId, src => src.MapFrom(x => x.PetType.Id))
                ;

            CreateMap<MyPet, MyPetDetailsViewModel>()
                .ForMember(dest => dest.PetBreedName, src => src.MapFrom(x => x.PetBreed.Name))
                .ForMember(dest => dest.PetTypeName, src => src.MapFrom(x => x.PetType.Name))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.AspNetUsers.UserName))
                .ForMember(dest => dest.PetBreedId, src => src.MapFrom(x => x.PetBreed.Id))
                .ForMember(dest => dest.PetTypeId, src => src.MapFrom(x => x.PetType.Id))
                .ForMember(dest => dest.UserId, src => src.MapFrom(x => x.AspNetUsers.Id))
                ;

            CreateMap<PetCreateViewModel, Pet>();
            CreateMap<Pet, PetCreateViewModel>();

            CreateMap<CreateBreedViewModel, PetBreed>();
            CreateMap<PetBreed, CreateBreedViewModel>();

            CreateMap<BreedDetailsViewModel, PetBreed>();
            CreateMap<PetBreed, BreedDetailsViewModel>();

            CreateMap<CreateTypeViewModel, PetType>();
            CreateMap<PetType, CreateTypeViewModel>();

            CreateMap<TypeDetailsViewModel, PetType>();
            CreateMap<PetType, TypeDetailsViewModel>();

            CreateMap<UserCreateViewModel, IdentityUser>();
            CreateMap<IdentityUser, UserCreateViewModel>();
        }
    }
}
