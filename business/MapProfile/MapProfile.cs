﻿using AutoMapper;
using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MapProfile
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorListDto>().ReverseMap();
        }
    }
}
