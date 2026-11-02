using System.Xml.Linq;
using StudentRecords.Core;
using Xunit;

namespace StudentRecords.Tests;

public class DebuggingRegressionTests
{
    [Fact]
    public void QueryingUnknownStudent_DoesNotPretendToHaveNoGrades()
    {
        var service = new GradeService(new StudentRegistry());
        Assert.Throws<ArgumentException>(() => service.GetGrades(Guid.NewGuid()));
        Assert.Throws<ArgumentException>(() => service.GetAverage(Guid.NewGuid()));
    }

    [Fact]
    public void StudentsWithSameNameInDifferentGroups_HaveDistinctDisplayedLabels()
    {
        var registry = new StudentRegistry();
        var first = registry.Add("Иван Петров", "ИС-21");
        var second = registry.Add("Иван Петров", "ИС-22");
        var markup = XDocument.Load(Path.Combine(AppContext.BaseDirectory, "MainWindow.xaml"));
        XNamespace wpf = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
        var list = markup.Descendants(wpf + "ListBox").Single(e => (string?)e.Attribute(x + "Name") == "StudentsList");
        var propertyName = (string?)list.Attribute("DisplayMemberPath");
        var property = typeof(Student).GetProperty(propertyName ?? "");
        Assert.NotNull(property);
        Assert.NotEqual(property.GetValue(first), property.GetValue(second));
    }
}
