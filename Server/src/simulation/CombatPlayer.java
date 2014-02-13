package simulation;



import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;

// Player class representing an individual soldier in the world simulation
public class CombatPlayer {

	public static final int maxHealth = 100; // Maximum amount of health for a player

	private User sfsUser; // SFS user that corresponds to this player

	private Transform transform; // Transform of the player that is synchronized with clients

	private int health = 100;
	private int score = 0;

	public boolean isDead() {
		return health <= 0;
	}

	public void removeHealth(int count) {
		health -= count;
	}

	public User getSfsUser() {
		return sfsUser;
	}

	public Transform getTransform() {
		return transform;
	}

	public CombatPlayer(User sfsUser) {
		this.sfsUser = sfsUser;
		this.transform = Transform.random();
	}

	public void toSFSObject(ISFSObject data) {
		ISFSObject playerData = new SFSObject();

		playerData.putInt("id", sfsUser.getId());
		playerData.putInt("score", this.score);

		transform.toSFSObject(playerData);
		data.putSFSObject("player", playerData);
	}
//
//	public double getX() {
//		return this.collider.getCenterx() + this.transform.getX();
//	}
//
//	public double getY() {
//		return this.collider.getCentery() + this.transform.getY();
//	}
//
//	public double getZ() {
//		return this.collider.getCenterz() + this.transform.getZ();
//	}

	public int getHealth() {
		return health;
	}

	public void addKillToScore() {
		this.score++; 
	}

	public int getScore() {
		return score;
	}
}
