﻿using ChatWS.Models;
using ChatWS.Models.Exceptions;
using ChatWS.Models.Requests;
using ChatWS.Models.Responses;
using ChatWS.Services;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatWS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService service)
        {
            _userService = service;
        }

        [HttpGet]
        public IActionResult GetByName(string name)
        {
            Response response = new Response();
            try
            {
                var users = _userService.GetUsers(name);
                response.Success = 1;
                response.Data = users;
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> AddContact(AddContactRequest request)
        {
            Response response = new Response();
            try
            {
                await _userService.AddContact(request);
                response.Success = 1;
                return Ok(response);
            }
            catch (NotExistException ex)
            {
                response.Message = ex.Message;
                return NotFound(response);
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
