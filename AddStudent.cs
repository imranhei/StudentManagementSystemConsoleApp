using Newtonsoft.Json.Linq;
public class Student{

    public string fName{ get; set; } = default!;
    public string mName{ get; set; } = default!;
    public string lName{ get; set; } = default!;
    public string id{ get; set; } = default!;
    public string batch{ get; set; } = default!;
    public string department{ get; set; } = default!;
    public string degree{ get; set; } = default!;
    public JArray semester{ get; set; } = default!;
}