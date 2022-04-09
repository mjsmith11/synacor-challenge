// See https://aka.ms/new-console-template for more information

// one code in spec (1/8)

// two codes (3/8)
//bootAndSelfTest();

// one code (4/8)
//getTabletCode();

// one code (5/8)
//findWallCode();

// one code (6/8)
//solveDoor();

// one code (7/8)
teleport();
//ConfirmationBypasser c = new ConfirmationBypasser(); //25734
//c.solveInThread();


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

    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin",true);
    vm.primeInputBuffer(inputs);
    vm.execute(); 

    // brute force the door
    List<List<int>> coinPermutations = generatePermutations(new List<List<int>>(), new List<int>());
    List<string> coins = new List<string>();
    coins.Add("corroded coin");
    coins.Add("concave coin");
    coins.Add("red coin");
    coins.Add("blue coin");
    coins.Add("shiny coin");

    foreach(List<int> perm in coinPermutations) {
        inputs = new List<string>();
        for(int i=0; i<5; i++) {
            inputs.Add($"use {coins[perm[i]]}");
        }
        vm.primeInputBuffer(inputs);
        vm.execute();
        string output = vm.getOutput();
        if (output.Contains("they are all released onto the floor")) {
            // pick them up and get ready to try again
            inputs = new List<string>();
            for(int i=0; i<5; i++) {
                inputs.Add($"take {coins[perm[i]]}");
            }
            vm.primeInputBuffer(inputs);
            vm.execute();
        } else {
            //Found the answer. Return control to console
            inputs = new List<string>();
            inputs.Add("north");
            inputs.Add("take teleporter");
            inputs.Add("use teleporter");
            vm.primeInputBuffer(inputs);
            vm.execute();
            Console.Write(vm.getOutput());
            break;
        }
    }

}

static void teleport() {
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
    // door solution
    inputs.Add("use blue coin");
    inputs.Add("use red coin");
    inputs.Add("use shiny coin");
    inputs.Add("use concave coin");
    inputs.Add("use corroded coin");

    inputs.Add("north");
    inputs.Add("take teleporter");
    inputs.Add("use teleporter");

    VirtualMachine vm = new VirtualMachine("/work/problem-statement/challenge.bin",true);
    vm.primeInputBuffer(inputs);
    vm.execute(); 

    inputs = new List<string>();
    inputs.Add("use teleporter");
    vm.hackTheReg = true;
    vm.primeInputBuffer(inputs);
    vm.execute();
    Console.WriteLine(vm.getOutput());
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