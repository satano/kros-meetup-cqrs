namespace StudentManagement.Data
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new List<Enrollment>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual IList<Enrollment> Enrollments { get; set; }

        public Enrollment GetEnrollment(int index)
        {
            if (Enrollments.Count > index)
            {
                return Enrollments[index];
            }
            return null;
        }

        public virtual Enrollment Enrollment1 => GetEnrollment(0);
        public virtual Enrollment Enrollment2 => GetEnrollment(1);

        public virtual void Enroll(Course course)
        {
            if (Enrollments.Count >= 2)
            {
                throw new Exception("Cannot have more than 2 enrollments");
            }

            var enrollment = new Enrollment
            {
                Student = this,
                Course = course,
                EnrollmentDate = DateTime.Now.Date
            };
            Enrollments.Add(enrollment);
        }

        public virtual void RemoveEnrollment(Enrollment enrollment)
        {
            Enrollments.Remove(enrollment);
        }
    }
}
