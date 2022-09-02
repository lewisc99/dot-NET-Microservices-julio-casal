using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Common.Settings
{
    public class RabbitMQSettings
    {


        //as init as opposed to set because and
        // nobody should be setting these these properties after they have been uh deserialized from the configuration file
        public string Host { get; init; }

        }
}
