﻿using System;
using ServerUtility;
using Proto;
using XNet.Libs.Net;

namespace LoginServer.Responsers
{
    [HandleType(typeof(B2L_CheckSession))]
    public class BattleServerCheckSessionResponser:Responser<B2L_CheckSession,L2B_CheckSession>
    {
        public BattleServerCheckSessionResponser()
        {
            NeedAccess = true;
        }

        public override L2B_CheckSession DoResponse(B2L_CheckSession request, Client client)
        {
            if (Appliaction.Current.GetSession(request.UserID) == request.SessionKey)
            {
                return new L2B_CheckSession { Code = ErrorCode.OK };
            }
            else {
                return new L2B_CheckSession { Code = ErrorCode.Error };
            }
        }
    }
}

