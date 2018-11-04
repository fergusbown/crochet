using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public static class CommandDiscovery
    {
        public static IReadOnlyDictionary<TCommandType, Type> FindCommands<TCommandAttribute, TCommandType>(Assembly assembly)
            where TCommandAttribute : Attribute
            where TCommandType : System.Enum
        {
            Dictionary<TCommandType, Type> result = new Dictionary<TCommandType, Type>();

            HashSet<string> searchAssemblies = new HashSet<string>();
            Stack<AssemblyName> pending = new Stack<AssemblyName>();

            searchAssemblies.Add(assembly.GetName().ToString());
            pending.Push(assembly.GetName());

            while (pending.Count > 0)
            {
                AssemblyName processingName = pending.Pop();

                Assembly processing = Assembly.Load(processingName);

                foreach (var assemblyName in processing.GetReferencedAssemblies())
                {
                    if (searchAssemblies.Add(assemblyName.ToString()))
                    {
                        pending.Push(assemblyName);
                    }
                }
            }

            foreach (Assembly searchAssembly in searchAssemblies.Select(a => Assembly.Load(a)))
            {
                foreach (Type type in searchAssembly.GetTypes())
                {
                    CustomAttributeData attribute = type.CustomAttributes.FirstOrDefault(ca => ca.AttributeType == typeof(TCommandAttribute));

                    if (attribute != null && attribute.ConstructorArguments != null && attribute.ConstructorArguments.Count > 0)
                    {
                        var argument = attribute.ConstructorArguments[0];

                        if (argument.ArgumentType == typeof(TCommandType))
                        {
                            result.Add((TCommandType)argument.Value, type);
                        }
                    }
                }
            }

            return result;
        }
    }
}
