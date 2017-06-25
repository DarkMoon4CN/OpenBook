using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public enum PaginationTypes
    {
        /// <summary>
        /// 无分页
        /// </summary>
        None = 0,
        /// <summary>
        /// 经典翻页模式
        /// </summary>
        Classic = 1,
        /// <summary>
        /// 瀑布滚动翻页模式
        /// </summary>
        Scrolling = 2

    }

    public enum OrderByFileds
    {
        Default = 0,
        MonthSales_Loc = 1,
        YearSales_Loc = 2,
        HistorySales_Loc = 3,
        MonthSales_Web = 4,
        YearSales_Web = 5,
        HistorySales_Web = 6,
        MonthSales_Lib = 7,
        YearSales_Lib = 8,
        HistorySales_Lib = 9,
        MonthSales_Mix = 10,
        YearSales_Mix = 11,
        HistorySales_Mix = 12,
        WeekSales_Loc = 13,
        Last4WeekSales_Loc = 14,
        SampleMonthSales_Loc = 15,
        SampleStockNum_Loc = 16,
        C_OnSaleRoomCnt_SampleRoomCnt = 17,
        C_SaledRoomCnt_SampleRoomCnt = 18,
        C_SaledRoomCnt_OnSaleRoomCnt = 19,
        C_SampleStockNum_SampleMonthSales = 20,


        ///指标
        SaleRate = 21,
        SaledBreed = 22,
        SaledBreed_N = 23,
        SaledBreedRates = 24,
        SaledNum = 25,

        SaleRate_Mix = 26,
        SaledBreed_Mix = 27,
        SaledBreed_N_Mix = 28,
        SaledBreedRates_Mix = 29,
        Pub_Efficiency_Mix = 30,


        SaleRate_Loc = 31,
        SaledBreed_Loc = 32,
        SaledBreed_N_Loc = 33,
        SaledBreedRates_Loc = 34,
        Pub_Efficiency_Loc = 35,

        SaleRate_Web = 36,
        SaledBreed_Web = 37,
        SaledBreed_N_Web = 38,
        SaledBreedRates_Web = 39,
        Pub_Efficiency_Web = 40,

        SaleRate_Lib = 41,
        SaledBreed_Lib = 42,
        SaledBreed_N_Lib = 43,
        SaledBreedRates_Lib = 44,
        SaledNum_Lib = 45,

        OnSaleRate = 46,
        OnSaleBreed = 47,
        OnSale_SaledRate = 48,
        OnSale_StockSaleRate = 49,

        KindID = 50,

        //补充

        SaledNumRates_Mix = 51,
        SaledNumRates_Loc = 52,
        SaledNumRates_Web = 53,
        SaledNumRates_Lib = 54,

        OnSaledBreed = 55,

        //其他业务字段
        SearchTime = 10000

    }

    /// <summary>
    /// 修改数据 页面提示框
    /// </summary>
    public enum enumPageAlert
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,
        /// <summary>
        /// 系统不存在该数据
        /// </summary>
        NoExists = 2
    }

    /// <summary>
    /// 用户邮件状态  0新加 1处理中 2发送失败 3发送成功
    /// </summary>
    public enum UserMailStatus
    {
        /// <summary>
        /// 新加
        /// </summary>
        Newadd = 0,

        /// <summary>
        /// 处理进行中
        /// </summary>
        DealUnderWay = 1,

        /// <summary>
        /// 发送失败
        /// </summary>
        SendFail = 2,

        /// <summary>
        /// 发送成功
        /// </summary>
        SendSuccess = 3
    }

    public enum EventItemPictureState
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 有封面图片
        /// </summary>
        CoverPicture = 1,

        /// <summary>
        /// 有发现轮播图片
        /// </summary>
        DiscoverPicture =2
    }

    /// <summary>
    /// 通用状态枚举
    /// </summary>
    public enum GeneralStateType
    {
        /// <summary>
        /// 停用
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// 启用
        /// </summary>
        Enabled = 1
    }

    public enum CheckType
    {
        /// <summary>
        /// 未通过
        /// </summary>
        Failed = -1,

        /// <summary>
        /// 未检测
        /// </summary>
        NotCheck =0,
        /// <summary>
        /// 通过
        /// </summary>
        Passed=1

    }

    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType 
    {
        RelateLike=1, //与我相关 赞类型
        RelateReply,  //与我相关 回复类型
    }


    /// <summary>
    /// 订阅状态枚举
    /// </summary>
    public enum SubQueryType
    {
        YES=1, //已订阅
        NO,    //未订阅
    }
}
