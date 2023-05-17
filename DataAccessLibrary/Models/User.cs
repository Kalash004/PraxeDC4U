using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Models
{
    public class User : IBaseClass
    {
        private int id;
        private string name;
        private string hashedPassword;
        private int currentCredits;

        public User(string name, string hashedPassword, int currentCredits)
        {
            Name = name;
            HashedPassword = hashedPassword;
            CurrentCredits = currentCredits;
        }

        public User(int iD, string name, string hashedPassword, int currentCredits)
        {
            ID = iD;
            Name = name;
            HashedPassword = hashedPassword;
            CurrentCredits = currentCredits;
        }

        public User(string name, string hashedPassword)
        {
            Name = name;
            HashedPassword = hashedPassword;
            CurrentCredits = 0;
        }

        public int ID { get { return id; } set { this.id = value; } }
        public string Name { get => name; set => name = value; }
        public string HashedPassword { get => hashedPassword; set => hashedPassword = value; }
        public int CurrentCredits { get => currentCredits; set => currentCredits = value; }


    }
}
