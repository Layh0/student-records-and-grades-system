namespace StudentRecords.Core;

public sealed record Grade(Guid StudentId, string Subject, int Value);
