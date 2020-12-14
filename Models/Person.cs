namespace ElevatorDensityProject.Models
{
    public class Person
    {
        public int personID { get; set; }
        public int currentFloor { get; set; }
        public int targetFloor { get; set; }
        public int lineNumber { get; set; }
        public bool inLine { get; set; }
        public bool inStore { get; set; }
        public bool inElevator { get; set; }
    }
}