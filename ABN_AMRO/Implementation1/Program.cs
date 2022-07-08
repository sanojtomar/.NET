using System;
using System.Collections.Generic;

namespace Implementation1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Please enter your first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Please enter your last name: ");
            string lastName = Console.ReadLine();

            DoWork doWork = new DoWork();
            IList<string> listItems = doWork.Process(firstName, lastName);
            foreach (var item in listItems)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();

        }
    }

    public class DoWork
    {
        public IList<string>Process(string firstName, string lastName)
        {
            if(string.IsNullOrEmpty(firstName))
                throw new ArgumentException("firstName parameter is required", "firstName");
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("lastName parameter is required", "lastName");


            IList<string> items = new List<string>();

            for (int i = 1; i <= 100; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    items.Add(string.Format("{0} {1}", firstName, lastName));
                    continue;
                }

                if (i % 3 == 0)
                {
                    items.Add(firstName);
                    continue;
                }

                if (i % 5 == 0)
                {
                    items.Add(lastName);
                    continue;
                }

                items.Add(i.ToString());
            }
            return items;

        }
    }
}
