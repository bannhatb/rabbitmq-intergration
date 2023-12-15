using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Repositories;
using ConsumerService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IEventBusService _eventBus;

        public ExamController(IExamRepository examRepository, IQuestionRepository questionRepository, IEventBusService eventBus)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _eventBus = eventBus;
        }

        [HttpPost("add-exam")]
        public IActionResult AddExam([FromBody] ExamDto examDto)
        {
            try
            {
                Exam examAdd = new Exam()
                {
                    Title = examDto.Title,
                    CreateBy = "bannhatb",
                    Time = examDto.Time,
                    QuestionCount = examDto.QuestionCount
                };
                _examRepository.Add(examAdd);
                _examRepository.IsSaveChanges();
                return Ok("Add Exam Success");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-exam")]
        public IActionResult UpdateExam([FromBody] ExamDto examDto)
        {
            Exam examUpdate = _examRepository.Exams.FirstOrDefault(x => x.Id == examDto.Id);
            if (examUpdate == null)
            {
                return NotFound("Not Found Exam");
            }
            try
            {
                examUpdate.Title = examDto.Title;
                examUpdate.CreateBy = "bannhatb";
                examUpdate.Time = examDto.Time;
                examUpdate.QuestionCount = examDto.QuestionCount;
                _examRepository.Update(examUpdate);
                _examRepository.IsSaveChanges();
                return Ok("Update Exam Success: " + examDto.Id);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-exam/{id}")]
        public IActionResult DeleteExam(int id)
        {
            Exam examDelete = _examRepository.Exams.FirstOrDefault(x => x.Id == id);
            if (examDelete == null)
            {
                return NotFound("Not Found Exam");
            }
            try
            {
                _examRepository.Delete(examDelete);
                _examRepository.IsSaveChanges();
                return Ok("Delete Exam Success: " + id);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-list-exam")]
        public IActionResult GetListExam()
        {
            try
            {
                return Ok(_examRepository.GetListExam());
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-exam-detail/{id}")]
        public IActionResult GetExamDetail(int id)
        {
            try
            {
                Exam exam = _examRepository.GetExamById(id);
                if (exam == null)
                {
                    return NotFound("Not Found Exam");
                }

                ExamDetailDto examDetail = new ExamDetailDto
                {
                    Id = exam.Id,
                    Title = exam.Title,
                    Time = exam.Time,
                    QuestionCount = exam.QuestionCount,
                    CreateBy = exam.CreateBy,
                    QuestionDtos = _questionRepository.GetListQuestionDtoByExamId(id)
                };

                return Ok(examDetail);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [HttpPost("subscribe-cham-diem")]
        // public IActionResult Publish([FromBody] TestResultDto message)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _eventBus.Publish(message);
        //         Console.WriteLine("test result has been receive" + message);

        //         return Ok(new { msg = "message has been receive" });
        //     }

        //     return BadRequest(new { msg = "invalid message" });
        // }
    }
}