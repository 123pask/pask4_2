using MyAlias = System.Collections.Generic.List<Student>;
//наш динамічний список
struct Student
{
    public string LastName;
    public string FirstName;
    public string Patronymic;
    public char Gender;
    public int BirthDay;
    public int BirthMonth;
    public int BirthYear;
    public int MathGrade;
    public int PhysGrade;
    public int InfoGrade;
    public int Scholarship;
}

class Program
{
    static Student[] ReadStudents(string path)
    {
        var list = new MyAlias();

        foreach (string line in File.ReadLines(path, System.Text.Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] p = line.Split(new char[] { ' ', '\t' },
                                    StringSplitOptions.RemoveEmptyEntries);
            if (p.Length < 9) continue;

            Student s = new Student(); //запис ІПБ
            s.LastName = p[0];
            s.FirstName = p[1];
            s.Patronymic = p[2];

            char g = char.ToUpper(p[3][0]);//стать
            s.Gender = (g == 'M' || g == 'М' || g == 'Ч') ? 'Ч' : 'Ж';

            string[] d = p[4].Split('.');//дата
            s.BirthDay = int.Parse(d[0]);
            s.BirthMonth = int.Parse(d[1]);
            s.BirthYear = int.Parse(d[2]);

            s.MathGrade = p[5] == "-" ? 0 : int.Parse(p[5]);//оцінка з математкии
            s.PhysGrade = p[6] == "-" ? 0 : int.Parse(p[6]);//з фізики 
            s.InfoGrade = p[7] == "-" ? 0 : int.Parse(p[7]);// з інформатики
            s.Scholarship = int.Parse(p[8]);

            list.Add(s);//Додаємо заповненого студента до списку.
        }

        return list.ToArray();
    }

    static int GetAge(Student s)
    {
        DateTime today = DateTime.Now;
        int age = today.Year - s.BirthYear;//рахує поточний вік студента

        bool birthdayPassed =
            s.BirthMonth < today.Month ||
            (s.BirthMonth == today.Month && s.BirthDay <= today.Day); //перевірка на др цього року

        if (!birthdayPassed) age--;// якщо др не настав іще -> мінус рік
        return age;
    }

    static void Task22(Student[] students)
    {
        Console.WriteLine("Варіант 22: студенти до 18 років із хоча б однією незданою дисципліною:\n");

        bool anyFound = false;

        foreach (Student s in students)
        {
            if (GetAge(s) >= 18) continue; // чи повнолітній перевірка

            bool failed = s.MathGrade == 0 || s.MathGrade == 2 ||
                          s.PhysGrade == 0 || s.PhysGrade == 2 ||
                          s.InfoGrade == 0 || s.InfoGrade == 2;  // мають неявки чи двійки

            if (!failed) continue;

            string birthDate = $"{s.BirthDay:D2}.{s.BirthMonth:D2}.{s.BirthYear}";
            Console.WriteLine($"{s.LastName} {s.FirstName} {s.Patronymic}   {birthDate}");
            anyFound = true;
        }

        if (!anyFound)
            Console.WriteLine("Таких студентів немає.");
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string path = "input.txt";

        if (!File.Exists(path))
        {
            Console.WriteLine($"Помилка: файл '{path}' не знайдено.");
            return;
        }

        Student[] students = ReadStudents(path);
        Console.WriteLine($"Прочитано студентів: {students.Length}\n");

        Task22(students);
    }
}
