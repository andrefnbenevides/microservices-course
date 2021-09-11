using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("PC-GP - Getting platforms...");
            var platformItems = _repository.GetAllPlatforms();
            
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        } 

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("PC-GP - Getting single platform...");
            var platform = _repository.GetPlatformById(id);
            if(platform != null){
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto );
        }
    }
}