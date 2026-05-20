import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.ArrayList;
import java.util.HashMap;

public class FootballScore extends UnicastRemoteObject implements IFootballScore {
  public HashMap<Integer, Match> matches = new HashMap<>();
  private HashMap<Integer, ArrayList<ICallback>> callbacksByMatch = new HashMap<>();

  protected FootballScore() throws RemoteException {
    super();

    Stadium s1 = new Stadium("Stadium 1", "City 1");
    Match m1 = new Match(1, "Team A", "Team B", s1);

    Stadium s2 = new Stadium("Stadium 2", "City 2");
    Match m2 = new Match(2, "Team C", "Team D", s2);

    matches.put(m1.id, m1);
    matches.put(m2.id, m2);
  }

  @Override
  public String getMatches() throws RemoteException {
    String s = "";

    for (Match match : matches.values()) {
      s += match.getMatchIdentifier() + "\n";
    }

    return s;
  }

  @Override
  public Match getMatch(int id) throws RemoteException {
    Match match = matches.get(id);

    if (match != null) {
      return match;
    }

    throw new RemoteException("Match not found");
  }

  @Override
  public void register(int matchId, ICallback cb) throws RemoteException {
    ArrayList<ICallback> callbacks = callbacksByMatch.get(matchId);
    if (callbacks == null) {
      callbacks = new ArrayList<>();
      callbacksByMatch.put(matchId, callbacks);
    }
    callbacks.add(cb);
  }

  @Override
  public void unregister(int matchId, ICallback cb) throws RemoteException {
    ArrayList<ICallback> callbacks = callbacksByMatch.get(matchId);
    if (callbacks != null) {
      callbacks.remove(cb);
    }
  }

  private void callCallbacks(int matchId) {
    ArrayList<ICallback> callbacks = callbacksByMatch.get(matchId);
    if (callbacks == null) {
      return;
    }
    for (ICallback cb : callbacks) {
      try {
        cb.resultChanged(matchId);
      } catch (RemoteException e) {
        e.printStackTrace();
      }
    }
  }

  @Override
  public void addGoalsToHomeTeam(int matchId, int goals) throws RemoteException {
    Match match = matches.get(matchId);

    if (match != null) {
      match.addGoalsToHomeTeam(goals);
      callCallbacks(matchId);
      return;
    }

    throw new RemoteException("Match not found");
  }

  @Override
  public void addGoalsToAwayTeam(int matchId, int goals) throws RemoteException {
    Match match = matches.get(matchId);

    if (match != null) {
      match.addGoalsToAwayTeam(goals);
      callCallbacks(matchId);
      return;
    }

    throw new RemoteException("Match not found");
  }
}
