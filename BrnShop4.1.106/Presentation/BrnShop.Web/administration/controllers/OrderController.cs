using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnShop.Core;
using BrnShop.Services;
using BrnShop.Web.Framework;
using BrnShop.Web.Admin.Models;

namespace BrnShop.Web.Admin.Controllers
{
    /// <summary>
    /// 后台订单控制器类
    /// </summary>
    public partial class OrderController : BaseAdminController
    {
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <param name="accountName">账户名</param>
        /// <param name="consignee">收货人</param>
        /// <param name="orderState">订单状态</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public ActionResult OrderList(string osn, string accountName, string consignee, int orderState = 0, int pageSize = 15, int pageNumber = 1)
        {
            //获取用户id
            int uid = Users.GetUidByAccountName(accountName);

            string condition = AdminOrders.GetOrderListCondition(osn, uid, consignee, orderState);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrders.GetOrderCount(condition));

            List<SelectListItem> orderStateList = new List<SelectListItem>();
            orderStateList.Add(new SelectListItem() { Text = "全部", Value = "0" });
            orderStateList.Add(new SelectListItem() { Text = "等待付款", Value = ((int)OrderState.WaitPaying).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "待确认", Value = ((int)OrderState.Confirming).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已确认", Value = ((int)OrderState.Confirmed).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "备货中", Value = ((int)OrderState.PreProducting).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已发货", Value = ((int)OrderState.Sended).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已收货", Value = ((int)OrderState.Received).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已锁定", Value = ((int)OrderState.Locked).ToString() });
            orderStateList.Add(new SelectListItem() { Text = "已取消", Value = ((int)OrderState.Cancelled).ToString() });

            OrderListModel model = new OrderListModel()
            {
                OrderList = AdminOrders.GetOrderList(pageModel.PageSize, pageModel.PageNumber, condition),
                PageModel = pageModel,
                OSN = osn,
                AccountName = accountName,
                Consignee = consignee,
                OrderState = orderState,
                OrderStateList = orderStateList
            };
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&OSN={3}&AccountName={4}&Consignee={5}&OrderState={6}",
                                                          Url.Action("orderlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          osn, accountName, consignee, orderState));

            return View(model);
        }

        /// <summary>
        /// 订单信息
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public ActionResult OrderInfo(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");

            OrderInfoModel model = new OrderInfoModel();
            model.OrderInfo = orderInfo;
            model.RegionInfo = Regions.GetRegionById(orderInfo.RegionId);
            model.UserInfo = Users.GetUserById(orderInfo.Uid);
            model.UserRankInfo = AdminUserRanks.GetUserRankById(model.UserInfo.UserRid);
            model.OrderProductList = AdminOrders.GetOrderProductList(oid);
            model.OrderActionList = OrderActions.GetOrderActionList(oid);

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 操作订单
        /// </summary>
        [HttpGet]
        public ActionResult OperateOrder(int oid = -1, int actionType = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");

            OrderActionType orderActionType = (OrderActionType)actionType;
            OrderState orderState = (OrderState)orderInfo.OrderState;

            if (orderActionType == OrderActionType.Confirm)//确认订单
            {
                if (orderState != OrderState.Confirming)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "买家还未付款，不能确认订单");
            }
            else if (orderActionType == OrderActionType.PreProduct)//备货
            {
                if (orderState != OrderState.Confirmed)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未确认，不能备货");
            }
            else if (orderActionType == OrderActionType.Send)//发货
            {
                if (orderState != OrderState.PreProducting)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未备货，不能发货");
            }
            else if (orderActionType == OrderActionType.Lock)//锁定订单
            {
                if (!(orderState == OrderState.WaitPaying || (orderState == OrderState.Confirming && orderInfo.PayMode == 0)))
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能锁定");
            }
            else if (orderActionType == OrderActionType.Cancel)//取消订单
            {
                if (!(orderState == OrderState.WaitPaying || (orderState == OrderState.Confirming && orderInfo.PayMode == 0)))
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能取消");
            }
            else
            {
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "当前操作不存在");
            }

            OperateOrderModel model = new OperateOrderModel();
            model.Oid = oid;
            model.OrderInfo = orderInfo;
            model.OrderActionType = orderActionType;
            model.ActionDes = "";

            return View(model);
        }

        /// <summary>
        /// 操作订单
        /// </summary>
        [HttpPost]
        public ActionResult OperateOrder(int oid = -1, int actionType = -1, string actionDes = "")
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");

            if (actionDes.Length > 125)
            {
                OperateOrderModel model = new OperateOrderModel();
                model.Oid = oid;
                model.OrderInfo = orderInfo;
                model.OrderActionType = (OrderActionType)actionType;
                model.ActionDes = actionDes;

                ModelState.AddModelError("actionDes", "最多只能输入125个字");
                return View(model);
            }

            OrderActionType orderActionType = (OrderActionType)actionType;
            OrderState orderState = (OrderState)orderInfo.OrderState;

            if (orderActionType == OrderActionType.Confirm)//确认订单
            {
                if (orderState != OrderState.Confirming)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "买家还未付款，不能确认订单");

                AdminOrders.ConfirmOrder(orderInfo);
                CreateOrderAction(oid, orderActionType, actionDes.Length == 0 ? "您的订单已经确认" : actionDes);
            }
            else if (orderActionType == OrderActionType.PreProduct)//备货
            {
                if (orderState != OrderState.Confirmed)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未确认，不能备货");

                AdminOrders.PreProduct(orderInfo);
                CreateOrderAction(oid, orderActionType, actionDes.Length == 0 ? "您的订单正在备货" : actionDes);
            }
            else if (orderActionType == OrderActionType.Send)//发货
            {
                if (orderState != OrderState.PreProducting)
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未备货，不能发货");

                string shipSN = WebHelper.GetFormString("shipSN").Trim();
                if (shipSN.Length < 1)
                {
                    OperateOrderModel model = new OperateOrderModel();
                    model.Oid = oid;
                    model.OrderInfo = orderInfo;
                    model.OrderActionType = orderActionType;
                    model.ActionDes = actionDes;

                    ModelState.AddModelError("shipSN", "请填写配送单号");
                    return View(model);
                }
                AdminOrders.SendOrder(oid, OrderState.Sended, shipSN, DateTime.Now);
                CreateOrderAction(oid, orderActionType, actionDes.Length == 0 ? "您的订单已经发货,发货方式为:" + orderInfo.ShipFriendName + "，单号为：" + shipSN : actionDes);
            }
            else if (orderActionType == OrderActionType.Lock)//锁定订单
            {
                if (!(orderState == OrderState.WaitPaying || (orderState == OrderState.Confirming && orderInfo.PayMode == 0)))
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能锁定");

                AdminOrders.LockOrder(orderInfo);
                CreateOrderAction(oid, orderActionType, "订单已锁定：" + actionDes);
            }
            else if (orderActionType == OrderActionType.Cancel)//取消订单
            {
                if (!(orderState == OrderState.WaitPaying || (orderState == OrderState.Confirming && orderInfo.PayMode == 0)))
                    return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能取消");

                PartUserInfo partUserInfo = Users.GetPartUserById(orderInfo.Uid);
                AdminOrders.CancelOrder(ref partUserInfo, orderInfo, WorkContext.Uid, DateTime.Now);
                CreateOrderAction(oid, orderActionType, actionDes.Length == 0 ? "订单已取消" : actionDes);
            }
            else
            {
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "当前操作不存在");
            }

            AddAdminOperateLog("操作订单", "操作订单,订单ID为:" + oid);
            return PromptView(Url.Action("orderinfo", new { oid = oid }), "操作已完成");
        }

        /// <summary>
        /// 打印订单
        /// </summary>
        /// <param name="oid">订单id</param>
        /// <returns></returns>
        public ActionResult PrintOrder(int oid)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");

            PrintOrderModel model = new PrintOrderModel()
            {
                OrderInfo = orderInfo,
                RegionInfo = Regions.GetRegionById(orderInfo.RegionId),
                OrderProductList = AdminOrders.GetOrderProductList(oid),
                AdminRealName = AdminUsers.GetUserDetailById(WorkContext.Uid).RealName
            };

            return View(model);
        }

        /// <summary>
        /// 创建订单行为
        /// </summary>
        private void CreateOrderAction(int oid, OrderActionType orderActionType, string actionDes)
        {
            OrderActions.CreateOrderAction(new OrderActionInfo()
            {
                Oid = oid,
                Uid = WorkContext.Uid,
                RealName = AdminUsers.GetUserDetailById(WorkContext.Uid).RealName,
                AdminGid = WorkContext.AdminGid,
                AdminGTitle = WorkContext.AdminGTitle,
                ActionType = (int)orderActionType,
                ActionTime = DateTime.Now,
                ActionDes = actionDes
            });
        }





        /// <summary>
        /// 订单退款列表
        /// </summary>
        /// <param name="osn">订单编号</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public ActionResult RefundList(string osn, int pageSize = 15, int pageNumber = 1)
        {
            string condition = AdminOrderRefunds.GetOrderRefundListCondition(osn);
            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrderRefunds.GetOrderRefundCount(condition));

            OrderRefundListModel model = new OrderRefundListModel()
            {
                PageModel = pageModel,
                OrderRefundList = AdminOrderRefunds.GetOrderRefundList(pageModel.PageSize, pageModel.PageNumber, condition),
                OSN = osn
            };
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&OSN={3}",
                                                          Url.Action("refundlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize,
                                                          osn));


            return View(model);
        }





        /// <summary>
        /// 订单售后服务列表
        /// </summary>
        /// <param name="accountName">账户名</param>
        /// <param name="applyStartTime">申请开始时间</param>
        /// <param name="applyEndTime">申请结束时间</param>
        /// <param name="oid">订单id</param>
        /// <param name="state">状态</param>
        /// <param name="type">类型</param>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public ActionResult AfterServiceList(string accountName, string applyStartTime, string applyEndTime, int oid = 0, int state = -1, int type = -1, int pageSize = 15, int pageNumber = 1)
        {
            //获取用户id
            int uid = Users.GetUidByAccountName(accountName);

            string condition = AdminOrderAfterServices.GetOrderProductAfterServiceListCondition(uid, oid, state, type, applyStartTime, applyEndTime);

            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrderAfterServices.GetOrderProductAfterServiceCount(condition));

            List<SelectListItem> stateList = new List<SelectListItem>();
            stateList.Add(new SelectListItem() { Text = "全部", Value = "-1" });
            stateList.Add(new SelectListItem() { Text = "审核中", Value = ((int)OrderAfterServiceState.Checking).ToString() });
            stateList.Add(new SelectListItem() { Text = "审核通过", Value = ((int)OrderAfterServiceState.CheckAgree).ToString() });
            stateList.Add(new SelectListItem() { Text = "审核拒绝", Value = ((int)OrderAfterServiceState.CheckRefuse).ToString() });
            stateList.Add(new SelectListItem() { Text = "客户已邮寄", Value = ((int)OrderAfterServiceState.Sended).ToString() });
            stateList.Add(new SelectListItem() { Text = "商城已收货", Value = ((int)OrderAfterServiceState.Received).ToString() });
            stateList.Add(new SelectListItem() { Text = "商城已发货", Value = ((int)OrderAfterServiceState.Backed).ToString() });
            stateList.Add(new SelectListItem() { Text = "完成", Value = ((int)OrderAfterServiceState.Completed).ToString() });

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "全部", Value = "-1" });
            typeList.Add(new SelectListItem() { Text = "退货", Value = "0" });
            typeList.Add(new SelectListItem() { Text = "换货", Value = "1" });
            typeList.Add(new SelectListItem() { Text = "维修", Value = "2" });

            OrderAfterServiceListModel model = new OrderAfterServiceListModel()
            {
                PageModel = pageModel,
                OrderAfterServiceList = AdminOrderAfterServices.GetOrderProductAfterServiceList(pageModel.PageSize, pageModel.PageNumber, condition),
                Oid = oid,
                AccountName = accountName,
                State = state,
                StateList = stateList,
                Type = type,
                TypeList = typeList,
                ApplyStartTime = applyStartTime,
                ApplyEndTime = applyEndTime
            };
            ShopUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&oid={3}&AccountName={4}&state={5}&type={6}&applyStartTime={7}&applyEndTime={8}",
                                                            Url.Action("afterservicelist"),
                                                            pageModel.PageNumber, pageModel.PageSize,
                                                            oid, accountName, state, type, applyStartTime, applyEndTime));

            return View(model);
        }

        /// <summary>
        /// 订单售后服务信息
        /// </summary>
        /// <param name="asId">售后服务id</param>
        /// <returns></returns>
        public ActionResult OrderAfterServiceInfo(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");

            OrderAfterServiceModel model = new OrderAfterServiceModel();
            model.OrderAfterServiceInfo = orderAfterServiceInfo;
            model.RegionInfo = Regions.GetRegionById(orderAfterServiceInfo.RegionId);

            ViewData["referer"] = ShopUtils.GetAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        [HttpGet]
        public ActionResult CheckOrderAfterService(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");
            if (orderAfterServiceInfo.State != (int)OrderAfterServiceState.Checking)
                return PromptView("不能执行此操作");

            CheckOrderAfterServiceModel model = new CheckOrderAfterServiceModel();
            ViewData["asId"] = asId;
            return View(model);
        }

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        [HttpPost]
        public ActionResult CheckOrderAfterService(int asId, CheckOrderAfterServiceModel model)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");
            if (orderAfterServiceInfo.State != (int)OrderAfterServiceState.Checking)
                return PromptView("不能执行此操作");

            if (ModelState.IsValid)
            {
                if (model.State == 0)
                    AdminOrderAfterServices.CheckRefuseOrderAfterService(asId, model.CheckResult ?? "", DateTime.Now);
                else
                    AdminOrderAfterServices.CheckAgreeOrderAfterService(asId, model.CheckResult ?? "", DateTime.Now);
                return PromptView("操作成功");
            }
            ViewData["asId"] = asId;
            return View(model);
        }

        /// <summary>
        /// 商城收到客户邮寄的商品
        /// </summary>
        public ActionResult ReceiveOrderAfterService(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");
            if (orderAfterServiceInfo.State != (int)OrderAfterServiceState.Sended)
                return PromptView("不能执行此操作");

            if (orderAfterServiceInfo.Type == 0)
            {
                AdminOrderAfterServices.CompleteOrderAfterService(asId);
                OrderInfo orderInfo = AdminOrders.GetOrderByOid(orderAfterServiceInfo.Oid);
                OrderRefunds.ApplyRefund(new OrderRefundInfo()
                {
                    State = (int)OrderRefundState.Applied,
                    Oid = orderInfo.Oid,
                    OSN = orderInfo.OSN,
                    Uid = orderInfo.Uid,
                    ASId = orderAfterServiceInfo.ASId,
                    PaySystemName = orderInfo.PaySystemName,
                    PayFriendName = orderInfo.PayFriendName,
                    PaySN = orderInfo.PaySN,
                    PayMoney = orderAfterServiceInfo.Money,
                    RefundMoney = orderAfterServiceInfo.Money,
                    ApplyTime = DateTime.Now
                });
            }
            else
            {
                AdminOrderAfterServices.ReceiveOrderAfterService(asId, DateTime.Now);
            }
            return PromptView("操作成功");
        }

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        [HttpGet]
        public ActionResult BackOrderAfterService(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");
            if (orderAfterServiceInfo.State != (int)OrderAfterServiceState.Received)
                return PromptView("不能执行此操作");

            BackOrderAfterServiceModel model = new BackOrderAfterServiceModel();

            ViewData["asId"] = asId;
            ViewData["orderAfterServiceInfo"] = orderAfterServiceInfo;
            List<SelectListItem> shipCompanyList = new List<SelectListItem>();
            shipCompanyList.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            foreach (ShipCompanyInfo item in ShipCompanies.GetShipCompanyList())
            {
                shipCompanyList.Add(new SelectListItem() { Text = item.Name, Value = item.ShipCoId.ToString() });
            }
            ViewData["shipCompanyList"] = shipCompanyList;
            return View(model);
        }

        /// <summary>
        /// 邮寄给客户
        /// </summary>
        [HttpPost]
        public ActionResult BackOrderAfterService(int asId, BackOrderAfterServiceModel model)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null)
                return PromptView("订单售后服务不存在");
            if (orderAfterServiceInfo.State != (int)OrderAfterServiceState.Received)
                return PromptView("不能执行此操作");

            ShipCompanyInfo shipCompanyInfo = ShipCompanies.GetShipCompanyById(model.ShipCoId);
            if (shipCompanyInfo == null)
                ModelState.AddModelError("ShipCoId", "配送公司不存在");

            if (ModelState.IsValid)
            {
                AdminOrderAfterServices.BackOrderAfterService(asId, model.ShipCoId, shipCompanyInfo.Name, model.ShipSN);
                return PromptView("操作成功");
            }

            ViewData["asId"] = asId;
            ViewData["orderAfterServiceInfo"] = orderAfterServiceInfo;
            ViewData["orderAfterServiceInfo"] = orderAfterServiceInfo;
            List<SelectListItem> shipCompanyList = new List<SelectListItem>();
            shipCompanyList.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            foreach (ShipCompanyInfo item in ShipCompanies.GetShipCompanyList())
            {
                shipCompanyList.Add(new SelectListItem() { Text = item.Name, Value = item.ShipCoId.ToString() });
            }
            ViewData["shipCompanyList"] = shipCompanyList;
            return View(model);
        }
    }
}
