public class VirtualMachine {
    private ushort[] memory;
    private ushort[] registers;
    private Stack<ushort> stack;
    private ushort memPointer;
    private bool shouldHalt;

    enum OpCode : ushort {
        halt = 0,
        output = 19,
        noop = 21
    }

    public VirtualMachine() {
        // these objects don't get used but they eliminate warnings about class members remaining null when the constructor exits.
        memory = new ushort[1];
        registers = new ushort[1];
        stack = new Stack<ushort>();
        
        reset();
    }

    public VirtualMachine(string filename) {
        // these objects don't get used but they eliminate warnings about class members remaining null when the constructor exits.
        memory = new ushort[1];
        registers = new ushort[1];
        stack = new Stack<ushort>();

        reset();
        LoadProgramFromFile(filename);
    }

    public void reset() {
        memory = new ushort[32768];
        registers = new ushort[8];
        stack = new Stack<ushort>();
        memPointer = 0;
        shouldHalt = false;
    }

    public void execute() {
        while(!shouldHalt) {
            OpCode opcode = (OpCode)getMemoryValueAtPointer();
            switch(opcode) {
                case OpCode.halt:
                    halt();
                    break;
                case OpCode.output:
                    output();
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

    private void halt() {
        shouldHalt = true;
    }

    private void output() {
        ushort a = getMemoryValueAtPointer();
        char charToPrint = (char)getValue(a);
        Console.Write(charToPrint);
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
            return registers[number - 32776];
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

    private void unknownOpcode(OpCode opcode) {
        Console.WriteLine($"EXCEPTION: Unknown opcode {opcode}");
        shouldHalt = true;
    }
}