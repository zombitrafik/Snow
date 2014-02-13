package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import helpers.RoomHelper;
import simulation.CombatPlayer;
import simulation.World;

//This request is sent when player shots
public class UpdateHealthHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
            World world = RoomHelper.getWorld(this);
            CombatPlayer to = world.getPlayer(u);
            String name = data.getUtfString("shooterName");
            int damage = data.getInt("damage");
            CombatPlayer from = world.getPlayer(RoomHelper.getCurrentRoom(this).getUserByName(name));
            world.playerHit(from, to, damage);
	}

}
