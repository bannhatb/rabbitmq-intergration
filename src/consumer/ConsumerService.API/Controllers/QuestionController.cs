using System.Formats.Asn1;
using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Repositories;
using ConsumerService.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerService.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class QuestionController : ControllerBase
    {
        private readonly IEventBusService _eventBus;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IEventBusService eventBus, IExamRepository examRepository, IQuestionRepository questionRepository)
        {
            _eventBus = eventBus;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
        }

        [HttpPost("add-question")]
        public IActionResult AddQuestion([FromBody] QuestionDto questionDto)
        {
            try
            {
                Question questionAdd = new Question()
                {
                    QuestionContent = questionDto.QuestionContent,
                    Explaint = questionDto.Explaint
                };
                _questionRepository.Add(questionAdd);
                _questionRepository.IsSaveChanges();
                foreach (var item in questionDto.AnswerDtos)
                {
                    Answer answerAdd = new Answer()
                    {
                        AnswerContent = item.AnswerContent,
                        RightAnswer = item.RightAnswer,
                        QuestionId = questionAdd.Id
                    };
                    _questionRepository.Add(answerAdd);
                }
                _questionRepository.IsSaveChanges();
                return Ok("Add Question Success");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-question/{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            Question questionDelete = _questionRepository.Question.FirstOrDefault(x => x.Id == id);
            if (questionDelete == null)
            {
                return NotFound("Not Found Question");
            }
            List<Answer> answersDelete = _questionRepository.Answers.Where(x => x.QuestionId == id).ToList();
            try
            {
                _questionRepository.Delete(questionDelete);
                foreach (var item in answersDelete)
                {
                    _questionRepository.Delete(item);
                }
                _questionRepository.IsSaveChanges();
                return Ok("Delete Exam Success: " + id);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-list-question")]
        public IActionResult GetListQuestion()
        {
            try
            {
                return Ok(_questionRepository.GetListQuestion());
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-question-detail/{id}")]
        public IActionResult GetQuestionDetail(int id)
        {
            Question questionDelete = _questionRepository.Question.FirstOrDefault(x => x.Id == id);
            if (questionDelete == null)
            {
                return NotFound("Not Found Question");
            }
            try
            {
                Question question = _questionRepository.GetQuestionById(id);
                List<AnswerDto> answers = _questionRepository.GetListAnswerDtoByQuestionId(id);
                QuestionDto questionDetail = new QuestionDto()
                {
                    QuestionId = id,
                    QuestionContent = question.QuestionContent,
                    Explaint = question.Explaint,
                    AnswerDtos = answers
                };

                return Ok(questionDetail);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-question-to-exam")]
        public IActionResult AddQuestionToExam([FromBody] QuestionExamDto questionExamDto)
        {
            Exam exam = _examRepository.Exams.FirstOrDefault(x => x.Id == questionExamDto.ExamId);
            if (exam == null)
            {
                return NotFound("Not Found Exam");
            }
            try
            {
                foreach (var item in questionExamDto.QuestionIds)
                {
                    Question question = _questionRepository.Question.FirstOrDefault(x => x.Id == item);
                    if (question == null)
                    {
                        return NotFound("Not Found Question: " + item);
                    }
                }
                foreach (var item in questionExamDto.QuestionIds)
                {
                    QuestionExam questionExam = new QuestionExam()
                    {
                        ExamId = questionExamDto.ExamId,
                        QuestionId = item
                    };
                    _questionRepository.Add(questionExam);
                }
                _questionRepository.IsSaveChanges();
                return Ok("Add Question To Exam Success");
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}