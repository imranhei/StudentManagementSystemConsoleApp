using Newtonsoft.Json.Linq;
public class AddSemester{

    public string semCode{ get; set; } = default!;
    public string year{ get; set; } = default!;
    public JArray courses{ get; set; } = default!;
}
public class AddCourses{

    public string course_id{ get; set; } = default!;
    public string course_name{ get; set; } = default!;
    public string instructor_name{ get; set; } = default!;
    public int credits{ get; set; } = default!;
}