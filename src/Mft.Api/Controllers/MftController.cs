using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc; using Mft.Application.Abstractions; using Mft.Application.Contracts;
namespace Mft.Api.Controllers;
[ApiController][Route("api")][Authorize] public sealed class MftController(IMftService s):ControllerBase{
[HttpGet("students")] public Task<IReadOnlyList<StudentDto>> Students(string? search,Guid? departmentId,CancellationToken ct)=>s.StudentsAsync(search,departmentId,ct);
[HttpPost("students")] public Task<StudentDto> CreateStudent(CreateStudentRequest r,CancellationToken ct)=>s.CreateStudentAsync(r,ct);
[HttpDelete("students/{id:guid}")] public async Task<IActionResult> DeleteStudent(Guid id,CancellationToken ct){await s.DeleteStudentAsync(id,ct);return NoContent();}
[HttpGet("teachers")] public Task<IReadOnlyList<TeacherDto>> Teachers(string? search,Guid? departmentId,CancellationToken ct)=>s.TeachersAsync(search,departmentId,ct);
[HttpPost("teachers")] public Task<TeacherDto> CreateTeacher(CreateTeacherRequest r,CancellationToken ct)=>s.CreateTeacherAsync(r,ct);
[HttpGet("departments")] public Task<IReadOnlyList<DepartmentDto>> Departments(CancellationToken ct)=>s.DepartmentsAsync(ct);
[HttpPost("departments")] public Task<DepartmentDto> CreateDepartment(CreateDepartmentRequest r,CancellationToken ct)=>s.CreateDepartmentAsync(r,ct);
[HttpDelete("departments/{id:guid}")] public async Task<IActionResult> DeleteDepartment(Guid id,CancellationToken ct){await s.DeleteDepartmentAsync(id,ct);return NoContent();}
[HttpGet("questions")] public Task<IReadOnlyList<QuestionDto>> Questions(Guid? departmentId,CancellationToken ct)=>s.QuestionsAsync(departmentId,ct);
[HttpPost("questions")] public Task<QuestionDto> CreateQuestion(CreateQuestionRequest r,CancellationToken ct)=>s.CreateQuestionAsync(r,ct);
[HttpGet("exams")] public Task<IReadOnlyList<ExamDto>> Exams(CancellationToken ct)=>s.ExamsAsync(ct);
[HttpPost("exams")] public Task<ExamDto> CreateExam(CreateExamRequest r,CancellationToken ct)=>s.CreateExamAsync(r,ct);
[HttpGet("dashboard")] public Task<object> Dashboard(CancellationToken ct)=>s.DashboardAsync(ct);
}