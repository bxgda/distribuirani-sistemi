
import java.io.Serializable;

public class Match implements Serializable {
  public int id;
  public String homeTeam;
  public String awayTeam;
  public int homeGoals;
  public int awayGoals;
  public Stadium stadium;

  public Match(int id, String homeTeam, String awayTeam, Stadium stadium) {
    this.id = id;
    this.homeTeam = homeTeam;
    this.awayTeam = awayTeam;
    this.homeGoals = 0;
    this.awayGoals = 0;
    this.stadium = stadium;
  }

  public void addGoalsToHomeTeam(int goals) {
    this.homeGoals += goals;
  }

  public void addGoalsToAwayTeam(int goals) {
    this.awayGoals += goals;
  }

  public Stadium getStadium() {
    return this.stadium;
  }

  public String getMatchIdentifier() {
    return this.id + "_" + this.homeGoals + ":" + this.awayGoals;
  }
}
