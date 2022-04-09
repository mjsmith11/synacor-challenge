using System.Threading;
// this is totally stolen because I wasn't up to analyzing the assembly.
public class ConfirmationBypasser {
    public ConfirmationBypasser() {
    }

    public void solve() {
        for(int i=1; i<32768; i++) {
            int result = ackerman(4,1,i,new Dictionary<string,int>()); // registers 0 and 1 are initialized to 4 & 1
            Console.WriteLine($"{i} -> {result}");
            if (result == 6) { // it looks for 6 left in r0
                Console.WriteLine($"ANSWER: Set r7 to {i}");
                break;
            }
        }
    }

    int ackerman(int m, int n, int k, Dictionary<string,int> cache) {
        if (cache.ContainsKey($"{m}|{n}")) {
            return cache[$"{m}|{n}"];
        }
        int result;
        if (m==0) {
            result = (n+1)%32768;
        } else if(n==0) {
            result = ackerman(m-1,k,k, cache);
        } else {
            int x = ackerman(m, n-1, k, cache);
            result = ackerman(m-1,x,k,cache);
        }
        cache[$"{m}|{n}"] = result;
        return result;
    }

    // necessary so stack size can be increased beyond default
    public void solveInThread() {
        var th = new Thread(solve, 128000000);
        th.Start();
        Thread.Sleep(1000);
    }
}
