using StudentRecords.Core;
using Xunit;

namespace StudentRecords.Tests;

public class IntegrationTests
{
    [Fact]
    public void RegisteredStudent_CanReceiveGradesAndAverage()
    {
        var students = new StudentRegistry();
        var student = students.Add("Иван Петров", "ИС-21");
        var grades = new GradeService(students);
        grades.AddGrade(student.Id, "Математика", 5);
        grades.AddGrade(student.Id, "Физика", 3);
        Assert.Equal(2, grades.GetGrades(student.Id).Count);
        Assert.Equal(4.0, grades.GetAverage(student.Id));
    }

    [Fact]
    public void UnknownStudent_CannotReceiveGrade()
    {
        var grades = new GradeService(new StudentRegistry());
        Assert.Throws<ArgumentException>(() => grades.AddGrade(Guid.NewGuid(), "Математика", 5));
    }

    [Fact]
    public void InvalidGrade_IsRejected()
    {
        var students = new StudentRegistry();
        var student = students.Add("Иван Петров", "ИС-21");
        var grades = new GradeService(students);
        Assert.Throws<ArgumentOutOfRangeException>(() => grades.AddGrade(student.Id, "Математика", 6));
        Assert.Empty(grades.GetGrades(student.Id));
    }
}
