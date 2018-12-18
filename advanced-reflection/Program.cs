#define Address
#undef Price
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace advanced_reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Creator: Felipe Bossolani - fbossolani[at]gmail.com");
            Console.WriteLine(@"Examples based on: http://returnsmart.blogspot.com/2015/08/mcsd-programming-in-c-part-8-70-483.html");
            Console.WriteLine("Choose a Thread Method: ");
            Console.WriteLine("01- Serializable/Conditional Attribute - Reflection");
            Console.WriteLine("02- Custom Attribute");
            Console.WriteLine("03- Generate Code at Runtime");
            Console.WriteLine("04- ");
            Console.WriteLine("05- ");

            int option = 0;
            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        House.Init();                        
                        break;
                    }
                case 2:
                    {
                        var d = new Developer();
                        d.GetAttributes();
                        break;
                    }
                case 3:
                    {
                        GenerateCode();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid option...");
                        break;
                    }
            }
        }

        private static void GenerateCode()
        {
            BlockExpression blockExpression = Expression.Block(
                Expression.Call(
                        null, 
                        typeof(Console).GetMethod("Write", new Type[] { typeof(String) }),
                        Expression.Constant("Hello ")
                    ),
                Expression.Call(
                        null,
                        typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String)} ), 
                        Expression.Constant("World!")
                    )
                );

            Expression.Lambda<Action>(blockExpression).Compile()();
        }

        /// <summary>
        /// Category can be used for both classes and methods.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
        public class CategoryAttribute : Attribute
        {
            public string Name { get; set; }

            public CategoryAttribute(string value) 
                : base() {
                this.Name = value;
            }
        }

        public class UnitTestAttribute : CategoryAttribute
        {
            public UnitTestAttribute() : base("Unit Test") { }
        }

        [Category("Human")]
        [Category("Person")]
        [UnitTest]
        class Developer
        {
            public void GetAttributes()
            {
                foreach(var a in Attribute.GetCustomAttributes(typeof(Developer), typeof(CategoryAttribute))){
                    Console.WriteLine($"Attribute name: {((CategoryAttribute)a).Name}");
                }
            }
        }

        [Serializable]
        class House
        {
            public static void Init()
            {
                if (Attribute.IsDefined(typeof(House), typeof(SerializableAttribute)))
                {
                    Console.WriteLine("I'm serializable");
                }
                else
                {
                    Console.WriteLine("I am not serailizable");
                }

                PrintAddress();
                PrintPrice();
            }

            [Conditional("Address")]
            static void PrintAddress()
            {
                Console.WriteLine("My address goes here.");
            }

            [Conditional("Price")]
            static void PrintPrice()
            {
                Console.WriteLine("My price goes here.");
            }
        }
    }
}
