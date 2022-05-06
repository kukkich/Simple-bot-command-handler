using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramTestBot.Handlers;

namespace TelegramTestBot
{
    public class MessageTelegramBotClient
    {
        private readonly ITelegramBotClient _bot;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ICommandHanlder _commandHanlder;

        public MessageTelegramBotClient(string token, CancellationTokenSource cancellationTokenSource)
        {
            _bot = new TelegramBotClient(token);
            _cancellationTokenSource = cancellationTokenSource;
            _cancellationToken = cancellationTokenSource.Token;

            _commandHanlder = new ReflectionCommandHandler(this);
        }

        public void StartReceiving()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };

            _bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                _cancellationToken
            );
        }

        [Command("/start")]
        public async Task OnStart(Message message)
        {
            await _bot.SendTextMessageAsync(message.Chat, $"{message.From.Username}, Вы начали нереальную работу с ботом");
        }

        public void Cancel()
        {
            if (_cancellationToken.CanBeCanceled)
                _cancellationTokenSource.Cancel();
            else
                throw new InvalidOperationException();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(update, Formatting.Indented));

            if (update.Type == UpdateType.Message)
                await TextMessageHandler(botClient, update.Message);
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }

        private async Task TextMessageHandler(ITelegramBotClient botClient, Message message)
        {
            await _commandHanlder.HandleAsync(message.Text.ToLower(), new object[] { message });
        }
    }
}
