package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import helpers.RoomHelper;

//This request is sent when player shots
public class ShotHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
		RoomHelper.getWorld(this).processShot(u,data);

	}

}
