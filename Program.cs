using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#nullable disable
class Program{

    enum Department{
        CSE = 1,
        BBA,
        English
    }
    enum Degree{
        BSC = 1,
        BBA,
        BA,
        MSC,
        MBA,
        MA
    }
    enum SemesterCode{
        Summer = 1,
        Fall,
        Spring
    }
    private Dictionary<string, string[]> Courses = new Dictionary<string, string[]>(){
            {"CSE", new string[]{
                "CSE 101: Introduction to Computer Science (3 credits)",
                "CSE 110: Programming Language (3 credits)",
                "CSE 220: Data Structures (3 credits)",
                "CSE 221: Algorithms (3 credits)",
                "CSE 230: Discrete Mathematics (3 credits)",
                "CSE 250: Circuits and Electronics (3 credits)",
                "CSE 251: Electronic Devices and Circuits (3 credits)",
                "CSE 260: Digital Logic Design (3 credits)",
                "CSE 321: Operating Systems ( 3 credits)",
                "CSE 330: Numerical Methods ( 3 credits)",
                "CSE 340: Computer Architecture (3 credits)",
                "CSE 341: Microprocessors (3 credits)"
            }},
            {"BBA", new string[]{
                "BBA 101:	INTRODUCTION TO BUSINESS (3 credits)",
                "BBA 103:	BUSINESS MATH (3 credits)",
                "BBA 102:	FINANCIAL ACCOUNTING (3 credits)",
                "BBA 105:	BANGLADESH STUDIES (3 credits)",
                "BBA 110:	MICRO ECONOMICS (3 credits)",
                "BBA 107:	PRINCIPLES OF MARKETING (3 credits)"
            }},
            {"English", new string[]{
                "ENG 111: Fundamentals of English (3 credits)",
                "ENG 112: Composition (3 credits)",
                "ENG 122: Introduction to Literature (3 credits)",
                "ENG 222: Metaphysical Poetry (3 credits)",
                "ENG 331: Literary Theory (3 credits)",
                "ENG 432: Research Paper (3 credits)"
            }}
        };

    private void AddStudent(){
        
        var (FirstName, MiddleName, LastName) = new ValidityCheck().NameCheck();
        var SId = new ValidityCheck().IdCheck();
        DateTime Today = DateTime.Now;
        int Month = Int16.Parse(Today.ToString("MM"));
        string joiningBatch = Month < 5 ? "Summer" : Month < 9 ? "Fall" : "Spring";

        var Dept = (Department) new ValidityCheck().Select("Choose Department:\n1.CS\t2.BBA\t3.English\nChoose: ", 3);
        var Deg = (Degree) new ValidityCheck().Select("Choose Degree:\n1.BSC\t2.BBA\t3.BA\t4.MSC\t5.MBA\t6.MA\nChoose: ", 6);

        var std = new Student()
        {
            fName = FirstName,
            mName = MiddleName,
            lName = LastName,
            id = SId,
            batch = joiningBatch,
            department = Dept.ToString(),
            degree = Deg.ToString(),
            semester = new JArray()
        };
        
        var Json = File.ReadAllText(@"student.json");
        JArray Student = JArray.Parse(Json);
        var StudentToString = JsonConvert.SerializeObject(std, Formatting.Indented);
        var Jobject = JObject.Parse(StudentToString);
        Student.Add(Jobject);
        string NewJson = JsonConvert.SerializeObject(Student, Formatting.Indented);
        File.WriteAllText(@"student.json", NewJson);
        new Format().ViewStudentList();
    }

    public void ViewStudent(){
        try{
            Console.Write("Enter Student ID: ");
            string SId = Console.ReadLine();
            bool Find = false;
            string Json = File.ReadAllText(@"student.json");
            JArray JsonArray = JArray.Parse(Json);
            int Index = 0;
            string StdJson = null;
            foreach(var std in JsonArray){
                if(SId == (string)std["id"]){
                    StdJson = JsonConvert.SerializeObject(std, Formatting.Indented);
                    Find = true;
                    break;
                }
                Index++;
            }
            if(!Find){
                string msg = SId.Length > 10 ? "Id Should not be more than 10 digits" : "ID not found, Please Enter valid Id.";
                Console.WriteLine(msg);
                ViewStudent();
            }
            else{
                var JsonObj = JObject.Parse(StdJson);
                new Format().ViewDetails(JsonObj);

                string sem;
                do{
                    Console.WriteLine("1.Add new semester\n2.Exit");
                    sem = Console.ReadLine();
                    if(sem == "1"){
                        var NewJsonObj = AddNewSemester(JsonObj);
                        JsonArray[Index] = NewJsonObj;
                        string NewJson = JsonConvert.SerializeObject(JsonArray, Formatting.Indented);
                        File.WriteAllText(@"student.json", NewJson);
                    }
                    else if(sem == "2"){
                        Main(null);
                    }
                    else{
                        Console.WriteLine("Please choose valid option.");
                    }
                }while(sem != "1" && sem != "2");
                new Format().ViewDetails(JsonObj);
            }
        }
        catch (Exception)  
            {  
                Console.WriteLine("Please Write a valid integer ID");
                ViewStudent();  
            }
    }

    public JObject AddNewSemester(JObject JsonObj){

        var Taken = new string[]{};
        var Dept = Courses[JsonObj["department"].ToString()];
        var SemesterArray = (JArray)JsonObj["semester"];
        
        foreach(var semester in SemesterArray){
            
            var Courses = (JArray)semester["courses"];
            foreach(var course in Courses){
                string Temp = $"{course["course_id"].ToString()}: {course["course_name"].ToString()} ({course["credits"].ToString()} credits)";
                Taken = Taken.Append(Temp).ToArray();
            }
        }
        
        var SemCode = (SemesterCode) new ValidityCheck().Select("Semester Code:\n1.Summer\t2.Fall\t3.Spring\nChoose option: ", 3);
        Console.Write("Year: ");
        string Year = Console.ReadLine();
        Console.Write("How many courses you want to assign: ");
        int NumberOfCourses = Int16.Parse(Console.ReadLine());
        JArray Course = new JArray();
        
        for(int i=0; i<NumberOfCourses; i++){
            Console.WriteLine("Available course list:");
            var Remains = Dept.Except(Taken).ToArray();
            
            for(int j=0; j<Remains.Length; j++){
                Console.WriteLine(j+1 + "." + Remains[j]);
            }
            Console.Write("Select course: ");
            int Take = Int16.Parse(Console.ReadLine());
            string CourseDetails = Remains[Take-1];
            Taken = Taken.Append(CourseDetails).ToArray();
            Console.Write("Instructor Name: ");
            string InstructorName = Console.ReadLine();

            int index = 0;
            while(CourseDetails[index++]!=':'){}

            string CourseId = CourseDetails.Substring(0, index-1);   
            string CourseName = CourseDetails.Substring(index+1, CourseDetails.Length-13-index);
            int Credit = CourseDetails[CourseDetails.Length-10] - '0';

            var course = new AddCourses()
            {
                course_id = CourseId,
                course_name = CourseName,
                instructor_name = InstructorName,
                credits = Credit
            };
            var CourseJson = JsonConvert.SerializeObject(course, Formatting.Indented);
            var CourseJsonObj = JObject.Parse(CourseJson);
            Course.Add(CourseJsonObj);
        }
        var sem = new AddSemester()
        {
            semCode = SemCode.ToString(),
            year = Year,
            courses = Course
        };

        var NewJson = JsonConvert.SerializeObject(sem, Formatting.Indented);
        var NewSemester = JObject.Parse(NewJson);
        var Semester = JsonObj.GetValue("semester") as JArray;
        Semester.Add(NewSemester);
        JsonObj["semester"] = Semester;

        return JsonObj;
    }

    public void DeleteStudent(){
        Console.Write("Which Id you want to delete: ");
        string Id = Console.ReadLine();
        string Json = File.ReadAllText(@"student.json");
        JArray JsonArray = JArray.Parse(Json);

        if(JsonArray.Count()>0){
            bool Find = false;
            int Index = 0;

            foreach(var std in JsonArray){
                if(Id == (string)std["id"]){
                    Find = true;
                    break;
                }
                Index++;
            }
            if(!Find){
                string msg = Id.Length > 10 ? "Id Should not be more than 10 digits" : "ID not found, Please Enter valid Id.";
                Console.WriteLine(msg);
                DeleteStudent();
            }
            else{
                JsonArray.Remove(JsonArray[Index]);
                string newJson = JsonConvert.SerializeObject(JsonArray, Formatting.Indented);
                File.WriteAllText(@"student.json", newJson);
            }
            new Format().ViewStudentList();
        }
        else{
            Console.WriteLine("No student availale");
        }
    }

    public void createJson(){
        try{
            string json = File.ReadAllText(@"student.json");
            if(json.Length==0){
                File.WriteAllText(@"student.json", "[]");
            }
        }
        catch(Exception){
            File.WriteAllText(@"student.json", "[]");
        }
    }

    static void Main(string[] args){
        Program obj = new Program();
        obj.createJson();
        new Format().ViewStudentList();
        
        while(true){
            Console.WriteLine("Which Operation you want to do:");
            Console.WriteLine("1.Add Student\n2.View Student Details\n3.Delete Student\n4.View Student List");
            var operation = Console.ReadLine();

            switch (operation)
            {
                case "1":
                    obj.AddStudent();
                    break;
                case "2":
                    obj.ViewStudent();
                    break;
                case "3":
                    obj.DeleteStudent();
                    break;
                case "4":
                    new Format().ViewStudentList();
                    break;
                default:
                    Console.WriteLine("Please choose valid option.");
                    break;
            }
        }
    }
}
