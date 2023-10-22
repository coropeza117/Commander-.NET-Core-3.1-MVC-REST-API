using AutoMapper;
using Commander.Controllers;
using Commander.Data;
using Commander.Dtos;
using Commander.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Command.Tests.Controller_Test
{
    public class CommandsControllerTest
    {
        private readonly Mock<ICommanderRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;

        private readonly CommandsController _controller;
        

        public CommandsControllerTest()
        {
            _repositoryMock = new Mock<ICommanderRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();

            _controller = new CommandsController(_repositoryMock.Object,_mapperMock.Object, _loggerFactoryMock.Object);
        }

        [Fact]
        private void  GetAllCommands_SuccessCase_ReturnsCollectionOfCommands()
        {   
            // First thing we do is check the controller code to see what objects it is using
            //  We see it's using _repository & _Mapper
            //  SO we have to create a Mock repository & a Mock Mapper
            
            //  create a Mock object for the fake repository to return
            var dummyRepositoryReturnObject = new List<Commander.Models.Command>()  //var command = dummy command
            {
                new Commander.Models.Command
                { Id = 1,
                  HowTo = "How to create migrations",
                  Line = "dotnet ef migrations add <Name of Migrations>"
                }   
            };

            //  Set up Mock repo to return our dummy object
            _repositoryMock.Setup(repo => repo.GetAllCommands()).Returns(dummyRepositoryReturnObject);




            //  Mock result from the Mapper call
            var dummyMapperReturnObject = new List<CommandReadDto>
            {
                new CommandReadDto
                {
                    Id = 1,
                    HowTo = "one",
                    Line = "line one",
                },
                new CommandReadDto
                {
                    Id = 2,
                    HowTo = "two",
                    Line = "line two",
                },
                new CommandReadDto
                {
                    Id = 3,
                    HowTo = "three",
                    Line = "line three",
                }
            };







            //  Set up Mock Mapper to return our dummy object
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CommandReadDto>>(dummyRepositoryReturnObject)).Returns(dummyMapperReturnObject);
            //  first ^ line only trigger when the mapper.Map is called with this exact object
            //  vv second line will be triggered mapper.Map is call with ANY object of this type
            //  in real code we would have only 1 of these lines
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CommandReadDto>>(It.IsAny<IEnumerable<Commander.Models.Command>>())).Returns(dummyMapperReturnObject);



            //  Test
            ActionResult<IEnumerable<CommandReadDto>> response = _controller.GetAllCommands();
            //ActionResult<IEnumerable<CommandReadDto>> response = _controller.GetAllCommands();




            //  Verify
            Assert.NotNull(response);
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(response);
            Assert.IsType<OkObjectResult>(response.Result);

           


            _repositoryMock.Verify(repo => repo.GetAllCommands(), Times.Once);

            //  Make sure the mapper was only called 1 time, & called with the correct argument
            //  The following 3 lines do the exact same thing in order from least explicit to most explicit
            //  In real code we would only need Final Line
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<CommandReadDto>>(It.IsAny<object>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<CommandReadDto>>(It.IsAny<IEnumerable<Commander.Models.Command>>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<CommandReadDto>>(dummyRepositoryReturnObject), Times.Once);

            



            //  putting the response in a different format to make it easier for checking the contents of the list
            OkObjectResult result = (OkObjectResult) response.Result;
            List<CommandReadDto> responseList = (List<CommandReadDto>) result.Value;
           
            var orderedResponseList = responseList.OrderBy(id => id.Id); // not tested yet



            //  Now do the actual verification of content
            Assert.Equal(3, responseList?.Count);            //  (expected, actual)

            Assert.IsType<CommandReadDto>(responseList[0]);
            Assert.Equal(1, responseList[0].Id);
            Assert.Equal("one", responseList[0].HowTo);
            Assert.Equal("line one", responseList[0].Line);

            Assert.IsType<CommandReadDto>(responseList[1]);
            Assert.Equal(2, responseList[1].Id);
            Assert.Equal("two", responseList[1].HowTo);
            Assert.Equal("line two", responseList[1].Line);

            Assert.IsType<CommandReadDto>(responseList[2]);
            Assert.Equal(3, responseList[2].Id);
            Assert.Equal("three", responseList[2].HowTo);
            Assert.Equal("line three", responseList[2].Line);

         
        }


        [Fact]
        private void GetCommandById_ValidIdProvided_SuccesfulyReturnsOneCommand()
        {           //^function to be tested..^the scenario we're setting up..^what our expected result is
            int id = 1;



            var dummyRepositoryReturnObject = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "How to create migrations",
                Line = "dotnet ef migrations add <Name of Migrations>"
            };



            _repositoryMock.Setup(repo => repo.GetCommandById(id)).Returns(dummyRepositoryReturnObject);
            



            //  Mock result from the Mapper call
            var dummyMapperReturnObject = new CommandReadDto
            {
                    Id = 1,
                    HowTo = "one",
                    Line = "line one"
            };
            
            
             

            //  Set up Mock Mapper to return our dummy object
            _mapperMock.Setup(mapper => mapper.Map<CommandReadDto>(dummyRepositoryReturnObject)).Returns(dummyMapperReturnObject);

            //  Test
            ActionResult<CommandReadDto> response = _controller.GetCommandById(id);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<ActionResult<CommandReadDto>>(response);
            Assert.IsType<OkObjectResult>(response.Result);

            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(id), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<CommandReadDto>(It.IsAny<object>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<CommandReadDto>(dummyRepositoryReturnObject), Times.Once);

            //  putting the response in a different format to make it easier for checking the contents of the list
            OkObjectResult result = (OkObjectResult) response.Result;
            CommandReadDto returnedModel = (CommandReadDto) result.Value;






             //  (expected, actual)

            Assert.IsType<CommandReadDto>(returnedModel);
            Assert.Equal(1, returnedModel.Id);
            Assert.Equal("one", returnedModel.HowTo);
            Assert.Equal("line one", returnedModel.Line);
        }

        [Fact]
        private void GetCommandById_InvalidIdProvided_ReturnsNotFound()             //  IF NULL 
        {
            // no need to create dummy repo object
            // do repo.setup so that when called with invalid #(99) returns a null/not found
            // verify mapper is never called

            Commander.Models.Command commandModel = null;

            _repositoryMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns(commandModel);

            //  Test
            ActionResult<CommandReadDto> response = _controller.GetCommandById(100);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response.Result);
            NotFoundResult result = (NotFoundResult) response.Result;
            Assert.Equal(404, result.StatusCode);                               


            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(100), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<CommandReadDto>(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        private void CreateCommand_ValidStringsProvided_SuccessfullyCreatedNewCommand()
        {


            var dummyCommandCreateDto = new CommandCreateDto
            {
                HowTo = "one",
                Line = "line one",
                Platform = "platform one"
            };


            //  create a Mock object for the fake repository to return
            var dummyRepositoryReturnObject = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "How to create migrations",
                Line = "dotnet ef migrations add <Name of Migrations>"
            };


            //  Mock result from the Mapper call
            var dummyMapperReturnObject = new CommandReadDto
            {

                Id = 1,
                HowTo = "one",
                Line = "line one",
            };

            _mapperMock.Setup(mapper => mapper.Map<Commander.Models.Command>(It.IsAny<CommandCreateDto>())).Returns(dummyRepositoryReturnObject);

            _mapperMock.Setup(mapper => mapper.Map<CommandReadDto>(It.IsAny<Commander.Models.Command>())).Returns(dummyMapperReturnObject);

            
            //  Set up Mock repo to return our dummy object
            _repositoryMock.Setup(repo => repo.CreateCommand(dummyRepositoryReturnObject));

            _repositoryMock.Setup(repo => repo.SaveChanges());




            //  Test
            ActionResult<CommandReadDto> response = _controller.CreateCommand(dummyCommandCreateDto);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<ActionResult<CommandReadDto>>(response);
            Assert.IsType<CreatedAtRouteResult>(response.Result);


            _mapperMock.Verify(mapper => mapper.Map<Commander.Models.Command>(It.IsAny<CommandCreateDto>()),Times.Once);

            _repositoryMock.Verify(repo => repo.CreateCommand(dummyRepositoryReturnObject), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);

      
            _mapperMock.Verify(mapper => mapper.Map<CommandReadDto>(dummyRepositoryReturnObject), Times.Once);



            //  putting the response in a different format to make it easier for checking the contents
            CreatedAtRouteResult result = (CreatedAtRouteResult) response.Result;
            CommandReadDto returnedModel = (CommandReadDto) result.Value;



            Assert.Equal((double) HttpStatusCode.Created,(double) result.StatusCode);

            Assert.IsType<CommandReadDto>(returnedModel);
            Assert.Equal(1, returnedModel.Id);
            Assert.Equal("one", returnedModel.HowTo);
            Assert.Equal("line one", returnedModel.Line);
        }

        [Fact]
        private void UpdateCommand_ValidIdProvided_CommandBodyFormatSuccessfullyUpdated()
        {
            int id = 1;

            var dummyCommandUpdateDto = new CommandUpdateDto
            {
                HowTo = "one",
                Line = "line one",
                Platform = "platform one"
            };

            var dummyRepositoryReturnObject = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "How to create migrations",
                Line = "dotnet ef migrations add <Name of Migrations>"
            };

            _repositoryMock.Setup(repo => repo.GetCommandById(id)).Returns(dummyRepositoryReturnObject);


            //  Set up Mock Mapper to return our dummy object
            _mapperMock.Setup(mapper => mapper.Map<CommandUpdateDto, Commander.Models.Command>(dummyCommandUpdateDto, dummyRepositoryReturnObject))
                .Returns(dummyRepositoryReturnObject);

            _repositoryMock.Setup(repo => repo.UpdateCommand(dummyRepositoryReturnObject));
            _repositoryMock.Setup(repo => repo.SaveChanges());

            //  Test
            ActionResult response = _controller.UpdateCommand(id, dummyCommandUpdateDto);

            //  Verify
            Assert.NotNull(response);
            Assert.IsAssignableFrom<ActionResult>(response);
            Assert.IsType<NoContentResult>(response);

            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(id), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map(dummyCommandUpdateDto,dummyRepositoryReturnObject), Times.Once);

            _repositoryMock.Verify(repo => repo.UpdateCommand(dummyRepositoryReturnObject), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);




            //  putting the response in a different format to make it easier for checking the contents
           NoContentResult result = (NoContentResult) response;
           NoContentResult returnedModel = result;

            Assert.Equal((double) HttpStatusCode.NoContent,(double) result.StatusCode);

            Assert.IsAssignableFrom<ActionResult>(returnedModel);
        }

        [Fact]
        private void UpdateCommand_InvalidIdProvided_ReturnsNotFound()              //  IF NULL 
        {

            CommandUpdateDto dummyCommandUpdateDto = new CommandUpdateDto
            {
                HowTo = "one",
                Line = "line one",
                Platform = "platform one"
            };

            Commander.Models.Command commandModel = null;

            _repositoryMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns(commandModel);



            //  Test
            ActionResult response = _controller.UpdateCommand(100, dummyCommandUpdateDto);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
            NotFoundResult result = (NotFoundResult) response;
            Assert.Equal(404, result.StatusCode);                               


            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(100), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map(It.IsAny<CommandUpdateDto>(), It.IsAny<Commander.Models.Command>()), Times.Never);

            _repositoryMock.Verify(repo => repo.UpdateCommand(It.IsAny<Commander.Models.Command>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        private void PartialCommandUpdate_ValidCommandSelectedById_CommandBodyFormatSuccessfullyPartiallyUpdated()
        {
            int id = 1;


            var dummyPatchDoc = new JsonPatchDocument<CommandUpdateDto>
            {
                Operations =
                {
                    new Operation<CommandUpdateDto>
                    {
                        op = "replace",
                        path = "HowTo",
                        value = "unit test"
                    }
                }
            };


            //  create a Mock object for the fake repository to return
            var dummyRepositoryReturnObjectOriginal = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "the original howto value",
                Line = "the original line value",
                Platform = "the original platform value"
            };
            _repositoryMock.Setup(repo => repo.GetCommandById(id)).Returns(dummyRepositoryReturnObjectOriginal);


            var dummyCommandtoPatchOriginal = new CommandUpdateDto
            {

                HowTo = "the original howto value",
                Line = "the original line value",
                Platform = "the original platform value"
            };


            //  var dummyCommandtoPatchModified - represents the state after it has been patched by the patchdoc.applyto - rep the modified field 
            //  verify that this version of the object gets passed into mapper.map
            var dummyCommandtoPatchModified = new CommandUpdateDto
            {

                HowTo = "unit test",
                Line = "the original line value",
                Platform = "the original platform value"
            };

            var dummyRepositoryReturnObjectModified = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "unit test",
                Line = "the original line value",
                Platform = "the original platform value"
            };


            var mapToCommandUpdateDtoCallBack = new Commander.Models.Command();
            _mapperMock.Setup(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<object>()))
                .Callback<object>(arg1 => mapToCommandUpdateDtoCallBack = (Commander.Models.Command) arg1)
                .Returns(dummyCommandtoPatchOriginal);









            var commandToPatchCallBack = new CommandUpdateDto();                // creating empty box to store what gets passed into mapper.Map
            var commanderCommandCallBack = new Commander.Models.Command();      // creating an empty box to store what gets passed into mapper.Map
            //  expecting 1st argument to have the modified command patch 
            //  expecting 2nd command to have original Command repo object



            _mapperMock.Setup(mapper => mapper
            .Map<CommandUpdateDto,Commander.Models.Command>(It.IsAny<CommandUpdateDto>(), It.IsAny<Commander.Models.Command>()))
               .Callback<CommandUpdateDto,Commander.Models.Command>(
               (arg1,arg2) => { commandToPatchCallBack = arg1; commanderCommandCallBack = arg2; })
               .Returns(dummyRepositoryReturnObjectModified);






            var repoUpdateCommandCallBack = new Commander.Models.Command();
            _repositoryMock.Setup(repo => repo.UpdateCommand(It.IsAny<Commander.Models.Command>()))
                .Callback<Commander.Models.Command>(arg1 => repoUpdateCommandCallBack = arg1);

            _repositoryMock.Setup(repo => repo.SaveChanges());



 

            //  Here we are mocking validation of the TryValidateModel block on line 127 in the Controller code to return true
            var objectValidator = new Mock<IObjectModelValidator>();

            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<CommandUpdateDto>()));

            _controller.ObjectValidator = objectValidator.Object;



            //  Test
            ActionResult response = _controller.PartialCommandUpdate(1, dummyPatchDoc);




            //  Verify
            Assert.NotNull(response);
            Assert.IsAssignableFrom<ActionResult>(response);
            Assert.IsType<NoContentResult>(response);

            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(1), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<object>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<Commander.Models.Command>()), Times.Once);
            
            //  Verify that what was passed into mapper.Map (now stored inside of our callback variable) is what we expect it to be (the original thing we got from repository)
            Assert.Equal(1, mapToCommandUpdateDtoCallBack.Id);
            Assert.Equal("the original howto value", mapToCommandUpdateDtoCallBack.HowTo);
            Assert.Equal("the original line value", mapToCommandUpdateDtoCallBack.Line);
            Assert.Equal("the original platform value", mapToCommandUpdateDtoCallBack.Platform);


            _mapperMock.Verify(mapper => mapper.Map(It.IsAny<object>(), It.IsAny<object>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map(It.IsAny<CommandUpdateDto>(),It.IsAny<Commander.Models.Command>()), Times.Once);
            
            //  Verify that what was passed into mapper.Map (now stored inside of our callback variable) is what we expect it to be 
            // arg1 expected to be (modified version of command to patch)
            Assert.Equal("unit test", commandToPatchCallBack.HowTo);
            Assert.Equal("the original line value", commandToPatchCallBack.Line);
            Assert.Equal("the original platform value", commandToPatchCallBack.Platform);
            // arg2 expected to be (original version of repo command)
            Assert.Equal(1, commanderCommandCallBack.Id);
            Assert.Equal("the original howto value", commanderCommandCallBack.HowTo);
            Assert.Equal("the original line value", commanderCommandCallBack.Line);
            Assert.Equal("the original platform value", commanderCommandCallBack.Platform);


            _repositoryMock.Verify(repo => repo.UpdateCommand(It.IsAny<Commander.Models.Command>()), Times.Once);
            
            //  Verify that what was pased into repo.UpdateCommand (now stored inside of our callback variable) is what we expect it to be (the modified command accomplished from patching) 
            Assert.Equal(1, repoUpdateCommandCallBack.Id);
            Assert.Equal("unit test", repoUpdateCommandCallBack.HowTo);
            Assert.Equal("the original line value", repoUpdateCommandCallBack.Line);
            Assert.Equal("the original platform value", repoUpdateCommandCallBack.Platform);

            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);

            objectValidator.Verify(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<object>()),Times.Once);

        }

        [Fact]
        private void PartialCommandUpdate_BadIdProvided_ReturnsNotFound()              //  IF NULL 
        {
            var dummyPatchDoc = new JsonPatchDocument<CommandUpdateDto>
            {
                Operations =
                {
                    new Operation<CommandUpdateDto>
                    {
                        op = "replace",
                        path = "HowTo",
                        value = "unit test"
                    }
                }
            };

            Commander.Models.Command commandModel = null;

            _repositoryMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns(commandModel);

            //  Test
            ActionResult response = _controller.PartialCommandUpdate(100,dummyPatchDoc);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
            NotFoundResult result = (NotFoundResult) response;
            Assert.Equal(404,result.StatusCode);


            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()),Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(100),Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<object>()),Times.Never);


            _mapperMock.Verify(mapper => mapper.Map(It.IsAny<object>(),It.IsAny<object>()),Times.Never);

            _repositoryMock.Verify(repo => repo.UpdateCommand(It.IsAny<Commander.Models.Command>()),Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChanges(),Times.Never);
        }

        [Fact]
        private void PartialCommandUpdate_TryValidateModelFail_ReturnsValidationProblem()              //  IF FAIL VALIDATE MODEL
        {



            var dummyPatchDoc = new JsonPatchDocument<CommandUpdateDto>()
            {
                Operations =
                {
                    new Operation<CommandUpdateDto>
                    {
                        op = "replace",
                        path = "howto",
                        value = "unit test"
                    }
                }
            };


            //  create a Mock object for the fake repository to return
            var dummyRepositoryReturnObjectOriginal = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "the original howto value",
                Line = "the original line value",
                Platform = "the original platform value"
            };
            _repositoryMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns(dummyRepositoryReturnObjectOriginal);


            var dummyCommandtoPatchOriginal = new CommandUpdateDto
            {

                HowTo = "the original howto value",
                Line = "the original line value",
                Platform = "the original platform value"

            };
            


            var mapToCommandUpdateDtoCallBack = new Commander.Models.Command();
            _mapperMock.Setup(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<object>()))
                .Callback<object>(arg1 => mapToCommandUpdateDtoCallBack = (Commander.Models.Command) arg1)
                .Returns(dummyCommandtoPatchOriginal);



            //  560-568 we are mocking validation of the TryValidateModel block on line 127 in the Controller code to return true
            var objectValidator = new Mock<IObjectModelValidator>();

            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<object>()));

            _controller.ObjectValidator = objectValidator.Object;



            var ctx = new ControllerContext() { HttpContext = new DefaultHttpContext() };
            _controller.ControllerContext = ctx;

            

            var problemDetails = new ValidationProblemDetails();
            var mock = new Mock<ProblemDetailsFactory>();
            mock
                .Setup(_ => _.CreateValidationProblemDetails(
                    It.IsAny<HttpContext>(),
                    It.IsAny<ModelStateDictionary>(),
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>())
                )
                .Returns(problemDetails).Verifiable();

            _controller.ProblemDetailsFactory = mock.Object;


            //  Adds error to ModelState failing validation and retunring 400 Status code Bad Request
            _controller.ModelState.AddModelError("Line","400");




            //  Test
            ActionResult response = _controller.PartialCommandUpdate(1,dummyPatchDoc);

            


            //  Verify
            Assert.NotNull(response);
            Assert.IsType<BadRequestObjectResult>(response);
            BadRequestObjectResult result = (BadRequestObjectResult) response;
            Assert.Equal(400, result.StatusCode);


            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(1), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<object>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<CommandUpdateDto>(It.IsAny<Commander.Models.Command>()), Times.Once);

            objectValidator.Verify(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<CommandUpdateDto>()), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map(It.IsAny<object>(),It.IsAny<object>()), Times.Never);

            _repositoryMock.Verify(repo => repo.UpdateCommand(It.IsAny<Commander.Models.Command>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Never);
        }

        [Fact]
        private void DeleteCommand_ValidIdProvided_SuccesfullyDeletedOneCommand()
        {
            int id = 1;


            var dummyRepositoryReturnObject = new Commander.Models.Command
            {
                Id = 1,
                HowTo = "How to create migrations",
                Line = "dotnet ef migrations add <Name of Migrations>"
            };

            _repositoryMock.Setup(repo => repo.GetCommandById(id)).Returns(dummyRepositoryReturnObject);

            _repositoryMock.Setup(repo => repo.DeleteCommand(dummyRepositoryReturnObject));
            _repositoryMock.Setup(repo => repo.SaveChanges());

            //  Test
            ActionResult response = _controller.DeleteCommand(id);

            //  Verify
            Assert.NotNull(response);
            Assert.IsAssignableFrom<ActionResult>(response);
            Assert.IsType<NoContentResult>(response);

            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(id), Times.Once);

            _repositoryMock.Verify(repo => repo.DeleteCommand(It.IsAny<Commander.Models.Command>()), Times.Once);
            _repositoryMock.Verify(repo => repo.DeleteCommand(dummyRepositoryReturnObject), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);

        }


        [Fact]
        private void DeleteCommand_BadIdProvided_ReturnsNotFound()              //  IF NULL 
        {
            Commander.Models.Command commandModel = null;

            _repositoryMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns(commandModel);

            //  Test
            ActionResult response = _controller.DeleteCommand(100);

            //  Verify
            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
            NotFoundResult result = (NotFoundResult) response;
            Assert.Equal(404, result.StatusCode);

            _repositoryMock.Verify(repo => repo.GetCommandById(It.IsAny<int>()), Times.Once);
            _repositoryMock.Verify(repo => repo.GetCommandById(100), Times.Once);

            _repositoryMock.Verify(repo => repo.DeleteCommand(It.IsAny<Commander.Models.Command>()), Times.Never);

            _repositoryMock.Verify(repo => repo.SaveChanges(), Times.Never);
        }
    }
}        
        
    

