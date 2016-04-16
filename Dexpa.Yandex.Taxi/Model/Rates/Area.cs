using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public enum Area
    {
        City,
        Suburb,

        //Only Moscow Areas Further

        /// <summary>
        ///     МКАД
        /// </summary>
        [YAXEnum("mkad")] MKAD,

        /// <summary>
        ///     ЮАО
        /// </summary>
        [YAXEnum("sao")] SAO,

        /// <summary>
        ///     Зеленоградский АО
        /// </summary>
        [YAXEnum("zelenogradsky")] Zelenogradsky,

        /// <summary>
        ///     ЮВАО
        /// </summary>
        [YAXEnum("seao")] SEAO,

        /// <summary>
        ///     ЗАО
        /// </summary>
        [YAXEnum("wao")] WAO,

        /// <summary>
        ///     СЗАО
        /// </summary>
        [YAXEnum("nwao")] NWAO,

        /// <summary>
        ///     ЮЗАО
        /// </summary>
        [YAXEnum("swao")] SWAO,

        /// <summary>
        ///     САО
        /// </summary>
        [YAXEnum("nao")] NAO,

        /// <summary>
        ///     ВАО
        /// </summary>
        [YAXEnum("eao")] EAO,

        /// <summary>
        ///     Троицкий АО
        /// </summary>
        [YAXEnum("troitsky")] Troitsky,

        /// <summary>
        ///     Новомосковский АО
        /// </summary>
        [YAXEnum("novomoskovsky")] Novomoskovsky,

        /// <summary>
        ///     СВАО
        /// </summary>
        [YAXEnum("neao")] NEAO,

        /// <summary>
        ///     ЦАО
        /// </summary>
        [YAXEnum("cao")] CAO,

        /// <summary>
        ///     Ярославский вокзал
        /// </summary>
        [YAXEnum("yaroslavsky")] Yaroslavsky,

        /// <summary>
        ///     Павелецкий вокзал
        /// </summary>
        [YAXEnum("paveletsky")] Paveletsky,

        /// <summary>
        ///     Киевский вокзал
        /// </summary>
        [YAXEnum("kiyevsky")] Kiyevsky,

        /// <summary>
        ///     аэропорт Внуково
        /// </summary>
        [YAXEnum("vko")] VKO,

        /// <summary>
        ///     Курский вокзал
        /// </summary>
        [YAXEnum("kursky")] Kursky,

        /// <summary>
        ///     Рижский вокзал
        /// </summary>
        [YAXEnum("rizhsky")] Rizhsky,

        /// <summary>
        ///     Савёловский вокзал
        /// </summary>
        [YAXEnum("savyolovsky")] Savyolovsky,

        /// <summary>
        ///     Казанский вокзал
        /// </summary>
        [YAXEnum("kazansky")] Kazansky,

        /// <summary>
        ///     аэропорт Шереметьево
        /// </summary>
        [YAXEnum("svo")] SVO,

        /// <summary>
        ///     аэропорт Домодедово
        /// </summary>
        [YAXEnum("dme")] DME,

        /// <summary>
        ///     Белорусский вокзал
        /// </summary>
        [YAXEnum("belorussky")] Belorussky,

        /// <summary>
        ///     Ленинградский вокзал
        /// </summary>
        [YAXEnum("leningradsky")] Leningradsky
    }
}