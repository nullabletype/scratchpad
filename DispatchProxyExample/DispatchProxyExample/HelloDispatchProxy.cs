using System.Reflection;

namespace DispatchProxyExample
{
    class HelloDispatchProxy<T> : DispatchProxy where T : class, IHello
    {
        private IHello Target { get; set; }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            // Commented out to give a fair test
            //System.Console.WriteLine($"Going to call {targetMethod.Name} with args ");

            var result = targetMethod.Invoke(Target, args);

            // Commented out to give a fair test
            //System.Console.WriteLine($"Done calling {targetMethod.Name} with args {string.Join(", ", args)}. Response was {result}");

            return result;
        }

        public static T CreateProxy(T target)
        {
            var proxy = Create<T, HelloDispatchProxy<T>>() as HelloDispatchProxy<T>;
            proxy.Target = target;
            return proxy as T;
        }
    }
}
