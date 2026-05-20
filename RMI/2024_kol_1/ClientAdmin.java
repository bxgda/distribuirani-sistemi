import java.rmi.Naming;

public class ClientAdmin {

  public static void main(String[] args) {
    try {

      IFootballScore fs = (IFootballScore) Naming.lookup("rmi://localhost:4096/Server");

      System.out.println("Click enter to add goal to home team for match 1...");
      System.in.read();

      fs.addGoalsToHomeTeam(1, 1);

      System.out.println("Added goal to home team for match 1.");

    } catch (Exception e) {
      e.printStackTrace();
    }
  }
}
