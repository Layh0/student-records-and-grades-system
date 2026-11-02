using System;
using System.Windows;
using System.Windows.Controls;
using StudentRecords.Core;

namespace student_records_and_grades_system;

public partial class MainWindow : Window
{
    private readonly StudentRegistry students = new();
    private readonly GradeService grades;

    public MainWindow()
    {
        InitializeComponent();
        grades = new GradeService(students);
    }

    private void AddStudent_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var student = students.Add(StudentNameInput.Text, GroupInput.Text);
            StudentsList.ItemsSource = students.GetStudents();
            StudentsList.SelectedItem = student;
            StudentNameInput.Clear();
            GroupInput.Clear();
        }
        catch (ArgumentException error)
        {
            MessageBox.Show(error.Message, "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void AddGrade_Click(object sender, RoutedEventArgs e)
    {
        if (StudentsList.SelectedItem is not Student student)
        {
            MessageBox.Show("Сначала выберите студента.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (!int.TryParse(GradeInput.Text, out int value))
        {
            MessageBox.Show("Введите целую оценку от 2 до 5.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        try
        {
            grades.AddGrade(student.Id, SubjectInput.Text, value);
            SubjectInput.Clear();
            GradeInput.Clear();
            RefreshGrades();
        }
        catch (ArgumentException error)
        {
            MessageBox.Show(error.Message, "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void StudentsList_SelectionChanged(object sender, SelectionChangedEventArgs e) => RefreshGrades();

    private void RefreshGrades()
    {
        if (StudentsList.SelectedItem is not Student student)
        {
            GradesGrid.ItemsSource = null;
            AverageText.Text = "Средний балл: -";
            return;
        }
        GradesGrid.ItemsSource = grades.GetGrades(student.Id);
        double? average = grades.GetAverage(student.Id);
        AverageText.Text = average.HasValue ? $"Средний балл: {average.Value:F2}" : "Средний балл: -";
    }
}
