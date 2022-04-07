// See https://aka.ms/new-console-template for more information

// one code in spec

// two codes
//bootAndSelfTest();

// one code
//getTabletCode();

// one code
//findWallCode();


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
