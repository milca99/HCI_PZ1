using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat
{
    public class RMPlayer
    {
        int number;
        string firstAndLastName;
        string position;
        DateTime dateOfBirth;
        string image;
        string biographyFile;

        public RMPlayer() { }
        public RMPlayer(int number, string firstAndLastName, string position, DateTime dateOfBirth, string image, string biographyFile)
        {
            this.number = number;
            this.firstAndLastName = firstAndLastName;
            this.position = position;
            this.dateOfBirth = dateOfBirth;
            this.image = image;
            this.biographyFile = biographyFile;
        }

        public int Number { get => number; set => number = value; }
        public string FirstAndLastName { get => firstAndLastName; set => firstAndLastName = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Image { get => image; set => image = value; }
        public string BiographyFile { get => biographyFile; set => biographyFile = value; }
        public string Position { get => position; set => position = value; }
    }
}
