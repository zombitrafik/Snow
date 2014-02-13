/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package Handlers;

import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.bitswarm.sessions.Session;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
/**
 *
 * @author Kimreik
 */
public class LoginEventHandler extends BaseServerEventHandler
{
   @Override

    public void handleServerEvent(ISFSEvent event) throws SFSException
    {
        String name = (String) event.getParameter(SFSEventParam.LOGIN_NAME);
        String bdpassword = null;
        String password = (String) event.getParameter(SFSEventParam.LOGIN_PASSWORD);
        ISession session = (Session) event.getParameter(SFSEventParam.SESSION);
        Zone zone = (Zone)event.getParameter(SFSEventParam.ZONE);
         IDBManager dbManager = zone.getDBManager();
         Connection connection = null;
         PreparedStatement stmt = null;
         ResultSet res = null;
         try {
             connection = dbManager.getConnection();
             stmt = connection.prepareStatement("SELECT Password FROM users WHERE Login = ?");
             stmt.setString(1, name);

// Execute query
        res = stmt.executeQuery();
        if(res.next()){
            bdpassword=res.getString("Password");
        }
               } catch (SQLException ex) {
                   trace(ex);
               }finally{
                   try{
                       connection.close();
                       res.close();
                       stmt.close();
                       trace("success");
                   }catch(Exception e){
                       e.printStackTrace();
                   }
         }
        if (!getApi().checkSecurePassword(session, bdpassword, password))
        {
            SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
            data.addParameter(name);
            throw new SFSLoginException("Login failed for user: "  + name, data);
        }
    }
}

