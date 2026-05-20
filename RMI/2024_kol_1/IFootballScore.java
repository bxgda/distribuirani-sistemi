import java.rmi.Remote;
import java.rmi.RemoteException;

public interface IFootballScore extends Remote {
  String getMatches() throws RemoteException;

  Match getMatch(int id) throws RemoteException;

  void addGoalsToHomeTeam(int matchId, int goals) throws RemoteException;

  void addGoalsToAwayTeam(int matchId, int goals) throws RemoteException;

  void register(int matchId, ICallback cb) throws RemoteException;

  void unregister(int matchId, ICallback cb) throws RemoteException;
}
