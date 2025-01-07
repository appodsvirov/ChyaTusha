using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChyaTusha
{
    public class MarkupBuilder
    {
        List<KeyboardButton> _buttons = new();
        public MarkupBuilder Add(string text)
        {
            _buttons.Add(new KeyboardButton(text));
            return this;
        }

        public MarkupBuilder AddRange(string[] texts)
        {
            _buttons.AddRange(texts.Select(t => new KeyboardButton(t)));
            return this;
        }

        public MarkupBuilder InsertAtStart(string text)
        {
            _buttons.Insert(0, text);
            return this;
        }

        public IReplyMarkup Build()
        {
            _buttons.ToArray();
            return new ReplyKeyboardMarkup(_buttons)
            {
                ResizeKeyboard = true
            };
        }
    }
}
