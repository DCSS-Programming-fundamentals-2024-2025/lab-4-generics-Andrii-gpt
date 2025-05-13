// Ткач Андрій КП-41 
using System;
using System.Collections.Generic;
using generics.Interfaces;

public class Student {
    public int Id { get; set;}
    public string Name { get; set;}

    public void SubmitWork() {
        Console.WriteLine($"{Name} submit work.");
    }

    public void SayName() {
        Console.WriteLine($"Student's name is {Name}.");
    }
}

public class Teacher {
    public int Id { get; set; }
    public string Name { get; set; }

    public void GradeStudent(Student student) {
        Console.WriteLine($"{Name} graded {student.Name}.");
    }

    public void ExpelStudent(Student student) {
        Console.WriteLine($"{student.Name} expele by {Name}.");
    }

    public void ShowPresentStudents(List<Student> students) {
        Console.WriteLine($"{Name} is present students:");
        foreach (var student in students)
        {
            Console.WriteLine($"- {student.Name}");
        }
    }
}

public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, new()
    where TKey : struct
{
    private readonly Dictionary<TKey, TEntity> _storage = new Dictionary<TKey, TEntity>();

    public void Add(TKey id, TEntity entity) {
        _storage[id] = entity;
    }

    public TEntity Get(TKey id) {
        if (_storage.TryGetValue(id, out var entity))
            return entity;

        throw new KeyNotFoundException($"No entity key {id}.");
    }

    public IEnumerable<TEntity> GetAll() {
        return _storage.Values;
    }

    public void Remove(TKey id) {
        if (!_storage.Remove(id))
        {
            throw new KeyNotFoundException($"Cannot remove.");
        }
    }
}

public class Group {
    public int Id { get; set; }
    public string Name { get; set; }

    private IRepository<Student, int> _students = new InMemoryRepository<Student, int>();

    public void AddStudent(Student s) {
        _students.Add(s.Id, s);
    }

    public void RemoveStudent(int studentId) {
        _students.Remove(studentId);
    }

    public IEnumerable<Student> GetAllStudents() {
        return _students.GetAll();
    }

    public Student FindStudent(int studentId) {
        return _students.Get(studentId);
    }
}

public class Faculty {
    public int Id { get; set; }
    public string Name { get; set; }

    private IRepository<Group, int> _groups = new InMemoryRepository<Group, int>();

    public void AddGroup(Group g) {
        _groups.Add(g.Id, g);
    }

    public void RemoveGroup(int id) {
        _groups.Remove(id);
    }

    public IEnumerable<Group> GetAllGroups() {
        return _groups.GetAll();
    }

    public Group GetGroup(int id) {
        return _groups.Get(id);
    }

    public void AddStudentToGroup(int groupId, Student s) {
        var group = GetGroup(groupId);
        group.AddStudent(s);
    }

    public void RemoveStudentFromGroup(int groupId, int studentId) {
        var group = GetGroup(groupId);
        group.RemoveStudent(studentId);
    }

    public IEnumerable<Student> GetAllStudentsInGroup(int groupId) {
        var group = GetGroup(groupId);
        return group.GetAllStudents();
    }

    public Student FindStudentInGroup(int groupId, int studentId) {
        var group = GetGroup(groupId);
        return group.FindStudent(studentId);
    }
}

class Program {
    static void Main() {
        var fpm = new Faculty { Id = 1, Name = "ФПМ" };

        var group1 = new Group { Id = 41, Name = "КП-41" };
        var group2 = new Group { Id = 42, Name = "КП-42" };
        var group3 = new Group { Id = 43, Name = "КП-43" };

        fpm.AddGroup(group1);
        fpm.AddGroup(group2);
        fpm.AddGroup(group3);

        var s1 = new Student { Id = 1, Name = "Андрій" };
        var s2 = new Student { Id = 2, Name = "Рома" };
        var s3 = new Student { Id = 3, Name = "Саша" };

        fpm.AddStudentToGroup(41, s1);
        fpm.AddStudentToGroup(41, s2);
        fpm.AddStudentToGroup(42, s3);

        Console.WriteLine("Студенти групи КП-41:");
        foreach (var student in fpm.GetAllStudentsInGroup(41))
        {
            Console.WriteLine($"- {student.Name}");
        }
    }
}
