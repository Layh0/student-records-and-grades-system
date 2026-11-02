using System.Diagnostics;

namespace StudentRecords.Core;

public sealed class GradeService
{
    private readonly IStudentLookup students;
    private readonly List<Grade> grades = [];

    public GradeService(IStudentLookup students)
    {
        this.students = students ?? throw new ArgumentNullException(nameof(students));
    }

    public Grade AddGrade(Guid studentId, string subject, int value)
    {
        if (students.Find(studentId) is null)
        {
            Trace.TraceWarning("Grade rejected: studentId={0}, reason=student_not_found", studentId);
            throw new ArgumentException("Студент не найден.", nameof(studentId));
        }
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Укажите предмет.", nameof(subject));
        if (value is < 2 or > 5)
            throw new ArgumentOutOfRangeException(nameof(value), "Оценка должна быть от 2 до 5.");

        var grade = new Grade(studentId, subject.Trim(), value);
        grades.Add(grade);
        Trace.TraceInformation("Grade added: studentId={0}, value={1}", studentId, value);
        return grade;
    }

    public IReadOnlyList<Grade> GetGrades(Guid studentId) =>
        grades.Where(grade => grade.StudentId == studentId).ToArray();

    public double? GetAverage(Guid studentId)
    {
        var studentGrades = GetGrades(studentId);
        return studentGrades.Count == 0 ? null : studentGrades.Average(grade => grade.Value);
    }
}
