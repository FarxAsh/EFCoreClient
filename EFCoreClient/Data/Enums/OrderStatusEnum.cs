using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreClient.Data.Enums
{
    public enum OrderStatusEnum
    {
        InStock = 1,
        RecievedByCourier = 2,
        Delivered = 3,
        Rejected = 4
    }
}
