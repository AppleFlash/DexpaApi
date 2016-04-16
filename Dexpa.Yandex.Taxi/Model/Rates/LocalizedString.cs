using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public class LocalizedString
    {
        [YAXAttributeForClass]
        [YAXSerializeAs("Lang")]
        public Language Language { get; set; }

        [YAXValueForClass]
        public string Value { get; set; }
    }

    public enum Language
    {
        [YAXEnum("en")] English,
        [YAXEnum("ru")] Russian
    }
}