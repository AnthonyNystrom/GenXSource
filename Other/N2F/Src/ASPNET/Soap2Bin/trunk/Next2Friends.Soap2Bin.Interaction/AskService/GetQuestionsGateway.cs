/* ------------------------------------------------
 * GetQuestionsGateway.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Next2Friends.Soap2Bin.Interaction.AskService
{
    sealed class GetQuestionsGateway : IGateway
    {
        private AskQuestion[] _result;

        public void Invoke(System.Web.SessionState.HttpSessionState session, Next2Friends.Soap2Bin.Core.DataInputStream input)
        {
            _result = HttpProcessor.GetClient<AskServiceSoapClient>(session).GetQuestions(
                input.ReadString(),
                input.ReadString());
        }

        public void Return(Next2Friends.Soap2Bin.Core.DataOutputStream output)
        {
            output.WriteGetQuestionsResult(_result);
        }
    }
}
