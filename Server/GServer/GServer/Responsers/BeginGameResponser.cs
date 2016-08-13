﻿using System;
using Proto;
using ServerUtility;
using XNet.Libs.Net;

namespace GServer.Responsers
{
    [HandleType(typeof(C2G_BeginGame))]
    public class BeginGameResponser:Responser<C2G_BeginGame,G2C_BeginGame>
    {
        public BeginGameResponser()
        {
            NeedAccess = true;
        }

        public override G2C_BeginGame DoResponse(C2G_BeginGame request, Client client)
        {
            var userID = (long)client.UserState;
            //var result = ErrorCode.Error;
            var req = Appliaction.Current.Client.CreateRequest<G2L_BeginBattle, L2G_BeginBattle>();
            req.RequestMessage.MapID = request.MapID;
            req.RequestMessage.UserID = userID;
           
            var response = new G2C_BeginGame
            {
                Code =  ErrorCode.Error
            };

            req.OnCompleted = (s, r) =>
            {
                response.Code = r.Code;
                if (r.Code == ErrorCode.OK)
                {
                    
                    response.ServerInfo = r.BattleServer;

                }
            };
            req.SendRequest();

            return response;

        }
    }
}

