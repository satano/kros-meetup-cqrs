namespace StudentManagement.Data
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
