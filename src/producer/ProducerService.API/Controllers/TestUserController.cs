using Microsoft.AspNetCore.Mvc;
using ProducerService.API.DTOs;
using ProducerService.API.Models.Entities;
using ProducerService.API.Models.Events;
using ProducerService.API.Repositories;
using ProducerService.API.Services;

namespace producer.ProducerService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestUserController : ControllerBase
    {
        private readonly ILogger<TestUserController> _logger;
        private readonly IEventBusService _eventBus;
        private readonly ITestUserRepository _testUserRepository;
        private readonly IUserRepository _userRepository;

        public TestUserController(IEventBusService eventBus, ILogger<TestUserController> logger = null, ITestUserRepository testUserRepository = null, IUserRepository userRepository = null)
        {
            _eventBus = eventBus;
            _logger = logger;
            _testUserRepository = testUserRepository;
            _userRepository = userRepository;
        }

        [HttpPost("add-test-user")]
        public IActionResult AddTestUser([FromBody] TestUserDto testUser)
        {
            try
            {
                TestUser testUserAdd = new TestUser()
                {
                    UserId = testUser.UserId,
                    ExamId = testUser.ExamId,
                    Point = testUser.Point
                };
                _testUserRepository.Add(testUserAdd);
                _testUserRepository.IsSaveChanges();
                return Ok("Add TestUser Success");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-test-user")]
        public IActionResult UpdateTestUser([FromBody] TestUserDto testUser)
        {
            TestUser testUserUpdate = _testUserRepository.GetTestUserById((int)testUser.Id);
            if (testUserUpdate == null)
            {
                return NotFound("Not found TestUser");
            }
            try
            {
                testUserUpdate.Point = testUser.Point;
                _testUserRepository.Update(testUserUpdate);
                _testUserRepository.IsSaveChanges();
                return Ok("Update TestUser Success: " + testUser.Id);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-test-user/{id}")]
        public IActionResult DeleteTestUserById(int id)
        {
            TestUser testUserDelete = _testUserRepository.GetTestUserById(id);
            if (testUserDelete == null)
            {
                return NotFound("Not found TestUser");
            }
            try
            {
                _testUserRepository.Delete(testUserDelete);
                _testUserRepository.IsSaveChanges();
                return Ok("Delete TestUser Success: " + id);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-list-test-user")]
        public IActionResult GetListTestUser()
        {
            try
            {
                return Ok(_testUserRepository.GetListTestUser());
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-rank-user-by/{examId}")]
        public IActionResult GetListTestUser(int examId)
        {
            try
            {
                return Ok(_testUserRepository.GetRankUserByExamId(examId));
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("send-test-user-choose")]
        public IActionResult SendTestUserChoose([FromBody] TestResultEvent testResult)
        {
            if (ModelState.IsValid)
            {
                _eventBus.Publish(testResult);

                return Ok(new { msg = "The answer has been sent successfully" });
            }

            return BadRequest(new { msg = "The answer has been sent failed" });
        }
    }
}