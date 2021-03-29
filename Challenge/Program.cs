using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Challenge
{
    class Program
    {

        //public int[,] neighbors = {{1,0}, {-1,0}, {0,1},{0,-1}};
        static void Main(string[] args)
        {


            String file = File.ReadAllText( @"..\map.txt" );
            int[,] map =  ReadMatrix( file ); 

            List<int> asw = FindLogestPath(map);

            Console.Write("Length: {0} \n\nDrop: {1} \n\n", asw.Count, asw[0]-asw[asw.Count-1]  );
            Console.Write("Calculated: ");
            asw.ForEach(i => Console.Write("{0}-", i));
            Console.Write("\n\n");  
        }

        /// <summary>
        /// FindLogestPath
        /// Iterates over each cell to find its path and return the longest path and the steppest
        ///</summary>
        private static List<int> FindLogestPath(int[,] map)
        {
            int row = map.GetLength(0);
            int cols = map.GetLength(1);

            int[,] pathMap = new int[row,cols]; 

            int maxPathLenght = 0;
            int[] endPoint = {0,0} ;

            List<int> finalPath = new List<int>();

            List<int[]> MaxPoints = new List<int[]>();
            

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int pathLenght = FindPath( i,  j,  row,  cols,  map, pathMap  );

                    if(pathLenght==maxPathLenght) {

                       MaxPoints.Add(new int[]{i,j} );
                    }
 
                    if (  pathLenght > maxPathLenght  ){                       
                        MaxPoints = new List<int[]>();
                        MaxPoints.Add(new int[]{i,j});
                    }   
                    maxPathLenght = Math.Max(maxPathLenght, pathLenght);
                }
                
            }

            //Uncomment the following section to print posible start points of longests paths
            /*
            foreach (var point in MaxPoints)
            {
                foreach (var coor in point)
                {
                     Console.Write("{0}\t", coor) ;
                }
            }
            */


            finalPath = FindSteps( map,pathMap,  maxPathLenght, row, cols, MaxPoints);

            return finalPath;
        }


        /// <summary>
        /// FindSteps
        /// Allows to calculate each step of the longest paths and returns the stepper path of them
        ///</summary>
        private static List<int> FindSteps(int[,] map,int[,] pathMap, int maxPathLenght, int rows, int cols, List<int[]> MaxPoints){

            List<List<int>> posiblePaths = new List<List<int>> ();
            List<int> finalPath = new List<int>();
            int step =0;  
            int[,] neighbors = {{1,0}, {-1,0}, {0,1},{0,-1}};
            int begin = 0;
            int end =0;

            foreach (var point in MaxPoints)
            {

                int x0 = point[0];
                int y0 = point[1];
                begin=map[x0,y0];
                List<int> aa = new List<int>();
                aa.Add(map[x0,y0]);
                
                for(int iter=1; iter<= maxPathLenght; iter++){

                for( int dir=0; dir<4; dir++) {
                int x = neighbors[dir,0]+x0,  y = neighbors[dir,1]+y0;

                if (x>-1 && x < cols && y< rows && y>-1 && pathMap[x,y] == maxPathLenght-iter && map[x,y]<map[x0,y0]  ){

                    aa.Add(map[x,y]);

                    x0=x;
                    y0=y;
                     
                }

               }
               
               end=map[x0,y0];               

               if(begin - end > step){
                   step = begin - end;
                   finalPath = aa;
               }
            }
            posiblePaths.Add(aa);

            }


            //Uncomment to print all the possible paths of maxlenght

            /*
            Console.Write("\n\n"); 
            foreach (var cc in posiblePaths)
            {
                cc.ForEach(i => Console.Write("{0}\t", i));
                Console.Write("\n\n");   
            }
            Console.Write("\n\n");
            */
            return finalPath;
        }


        /// <summary>
        /// FindPath
        /// is a recursive function which evaluates the maximun path for each cell and move to the next one if its value is less
        ///</summary>
        private static int FindPath(int i, int j, int rows, int cols,  int[,] map, int[,] pathMap  )
        {
            if (pathMap[i,j] > 0) return pathMap[i,j] ;
            int[,] neighbors = {{1,0}, {-1,0}, {0,1},{0,-1}};
            int maxTemp = 0;

            for( int dir=0; dir<4; dir++) {
                int x = neighbors[dir,0]+i,  y = neighbors[dir,1]+j;

                if (x>-1 && x < cols && y< rows && y>-1 && map[x,y] < map[i,j] ){
                    int longest = FindPath(x,y,rows,cols, map , pathMap);
                    maxTemp = Math.Max(maxTemp, longest);
                }
            }

            pathMap[i,j] = maxTemp+1;
            return pathMap[i,j];            
        }

        
        /// <summary>
        /// ReadMatrix
        /// Read the input file and returns a matix with the map
        ///</summary>
        private static int[,] ReadMatrix(String file ){            
            var aux1 = file.Split('\n');
            int rows = int.Parse( aux1[0].Split(' ')[0].Trim()), cols = int.Parse(aux1[0].Split(' ')[1].Trim());  

            int[,] map = new int[rows, cols];
            int i,j=0;
            for( i=1; i<= rows; i++){
                var line = aux1[i].Split(' ');
                for( j=0; j< cols; j++){                    
                    map[i-1, j] = int.Parse(line[j].Trim());                                    
                }                
            }
            return map;
        }

        /// <summary>
        /// PrintMatrix
        /// It is udes to evaluate the matrix printing it in console
        ///</summary>   
        public static void PrintMatrix(int[,] arr){

            int row = arr.GetLength(0);
            int cols = arr.GetLength(1);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }
   
   
    }
}
