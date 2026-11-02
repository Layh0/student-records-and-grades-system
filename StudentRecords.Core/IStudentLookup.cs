namespace StudentRecords.Core;

public interface IStudentLookup
{
    Student? Find(Guid studentId);
}
