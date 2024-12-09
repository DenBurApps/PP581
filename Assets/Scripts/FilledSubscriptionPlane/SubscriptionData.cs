using System;

[Serializable]
public class SubscriptionData
{
    public string ServiceName;
    public string Date;
    public string NextPaymentDate;
    public string Price;
    public string Tariff;
    public bool IsArchived;

    public SubscriptionData(string serviceName, string date, string nextPaymentDate, string price, string tariff)
    {
        ServiceName = serviceName;
        Date = date;
        NextPaymentDate = nextPaymentDate;
        Price = price;
        Tariff = tariff;
    }
}
