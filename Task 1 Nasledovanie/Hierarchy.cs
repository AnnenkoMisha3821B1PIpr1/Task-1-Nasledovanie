using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1_Nasledovanie
{
    public class Person
    {
        string name;
        int age;
                
        public int Age
        {
            get { return age; }
            set { if (value < 0) { age = 0; } else if (value >= 0) { age = value; } }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Person()
        {
            name = " ";
            age = 0;
        }

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        static public Person[] persons = new Person[]
        {
            new Person("Семён", 19), new Person("Славя", 20),
            new Person("Маша", 18), new Person("Алиса", 17),
            new Person("Ваня", 19), new Person("Ольга", 21)
        };

        public virtual Person Clone()
        {
            return new Person(name, age);
        }

        public static Person RandomPerson(Random random)
        {
            if (random != null)
                return persons[random.Next(persons.Length)];
            else return persons[(new Random()).Next(persons.Length)].Clone();
        }

        public override string ToString()
        {
            return name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode() + age.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Person)) return false;
            Person ObjPerson = (Person)obj;
            if ((ObjPerson.Age == Age) && (ObjPerson.Name == Name)) return true;
            return false;
        }

        public virtual void Print()
        {
            Console.WriteLine("Имя: {0}; Возраст: {1}", name, age);
        }  
    }




    public class Student : Person
    {
        int course = 1;
        Teacher teacher;

        public int Course
        {
            get { return course; }
            set
            {
                if ((value < 1) || (value > 5)) course = 1;
                else course = value;
            }
        }

        public Teacher Teacher_
        {
            get { return teacher; }
            set { teacher = value; }
        }

        public Student() : base()
        {
            Course = 1;
            teacher = new Teacher();
        }

        public Student(string name, int age, int course, Teacher teacher = null) : base(name, age)
        {
            Course = course;
            Teacher_ = teacher;
        }

        static public Student[] students = new Student[] 
        { 
            new Student("Саша", 20, 2), new Student("Алёна", 23, 4),                                                 
            new Student("Миша", 21, 3), new Student("Петя", 17, 1),                                                 
            new Student("Лиза", 19, 2), new Student("Аня", 22, 3)
        };

        public override Student Clone()
        {
            if (teacher == null)
                return new Student(Name, Age, Course, null);
            Teacher Clone_TeacherClone = teacher.Clone();
            foreach (var student in Clone_TeacherClone.Students)
                if (this.Equals(student)) return student;
            return new Student(Name, Age, Course, Clone_TeacherClone);
        }

        public static Student RandomStudent(Random rnd)
        {
            if (rnd != null)
                return students[rnd.Next(students.Length)];
            else return students[(new Random()).Next(students.Length)].Clone();
        }
        
        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            int sum = base.GetHashCode() + course.GetHashCode();
            if (teacher != null)
                sum += teacher.GetHashCode();
            return sum;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Student)) return false;
            else
            {
                Student student = (Student)obj;
                if ((student.Name == Name) && (student.Age == Age) && (student.Course == Course))
                    if (teacher == null)
                        if (student.Teacher_ == null) return true;
                        else return false;
                    else if (student.Teacher_ == null) return false;
                    else return teacher.Equals(student.Teacher_);
            }
            return false;
        }

        public override void Print()
        {
            base.Print();
            Console.Write($"Курс: {Course}; Препод: ");
            if (teacher == null) Console.WriteLine("Отсутствует");
            else Console.WriteLine(teacher);
        }
        
        static void Print_Ancestors(Type type)
        {
            Console.WriteLine(type.Name);
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                Print_Ancestors(type.BaseType); // Рекурсивно вызываем для предка его предка, до тех пор пока не достигнем базового класса (object)
            }
        }

        public static void Ancestors_In_Student_class()
        {
            Console.WriteLine("Предки класса Student:");
            Print_Ancestors(typeof(Student));
            Console.WriteLine(typeof(object).Name);
        }
    }




    public class Teacher : Person
    {
        List<Student> students = new List<Student>();

        public List<Student> Students
        {
            get { return students; }
            set
            {
                students.Clear();
                students.AddRange(value);
            }
        }
        public Teacher() : base() { }

        public Teacher(string name, int age, List<Student> students = null) : base(name, age)
        {
            if (students != null)
            {
                this.students.AddRange(students);
                foreach (var student in students)
                {
                    student.Teacher_ = this;
                }
            }
        }

        public static Teacher[] teachers_ = new Teacher[]
        {
            new Teacher("Виктор Петрович", 30), new Teacher("Мария Александровна", 40),
            new Teacher("Елена Ивановна", 27), new Teacher("Михаил Юрьевич", 45),
            new Teacher("Ирина Викторовна", 35), new Teacher("Юрий Дмитриевич", 32)
        };

        public override Teacher Clone()
        {
            Teacher Clone_Teacher = new Teacher(Name, Age, null);
            List<Student> list_students = new List<Student>();
            Student Clone_Student;
            foreach (var student in students)
            {
                Clone_Student = new Student(student.Name, student.Age, student.Course, Clone_Teacher);
                list_students.Add(Clone_Student);
            }
            Clone_Teacher.students.AddRange(list_students);
            return Clone_Teacher;
        }

        public static Teacher RandomTeacher(Random random)
        {
            if (random != null)
            {
                return teachers_[random.Next(teachers_.Length)];
            }
            else 
            {
                return teachers_[(new Random()).Next(teachers_.Length)].Clone();
            }       
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            int sum = base.GetHashCode();
            foreach (var student in students)
                sum += student.Name.GetHashCode() + student.Age.GetHashCode() + student.Course.GetHashCode();
            return sum;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Teacher)) return false;
            Teacher Obj_Teacher = (Teacher)obj;
            if (base.Equals(obj))
                if (students.Count == Obj_Teacher.Students.Count)
                    foreach (var student_1 in students)
                    {
                        bool same = false;
                        foreach (var student_2 in student_1.Teacher_.Students)
                            if ((student_1.Age == student_2.Age) && (student_1.Name == student_2.Name) && (student_1.Course == student_2.Course))
                            {
                                same = true;
                                break;
                            }
                        if (same != true) return false;
                    }
                else return false;
            return true;
        }

        public override void Print()
        {
            base.Print();
            if (students.Count > 0)
                Console.Write("Студенты: ");
            else Console.Write("Студенты: Отсутствуют");
            foreach (var student in students) Console.Write($"{student}, ");
            Console.WriteLine();
        }

        public void AddStudent(Student student)
        {
            if (student.Teacher_ != null)
                student.Teacher_.Students.Remove(student);
            students.Add(student);
            student.Teacher_ = this;
        }
    }

}
