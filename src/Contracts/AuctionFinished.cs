using System;

namespace Contracts;

//Start : Section 6 GateWay SVC
public class AuctionFinished
{
    public bool ItemSold { get; set; }
    public string AuctionId { get; set; }
    public string Winner { get; set; }
    public string Seller { get; set; }
    public int? Amount { get; set; }
}
//End : Section 6 GateWay SVC
