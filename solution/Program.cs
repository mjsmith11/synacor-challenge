// See https://aka.ms/new-console-template for more information

// one code in spec (1/8)

// two codes (3/8)
//bootAndSelfTest();

// one code (4/8)
//getTabletCode();

// one code (5/8)
//findWallCode();

// one code (6/8)
solveDoor();


// boots and self tests the vm.  Could play manually with this
static void bootAndSelfTest() {
    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin");
    vm.execute(); 
}

// plays game to get code from the tablet
static void getTabletCode() {
    List<string> inputs = new List<string>();
    inputs.Add("take tablet");
    inputs.Add("use tablet");

    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin");
    vm.primeInputBuffer(inputs);
    vm.execute(); 
}

static void findWallCode() {
    List<string> inputs = new List<string>();
    inputs.Add("doorway");
    inputs.Add("north");
    inputs.Add("north");
    inputs.Add("bridge");
    inputs.Add("continue");
    inputs.Add("down");
    inputs.Add("west");
    inputs.Add("passage");
    inputs.Add("ladder");
    inputs.Add("west");
    inputs.Add("south");
    inputs.Add("north");

    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin");
    vm.primeInputBuffer(inputs);
    vm.execute(); 
}

static void solveDoor() {
    List<string> inputs = new List<string>();
    inputs.Add("doorway");
    inputs.Add("north");
    inputs.Add("north");
    inputs.Add("bridge");
    inputs.Add("continue");
    inputs.Add("down");
    inputs.Add("east");
    inputs.Add("take empty lantern");
    inputs.Add("west");
    inputs.Add("west");
    inputs.Add("passage");
    inputs.Add("ladder");
    inputs.Add("west");
    inputs.Add("south");
    inputs.Add("north");
    inputs.Add("take can");
    inputs.Add("use can");
    inputs.Add("use lantern");
    inputs.Add("west");
    inputs.Add("ladder");
    inputs.Add("darkness");
    inputs.Add("continue");
    inputs.Add("west");
    inputs.Add("west");
    inputs.Add("west");
    inputs.Add("west");
    inputs.Add("north");
    inputs.Add("take red coin");
    inputs.Add("north");
    inputs.Add("west");
    inputs.Add("take blue coin");
    inputs.Add("up");
    inputs.Add("take shiny coin");
    inputs.Add("down");
    inputs.Add("east");
    inputs.Add("east");
    inputs.Add("take concave coin");
    inputs.Add("down");
    inputs.Add("take corroded coin");
    inputs.Add("up");
    inputs.Add("west");

    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin");
    vm.primeInputBuffer(inputs);
    vm.execute(); 
}
// generate all permutations of 0, 1, 2, 3, 4
static List<List<int>> generatePermutations(List<List<int>> workingList, List<int> workingPermutation) {
    if (workingPermutation.Count < 5) {
        for(int i=0; i<5; i++) {
            if(!workingPermutation.Contains(i)) {
                List<int> newPerm = new List<int>(workingPermutation);
                newPerm.Add(i);
                generatePermutations(workingList,newPerm);
            }
        }
    } else {
        workingList.Add(workingPermutation);
    }
    return workingList;
}