using System;
using System.Data;

using BrnMall.Core;
using BrnMall.Services;

namespace BrnMall.EventStrategy.Timer
{
    /// <summary>
    /// 店铺评价事件
    /// </summary>
    public class StoreReviewEvent : IEvent
    {
        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventInfo">事件信息</param>
        public void Execute(object eventInfo)
        {
            EventInfo e = (EventInfo)eventInfo;

            //计算店铺评价
            DateTime startTime = DateTime.Now.AddDays(-30);
            DateTime endTime = DateTime.Now;

            DataTable storeIdList = Stores.GetStoreIdList();
            foreach (DataRow storeIdRow in storeIdList.Rows)
            {
                int flag = 1;
                int descriptionStar1 = 0, descriptionStar2 = 0, descriptionStar3 = 0, descriptionStar4 = 0, descriptionStar5 = 0;
                int serviceStar1 = 0, serviceStar2 = 0, serviceStar3 = 0, serviceStar4 = 0, serviceStar5 = 0;
                int shipStar1 = 0, shipStar2 = 0, shipStar3 = 0, shipStar4 = 0, shipStar5 = 0;

                int storeId = TypeHelper.ObjectToInt(storeIdRow["storeid"]);
                DataTable storeReview = Stores.SumStoreReview(storeId, startTime, endTime);
                foreach (DataRow storeReviewRow in storeReview.Rows)
                {
                    int star = TypeHelper.ObjectToInt(storeReviewRow["star"]);
                    int count = TypeHelper.ObjectToInt(storeReviewRow["count"]);
                    if (star == 0)
                        flag++;

                    if (flag == 1)
                    {
                        switch (star)
                        {
                            case 1:
                                descriptionStar1 = count;
                                break;
                            case 2:
                                descriptionStar2 = count;
                                break;
                            case 3:
                                descriptionStar3 = count;
                                break;
                            case 4:
                                descriptionStar4 = count;
                                break;
                            case 5:
                                descriptionStar5 = count;
                                break;
                        }
                    }
                    else if (flag == 2)
                    {
                        switch (star)
                        {
                            case 1:
                                serviceStar1 = count;
                                break;
                            case 2:
                                serviceStar2 = count;
                                break;
                            case 3:
                                serviceStar3 = count;
                                break;
                            case 4:
                                serviceStar4 = count;
                                break;
                            case 5:
                                serviceStar5 = count;
                                break;
                        }
                    }
                    else if (flag == 3)
                    {
                        switch (star)
                        {
                            case 1:
                                shipStar1 = count;
                                break;
                            case 2:
                                shipStar2 = count;
                                break;
                            case 3:
                                shipStar3 = count;
                                break;
                            case 4:
                                shipStar4 = count;
                                break;
                            case 5:
                                shipStar5 = count;
                                break;
                        }
                    }

                }
                decimal dePoint = 10.00m;
                int totalDescriptionStar = descriptionStar1 + descriptionStar2 + descriptionStar3 + descriptionStar4 + descriptionStar5;
                if (totalDescriptionStar != 0)
                    dePoint = (descriptionStar3 + descriptionStar4 + descriptionStar5) / totalDescriptionStar;
                decimal sePoint = 10.00m;
                int totalServiceStar = serviceStar1 + serviceStar2 + serviceStar3 + serviceStar4 + serviceStar5;
                if (totalServiceStar != 0)
                    sePoint = (serviceStar3 + serviceStar4 + serviceStar5) / totalServiceStar;
                decimal shPoint = 10.00m;
                int totalShipStar = shipStar1 + shipStar2 + shipStar3 + shipStar4 + shipStar5;
                if (totalShipStar != 0)
                    sePoint = (shipStar3 + shipStar4 + shipStar5) / totalShipStar;
                Stores.UpdateStorePoint(storeId, dePoint, sePoint, shPoint);
            }

            EventLogs.CreateEventLog(e.Key, e.Title, Environment.MachineName, DateTime.Now);
        }
    }
}
