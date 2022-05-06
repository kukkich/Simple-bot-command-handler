using System.Threading.Tasks;

namespace TelegramTestBot.Handlers
{
    public interface ICommandHanlder
    {
        public void Handle(string command, object[] parameters = null);
        public Task HandleAsync(string command, object[] parameters = null);
    }
}
