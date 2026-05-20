import java.rmi.*;
import java.rmi.registry.*;

public class Server {

  public static void main(String[] args) throws Exception {
    IProsti p = new Prosti();
    LocateRegistry.createRegistry(4096);
    Naming.rebind("rmi://localhost:4096/Server", p);

    System.out.println("Server is running...");
    System.in.read();
  }
}
