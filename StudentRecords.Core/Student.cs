namespace StudentRecords.Core;

public sealed record Student(Guid Id, string FullName, string Group)
{
    public string DisplayName => $"{FullName} - {Group} ({Id})";
}
