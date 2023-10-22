using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Commander.Controllers
{   
    //[Authorize]            //Basic Auth
    [Route("api/commands")] //  tells the project how to get to controller resources & endpoints
    [ApiController] //  good practice: this gives u default controller behaviors out the box
    public class CommandsController : ControllerBase 
    {
        private readonly ICommanderRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;           //  Global Exception Handling / LOGGING


        //  dependency injection here below for Repository & AutoMapper
        public CommandsController(ICommanderRepository repository,IMapper mapper,ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
                                                                              
            _logger = loggerFactory.CreateLogger<CommandsController>();             //LOGGING
        }

        //  1st ActionResult endpoint, relates to getting all resources
        //GET api/commands
        //[Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            
            var commandItems = _repository.GetAllCommands();

            //  coded out for server testing purposes
            var returnModel = _mapper.Map<IEnumerable<CommandReadDto>>(commandItems);



            return Ok(returnModel);

            //return Ok(commandItems);
        }

        //GET api/commands/{id}
        [HttpGet("{id}",Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);

            //  coded out for server testing purposes
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }

            return NotFound();

            //return Ok(commandItem);
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            //  coded out for server testing purposes
            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById),new { Id = commandReadDto.Id },commandReadDto);

            //return Ok();
        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            //  coded out for server testing purposes
            var commandModelFromRepository = _repository.GetCommandById(id);

            if (commandModelFromRepository == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto,commandModelFromRepository);

            _repository.UpdateCommand(commandModelFromRepository);

            _repository.SaveChanges();

            return NoContent();

            //return Ok();
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            //  coded our for server testing purposes
            Command commandModelFromRepository = _repository.GetCommandById(id);

            if (commandModelFromRepository == null)
            {
                return NotFound();
            }

            CommandUpdateDto commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepository);

            patchDoc.ApplyTo(commandToPatch,ModelState);
            //  at this point commandToPatch has been -modified-

            if (!TryValidateModel(commandToPatch))
            {
                //var todo = ValidationProblem(ModelState);
                //return todo;



                return BadRequest(ModelState);
            }

            Command commandModelFromRepositoryModified = _mapper.Map(commandToPatch,commandModelFromRepository);            // verify with modded commandtopatch and original repo command
            ////  at this point commandModelFromRepository has been -modified-

            _repository.UpdateCommand(commandModelFromRepositoryModified);              //  verify with modded repo command

            _repository.SaveChanges();                                          //  verify - try setup with returns and without or no set up at all

            return NoContent();

            //return Ok();

        }

        

        // try validate 

        //DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            //  coded out for server testing purposes
            var commandModelFromRepository = _repository.GetCommandById(id);

            if (commandModelFromRepository == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepository);

            _repository.SaveChanges();

            return NoContent();

            //return Ok();
        }


    }
}
