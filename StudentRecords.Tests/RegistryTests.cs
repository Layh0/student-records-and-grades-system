using StudentRecords.Core;
using Xunit;

namespace StudentRecords.Tests;

public class RegistryTests
{
    [Fact]
    public void Add_StoresStudentAndFindsById()
    {
        var registry = new StudentRegistry();
        var student = registry.Add("Иван Петров", "ИС-21");
        Assert.Equal(student, registry.Find(student.Id));
        Assert.Single(registry.GetStudents());
    }

    [Fact]
    public void Add_RejectsEmptyName()
    {
        var registry = new StudentRegistry();
        Assert.Throws<ArgumentException>(() => registry.Add(" ", "ИС-21"));
    }
}
