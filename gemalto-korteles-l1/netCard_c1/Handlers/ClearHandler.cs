using System;

namespace MyCompany.MyClientApp
{
    public class ClearHandler : SimpleCommandBase
    {
        public override CommandHandlerType Type => CommandHandlerType.Clear;
        public override string Command => "clear";
        public override string HelpTextWithoutCommand => "clear terminal window";


        public override bool Process(string input)
        {
            Console.Clear();
            return true;
        }
    }
}