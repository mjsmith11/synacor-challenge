public class GridSolver {
    private Queue<State> explorationQueue;

    public GridSolver() {
        explorationQueue = new Queue<State>();
    }

    public State solve() {
        string[,] grid = getGrid();
        State currentState = new State(0,3,22); // initial state 
        while(!currentState.isGoal()) {
            // north
            if (isValidMove(currentState,grid,currentState.locX,currentState.locY - 1)) {
                int gridNum;
                State newState = new State(currentState);
                newState.locY--;
                newState.path.Add("north");
                if(int.TryParse(grid[currentState.locX,currentState.locY - 1], out gridNum)) {
                    int newWeight = calcNewWeight(currentState,gridNum);
                    newState.weight = newWeight;
                } else {
                    newState.sign = grid[currentState.locX,currentState.locY - 1];
                }
                explorationQueue.Enqueue(newState);
            }

            // south
            if (isValidMove(currentState,grid,currentState.locX,currentState.locY + 1)) {
                int gridNum;
                State newState = new State(currentState);
                newState.locY++;
                newState.path.Add("south");
                if(int.TryParse(grid[currentState.locX,currentState.locY + 1], out gridNum)) {
                    int newWeight = calcNewWeight(currentState,gridNum);
                    newState.weight = newWeight;
                } else {
                    newState.sign = grid[currentState.locX,currentState.locY + 1];
                }
                explorationQueue.Enqueue(newState);
            }

            // west
            if (isValidMove(currentState,grid,currentState.locX - 1,currentState.locY)) {
                int gridNum;
                State newState = new State(currentState);
                newState.locX--;
                newState.path.Add("west");
                if(int.TryParse(grid[currentState.locX-1,currentState.locY], out gridNum)) {
                    int newWeight = calcNewWeight(currentState,gridNum);
                    newState.weight = newWeight;
                } else {
                    newState.sign = grid[currentState.locX-1,currentState.locY];
                }
                explorationQueue.Enqueue(newState);
            }

            // east
            if (isValidMove(currentState,grid,currentState.locX + 1,currentState.locY)) {
                int gridNum;
                State newState = new State(currentState);
                newState.locX++;
                newState.path.Add("east");
                if(int.TryParse(grid[currentState.locX+1,currentState.locY], out gridNum)) {
                    int newWeight = calcNewWeight(currentState,gridNum);
                    newState.weight = newWeight;
                } else {
                    newState.sign = grid[currentState.locX+1,currentState.locY];
                }
                explorationQueue.Enqueue(newState);
            }


            currentState = explorationQueue.Dequeue();
        }
        return currentState;
    }

    private bool isValidMove(State s, string[,] grid, int newx, int newy) {
        if (newx < 0 || newx > 3) {
            return false;
        }
        if (newy < 0 || newy > 3) {
            return false;
        }
        if (newx == 0 && newy == 3) {
            return false;
        }
        int gridNum;
        if(int.TryParse(grid[newx,newy], out gridNum)) {
            int newWeight=calcNewWeight(s,gridNum);
            if(newWeight < 0 || newWeight > 100) {
                return false;
            }
            if(newx == 3 && newy == 0 && newWeight != 30) {
                return false;
            }
        }
        return true;
    }

    private int calcNewWeight(State s, int gridNum) {
        switch(s.sign) {
            case "*":
                return s.weight * gridNum;
            case "+":
                return s.weight + gridNum;
            case "-":
                return s.weight - gridNum;
            default:
                Console.WriteLine("CalcNewWeight default case");
                return s.weight;
        }
    }

    private string[,] getGrid() {
        string[,] grid = new string[4,4];
        grid[0,0] = "*";
        grid[1,0] = "8";
        grid[2,0] = "-";
        grid[3,0] = "1";
        grid[0,1] = "4";
        grid[1,1] = "*";
        grid[2,1] = "11";
        grid[3,1] = "*";
        grid[0,2] = "+";
        grid[1,2] = "4";
        grid[2,2] = "-";
        grid[3,2] = "18";
        grid[0,3] = "22";
        grid[1,3] = "-";
        grid[2,3] = "9";
        grid[3,3] = "*";
        return grid;
    }

    public class State {
        public int locX;
        public int locY;
        public int weight;
        public string sign;
        public List<string> path;

        public State(int x, int y, int weight) {
            locX = x;
            locY = y;
            this.weight = weight;
            sign = "";
            path = new List<string>();
        }

        public State(State s) {
            this.locX = s.locX;
            this.locY = s.locY;
            this.weight = s.weight;
            this.sign = sign;
            this.path = new List<string>(s.path);
        }

        public bool isGoal() {
            if(locX != 3) { return false; }
            if(locY != 0) { return false; }
            if(weight != 30) { return false; }
            return true;
        }
    }
}