/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Extension;

/**
 *
 * @author Kimreik
 */
import Handlers.ChatMessageHandler;
import Handlers.GetTimeHandler;
import Handlers.LoginEventHandler;
import Handlers.NewPlayerRequestHandler;
import Handlers.OnUserGoneHandler;
import Handlers.SendTransformHandler;
import Handlers.ShotHandler;
import Handlers.UpdateHealthHandler;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.SFSExtension;
import helpers.RoomHelper;
import helpers.UserHelper;
import java.util.List;
import simulation.CombatPlayer;
import simulation.World;

public class SnowExtension extends SFSExtension{

    private World world; // Reference to World simulation model

	public World getWorld() {
		return world;
	}

  @Override
  public void init() {

    world = new World(this);

    addEventHandler(SFSEventType.USER_LOGIN, LoginEventHandler.class);
    addRequestHandler("sendTransform", SendTransformHandler.class);
    addRequestHandler("NewPlayer", NewPlayerRequestHandler.class);
    addRequestHandler("getTime", GetTimeHandler.class);
    addRequestHandler("shot", ShotHandler.class);
    addRequestHandler("updateHealth", UpdateHealthHandler.class);
    addRequestHandler("chatMessage", ChatMessageHandler.class);
      
    addEventHandler(SFSEventType.USER_DISCONNECT, OnUserGoneHandler.class);
    addEventHandler(SFSEventType.USER_LEAVE_ROOM, OnUserGoneHandler.class);
    addEventHandler(SFSEventType.USER_LOGOUT, OnUserGoneHandler.class);
  }

  public void clientInstantiatePlayer(CombatPlayer player) {
		clientInstantiatePlayer(player, null);
	}

	//Send the player instantiation message to all the clients or to a specified user only
    public void clientInstantiatePlayer(CombatPlayer player, User targetUser) {
            ISFSObject data = new SFSObject();

            player.toSFSObject(data);
            Room currentRoom = RoomHelper.getCurrentRoom(this);
            if (targetUser == null) {
                    // Sending to all the users
                    List<User> userList = UserHelper.getRecipientsList(currentRoom);
                    this.send("spawnPlayer", data, userList);
            }
            else {
                    // Sending to the specified user
                    this.send("spawnPlayer", data, targetUser);
            }
    }
    
    public void updatePlayerScore(CombatPlayer pl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("score", pl.getScore());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("score", data, userList);
    }

    public void clientKillPlayer(CombatPlayer pl, CombatPlayer killerPl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("killerId", killerPl.getSfsUser().getId());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("killed", data, userList);
    }

    public void clientEnemyShotFired(SFSObject answ) {
		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("enemyShotFired", answ, userList);
    }

    public void clientUpdateHealth(CombatPlayer pl) {
		ISFSObject data = new SFSObject();
		data.putInt("id", pl.getSfsUser().getId());
		data.putInt("health", pl.getHealth());

		Room currentRoom = RoomHelper.getCurrentRoom(this);
		List<User> userList = UserHelper.getRecipientsList(currentRoom);
		this.send("health", data, userList);
    }


}