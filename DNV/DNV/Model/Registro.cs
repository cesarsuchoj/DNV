using System;
using System.Collections.Generic;

namespace DNVDO.Model.Data
{
    public class Registro
    {
        public Registro(int registry_Number, string book_Name, int book_Number, int pageNumber, 
                        string registry_Date, string name, string sex, string birthDate, string birthHour, 
                        string fatherName, string fatherBirthDate, string fatherBirthCity, string motherName, 
                        string motherBirthDate, string motherBirthCity, string docmentNumber, int isOnDeadline)
        {
            Registry_Number = registry_Number;
            Book_Name = book_Name;
            Book_Number = book_Number;
            Page_Number = pageNumber;
            Registry_Date = DateTime.Parse(registry_Date);
            Name = name;
            Sex = Sex.MALE.value.Equals(sex) ? Sex.MALE : Sex.FEMALE;
            BirthDate = DateTime.Parse(birthDate);
            BirthHour = DateTime.Parse(birthHour);
            FatherName = fatherName;
            FatherBirthDate = DateTime.Parse(fatherBirthDate);
            FatherAge = BusinessLogicalLayer.IdadePai(FatherBirthDate);
            FatherBirthCity = fatherBirthCity;
            MotherName = motherName;
            MotherBirthDate = DateTime.Parse(motherBirthDate);
            if (isOnDeadline == 1)
                BusinessLogicalLayer.IdadeMae(MotherBirthDate, Registry_Date, isOnDeadline == 1 ? true : false);
            MotherBirthCity = motherBirthCity;
            DocumentNumber = docmentNumber;
            IsOnDeadline = isOnDeadline.Equals(0) ? false : true;
        }

        public int Registry_Number { get; set; }
	    public string Book_Name { get; set; }
	    public int Book_Number { get; set; }
	    public int Page_Number { get; set; }
	    public DateTime Registry_Date { get; set; }
	    public string Name { get; set; }
	    public Sex Sex { get; set; }
	    public DateTime BirthDate { get; set; }
	    public DateTimeOffset BirthHour { get; set; }
	    public string FatherName { get; set; }
	    public DateTime FatherBirthDate { get; set; }
        public int FatherAge { get; set; }
	    public string FatherBirthCity { get; set; }
	    public string MotherName { get; set; }
	    public DateTime MotherBirthDate { get; set; }
        public int MotherAge { get; set; }
	    public string MotherBirthCity { get; set; }
	    public string DocumentNumber { get; set; }
	    public bool IsOnDeadline { get; set; }
    }
    public class Sex
    {
        public static readonly Sex MALE = new Sex("Masculino");
        public static readonly Sex FEMALE = new Sex("Feminino");

        public static IEnumerable<Sex> values
        {
            get
            {
                yield return MALE;
                yield return FEMALE;
            }
        }

        public Sex(string _value)
        {
            value = _value;
        }

        public string value { get; set; }
    }
    public class Books
    {
        public static readonly Books DNV = new Books("A");
        public static readonly Books DO = new Books("C AUX");

        public static IEnumerable<Books> values
        {
            get
            {
                yield return DNV;
                yield return DO;
            }
        }

        public Books(string _value)
        {
            value = _value;
        }
        public string value { get; set; }
    }
}