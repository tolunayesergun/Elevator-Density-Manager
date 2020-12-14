using System.Collections.Generic;

namespace ElevatorDensityProject.Models
{
    internal class Elevator
    {
        public Elevator()
        {
            insideList = new List<Person>();
        }

        public int elevatorID { get; set; }
        public bool active { get; set; }
        public string mode { get; set; }
        public int floor { get; set; }
        public string direction { get; set; }
        public int capacity { get; set; }
        public int countInside { get; set; }
        public List<Person> insideList { get; set; }
    }
}