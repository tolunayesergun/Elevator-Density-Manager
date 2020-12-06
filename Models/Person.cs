using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorDensityProject.Models
{
    public class Person
    {
        public int personID { get; set; }
        public int currentFloor { get; set; }
        public int targetFloor { get; set; }
        public bool inLine { get; set; }
    }
}
