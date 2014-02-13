package simulation;

import com.smartfoxserver.v2.entities.User;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;
import Extension.SnowExtension;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;


// The main World server model. Contains players, items, and all the other needed world objects
public class World {

	// World bounds - to create random transforms
	public static final double minX = -35;
	public static final double maxX = 35;
	public static final double minZ = -70;
	public static final double maxZ = 5;

	private SnowExtension extension; // Reference to the server extension

	// Players
	private List<CombatPlayer> players = new ArrayList<CombatPlayer>();

	public World(SnowExtension extension) {
		this.extension = extension;
	}

	public List<CombatPlayer> getPlayers() {
		return players;
	}

	// Add new player if he doesn't exist, or resurrect him if he already added
	public void addPlayer(User user) {
                CombatPlayer player = new CombatPlayer(user);
                players.add(player);
                extension.clientInstantiatePlayer(player);
	}

	// Trying to move player. If the new transform is not valid, returns null
	public Transform movePlayer(User u, Transform newTransform) {
		CombatPlayer player = getPlayer(u);

		if (player.isDead()) {
			return player.getTransform();
		}

		if (isValidNewTransform(player, newTransform)) {
			player.getTransform().load(newTransform);

			return newTransform;
		}

		return null;
	}

	public Transform getTransform(User u) {
		CombatPlayer player = getPlayer(u);
		return player.getTransform();
	}

	private boolean isValidNewTransform(CombatPlayer player,
	                                    Transform newTransform) {

		// Check if the given transform is valid in terms of collisions, speed hacks etc
		// In this example, the server will always accept a new transform from the client

		return true;
	}

	// Gets the player corresponding to the specified SFS user
	public CombatPlayer getPlayer(User u) {
		for (CombatPlayer player : players) {
			if (player.getSfsUser().getId() == u.getId()) {
				return player;
			}
		}

		return null;
	}

	// Process the shot from client
	public void processShot(User fromUser, ISFSObject data) {
		CombatPlayer player = getPlayer(fromUser);
		if (player.isDead()) {
			return;
		}
                SFSObject answ = new SFSObject();
                answ.putDouble("sx", data.getDouble("sx"));
                answ.putDouble("sy", data.getDouble("sy"));
                answ.putDouble("sz", data.getDouble("sz"));
                answ.putDouble("ex", data.getDouble("ex"));
                answ.putDouble("ey", data.getDouble("ey"));
                answ.putDouble("ez", data.getDouble("ez"));
                answ.putInt("id", fromUser.getId());
		extension.clientEnemyShotFired(answ);

		// Determine the intersection of the shot line with any of the other players to check if we hit or missed
//		for (CombatPlayer pl : players) {
//			if (pl != player) {
//				boolean res = checkHit(player, pl);
//				if (res) {
//					playerHit(player, pl);
//					return;
//				}
//
//			}
//		}

		// if we are here - we missed
	}

	// Applying the hit to the player.
	// Processing the health and death
	public void playerHit(CombatPlayer fromPlayer, CombatPlayer pl, int health) {
		pl.removeHealth(health);

		if (pl.isDead()) {
                        //fromPlayer.addKillToScore();  // Adding frag to the player if he killed the enemy
			//extension.updatePlayerScore(fromPlayer);
			extension.clientKillPlayer(pl, fromPlayer);
		}
		else {
//			 Updating the health of the hit enemy
			extension.clientUpdateHealth(pl);
		}
	}

	// When user lefts the room or disconnects - removing him from the players list 
	public void userLeft(User user) {
		CombatPlayer player = this.getPlayer(user);
		if (player == null) {
			return;
		}
		players.remove(player);
	}

}
