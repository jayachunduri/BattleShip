using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid();
            //int userChoice = 0;

            //Greet the user
            grid.Greet();

            //build 5 ships
            Ship ship1 = new Ship(ShipType.Submarine);
            Ship ship2 = new Ship(ShipType.Minesweeper);
            Ship ship3 = new Ship(ShipType.Cruiser);
            Ship ship4 = new Ship(ShipType.Carrier);
            Ship ship5 = new Ship(ShipType.Battleship);

            //place the ships in ocean
            grid.PlaceShip(ship1);
            grid.PlaceShip(ship2);
            grid.PlaceShip(ship3);
            grid.PlaceShip(ship4);
            grid.PlaceShip(ship5);

            //Console.WriteLine("Enter 1 to play by yourself\nEnter 2 for computer to play");
            //userChoice = int.Parse(Console.ReadLine());

            //switch (userChoice)
            //{
            //    case 1:
            grid.PlayGame();
               // case 2:

        }

        

    }
}
