using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBot1
{
    public class Person
    {
        public string Name { get; set; }

        public string Expertise { get; set; }

        public string RoomNumber { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public Person(string name, string expertise, string roomNumber, string email, string phone)
        {
            Name = name;
            Expertise = expertise;
            RoomNumber = roomNumber;
            Email = email;
            Phone = phone;
        }
    }
}
