namespace Task4_BinaryFileReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the path to a Binary file: ");
            string filePath = Console.ReadLine();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist. Check the path to the file.");
                return;
            }

            string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //обратились к рабочему столу

            string studentFolder = Path.Combine(desktoppath, "Students");
            if (!Directory.Exists(studentFolder))
            {
                Directory.CreateDirectory(studentFolder);
            }

            List<Student> students = ReadStudents(filePath);
            WriteStudentsToFiles(students, studentFolder);
            Console.WriteLine("Data are procced and saved to text files.");
        }

        private static List<Student> ReadStudents(string filePath)
        {
            List<Student> students = new List<Student>();
            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                { 
                    string name = reader.ReadString();
                    string group = reader.ReadString();
                    long dateOfBirthTicks = reader.ReadInt64();
                    DateTime dateOfBirth = new DateTime(dateOfBirthTicks);
                    decimal _GPA = reader.ReadDecimal();

                    students.Add(new Student
                    {
                        Name = name,
                        Group = group,
                        DateOfBirth = dateOfBirth,
                        GPA = _GPA
                    });
                }
            }
            return students;
        }

        private static void WriteStudentsToFiles(List<Student> students, string directory)
        { 
            var groups = new Dictionary<string, List<Student>>();
            foreach (var student in students) 
            {
                if (!groups.ContainsKey(student.Group))
                {
                    groups[student.Group] = new List<Student>(); //создаем группу с таким ключем, т.к. его еще нет в словаре
                }
                groups[student.Group].Add(student);
            }

            foreach (var group in groups)
            {
                string filePath = Path.Combine(directory, $"{group.Key}.txt");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var student in group.Value)
                    { 
                        writer.WriteLine($"{student.Name}, {student.DateOfBirth:yyyy-MM-dd}, {student.GPA}"); 
                    }
                }
            }
        }
    }
}
