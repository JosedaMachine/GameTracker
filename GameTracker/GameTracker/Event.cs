using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    enum CaseType
    {
        START,
        EVENT,
        STOP,
    }

    struct StartArgs
    {
        public StartArgs(string userName, string sessionID, string gameID)
        {
            userName_ = userName;
            sessionID_ = sessionID;
            gameID_ = gameID;
        }

        public string userName_, sessionID_, gameID_;
    }
    struct EventArgs
    {
        public EventArgs(string eventDescription)
        {
            eventDescription_ = eventDescription;
        }

        public string eventDescription_;
    }

    struct Case_Union {
        public Case_Union() {

        }

        public Case_Union(Case_Union other) {
            startParams = other.startParams;
            eventArgs = other.eventArgs;
        }

        public StartArgs startParams;
        public EventArgs eventArgs;
    }

    internal class Event {
        public CaseType caseType_;
        public Case_Union case_;
    }
}
