using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public enum CustomerStateEnum
    {
        WaitTableAttribution,
        WaitLineChief,
        Installed,
        Ordering,
        TableDispose,
        Ordered,
        WaitEntree,
        WaitPlate,
        WaitDessert,
        WaitBill,
        Leave
    }
}
