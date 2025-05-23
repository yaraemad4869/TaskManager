﻿using AutoMapper;
using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Color, ColorDTO>();
            CreateMap<Note, NoteDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.color.Name));
            CreateMap<Category, CategoryDTO>();
            CreateMap<Color, ColorDTO>();
            CreateMap<ToDo, ToDoDTO>();
            CreateMap<ToDoList, ToDoListDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.color.Name));
            CreateMap<CategoryDTO, Category>();
            CreateMap<ColorDTO, Color>();
            CreateMap<ToDoDTO, ToDo>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<User, LoginDTO>();
            CreateMap<LoginDTO, User>();
        }
    }
}
