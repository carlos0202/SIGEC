using Microsoft.AspNet.Mvc;
using TheWorld.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System;

namespace TheWorld.Controllers.Api
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private IWorldRepository _repository;
        private UserManager<WorldUser> _userManager;

        public UsersController(IWorldRepository repository, UserManager<WorldUser> userManager)
        {
			_repository = repository;
			_userManager = userManager;
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string password)
        {
            var vm = new WorldUser(){
					UserName = "admin",
					Email = "admin@test.com",
					FirstTrip = DateTime.UtcNow
				};
            vm.FirstTrip = DateTime.UtcNow;
            if(string.IsNullOrWhiteSpace(password)){
                password = "P@ssword!";
            }
            try{
				var result = await _userManager.CreateAsync(vm, password);
                return Json(result);
            }catch(Exception ex){
                return Json(new{Message = ex.Message, Exception = ex });
            }
        }
    }
}