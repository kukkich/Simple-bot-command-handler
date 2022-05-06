using System;
using System.Threading;

namespace TelegramTestBot
{

    class Program
    {
        static void Main()
        {
            var messageBot = new MessageTelegramBotClient("Insert your token", new CancellationTokenSource());
            messageBot.StartReceiving();

            Console.ReadLine();
        }
    }
}