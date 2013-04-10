using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Security.Authentication;

namespace WellAuditService
{
    public class CustomValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            // authenticate user
            if (!(userName == "test" && password == "test"))
            {
                // This throws an informative fault to the client.
                throw new FaultException("SecurityFailed");
            }

            var ok = (userName == "Ole") && (password == "Pwd");
            if (ok == false)
                throw new AuthenticationException("u/p does not match");
        }

    }
}