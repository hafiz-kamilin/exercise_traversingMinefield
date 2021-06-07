using System;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

/*
    @author - Mohd Hafizuddin Bin Kamilin
    @date - 2 June 2021
*/

namespace q3_minefield_cs
{
    public static class Program
    {
        // for generating a pseudo-random number
        public static readonly Random rd = new();
        // for multiplying the string via multiplication
        public static string Multiply(this string source, int multiplier)
        {
            StringBuilder sb = new(multiplier * source.Length);
            for (int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        // class for generating a safe path to traverse via randomized single branch (path) depth first search (dfs)
        class SafePathGenerator
        {
            // get the traversal area where n (length) and m (height) is the max bounding area
            public int n;
            public int m;
            // method to create a path from south to north 
            // (assuming the starting location for totoshka and ally is at the south side of the map)
            public int[,] CreateNow()
            {
                // initialize the 2d array for storing the totoshka's route, ally, safe path and mine position on the minefield map
                var minefieldMap = new int[m, n];
                // fill the array with -1
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        minefieldMap[i, j] = -1;
                    }
                }
                // for storing the traversed nodes
                var traversedNodes = new ArrayList();
                //consider that counting the rows and columns start from zero
                int column = n - 1;
                int row = m - 1;
                // randomize the position of the root node (currentNode[0] = column, currentNode[1] = row)
                int[] currentNode = new int[2] { rd.Next(0, n), row };
                // add the randomly picked node to traversedNode
                traversedNodes.Add(currentNode);
                // because we only want to create a tree with a single branch (in other word, a path), we don't use recursive for the dfs
                while (true)
                {
                    // keep on randomizing the direction until a traversable path is found
                    while (true)
                    {
                        /*
                            NOTE: because we want to traverse from south to north, we ommited the movement to
                            south-east, south and south-west 

                              --  x-  +-      ↖  ↑  ↗
                              -y  xy  +y      ←      → 
                              -+  x+  ++      ↙  ↓  ↘     } movements in this section is ommited

                            randomly pick the branch direction with bias
                            north          0 <= α < 30      30%
                            east          30 <= α < 45      15%
                            north-east    45 <= α < 65      20%
                            west          65 <= α < 80      15%
                            north-west    80 <= α < 100     20%
                        */
                        int randomizedDirection = rd.Next(0, 100);
                        // check if it can traverse to north
                        if (((0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row)) && ((0 <= randomizedDirection) && (randomizedDirection < 30)))
                        {
                            var temp = new int[] { currentNode[0], currentNode[1] - 1 };
                            // if the newly generated position for the node to traverse have not yet been traversed
                            if (traversedNodes.Contains(temp) is false)
                            {
                                currentNode = temp;
                                traversedNodes.Add(temp);
                                break;
                            }
                        }
                        // check if it can traverse to east
                        else if (((0 <= currentNode[0] + 1) && (currentNode[0] + 1 <= column)) && ((30 <= randomizedDirection) && (randomizedDirection < 45)))
                        {
                            var temp = new int[] { currentNode[0] + 1, currentNode[1] };
                            // if the newly generated position for the node to traverse have not yet been traversed
                            if (traversedNodes.Contains(temp) is false)
                            {
                                currentNode = temp;
                                traversedNodes.Add(temp);
                                break;
                            }
                        }
                        // check if it can traverse to north-east
                        else if (((0 <= currentNode[0] + 1) && (currentNode[0] + 1 <= column)) && ((0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row)) && ((45 <= randomizedDirection) && (randomizedDirection < 65)))
                        {
                            var temp = new int[] { currentNode[0] + 1, currentNode[1] - 1 };
                            // if the newly generated position for the node to traverse have not yet been traversed
                            if (traversedNodes.Contains(temp) is false)
                            {
                                currentNode = temp;
                                traversedNodes.Add(temp);
                                break;
                            }
                        }
                        // check if it can traverse to west
                        else if (((0 <= currentNode[0] - 1) && (currentNode[0] - 1 <= row)) && ((65 <= randomizedDirection) && (randomizedDirection < 80)))
                        {
                            var temp = new int[] { currentNode[0] - 1, currentNode[1] };
                            // if the newly generated position for the node to traverse have not yet been traversed
                            if (traversedNodes.Contains(temp) is false)
                            {
                                currentNode = temp;
                                traversedNodes.Add(temp);
                                break;
                            }
                        }
                        else if (((0 <= currentNode[0] - 1) && (currentNode[0] - 1 <= column)) && ((0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row)) && ((80 <= randomizedDirection) && ((randomizedDirection < 100))))
                        {
                            var temp = new int[] { currentNode[0] - 1, currentNode[1] - 1 };
                            // if the newly generated position for the node to traverse have not yet been traversed
                            if (traversedNodes.Contains(temp) is false)
                            {
                                currentNode = temp;
                                traversedNodes.Add(temp);
                                break;
                            }
                        }

                    }
                    // if the traversed node is the final node
                    if (currentNode[1] == 0)
                    {
                        break;
                    }
                }
                // for every traversed path (representing the xy-axis of the safe path)
                foreach (int[] node in traversedNodes)
                {
                    // mark it on minefieldMap where -1 replaced with 0
                    minefieldMap[(int)node[1], (int)node[0]] = 0;

                }
                // two choice to pick, empty field (0) or a bomb (2)
                var emptyOrBomb = new List<int> { 0, 2 };
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (minefieldMap[i, j] == -1)
                        {
                            int picked = rd.Next(0, emptyOrBomb.Count);
                            // replace -1 with either 0 or 2
                            minefieldMap[i, j] = emptyOrBomb[picked];
                        }
                    }
                }
                return minefieldMap;
            }
        }
        class DisplayMinefield
        {
            /*
                empty field = 0
                √ = totoshka's route = 1
                X = bomb = 2
                (^_^) = ally = 3
                \(°ᴥ°)ﾉ = totoshka = 4   

                topview of the minefield
                +---------+----
                |         |    
                +---------+----
                |         |
            */
            // get the traversal area where n (length) and m (height) is the max bounding area
            public int n;
            public int m;
            // method for translating totoshka's starting position into an ascii art
            public void DrawTotoshka(int totoshkaStartingPosition)
            {
                // ascii art string output
                string displayContent = "";

                for (int i = 0; i < n; i++)
                {
                    // totoshka start at here
                    if (i == totoshkaStartingPosition)
                    {
                        displayContent += "  \\(°ᴥ°)ﾉ ";
                    }
                    // just an empty spacing
                    else
                    {
                        displayContent += "          ";
                    }
                }
                Console.WriteLine(displayContent);
            }
            // method for translating ally's starting position into an ascii art
            public void DrawAlly(int allyStartingPosition)
            {
                // ascii art string output
                string displayContent = "";

                for (int i = 0; i < n; i++)
                {
                    // ally start at here
                    if (i == allyStartingPosition)
                    {
                        displayContent += "   (^_^)  ";
                    }
                    // just an empty spacing
                    else
                    {
                        displayContent += "          ";
                    }
                }
                Console.WriteLine(displayContent);
            }
            // method for translating the totoshka's route, ally, safe path and mine position into an ascii art
            public static string DrawSymbol(int input)
            {
                // initialize the output string
                string output;
                // # empty field = 0, totoshka's route = 1, bomb = 2, ally = 3, totoshka
                if (input == 0)
                {
                    output = "         ";
                }
                else if (input == 1)
                {
                    output = "    √    ";
                }
                else if (input == 2)
                {
                    output = "    X    ";
                }
                else if (input == 3)
                {
                    output = "  (^_^)  ";
                }
                else
                {
                    output = " \\(°ᴥ°)ﾉ ";
                }
                return output;
            }
            // method for drawing the DisplayMinefield map
            public void DrawNow(int[,] minefieldMap)
            {
                // draw the bounding box
                for (int i = 0; i < m; i++)
                {
                    string displayContent0 = "+" + "---------+".Multiply(n);
                    Console.WriteLine(displayContent0);
                    string displayContent1 = "|";
                    for (int j = 0; j < n; j++)
                    {
                        displayContent1 += DrawSymbol(minefieldMap[i, j]) + "|";
                    }
                    Console.WriteLine(displayContent1);
                    // NOTE: dirty way to show the final line because displayContent0 got deleted after the i for-loop
                    if (i == m - 1)
                    {
                        Console.WriteLine(displayContent0);
                    }
                }
            }
        }
        class TraversingMinefield
        {
            // get the traversal area where n (length) and m (height) is the max bounding area
            public int n;
            public int m;
            // get minefieldMap
            public int[,] minefieldMap;
            // recording the number of steps taken to reach the destination
            public int stepTaken = 0;
            // position of totoshka and ally when standing outside of the minefield perimeter
            public int outsidePosition = 0;
            // recursiveDFS's markers
            static bool destinationReached = false;
            // path traversed by totoshka
            static readonly List<int[]> _totoshkaTraversedPath = new();
            // method to update the current location of totoshka and ally on the map
            public void UpdateMinefieldMap()
            {
                // for drawing the updated map later
                var dmfNew = new DisplayMinefield()
                {
                    n = n,
                    m = m,
                };
                // sleep for 2 second
                Thread.Sleep(2000);
                stepTaken += 1;
                // for each row
                for (int i = 0; i < m; i++)
                {
                    // for each column
                    for (int j = 0; j < n; j++)
                    {
                        // if the array location is not empty or has a bomb
                        if ((minefieldMap[i, j] != 0) && (minefieldMap[i, j] != 2))
                        {
                            // reset the array back to 0
                            minefieldMap[i, j] = 0;
                        }
                    }
                }
                if (destinationReached is false)
                {
                    int pathLength = _totoshkaTraversedPath.Count;
                    if (pathLength >= 2)
                    {
                        Console.WriteLine("\nStep " + stepTaken.ToString() + ":");
                        // get the current location of totoshka and ally
                        int[] totoshkaLocation = _totoshkaTraversedPath[^1];
                        int[] allyLocation = _totoshkaTraversedPath[^2];
                        // loop for each node in totoshkaTraversedPath except for the last 2 nodes
                        int iteration = 0;
                        foreach (int[] node in _totoshkaTraversedPath)
                        {
                            if (iteration < _totoshkaTraversedPath.Count - 2)
                            {
                                // mark the field passed by totoshka to reach the destination on the minefieldMap
                                minefieldMap[(int)node[1], (int)node[0]] = 1;
                            }
                            iteration++;
                        }
                        // mark the empty field with the current location of totoshka and ally
                        minefieldMap[allyLocation[1], allyLocation[0]] = 3;
                        minefieldMap[totoshkaLocation[1], totoshkaLocation[0]] = 4;
                        // draw the map
                        dmfNew.DrawNow(minefieldMap);
                    }
                    else if (pathLength == 1)
                    {
                        Console.WriteLine("\nStep " + stepTaken.ToString() + ":");
                        // get the current location of totoshka and ally
                        int[] totoshkaLocation = _totoshkaTraversedPath[0];
                        // for each element in list (except the last element)
                        int iteration = 0;
                        foreach (int[] node in _totoshkaTraversedPath)
                        {
                            if (iteration < _totoshkaTraversedPath.Count - 1)
                            {
                                // mark the field passed by totoshka to reach the destination on the self.minefieldMap
                                minefieldMap[(int)node[1], (int)node[0]] = 1;
                            }
                            iteration++;
                        }
                        // mark the empty field with the current location of totoshka
                        minefieldMap[totoshkaLocation[1], totoshkaLocation[0]] = 4;
                        // draw the map
                        dmfNew.DrawNow(minefieldMap);
                        dmfNew.DrawAlly(totoshkaLocation[0]);
                    }
                    else
                    {
                        Console.WriteLine("\nStep " + stepTaken.ToString() + ":");
                        dmfNew.DrawNow(minefieldMap);
                        dmfNew.DrawTotoshka(outsidePosition);
                        dmfNew.DrawAlly(outsidePosition);
                    }
                }
                else if (destinationReached is true)
                {
                    Console.WriteLine("\nStep " + stepTaken.ToString() + ":");
                    int[] totoshkaLocation = _totoshkaTraversedPath[^1];
                    // NOTE: displaying the last 2 steps
                    // for each element in list (except the last element)
                    int iteration = 0;
                    foreach (int[] node in _totoshkaTraversedPath)
                    {
                        if (iteration < _totoshkaTraversedPath.Count - 1)
                        {
                            // mark the field passed by totoshka to reach the destination on the self.minefieldMap
                            minefieldMap[(int)node[1], (int)node[0]] = 1;
                        }
                        iteration++;
                    }
                    // mark the empty field with the current location of ally
                    minefieldMap[totoshkaLocation[1], totoshkaLocation[0]] = 3;
                    // draw the map
                    dmfNew.DrawTotoshka(totoshkaLocation[0]);
                    dmfNew.DrawNow(minefieldMap);
                    // NOTE: displaying the last step
                    stepTaken += 1;
                    Thread.Sleep(2000);
                    Console.WriteLine("\nStep " + (stepTaken).ToString() + ":");
                    foreach (int[] node in _totoshkaTraversedPath)
                    {
                        // mark the field passed by totoshka to reach the destination on the self.minefieldMap
                        minefieldMap[(int)node[1], (int)node[0]] = 1;
                    }
                    // draw the map
                    dmfNew.DrawTotoshka(totoshkaLocation[0]);
                    dmfNew.DrawAlly(totoshkaLocation[0]);
                    dmfNew.DrawNow(minefieldMap);
                }
            }
            // method to perform the recursive dfs method for finding the safe route
            public void RecursiveDFS(int[] currentNode)
            {
                // consider that counting the rows and columns start from zero
                int column = n - 1;
                int row = m - 1;
                // sniff the adjacent based on these priorities
                // 1: north, 2: north-east, 3: north-west, 4: east, 5: west
                for (int direction = 0; direction < 5; direction++)
                {
                    int[] temp = new int[2] { -1, -1 };
                    // check if it can traverse to north
                    if ((direction == 0) && (0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row))
                    {
                        temp[0] = currentNode[0];
                        temp[1] = currentNode[1] - 1;
                    }
                    // check if it can traverse to north-east
                    else if ((direction == 1) && (0 <= currentNode[0] + 1) && (currentNode[0] + 1 <= column) && (0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row))
                    {
                        temp[0] = currentNode[0] + 1;
                        temp[1] = currentNode[1] - 1;
                    }
                    // check if it can traverse to north-west
                    else if ((direction == 2) && (0 <= currentNode[0] - 1) && (currentNode[0] - 1 <= column) && (0 <= currentNode[1] - 1) && (currentNode[1] - 1 <= row))
                    {
                        temp[0] = currentNode[0] - 1;
                        temp[1] = currentNode[1] - 1;
                    }
                    // check if it can traverse to east
                    else if ((direction == 3) && (0 <= currentNode[0] + 1) && (currentNode[0] + 1 <= column))
                    {
                        temp[0] = currentNode[0] + 1;
                        temp[1] = currentNode[1];
                    }
                    // check if it can traverse to west
                    else if ((direction == 4) && (0 <= currentNode[0] - 1) && (currentNode[0] - 1 <= row))
                    {
                        temp[0] = currentNode[0] - 1;
                        temp[1] = currentNode[1];
                    }
                    // if the newly generated possible node to traverse have not yet been traversed and the node is an empty field
                    if ((temp[0] != -1 ) && (_totoshkaTraversedPath.Contains(temp) is false) && (minefieldMap[temp[1], temp[0]] == 0))
                    {
                        // append it to the self.totoshkaTraversedPath
                        _totoshkaTraversedPath.Add(temp);
                        // update the map
                        UpdateMinefieldMap();
                        // if the node is our final destination
                        if (temp[1] == 0)
                        {
                            // stop the recursion
                            destinationReached = true;
                            break;
                        }
                        else
                        {
                            // perform recursive operation
                            RecursiveDFS(temp);
                            // if destination is not reachable even after the recursion
                            if (destinationReached is false)
                            {
                                // remove the newly added node
                                _totoshkaTraversedPath.RemoveAt(_totoshkaTraversedPath.Count - 1);
                                // update the map
                                UpdateMinefieldMap();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            public void MoveNow()
            {
                int[] temp = new int[2];
                // consider that counting the rows start from zero
                int row = m - 1;
                // for each entry point (root node) of the minefield, find the first entry point that can reach the destination
                for (int i = 0; i < n; i++)
                {
                    // update totoshka and ally position outside of the minefield perimeter
                    outsidePosition = i;
                    // update the minefieldMap, showing totoshka and ally standing outside before entering the minefield perimeter
                    UpdateMinefieldMap();
                    // find a node with an empty field to be picked as a root node
                    if (minefieldMap[row, i] != 2)
                    {
                        // the temp node is the current node totoshka's is standing right now
                        temp[0] = i;
                        temp[1] = row;
                        // record totoshka's position
                        _totoshkaTraversedPath.Add(temp);
                        UpdateMinefieldMap();
                        // call the recursiveDFS to search the safe path to reach the destination
                        RecursiveDFS(temp);
                        // if destination reached
                        if (destinationReached is true)
                        {
                            // update the minefieldMap, showing totoshka and ally standing outside before entering the minefield perimeter
                            UpdateMinefieldMap();
                            break;
                        }
                        // reach the dead-end route instead
                        else
                        {
                            // remove the newly added node
                            _totoshkaTraversedPath.RemoveAt(_totoshkaTraversedPath.Count - 1);
                            // update the minefieldMap
                            UpdateMinefieldMap();
                        }
                    }
                }
            }
        }
        static void Main()
        {
            // explanation overview of the program
            Console.WriteLine("\nThis program will create a minefield with randomly placed bombs and a safe path.");
            Console.WriteLine("From there, it will simulate the traversing behavior of Ally and Totoshka.");
            Console.WriteLine("\nLegends used:");
            Console.WriteLine("  X = Bomb");
            Console.WriteLine("  (^_^) = Ally");
            Console.WriteLine("  \\(°ᴥ°)ﾉ = Totoshka");
            Console.WriteLine("  √ = Totoshka's route");
            // get the input size to generate the minefield
            Console.Write("\nEnter the number of row for the minefield: ");
            string consoleInput = Console.ReadLine();
            int m = Convert.ToInt32(consoleInput);
            Console.Write("Enter the number of column for the minefield: ");
            consoleInput = Console.ReadLine();
            int n = Convert.ToInt32(consoleInput);
            // initialize a 2d array to represent a minefield that has an empty field and a bomb
            Console.WriteLine("\nInitial condition.");
            SafePathGenerator spg = new()
            {
                n = n,
                m = m
            };
            var minefieldMap = spg.CreateNow();
            // draw the initial state of the minefield
            DisplayMinefield dmf = new()
            {
                n = n,
                m = m
            };
            dmf.DrawNow(minefieldMap);
            // traverse the minefield
            TraversingMinefield tmf = new()
            {
                n = n,
                m = m,
                minefieldMap = minefieldMap
            };
            tmf.MoveNow();
            // shows the total steps taken to traverse the minefield
            Console.WriteLine("\n" + (tmf.stepTaken).ToString() + " steps taken to traverse the minefield.\n");
        }
    }
}
