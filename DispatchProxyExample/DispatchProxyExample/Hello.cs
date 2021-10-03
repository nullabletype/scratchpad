using System;

namespace DispatchProxyExample
{
    class Hello : IHello
    {
        public bool SayHello(string name)
        {
            Console.WriteLine($"Hello {name}");
            return true;
        }
    }
}
