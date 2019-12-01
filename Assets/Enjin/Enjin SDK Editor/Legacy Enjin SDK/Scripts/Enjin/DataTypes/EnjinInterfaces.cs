using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLegacyEnjinSDK
{
    public interface IErrorHandle
    {
        void SetErrorDetails(int code, string description);
    }

    public interface IEnjinIdentity
    {

    }
}