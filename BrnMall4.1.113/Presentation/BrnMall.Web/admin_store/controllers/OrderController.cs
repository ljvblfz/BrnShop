using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using BrnMall.Core;
using BrnMall.Services;
using BrnMall.Web.Framework;
using BrnMall.Web.StoreAdmin.Models;

namespace BrnMall.Web.StoreAdmin.Controllers
{
    /// <summary>
    /// 店铺后台订单控制器类
    /// </summary>
    public partial class OrderController : BaseStoreAdminController
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

            string condition = AdminOrders.GetOrderListCondition(WorkContext.StoreId, osn, uid, consignee, orderState);

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
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&OSN={3}&AccountName={4}&Consignee={5}&OrderState={6}",
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
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");

            OrderInfoModel model = new OrderInfoModel();
            model.OrderInfo = orderInfo;
            model.RegionInfo = Regions.GetRegionById(orderInfo.RegionId);
            model.UserInfo = Users.GetUserById(orderInfo.Uid);
            model.UserRankInfo = AdminUserRanks.GetUserRankById(model.UserInfo.UserRid);
            model.OrderProductList = AdminOrders.GetOrderProductList(oid);
            model.OrderActionList = OrderActions.GetOrderActionList(oid);

            ViewData["referer"] = MallUtils.GetStoreAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 更新订单配送费用
        /// </summary>
        [HttpGet]
        public ActionResult UpdateOrderShipFee(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "不能修改订单配送费用");

            UpdateOrderShipFeeModel model = new UpdateOrderShipFeeModel();
            model.ShipFee = orderInfo.ShipFee;
            ViewData["orderInfo"] = orderInfo;
            return View(model);
        }

        /// <summary>
        /// 更新订单配送费用
        /// </summary>
        [HttpPost]
        public ActionResult UpdateOrderShipFee(UpdateOrderShipFeeModel model, int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "不能修改订单配送费用");

            if (ModelState.IsValid)
            {
                decimal change = model.ShipFee - orderInfo.ShipFee;
                Orders.UpdateOrderShipFee(orderInfo.Oid, model.ShipFee, orderInfo.OrderAmount + change, orderInfo.SurplusMoney + change);
                CreateOrderAction(oid, OrderActionType.UpdateShipFee, "您订单的配送费用已经修改");
                AddStoreAdminLog("更新订单配送费用", "更新订单配送费用,订单ID为:" + oid);

                if ((orderInfo.SurplusMoney + change) <= 0)
                    AdminOrders.UpdateOrderState(oid, OrderState.Confirming);

                return PromptView(Url.Action("orderinfo", new { oid = oid }), "更新订单配送费用成功");
            }

            ViewData["orderInfo"] = orderInfo;
            return View(model);
        }

        /// <summary>
        /// 更新订单折扣
        /// </summary>
        [HttpGet]
        public ActionResult UpdateOrderDiscount(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "不能修改订单折扣");

            UpdateOrderDiscountModel model = new UpdateOrderDiscountModel();
            model.Discount = orderInfo.Discount;
            ViewData["orderInfo"] = orderInfo;
            return View(model);
        }

        /// <summary>
        /// 更新订单折扣
        /// </summary>
        [HttpPost]
        public ActionResult UpdateOrderDiscount(UpdateOrderDiscountModel model, int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "不能修改订单折扣");

            if (model.Discount > (orderInfo.SurplusMoney + orderInfo.Discount))
                ModelState.AddModelError("Discount", "折扣不能大于需支付金额");

            if (ModelState.IsValid)
            {
                decimal surplusMoney = orderInfo.SurplusMoney + orderInfo.Discount - model.Discount;
                Orders.UpdateOrderDiscount(orderInfo.Oid, model.Discount, surplusMoney);
                CreateOrderAction(oid, OrderActionType.UpdateDiscount, "您订单的折扣已经修改");
                AddStoreAdminLog("更新订单折扣", "更新订单折扣,订单ID为:" + oid);

                if (surplusMoney <= 0)
                    AdminOrders.UpdateOrderState(oid, OrderState.Confirming);

                return PromptView(Url.Action("orderinfo", new { oid = oid }), "更新订单折扣成功");
            }
            ViewData["orderInfo"] = orderInfo;
            return View(model);
        }

        /// <summary>
        /// 确认订单
        /// </summary>
        public ActionResult ConfirmOrder(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (orderInfo.OrderState != (int)OrderState.Confirming)
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "买家还未付款，不能确认订单");

            AdminOrders.ConfirmOrder(orderInfo);
            CreateOrderAction(oid, OrderActionType.Confirm, "您的订单已经确认,正在备货中");
            AddStoreAdminLog("确认订单", "确认订单,订单ID为:" + oid);
            return PromptView(Url.Action("orderinfo", new { oid = oid }), "确认订单成功");
        }

        /// <summary>
        /// 备货
        /// </summary>
        public ActionResult PreOrderProduct(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (orderInfo.OrderState != (int)OrderState.Confirmed)
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未确认，请先确认");

            AdminOrders.PreProduct(orderInfo);
            CreateOrderAction(oid, OrderActionType.PreProduct, "您的订单已经备货完成");
            AddStoreAdminLog("备货", "备货,订单ID为:" + oid);
            return PromptView(Url.Action("orderinfo", new { oid = oid }), "备货成功");
        }

        /// <summary>
        /// 发货
        /// </summary>
        [HttpGet]
        public ActionResult SendOrderProduct(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (orderInfo.OrderState != (int)OrderState.PreProducting)
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未完成备货,不能发货");

            SendOrderProductModel model = new SendOrderProductModel();
            Load(orderInfo);
            return View(model);
        }

        /// <summary>
        /// 发货
        /// </summary>
        [HttpPost]
        public ActionResult SendOrderProduct(SendOrderProductModel model, int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (orderInfo.OrderState != (int)OrderState.PreProducting)
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单还未完成备货,不能发货");

            ShipCompanyInfo shipCompanyInfo = ShipCompanies.GetShipCompanyById(model.ShipCoId);
            if (shipCompanyInfo == null)
                ModelState.AddModelError("ShipCoId", "请选择配送公司");

            if (ModelState.IsValid)
            {
                AdminOrders.SendOrder(oid, OrderState.Sended, model.ShipSN, model.ShipCoId, shipCompanyInfo.Name, DateTime.Now);
                CreateOrderAction(oid, OrderActionType.Send, "您的订单已经发货,发货方式为:" + shipCompanyInfo.Name + "，单号为：" + model.ShipSN);
                AddStoreAdminLog("发货", "发货,订单ID为:" + oid);
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "发货成功");
            }
            Load(orderInfo);
            return View(model);
        }

        /// <summary>
        /// 锁定订单
        /// </summary>
        public ActionResult LockOrder(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能锁定");

            AdminOrders.LockOrder(orderInfo);
            CreateOrderAction(oid, OrderActionType.Lock, "订单已锁定");
            AddStoreAdminLog("锁定", "锁定,订单ID为:" + oid);
            return PromptView(Url.Action("orderinfo", new { oid = oid }), "锁定成功");
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        public ActionResult CancelOrder(int oid = -1)
        {
            OrderInfo orderInfo = AdminOrders.GetOrderByOid(oid);
            if (orderInfo == null)
                return PromptView("订单不存在");
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");
            if (!(orderInfo.OrderState == (int)OrderState.WaitPaying || (orderInfo.OrderState == (int)OrderState.Confirming && orderInfo.PayMode == 0)))
                return PromptView(Url.Action("orderinfo", new { oid = oid }), "订单当前不能取消");

            PartUserInfo partUserInfo = Users.GetPartUserById(orderInfo.Uid);
            AdminOrders.CancelOrder(ref partUserInfo, orderInfo, WorkContext.Uid, DateTime.Now);
            CreateOrderAction(oid, OrderActionType.Cancel, "订单已取消");
            AddStoreAdminLog("取消订单", "取消订单,订单ID为:" + oid);
            return PromptView(Url.Action("orderinfo", new { oid = oid }), "取消订单成功");
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
            if (orderInfo.StoreId != WorkContext.StoreId)
                return PromptView("不能操作其它店铺的订单");

            PrintOrderModel model = new PrintOrderModel()
            {
                OrderInfo = orderInfo,
                RegionInfo = Regions.GetRegionById(orderInfo.RegionId),
                OrderProductList = AdminOrders.GetOrderProductList(oid),
            };

            return View(model);
        }

        private void Load(OrderInfo orderInfo)
        {
            ViewData["orderInfo"] = orderInfo;
            List<SelectListItem> shipCompanyList = new List<SelectListItem>();
            shipCompanyList.Add(new SelectListItem() { Text = "请选择", Value = "0" });
            foreach (ShipCompanyInfo shipCompanyInfo in ShipCompanies.GetShipCompanyList())
            {
                shipCompanyList.Add(new SelectListItem() { Text = shipCompanyInfo.Name, Value = shipCompanyInfo.ShipCoId.ToString() });
            }
            ViewData["shipCompanyList"] = shipCompanyList;
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
            string condition = AdminOrderRefunds.GetOrderRefundListCondition(WorkContext.StoreId, osn);
            PageModel pageModel = new PageModel(pageSize, pageNumber, AdminOrderRefunds.GetOrderRefundCount(condition));

            OrderRefundListModel model = new OrderRefundListModel()
            {
                PageModel = pageModel,
                OrderRefundList = AdminOrderRefunds.GetOrderRefundList(pageModel.PageSize, pageModel.PageNumber, condition),
                OSN = osn
            };
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&OSN={3}",
                                                          Url.Action("refundlist"),
                                                          pageModel.PageNumber,
                                                          pageModel.PageSize, osn));


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

            string condition = AdminOrderAfterServices.GetOrderProductAfterServiceListCondition(WorkContext.StoreId, uid, oid, state, type, applyStartTime, applyEndTime);

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
            MallUtils.SetAdminRefererCookie(string.Format("{0}?pageNumber={1}&pageSize={2}&oid={3}&AccountName={4}&state={5}&type={6}&applyStartTime={7}&applyEndTime={8}",
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
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
                return PromptView("订单售后服务不存在");

            OrderAfterServiceModel model = new OrderAfterServiceModel();
            model.OrderAfterServiceInfo = orderAfterServiceInfo;
            model.RegionInfo = Regions.GetRegionById(orderAfterServiceInfo.RegionId);

            ViewData["referer"] = MallUtils.GetMallAdminRefererCookie();
            return View(model);
        }

        /// <summary>
        /// 审核订单售后服务
        /// </summary>
        [HttpGet]
        public ActionResult CheckOrderAfterService(int asId)
        {
            OrderAfterServiceInfo orderAfterServiceInfo = AdminOrderAfterServices.GetOrderAfterServiceByASId(asId);
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
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
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
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
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
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
                    StoreId = orderInfo.StoreId,
                    StoreName = orderInfo.StoreName,
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
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
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
            if (orderAfterServiceInfo == null || orderAfterServiceInfo.StoreId != WorkContext.StoreId)
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
