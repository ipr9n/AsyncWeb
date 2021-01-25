using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace v1web.Models
{
    public class CancelationModel
    {

        public CancellationTokenSource TokenSource { get; }
        public string UserId { get; set; }
        public ActionTypeEnum ActionType { get; set; }

        public CancelationModel()
        {
            TokenSource = new CancellationTokenSource();
        }

    }
}