namespace Yandex.Taxi.Model.Rates.Services
{
    public enum ServiceType
    {
        /// <summary>
        ///     Базовая услуга перевозки
        /// </summary>
        Taximeter,

        /// <summary>
        ///     Платный выезд (отдельная плата за вызов машины)
        /// </summary>
        PaidDispatch,

        /// <summary>
        ///     Продолжение трансфера после конечной точки или начало трансфера перед формальной отправной точкой
        /// </summary>
        ContinueTransfer,

        /// <summary>
        ///     Ожидание клиента
        /// </summary>
        Waiting,

        /// <summary>
        ///     Кондиционер в машине
        /// </summary>
        Conditioner,

        /// <summary>
        ///     Некурящий водитель
        /// </summary>
        NoSmoking,

        /// <summary>
        ///     Детское кресло в машине
        /// </summary>
        ChildChair,

        /// <summary>
        ///     Подача машины с кузовом «универсал»
        /// </summary>
        Universal,

        /// <summary>
        ///     Перевозка животных
        /// </summary>
        AnimalTransport,

        /// <summary>
        ///     Дополнительная услуга, предоставляемая Службой Такси
        /// </summary>
        Other
    }
}