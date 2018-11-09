﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Common
{
    public interface IShowMessage
    {
        void ShowMessage(string message);

        bool Confirm(string message, out bool cancelled);
    }
}
