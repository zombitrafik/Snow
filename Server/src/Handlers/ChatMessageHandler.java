package Handlers;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import helpers.RoomHelper;

//This request is sent when player shots
public class ChatMessageHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User u, ISFSObject data) {
            SFSObject answ = new SFSObject();
            answ.putUtfString("message", u.getName()+": "+data.getUtfString("message")+"\n");
            send("chatMessage", answ, RoomHelper.getCurrentRoom(this).getUserList());
	}

}
