using System;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {   
        CreateMap<AuctionCreated, Item>();
        //Start - Challenge
        CreateMap<AuctionUpdated, Item>();
        //End - Challenge
    }
}
