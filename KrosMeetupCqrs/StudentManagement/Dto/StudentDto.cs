namespace StudentManagement.Dto
{
    public class StudentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Course1 { get; set; }
        public string Course2 { get; set; }
    }

    public class NewStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Course1 { get; set; }
        public string Course2 { get; set; }
    }

    public class StudentEnrollDto
    {
        public string Course { get; set; }
    }

    public class StudentChangeEnrollmentDto
    {
        public string Course { get; set; }
    }

    public class StudentPersonalInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
