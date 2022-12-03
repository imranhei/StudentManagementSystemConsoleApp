using Newtonsoft.Json.Linq;
#nullable disable
public class Format{
    private string FinalString(JToken x, int width){
        string data = x.ToString();
        int s = width - data.Length;
        string final = new string(' ', s/2) + data + new string(' ', s%2==0 ? s/2 : (s/2)+1);
        return final;
    }
    public void ViewStudentList(){

        string Json = File.ReadAllText(@"student.json");
        JArray JsonObj = JArray.Parse(Json);
    
        if(JsonObj.Count()>0){
            Console.WriteLine("List of the Students");
            Console.WriteLine(new string('-', 39));
            Console.WriteLine("|     ID     |      Student Name      |");
            Console.WriteLine(new string('-', 39));

            foreach(var student in JsonObj){
                string name = student["fName"].ToString() + " " + student["mName"].ToString() + " " + student["lName"].ToString();
                Console.WriteLine($"|{FinalString(student["id"], 12)}|{FinalString(name, 24)}|");
                Console.WriteLine(new string('-', 39));
            }
        }
    }
    public void ViewDetails(JObject JsonObj){

        Console.WriteLine(new string('-', 78));
        Console.WriteLine($"|{new string(' ', 5)}ID{new string(' ', 5)}|{new string(' ', 6)}Student Name{new string(' ', 6)}| Joining Batch  | Department | Degree |");
        Console.WriteLine(new string('-', 78));
        string name = JsonObj["fName"].ToString() + " " + JsonObj["mName"].ToString() + " " + JsonObj["lName"].ToString();
        Console.WriteLine($"|{FinalString(JsonObj["id"], 12)}|{FinalString(name, 24)}|{FinalString(JsonObj["batch"], 16)}|{FinalString(JsonObj["department"], 12)}|{FinalString(JsonObj["degree"],8)}|");
                
        Console.WriteLine(new string('-', 78));
        var JsonArray = (JArray)JsonObj["semester"];
        foreach(var Semester in JsonArray){
            Console.WriteLine("\nSemester Code: " + Semester["semCode"] + "\tYear: " + Semester["year"]);
            Console.WriteLine(new string('-', 83));
            var Courses = (JArray)Semester["courses"];
            Console.WriteLine($"| Course ID |{new string(' ', 12)}Course Name{new string(' ', 12)}|    Instructor Name    | Credits |");
            Console.WriteLine(new string('-', 83));
            foreach(var Course in Courses){
                Console.WriteLine("|" + FinalString(Course["course_id"], 11) + "|" + FinalString(Course["course_name"], 35) + "|" + FinalString(Course["instructor_name"], 23) + "|" + FinalString(Course["credits"], 9) + "|");
                Console.WriteLine(new string('-', 83));
            }
        }
    }
}