namespace StudentRecords.Core;

public sealed class StudentRegistry : IStudentLookup
{
    private readonly List<Student> students = [];

    public Student Add(string fullName, string group)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Укажите имя студента.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(group))
            throw new ArgumentException("Укажите группу.", nameof(group));

        var student = new Student(Guid.NewGuid(), fullName.Trim(), group.Trim());
        students.Add(student);
        return student;
    }

    public Student? Find(Guid studentId) => students.Find(student => student.Id == studentId);

    public IReadOnlyList<Student> GetStudents() => students.ToArray();
}
