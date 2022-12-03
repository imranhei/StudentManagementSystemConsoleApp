using Newtonsoft.Json.Linq;
#nullable disable
public class ValidityCheck{
    
    public (string, string, string) NameCheck(){
        
        Console.Write("First Name: ");
        string first = Console.ReadLine();
        Console.Write("Middle Name: ");
        string middle = Console.ReadLine();
        Console.Write("Last Name: ");
        string last = Console.ReadLine();

        string name = first + " " + middle + " " + last;
        if(name.Length>24){
            Console.WriteLine("Please write less than 24 charecter name!");
            return NameCheck();
        }
        return (first, middle, last);
    } 
    public string IdCheck(){
        Console.Write("Student ID: ");
        string StudentId = Console.ReadLine();
        bool isNumber = int.TryParse(StudentId, out int numericValue);
        if(isNumber){
            string Json = File.ReadAllText(@"student.json");
            JArray JsonArray = JArray.Parse(Json);
            foreach(var std in JsonArray){
                if(StudentId == (string)std["id"]){
                    Console.WriteLine("Id already taken! Please enter an unique id.");
                    return IdCheck();
                    // break;
                }
            }
        }
        else{
            Console.WriteLine("Please enter Numeric value.");
            return IdCheck();
        }
        return StudentId;
    }
    public int Select(string msg, int option){
        int select; 
        try{
            Console.Write(msg);
            select = Int16.Parse(Console.ReadLine());
            if(select>option || select<0){
                Console.WriteLine("Please choose valid option");
                return Select(msg, option);
            }
        }
        catch(Exception){
            Console.WriteLine("Please enter numeric value");
            return Select(msg, option);
        }
        return select;
    }
}