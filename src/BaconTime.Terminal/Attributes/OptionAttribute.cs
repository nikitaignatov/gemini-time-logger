using System;

namespace BaconTime.Terminal.Attributes
{
    public class OptionAttribute : Attribute
    {
        public string Option { get; }

        public OptionAttribute(string option)
        {
            Option = option;
        }
    }
}