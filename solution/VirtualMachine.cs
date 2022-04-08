public class VirtualMachine {
    private ushort[] memory;
    private ushort[] registers;
    private Stack<ushort> stack;
    private Queue<char> inputBuffer;
    private ushort memPointer;
    private bool shouldHalt;
    public bool programaticIO;
    private StringBuilder outputBuffer;

    enum OpCode : ushort {
        halt = 0,
        set = 1,
        push = 2,
        pop = 3,
        eq = 4,
        gt = 5,
        jmp = 6,
        jt = 7,
        jf = 8,
        add = 9,
        mult = 10,
        mod = 11,
        and = 12,
        or = 13,
        not = 14,
        rmem = 15,
        wmem = 16,
        call = 17,
        ret = 18,
        output = 19,
        input = 20,
        noop = 21
    }

    public VirtualMachine() {
        // these objects don't get used but they eliminate warnings about class members remaining null when the constructor exits.
        memory = new ushort[1];
        registers = new ushort[1];
        stack = new Stack<ushort>();
        inputBuffer = new Queue<char>();
        programaticIO = false;
        reset();
    }

    public VirtualMachine(string filename, programaticIO = false) {
        // these objects don't get used but they eliminate warnings about class members remaining null when the constructor exits.
        memory = new ushort[1];
        registers = new ushort[1];
        stack = new Stack<ushort>();
        inputBuffer = new Queue<char>();
        this.programaticIO = programaticIO;
        reset();
        LoadProgramFromFile(filename);
    }

    public void reset() {
        memory = new ushort[32768];
        registers = new ushort[8];
        stack = new Stack<ushort>();
        memPointer = 0;
        shouldHalt = false;
        inputBuffer = new Queue<char>();
    }
    public string getOutput() {
        outputBuffer.ToString();
    }
    public void primeInputBuffer(List<string> inputs) {
        foreach(string s in inputs) {
            foreach(char c in s) {
                inputBuffer.Enqueue(c);
            }
            inputBuffer.Enqueue('\n');
        }
    }

    public void execute() {
        if(programaticIO) {
            outputBuffer = new StringBuilder();
        }
        while(!shouldHalt) {
            OpCode opcode = (OpCode)getMemoryValueAtPointer();
            switch(opcode) {
                case OpCode.halt:
                    halt();
                    break;
                case OpCode.set:
                    set();
                    break;
                case OpCode.push:
                    push();
                    break;
                case OpCode.pop:
                    pop();
                    break;
                case OpCode.eq:
                    equal();
                    break;
                case OpCode.gt:
                    greaterThan();
                    break;
                case OpCode.jmp:
                    unconditionalJump();
                    break;
                case OpCode.jt:
                    jumpIfTrue();
                    break;
                case OpCode.jf:
                    jumpIfFalse();
                    break;
                case OpCode.add:
                    add();
                    break;
                case OpCode.mult:
                    multiply();
                    break;
                case OpCode.mod:
                    modulo();
                    break;
                case OpCode.and:
                    and();
                    break;
                case OpCode.or:
                    or();
                    break;
                case OpCode.not:
                    not();
                    break;
                case OpCode.rmem:
                    readMemory();
                    break;
                case OpCode.wmem:
                    writeMemory();
                    break;
                case OpCode.call:
                    call();
                    break;
                case OpCode.ret:
                    ret();
                    break;
                case OpCode.output:
                    output();
                    break;
                case OpCode.input:
                    input();
                    break;
                case OpCode.noop:
                    break;
                default:
                    unknownOpcode(opcode);
                    break;
            }
        }
    }

    public void LoadProgramFromFile(string filepath) {
        using (var stream = File.Open(filepath, FileMode.Open)) {
            using (var reader = new BinaryReader(stream)) {
                ushort memAddress = 0;
                while (stream.Position != stream.Length) {
                    memory[memAddress] = reader.ReadUInt16();
                    memAddress++;
                }
            }
        }
    }
#region opcodes
    //halt: 0
    //  stop execution and terminate the program
    private void halt() {
        shouldHalt = true;
    }
    //set: 1 a b
    //  set register <a> to the value of <b>
    private void set() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        writeRegister(getValue(b),a);
    }
    //push: 2 a
    //  push <a> onto the stack
    private void push() {
        ushort a = getMemoryValueAtPointer();
        stack.Push(getValue(a));
    }
    //pop: 3 a
    //  remove the top element from the stack and write it into <a>; empty stack = error
    private void pop() {
        if (stack.Count() == 0) {
            Console.WriteLine("EXCEPTION: Popping empty stack");
        } else {
            ushort a = getMemoryValueAtPointer();
            writeRegister(stack.Pop(),a);
        }
    }
    //eq: 4 a b c
    //  set <a> to 1 if <b> is equal to <c>; set it to 0 otherwise
    private void equal() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        if (getValue(b) == getValue(c)) {
            writeRegister(1,a);
        } else {
            writeRegister(0,a);
        }
    }
    //gt: 5 a b c
    //  set <a> to 1 if <b> is greater than <c>; set it to 0 otherwise
    private void greaterThan() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        if (getValue(b) > getValue(c)) {
            writeRegister(1,a);
        } else {
            writeRegister(0,a);
        }
    }
    //jmp: 6 a
    //  jump to <a>
    private void unconditionalJump() {
        ushort a = getMemoryValueAtPointer();
        memPointer = getValue(a);
    }
    //jt: 7 a b
    // if <a> is nonzero, jump to <b>
    private void jumpIfTrue() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        if (getValue(a)!=0) {
            memPointer = getValue(b);
        }
    }
    //jf: 8 a b
    //  if <a> is zero, jump to <b>
    private void jumpIfFalse() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        if (getValue(a)==0) {
            memPointer = getValue(b);
        }
    }
    //add: 9 a b c
    //  assign into <a> the sum of <b> and <c> (modulo 32768)
    private void add() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        ushort sum = (ushort)(getValue(b) + getValue(c)); // overflow?
        sum %= 32768;
        writeRegister(sum,a);
    }
    //mult: 10 a b c
    //  assign into <a> the product of <b> and <c> (modulo 32768)
    private void multiply() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        ushort prod = (ushort)((getValue(b) * getValue(c))%32768); // overflow?
        writeRegister(prod,a);
    }
    //mod: 11 a b c
    //  assign into <a> the remainer of <b> divided by <c>
    private void modulo() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        ushort mod = (ushort)(getValue(b) % getValue(c)); // overflow?
        writeRegister(mod,a);
    }
    //and: 12 a b c
    //  stores into <a> the bitwise and of <b> and <c>
    private void and() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        ushort and = (ushort)(getValue(b) & getValue(c)); // casting issue?
        writeRegister(and,a);
    }
    //or: 13 a b c
    //  stores into <a> the bitwise or of <b> and <c>
    private void or() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort c = getMemoryValueAtPointer();
        ushort or = (ushort)(getValue(b) | getValue(c)); // casting issue?
        writeRegister(or,a);
    }
    //not: 14 a b
    //  stores 15-bit bitwise inverse of <b> in <a>
    private void not() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        ushort not = (ushort)(getValue(b) ^ 0x7FFF); //use xor to flip all the bits except most significant since it specifies a 15 bit inverse
        writeRegister(not,a);
    }
    //rmem: 15 a b
    //  read memory at address <b> and write it to <a>
    private void readMemory() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        writeRegister(memory[getValue(b)],a);
    }
    //wmem: 16 a b
    //  write the value from <b> into memory at address <a>
    private void writeMemory() {
        ushort a = getMemoryValueAtPointer();
        ushort b = getMemoryValueAtPointer();
        memory[getValue(a)] = getValue(b);
    }
    //call: 17 a
    //  write the address of the next instruction to the stack and jump to <a>
    private void call() {
        ushort a = getMemoryValueAtPointer();
        stack.Push(memPointer);
        memPointer = getValue(a);
    }
    //ret: 18
    //  remove the top element from the stack and jump to it; empty stack = halt
    private void ret()
    {
        if (stack.Count() == 0) {
            shouldHalt = true;
        } else {
            memPointer = stack.Pop();
        }
    }
    //in: 20 a
    //  read a character from the terminal and write its ascii code to <a>; it can be assumed that once input starts, it will continue until a newline is encountered; this means that you can safely read whole lines from the keyboard and trust that they will be fully read
    private void input() {
        if (inputBuffer.Count == 0) {
            if (programaticIO) {
                // code needs to give more input
                memPointer--; // so we do the input again on restart
                shouldHalt = true;
                return;
            } else {
                keyboardRead();
            }
        }
        ushort a = getMemoryValueAtPointer();
        writeRegister((ushort)inputBuffer.Dequeue(), a);
    }
    //out: 19 a
    //    write the character represented by ascii code <a> to the terminal
    private void output() {
        ushort a = getMemoryValueAtPointer();
        char charToPrint = (char)getValue(a);
        if(programaticIO) {
            outputBuffer.Append(charToPrint);
        } else {
            Console.Write(charToPrint);
        }
    }

#endregion

    /*
    - numbers 0..32767 mean a literal value
    - numbers 32768..32775 instead mean registers 0..7
    - numbers 32776..65535 are invalid
    */
    private ushort getValue(ushort number) {
        if(number < 32768) {
            return number;
        }
        if (number < 32776) {
            return registers[number - 32768];
        }
        Console.WriteLine($"EXCEPTION: Invalid Number {number}");
        shouldHalt = true;
        return 0;
    }

    private ushort getMemoryValueAtPointer() {
        ushort value = memory[memPointer];
        memPointer++;
        return value;
    }
    //    - numbers 32768..32775 instead mean registers 0..7
    private void writeRegister(ushort value, ushort regNumber) {
        registers[regNumber - 32768] = value;
    }

    private void unknownOpcode(OpCode opcode) {
        Console.WriteLine($"EXCEPTION: Unknown opcode {opcode}");
        shouldHalt = true;
    }

    private void keyboardRead()
    {
        string input = Console.ReadLine();
        foreach(char c in input){
            inputBuffer.Enqueue(c);
        }
        inputBuffer.Enqueue('\n');
    }
}