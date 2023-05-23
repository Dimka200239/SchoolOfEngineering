using System;

namespace AbstractClassInstance
{
    public abstract class AbstractClass
    {
        private string _name;

        protected AbstractClass(string name)
        {
            this.Name = name;
        }

        public string Name { get { return _name; } set { _name = value; } }

        public abstract string GetName();
    }

    public class AbstractClassInheritor : AbstractClass
    {
        public override string GetName()
        {
            return this.Name;
        }

        public AbstractClassInheritor(string name)
            : base(name)
        {

        }
    }

    public static class Program
    {
        static void Main(string[] args)
        {
            // Первый вариант
            AbstractClass newAbstractClass = new AbstractClassInheritor("Это точно экземпляр абстрактного класса?");

            Console.WriteLine(newAbstractClass.GetName());

            // Второй вариант с помощью рефлексии
            Type type = typeof(AbstractClassInheritor);

            AbstractClass abstractClassInstance = (AbstractClass)Activator.CreateInstance(type, "Это точно экземпляр абстрактного класса?");

            Console.WriteLine(abstractClassInstance.GetName());
        }
    }
}
