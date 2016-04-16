using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public enum RequirementType
    {
        [YAXEnum("animal_transport")]
        AnimalTransport,
        [YAXEnum("check")]
        Check,
        [YAXEnum("child_chair")]
        ChildChair,
        [YAXEnum("has_conditioner")]
        HasConditioner,
        [YAXEnum("no_smoking")]
        NoSmoking,
        [YAXEnum("universal")]
        Universal,
        [YAXEnum("yamoney")]
        YandexMoney,
        [YAXEnum("coupon")]
        Coupon
    }
}
