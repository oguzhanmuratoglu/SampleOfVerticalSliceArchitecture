using System;
using TaskForMoodivationStack.WebApi.Contexts;

namespace TaskForMoodivationStack.WebApi.Domain.Services;

public class OrderNumberService(ApplicationDbContext context)
{

    public string GetNewOrderNumber()
    {
        string initialLetter = "OGZHN";
        string year = DateTime.Now.Year.ToString();
        string newOrderNumber = initialLetter + year;

        var lastOrder = context.Orders.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
        string currentOrderNumber = lastOrder?.OrderNumber;

        if (currentOrderNumber != null)
        {
            string currentYear = currentOrderNumber.Substring(3, 4);
            int startIndex = (currentYear == year) ? 7 : 0;
            GenerateUniqueOrderNumber(ref newOrderNumber, currentOrderNumber.Substring(startIndex));
        }
        else
        {
            newOrderNumber += "000000001";
        }

        return newOrderNumber;
    }

    private void GenerateUniqueOrderNumber(ref string newOrderNumber, string currentOrderNumStr)
    {
        int currentOrderNumberInt = int.TryParse(currentOrderNumStr, out var num) ? num : 0;
        bool isOrderNumberUnique = false;

        while (!isOrderNumberUnique)
        {
            currentOrderNumberInt++;
            newOrderNumber += currentOrderNumberInt.ToString("D9");
            string checkOrderNumber = newOrderNumber;

            var order = context.Orders.FirstOrDefault(o => o.OrderNumber == checkOrderNumber);
            if (order == null)
            {
                isOrderNumberUnique = true;
            }
        }
    }
}

