﻿using System;
using LoginServer.Managers;
using Proto;
using XNet.Libs.Net;

namespace LoginServer.Responsers
{
    [ServerUtility.HandleType(typeof(B2L_RegBattleServer))]
    public class BattleServerRegResponser :ServerUtility.Responser<B2L_RegBattleServer,L2B_RegBattleServer>
    {
        public BattleServerRegResponser()
        {
            NeedAccess = false;
        }

        public override L2B_RegBattleServer DoResponse(B2L_RegBattleServer request, Client client)
        {

            var server = new GameServerInfo
            {
                Host = request.ServiceHost,
                Port = request.ServicePort,
                ServerID = -1,
                MaxServiceCount = request.MaxBattleCount
            };

            var serverIndex = ServerManager.Singleton.AddBattleServer(client.ID, server);
            server.ServerID = serverIndex;
            client.UserState = serverIndex;
            client.HaveAdmission = true;
            client.OnDisconnect += UnRegWhenDisconnect;
            return new L2B_RegBattleServer { Code = ErrorCode.OK, ServiceServerID = serverIndex };
        }

        public static void UnRegWhenDisconnect(object sender, EventArgs e)
        {
            var client = sender as Client;
            var serverID = (int)client.UserState;
            ServerManager.Singleton.RemoveBattleServer(serverID);
        }
    }
}

