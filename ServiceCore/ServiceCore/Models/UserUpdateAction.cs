using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCore.Models
{
    public class UserUpdateAction
    {
        public enum Actions
        {
            None,
            ChangeEmail,
            ChangeCallsign,
            AddCallsign,
            RemoveCallsign,
            ChangePassword,
            VerifyEmail,
            RecoverPassword,
        }
        public Actions EditAction = Actions.None;
        public string OldValue = string.Empty;
        public string NewValue = string.Empty;

        public string Token = string.Empty;
    }
}
