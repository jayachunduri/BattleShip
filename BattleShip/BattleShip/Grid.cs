using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    public enum PlaceShipDirection
    {
        Horizontal = 1,
        Vertical = 2
    }

    class Grid
    {
        //properties
        public string name;
        public static Point[,] Ocean { get; set; }
        public static List<Ship> ListOfShips { get; set; }
        public static bool AllShipsDestroyed {
            get
            {
                return ListOfShips.All(x => x.IsDistroyed);
            }
        }
        public static int CombatRound { get; set; }
        public static int Score { get; set; }

        public Grid()
        {
            //initialize ocean
            Ocean = new Point[10, 10];
            ListOfShips = new List<Ship>();
            Score = 100;

            //loop to initialize all points
            for (int x = 0; x < 10; x++) //x coordinate
            {
                for (int y = 0; y < 10; y++) //y coordinate
                {
                    Ocean[x, y] = new Point(x, y, PointStatus.Empty);
                }
            }

        }

        //Gree the user
        public void Greet()
        {
            Console.Write("Enter Your Name: ");
            name = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("\nWelcome to Battle Ship, " +name);
            Console.WriteLine("\ntype \"HELP\" for the introduction \n or Press enter to skip the introduction:");
            string temp = Console.ReadLine();

            if (temp == "")
                return;
            else if (temp.ToLower() == "help")
            {
                Console.WriteLine(@"
THE STORY: The enemies placed their ships in ocean.
We don't know where they are,
but we do know they placed 5 ships.
We need to destroy their ships. 

You have to choose x and y coordinates to hit.
It will display red mark, when it hits a ship.
It will display green mark, when it misses.

All the best!

    ");
            }
        }

        //method to place ships
        public void PlaceShip(Ship shipToPlace)
        {
            bool yesShip = false;
            int startx = 0, starty = 0;
            int direction = 0; 

            //need to generate a set of x-coordinate and y-coordinate, so that they are valid
            while(!yesShip)
            {
            Random rng = new Random();
            
            //random coordinates
            startx = rng.Next(0, 10);
            starty = rng.Next(0, 10);

            //random direction
            direction = rng.Next(1, 3);
            yesShip = CanPlaceShip(shipToPlace, (PlaceShipDirection)direction, startx, starty);
            }            

            //make sure there is no ship in that part of ocean
            for (int i = 0; i < shipToPlace.length; i++)
                {
                    //change the status of that point in ocean (from empty) to ship
                    Ocean[startx, starty].status = PointStatus.Ship;
                    //add that point to ship's occupaid points
                    shipToPlace.occupiedPoint.Add(Ocean[startx, starty]);

                    if ((PlaceShipDirection)direction == PlaceShipDirection.Horizontal)
                    {
                        startx++;
                    }
                    else
                        starty++;
                }
                //add that ship to list of ships in the ocean
                ListOfShips.Add(shipToPlace);
        }

        //method to display grid to user
        public static void DisplayOcean() 
        {
            //Console.WriteLine("***  ****");
            //Console.WriteLine("*  * *  *");
            //Console.WriteLine("*  * *  *");
            //Console.WriteLine("**** ****");
            //Console.WriteLine("*  * *  *");
            //Console.WriteLine("*  * *  *");
            //Console.WriteLine("**** *  *");
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("    0  1  2  3  4  5  6  7  8  9  X");
            Console.WriteLine("===================================");
            Console.ResetColor();

            //loop for y axis
            for (int y = 0; y < 10; y++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("{0}||", y);
                    Console.ResetColor();

                    for(int x=0; x < 10; x++)
                    {
                        if (Ocean[x, y].status == PointStatus.Empty || Ocean[x, y].status == PointStatus.Ship)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("[ ]");
                            Console.ResetColor();
                        }
                        else if (Ocean[x, y].status == PointStatus.Hit)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[X]");
                            Console.ResetColor();
                        }
                        else if (Ocean[x, y].status == PointStatus.Miss)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("[O]");
                            Console.ResetColor();
                        }
                    }
                    Console.ResetColor();
                   Console.WriteLine();
	            }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Y||");
                Console.ResetColor();
           }

        //determine the logic hits or misses
        public void Target(int x, int y)
        {
            if (Ocean[x, y].status == PointStatus.Ship)
            {
                Ocean[x, y].status = PointStatus.Hit;
                Score += 100;
            }
            else if(Ocean[x, y].status == PointStatus.Empty)
            {
                Ocean[x, y].status = PointStatus.Miss;
                Score -= 10;
            }
        }

        //logic for playing the game
        public void PlayGame()
        {
           while (!AllShipsDestroyed)
            {
                DisplayOcean();

                int x = -1, y = 10;
                //ask user to enter x and y coordinates
                while (x < 0 || x > 9)
                {
                    string xException = "";
                    while (xException == "" || !("0123456789".Contains(xException)))
                    {
                        Console.WriteLine("Enter X coordinate");
                        xException = Console.ReadLine();
                    }
                    //after xException gets a good value from user. Then pracing it to int and putting in x
                    x = int.Parse(xException);
                }

                while (y < 0 || y > 9)
                {
                    string yException = "";
                    while (yException == "" || !("0123456789".Contains(yException)))
                    {
                        Console.WriteLine("Enter Y coordinate");
                        yException = Console.ReadLine();
                    }
                    //after yException gets a good value from user. Then pracing it to int and putting in y
                    y = int.Parse(yException);
                }

                //now that we have good values in x and y, calling target function
                Score--;
                Target(x, y);
                CombatRound++;
               
            }
            DisplayOcean();
            Console.WriteLine("Congratulations you WON!");
            Console.WriteLine("It took " + CombatRound + " rounds to finish the game");
            Console.WriteLine("\n Your Score: " + Score);
            //Console.WriteLine("Press Enter to see 10 high scores");
            //Console.ReadKey();

            //Add high score to data base
            //AddHighScoreToDB();
            //DisplayHighScore();
        }

        //function returns TRUE if there is a ship in that part of ocean or reached end of ocean
        public bool CanPlaceShip(Ship shipToPlace, PlaceShipDirection direction, int startx, int starty)
        {
           //Make sure there is not already ship in that part of ocean
            //Also can't place a ship out of a ocean
            for (int i = 0; i < shipToPlace.length; i++) 
            {
                if (Ocean[startx, starty].status == PointStatus.Ship)
                    return false;

                if (direction == PlaceShipDirection.Vertical)
                {
                    starty++;
                    if (starty > 9)
                        return false;  
                }
                else if (direction == PlaceShipDirection.Horizontal)
                {
                    startx++;
                    if (startx > 9)
                        return false;
                }
               
            }
            return true;
 
        }

        //function to add high score to the data base
        //commented to place in portfolio
        //public static void AddHighScoreToDB()
        //{
        //    //create a connection to the data base
        //    JayaEntities db = new JayaEntities();

        //    //create an instance of class High Score(from data base table)
        //    HighScore currentScore = new HighScore();

        //    //added all information to current score object
        //    currentScore.DateCreated = DateTime.Now;
        //    currentScore.Game = "Battle Ship";
        //    currentScore.Score = Score;
        //    Console.Clear();
        //    Console.WriteLine("\n\nEnter your name");
        //    currentScore.Name = Console.ReadLine();

        //    //add it to the data base
        //    db.HighScores.Add(currentScore);

        //    //commit the changes to the data base
        //    db.SaveChanges();
        //}

        //function to get high score from the data base table HighScores and display to the user
        //Commented it to place in portfolio
        //public static void DisplayHighScore()
        //{
        //    //clear the console before displaying the high score
        //    Console.Clear();
        //    Console.WriteLine("Your score is: " + Score);
        //    Console.WriteLine("Battle Ship High Scores");
        //    Console.WriteLine("**********************");

        //    //create a connection to the data base
        //    JayaEntities db = new JayaEntities();

        //    //get the top high scores from data base table using db object
        //    //List<HighScore> highScoreList = db.HighScores.Where(x => x.Name == "Battle Ship").OrderByDescending(x => x.Score).Take(10).ToList();

        //    //Display that list to the user
        //    foreach (var item in db.HighScores.Where(x => x.Game == "Battle Ship").OrderByDescending(x => x.Score).Take(10))
        //    {
        //        //Console.WriteLine("{0}, {1} - {2}", highScoreList.IndexOf(highScore) + 1, highScore.Name, highScore.Score);
        //        Console.WriteLine(item.Name + " " + item.Score);
        //    }
        //}

        }

    }

