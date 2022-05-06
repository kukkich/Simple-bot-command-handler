using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TelegramTestBot.Handlers
{
    public class ReflectionCommandHandler : ICommandHanlder
    {
        private readonly object _target;
        private readonly Type _targetType;
        private readonly MethodInfo[] _methods;
        private static readonly Type _atrributeType = typeof(CommandAttribute);

        public ReflectionCommandHandler(object target)
        {
            _target = target;
            _targetType = _target.GetType();
            _methods = _targetType.GetMethods()
                .Where(method => method.GetCustomAttribute(_atrributeType) != null)
                .ToArray();
        }

        public void Handle(string command, object[] parameters = null)
        {

            MethodInfo method = _methods.FirstOrDefault(method =>
            {
                var attribute = method.GetCustomAttribute(_atrributeType);
                if (attribute is CommandAttribute commandAttribute)
                    return ((CommandAttribute)attribute).Name == command;
                else
                    return false;
            });
            if (!(method is null))
                method.Invoke(_target, parameters);
        }

        public async Task HandleAsync(string command, object[] parameters = null)
        {
            var method = _methods.FirstOrDefault(method =>
            {
                var attribute = method.GetCustomAttribute(_atrributeType);
                if (attribute is CommandAttribute commandAttribute)
                    return ((CommandAttribute)attribute).Name == command;
                else
                    return false;
            });

            if (!(method is null))
                await Task.Run(() => method.Invoke(_target, parameters));
        }
    }
}
