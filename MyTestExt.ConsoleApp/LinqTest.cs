using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTestExt.ConsoleApp
{
    class LinqTest
    {
        public static void Test()
        {

            var persons = new List<dynamic>()
            {
                new {FirstName="Zhang",LastName="San"},
                new {FirstName="Li",LastName="Si"},
                new {FirstName="Wang",LastName="Wu"},
                new {FirstName="Zhao",LastName="Liu"},
            };
            var pets = new List<dynamic>()         { 
                new {PetName="Cat",OwnerName="Zhang"},
                new {PetName="Dog",OwnerName="Si"},
                new {PetName="Monkey",OwnerName="Wang"},
                new {PetName="Panda",OwnerName="Liu"},
                new {PetName="King Kong",OwnerName=""}
            };

            var q = from e in persons
                    from c in pets
                    where e.FirstName == c.OwnerName
                       || e.LastName == c.OwnerName
                    select new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        PetName = c.PetName,
                        OwnerName = c.OwnerName,
                    };

            StringBuilder aa = new StringBuilder();
            foreach (var item in q)
            {
                aa.AppendLine(string.Format("FirstName:{0}, \tLastName:{1}, PetName:{2}, OwnerName:{3}", item.FirstName, item.LastName, item.PetName, item.OwnerName));
            }

        }
    }

    class Pet
    {
        public string PetName { get; set; }
        public string OwnerName { get; set; }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
