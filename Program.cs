using System;
using System.Collections.Generic;

namespace RogueThing
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            string[,] world = initWorld(200,200, player);

            Console.Clear();
            renderWorld(world, player);
            

            //Game loop
            ConsoleKeyInfo input;

            while(input.Key != ConsoleKey.Escape) {
                input = Console.ReadKey(true);
                //Read inputs
                switch(input.Key) {
                    case ConsoleKey.LeftArrow:
                        Console.Clear();
                        player.moveLeft(world);
                        renderWorld(world,player);
                    break;
                    case ConsoleKey.RightArrow:
                        Console.Clear();
                        player.moveRight(world);
                        renderWorld(world,player);
                    break;
                    case ConsoleKey.UpArrow:
                        Console.Clear();
                        player.moveUp(world);
                        renderWorld(world,player);
                    break;
                    case ConsoleKey.DownArrow:
                        Console.Clear();
                        player.moveDown(world);
                        renderWorld(world,player);

                    break;
                }
                System.Console.WriteLine($"Player position: {player.x}, {player.y}");
                System.Console.WriteLine();
                System.Console.WriteLine($"DOLLARS: {player.dollars}");
                System.Console.WriteLine();
                System.Console.WriteLine("ACTIVITY LOG:");
                for(var log = Math.Max(0,player.log.Count-4);log<player.log.Count;log++) {
                    System.Console.WriteLine(player.log[log]);
                }
            }
        }

        //initWorld
        public static string[,] initWorld(int width, int height, Player player) {
            string[,] world = new string[width, height];
            Random rand = new Random();
            for(var j=0;j<height;j++) {
                for(var i=0; i<width;i++) {
                    if(j == 0 || j == height-1) {
                        world[i,j] = "#";
                    } else
                    if(i == 0 || i == width-1) {
                        world[i,j] = "#";
                    } else {
                        int thing = rand.Next(100);
                        if(thing < 10) {
                            world[i,j] = "~";
                        } else
                        if(thing > 10 && thing < 12) {
                            world[i,j] = "#"; //solid
                        } else
                        if(thing > 15 && thing < 18) {
                            world[i,j] = "A";
                        } else
                        if(thing == 99) {
                            world[i,j] = "$"; // monies
                        } else {
                            world[i,j] = "."; //empty
                        }
                    }
                }
                player.x = width/2;
                player.y = height/2;
                world[player.x, player.y] = "@";
                


                //Create rooms
                List<Room> roomList = new List<Room>();

                //Create starting room

                roomList.Add(new Room(world,player.x-8,player.y-8,16,16));

                // for(var ry=player.y-8;ry<player.y+9;ry++) {
                //     for(var rx=player.x-8;rx<player.x+9;rx++) {
                //         if(ry == player.y-8 || ry == player.y+8) {
                //             world[rx,ry] = "#";
                //         } 
                //         if(rx == player.x-8 || rx == player.x+8) {
                //             if(Math.Abs((ry-player.y)) > 3) {
                //                 world[rx,ry] = "#";
                //             }
                //         }
                //     }
                // }
                for(var rooms=0;rooms<rand.Next(3,4);rooms++) {
                    createRoom(world, roomList);
                }

            }
            return world;
        }

        public static void createRoom(string[,] world, List<Room> rooms, int type = 0) {
            switch(type) {
                //Regular room
                case 0:        
                    rooms.Add(new Room(world,rooms));
                break;
            }
        }

        public class Room {
            public int x;
            public int y;
            public int width;
            public int height;
            public int entrance;
            Random rand = new Random();

            //Regular room constructor
            public Room(string[,] world, List<Room> roomList) {

                //Check for collisions in list
                bool collision = true;
                while(collision) {
                    collision = false;

                    //Get random size
                    width = rand.Next(8,14);
                    height = rand.Next(8,14);

                    //Get coordinate of room
                    x = rand.Next(2,200-width);
                    y = rand.Next(2,200-height);

                    foreach(Room room in roomList) {
                        //Check rooms in list for collision
                        if( x < room.x + room.width &&
                            x + width > room.x &&
                            y < room.y + room.height &&
                            y + height > room.y) {
                            collision = true;
                        }
                    }
                }
                

                

                //Create Room
                for(var ry = y; ry < y+height; ry++) {
                    for(var rx = x; rx < x+width; rx++) {
                        if(ry == y || ry == y+height-1) {
                            //Vertical walls
                            world[rx,ry] = "#";
                        } else
                        if(rx == x || rx == x+width-1) {
                            //Horizontal walls
                            world[rx,ry] = "#";
                        } else {
                            int objChance = rand.Next(100);
                            if(objChance < 10) {
                                world[rx,ry] = "~"; //grass
                            } else
                            if(objChance > 10 && objChance < 12) {
                                world[rx,ry] = "."; //block
                            } else 
                            if(objChance == 99) {
                                world[rx,ry] = "$"; //monies
                            } else {
                                world[rx,ry] = "."; //blank
                            }
                        }
                    }
                }
                entrance = rand.Next(3);
                switch(entrance) {
                    case 0: //left
                        world[x,y+(height/2)] = ".";
                        world[x,y+(height/2)+1] = ".";
                    break;
                    case 1: //right
                        world[x+width-1,y+(height/2)] = ".";
                        world[x+width-1,y+(height/2)+1] = ".";
                    break;
                    case 2: //up
                        world[x+(width/2),y] = ".";
                        world[x+(width/2)+1,y] = ".";
                    break;
                    case 3: //down
                        world[x+(width/2),y+height-1] = ".";
                        world[x+(width/2)+1,y+height-1] = ".";
                    break;
                }
            }

            //Starting room Constructor
            public Room(string[,] world, int x, int y, int width, int height) {
                for(var ry = y; ry < y+height; ry++) {
                    for(var rx = x; rx < x+width; rx++) {
                        if(ry == y || ry == y+height-1) {
                            world[rx,ry] = "#";
                        } 
                        if(rx == x || rx == x+width-1) {
                            if(Math.Abs((ry-y+8)) > 3) {
                                world[rx,ry] = ".";
                            }
                        }
                    }
                }
            }
        }

        //Render world
        public static void renderWorld(string[,] world, Player player) {
            int viewWidth = 20;
            int viewHeight = 20;

            for(var j=Math.Max(0,player.y-(viewHeight/2));j<Math.Min(world.GetLength(1),player.y+(viewHeight/2));j++) {
                // string line = "";
                for(var i=Math.Max(0,player.x-(viewWidth/2));i<Math.Min(world.GetLength(0),player.x+(viewWidth/2));i++) {
                    //Check distance between player and view radius
                    double distance = Math.Sqrt(Math.Pow((i-player.x+1),2)+Math.Pow((j-player.y),2));
                    if(distance >= 9) {
                        // line += "  %  ";
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  %  ");
                    } else {
                        // line += "  "+world[i,j]+"  ";
                        //Reset background to black;
                        switch(world[i,j]) {
                            case "@":
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                            case "~":
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                            case "A":
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                            case ".":
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            break;
                            case "#":
                                Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                            case "$":
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                            default:
                                Console.ForegroundColor = ConsoleColor.White;
                            break;
                        }
                        Console.Write("  "+world[i,j]+"  ");
                    }
                }
                // System.Console.WriteLine(line);
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            
            Console.ForegroundColor = ConsoleColor.White;
        }

        public class Player {
            public int x;
            public int y;
            string prevTile = ".";
            public int dollars = 0;
            public List<string> log = new List<string>();

            public void moveLeft(string[,] world) {
                if(world[x-1,y] != "#") {
                    world[x,y] = prevTile;
                    x--;
                    prevTile = world[x,y];
                    //Check for dollars
                    if(prevTile == "$") {
                        prevTile = ".";
                        foundDollars();
                    }
                    world[x,y] = "@";
                }
            }

            public void moveRight(string[,] world) {
                if(world[x+1,y] != "#") {
                    world[x,y] = prevTile;
                    x++;
                    prevTile = world[x,y];
                    //Check for dollars
                    if(prevTile == "$") {
                        prevTile = ".";
                        foundDollars();
                    }
                    world[x,y] = "@";
                }
            }

            public void moveUp(string[,] world) {
                if(world[x,y-1] != "#") {
                    world[x,y] = prevTile;
                    y--;
                    prevTile = world[x,y];
                    //Check for dollars
                    if(prevTile == "$") {
                        prevTile = ".";
                        foundDollars();
                    }
                    world[x,y] = "@";
                }
            }

            public void moveDown(string[,] world) {
                if(world[x,y+1] != "#") {
                    world[x,y] = prevTile;
                    y++;
                    prevTile = world[x,y];
                    //Check for dollars
                    if(prevTile == "$") {
                        prevTile = ".";
                        foundDollars();
                    }
                    world[x,y] = "@";
                }
            }

            public void foundDollars() {
                Random rand = new Random();
                int amt = rand.Next(1,20);
                dollars += amt;
                log.Add($"You found {amt} dollar(s)...nice! You now have {dollars} dollars");
            }
        }
    }
}
