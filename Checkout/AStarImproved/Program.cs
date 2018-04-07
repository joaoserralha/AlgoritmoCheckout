using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace AStar

{

    class Program

    {



        public static class Globals

        {

            public static readonly List<KeyValuePair<int, int>> fourNeighbors = new List<KeyValuePair<int, int>>()

                                            { new KeyValuePair<int, int>(-1,0),

                                              new KeyValuePair<int, int>(0,1),

                                              new KeyValuePair<int, int>(1, 0),

                                              new KeyValuePair<int, int>(0,-1) };



            public static int maxX;

            public static int maxY;



            public static Dictionary<string, matrixNode> greens;

            public static Dictionary<string, matrixNode> reds;



            public static char[][] matrix;



        }



        static void Main(string[] args)

        {

            //    Globals.matrix = new char[][] {  new char[] {'-', 'S', '-', '-', 'X'},

            //                                     new char[] {'-', 'X', 'X', '-', 'X'},

            //                                     new char[] {'-', '-', 'X', 'X', 'X'},

            //                                     new char[] {'X', '-', 'X', 'E', '-'},

            //                                     new char[] {'-', '-', '-', '-', 'X'}};



            //Globals.matrix = new char[][] {  new char[] {'S', '-', '-', '-', '-'},

            //                                 new char[] {'-', '-', 'X', 'X', '-'},

            //                                 new char[] {'-', '-', 'X', 'X', '-'},

            //                                 new char[] {'X', 'X', 'X', 'X', '-'},

            //                                 new char[] {'E', '-', '-', '-', '-'}};



            Globals.matrix = new char[][] {  new char[] {'-', 'S', '-', '-', '-', '-', '-', '-'},

                                             new char[] {'-', '-', 'X', 'X', 'X', 'X', 'X', '-'},

                                             new char[] {'-', '-', '-', '-', '-', '-', 'X', '-'},

                                             new char[] {'-', '-', '-', '-', '-', '-', 'X', '-'},

                                             new char[] {'-', '-', '-', '-', '-', '-', 'X', '-'},

                                             new char[] {'X', 'X', 'X', 'X', 'X', 'X', 'X', '-'},

                                             new char[] { 'E', '-', '-', '-', '-', '-', '-', '-' }};





            //looking for shortest path from 'S' at (0,1) to 'E' at (3,3)

            //obstacles marked by 'X'

            int fromX = 0, fromY = 1, toX = 6, toY = 0;

            matrixNode endNode = AStar(Globals.matrix, fromX, fromY, toX, toY);



            //looping through the Parent nodes until we get to the start node

            Stack<matrixNode> path = new Stack<matrixNode>();



            while (endNode.x != fromX || endNode.y != fromY)

            {

                path.Push(endNode);

                endNode = endNode.parent;

            }

            path.Push(endNode);



            Console.WriteLine("The shortest path from  " +

                              "(" + fromX + "," + fromY + ")  to " +

                              "(" + toX + "," + toY + ")  is:  \n");



            while (path.Count > 0)

            {

                matrixNode node = path.Pop();

                Console.WriteLine("(" + node.x + "," + node.y + ")");

            }



            Console.WriteLine("Number of nodes analized:" + Globals.reds.Count);

        }







        public class matrixNode

        {

            public int fr = 0, to = 0, sum = 0;

            public int x, y;

            public matrixNode parent;

            public int neighbors;



            public matrixNode(int x, int y)

            {

                this.x = x;

                this.y = y;



                setNeighbors();

            }



            private void setNeighbors()

            {

                foreach (KeyValuePair<int, int> plusXY in Globals.fourNeighbors)

                {

                    int nbrX = x + plusXY.Key;

                    int nbrY = y + plusXY.Value;

                    string nbrKey = nbrX.ToString() + nbrY.ToString();

                    if (nbrX < 0 || nbrY < 0 || nbrX >= Globals.maxX || nbrY >= Globals.maxY

                        || Globals.matrix[nbrX][nbrY] == 'X' //obstacles marked by 'X'

                        || Globals.reds.ContainsKey(nbrKey))

                        continue;



                    neighbors++;

                }

            }

        }

        public static matrixNode AStar(char[][] matrix, int fromX, int fromY, int toX, int toY)

        {

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // in this version an element in a matrix can move left/up/right/down in one step, two steps for a diagonal move.

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



            //the keys for greens and reds are x.ToString() + y.ToString() of the matrixNode

            Globals.greens = new Dictionary<string, matrixNode>(); //open

            Globals.reds = new Dictionary<string, matrixNode>(); //closed



            matrixNode startNode = new matrixNode(fromX, fromY);





            //string key = startNode.x.ToString() + startNode.x.ToString();

            string key = startNode.x.ToString() + startNode.y.ToString();





            Globals.greens.Add(key, startNode);



            Func<KeyValuePair<string, matrixNode>> smallestGreen = () =>

            {

                KeyValuePair<string, matrixNode> smallest = Globals.greens.ElementAt(0);



                foreach (KeyValuePair<string, matrixNode> item in Globals.greens)

                {



                    //if (item.Value.sum < smallest.Value.sum)

                    //    smallest = item;

                    //else if (item.Value.sum == smallest.Value.sum

                    //        && item.Value.to < smallest.Value.to)

                    //    smallest = item;

                    int peso = 5;


                    if (item.Value.sum + item.Value.neighbors * peso < smallest.Value.sum + smallest.Value.neighbors * peso)

                        smallest = item;

                    else if (item.Value.sum + (item.Value.neighbors) * peso == smallest.Value.sum + (smallest.Value.neighbors) * peso

                            && item.Value.to + (item.Value.neighbors) * peso < smallest.Value.to + (smallest.Value.neighbors) * peso)

                        smallest = item;



                }



                return smallest;

            };



            Globals.maxX = matrix.GetLength(0);

            if (Globals.maxX == 0)

                return null;

            Globals.maxY = matrix[0].Length;



            while (true)

            {

                if (Globals.greens.Count == 0)

                    return null;



                KeyValuePair<string, matrixNode> current = smallestGreen();

                if (current.Value.x == toX && current.Value.y == toY)

                    return current.Value;



                Globals.greens.Remove(current.Key);

                Globals.reds.Add(current.Key, current.Value);



                foreach (KeyValuePair<int, int> plusXY in Globals.fourNeighbors)

                {

                    int nbrX = current.Value.x + plusXY.Key;

                    int nbrY = current.Value.y + plusXY.Value;

                    string nbrKey = nbrX.ToString() + nbrY.ToString();

                    if (nbrX < 0 || nbrY < 0 || nbrX >= Globals.maxX || nbrY >= Globals.maxY

                        || matrix[nbrX][nbrY] == 'X' //obstacles marked by 'X'

                        || Globals.reds.ContainsKey(nbrKey))

                        continue;



                    if (Globals.greens.ContainsKey(nbrKey))

                    {

                        matrixNode curNbr = Globals.greens[nbrKey];

                        int from = Math.Abs(nbrX - fromX) + Math.Abs(nbrY - fromY);

                        if (from < curNbr.fr)

                        {

                            curNbr.fr = from;

                            curNbr.sum = curNbr.fr + curNbr.to;

                            curNbr.parent = current.Value;

                        }

                    }

                    else

                    {

                        matrixNode curNbr = new matrixNode(nbrX, nbrY);

                        curNbr.fr = Math.Abs(nbrX - fromX) + Math.Abs(nbrY - fromY);

                        curNbr.to = Math.Abs(nbrX - toX) + Math.Abs(nbrY - toY);

                        curNbr.sum = curNbr.fr + curNbr.to;

                        curNbr.parent = current.Value;



                        Globals.greens.Add(nbrKey, curNbr);

                    }

                }

            }

        }

    }

}