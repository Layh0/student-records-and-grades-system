using StudentRecords.Core;
using Xunit;

namespace StudentRecords.Tests;

public class BoundaryTests
{
    [Fact]
    public void NewStudent_HasNoAverage()
    {
        var students = new StudentRegistry();
        var student = students.Add("Анна", "ИС-21");
        Assert.Null(new GradeService(students).GetAverage(student.Id));
    }

    [Fact]
    public void GradeLists_AreIsolatedByStudent()
    {
        var students = new StudentRegistry();
        var first = students.Add("Анна", "ИС-21");
        var second = students.Add("Мария", "ИС-21");
        var grades = new GradeService(students);
        grades.AddGrade(first.Id, "Физика", 4);
        Assert.Empty(grades.GetGrades(second.Id));
        Assert.Null(grades.GetAverage(second.Id));
        Assert.Single(grades.GetGrades(first.Id));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Grade_RejectsEmptySubject(string? subject)
    {
        var students = new StudentRegistry();
        var student = students.Add("Анна", "ИС-21");
        var grades = new GradeService(students);
        Assert.Throws<ArgumentException>(() => grades.AddGrade(student.Id, subject!, 4));
        Assert.Empty(grades.GetGrades(student.Id));
    }

    [Fact]
    public void Registry_TrimsInputAndReturnsSnapshot()
    {
        var students = new StudentRegistry();
        var first = students.Add(" Анна ", " ИС-21 ");
        var snapshot = students.GetStudents();
        students.Add("Мария", "ИС-22");
        Assert.Equal("Анна", first.FullName);
        Assert.Equal("ИС-21", first.Group);
        Assert.Single(snapshot);
    }

    [Fact]
    public void SameNameAndGroup_StillHaveDifferentLabels()
    {
        var students = new StudentRegistry();
        var first = students.Add("Анна", "ИС-21");
        var second = students.Add("Анна", "ИС-21");
        Assert.NotEqual(first.DisplayName, second.DisplayName);
    }

    [Fact]
    public void Service_RejectsNullLookup()
    {
        Assert.Throws<ArgumentNullException>(() => new GradeService(null!));
    }
}
