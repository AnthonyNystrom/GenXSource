using System;
using System.Collections.Generic;
using System.Text;

namespace CVLog
{
    interface IRepositoryInterface
    {
        //delegate void RepositoryEventHandler(IRepositoryInterface _interface);

        //event RepositoryEventHandler OnConnectionLost;

        void GetAllModules();

    }
}