using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinnerSimulator.Common.Model.Move;
namespace DinnerSimulator.DiningRoom.Model.Actors
{
    public class RoomClerk : Position, IMove
    {
        RoomClerk(int posX, int posY) : base(posX, posY) { }

        public RoomClerk() :base() { }

        public void Move(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
        }
    }
}
