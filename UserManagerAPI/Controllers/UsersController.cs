using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Dtos;
using UserManagerAPI.Exceptions;
using UserManagerAPI.Services.Interfaces;

namespace UserManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController( IUserService userService )
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAll()
        {
            return Ok( _userService.GetAll() );
        }

        [HttpGet("{id}")]
        public ActionResult<UserReadDto> Get(int id)
        {
            try
            {
                return Ok( _userService.GetById( id ) );
            }
            catch(ApiException ex)
            {
                return StatusCode(ex.StatusCode, new { error = true, message = ex.Message } );
            }
        }

        [HttpPut("{id}")]
        public ActionResult<UserReadDto> Put(int id, UserUpdateDto user)
        {
            user.Id = id;
            try
            {
                return Ok(_userService.Update( user ));
            }
            catch(ApiException ex)
            {
                return StatusCode( ex.StatusCode, new { error = true, message = ex.Message } );
            }
        }

        [HttpPost]
        public ActionResult<UserReadDto> Post(UserCreateDto user)
        {
            try
            {
                UserReadDto newUser = _userService.Create( user );
                return CreatedAtAction( "Get", new { id = newUser.Id }, newUser );
            }
            catch (ApiException ex)
            {
                return StatusCode( ex.StatusCode, new { error = true, message = ex.Message } );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.Delete( id );
                return NoContent();
            }
            catch (ApiException ex)
            {
                return StatusCode( ex.StatusCode, new { error = true, message = ex.Message } );
            }
        }

    }
}
