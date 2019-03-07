#region USINGS

using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

#endregion

// ReSharper disable UnusedMember.Global

namespace Z.MVVMHelper.AsyncTypes
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <param name="arg"></param>
    /// <returns></returns>
    public delegate Task<bool> AsyncPredicate<in TIn>(TIn arg);

    #region CancellableAction

    /// <summary>
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public delegate Task CancellableAction(CancellationToken token);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1>(CancellationToken token, T1 param1);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2>(CancellationToken token, T1 param1, T2 param2);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3>(CancellationToken token, T1 param1, T2 param2, T3 param3);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4, in T5>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4, in T5, in T6>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <param name="param9"></param>
    /// <returns></returns>
    public delegate Task CancellableAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9);

    #endregion CancellableAction

    #region CancellableFunction

    /// <summary>
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<TReturn>(CancellationToken token);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, TReturn>(CancellationToken token, T1 param1);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, TReturn>(CancellationToken token, T1 param1, T2 param2);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, in T5, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, in T5, in T6, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    /// <param name="token"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <param name="param9"></param>
    /// <returns></returns>
    public delegate Task<TReturn> CancellableFunction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TReturn>(CancellationToken token, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9);

    #endregion CancellableFunction

    #region Actions

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public delegate Task AsyncAction();

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="param1"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1>(T1 param1);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2>(T1 param1, T2 param2);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3>(T1 param1, T2 param2, T3 param3);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4>(T1 param1, T2 param2, T3 param3, T4 param4);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <param name="param9"></param>
    /// <returns></returns>
    public delegate Task AsyncAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9);

    #endregion

    #region Functions

    /// <summary>
    /// </summary>
    /// <typeparam name="TRet"></typeparam>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<TRet>();

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, TRet>(T1 param1);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, TRet>(T1 param1, T2 param2);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, TRet>(T1 param1, T2 param2, T3 param3);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, TRet>(T1 param1, T2 param2, T3 param3, T4 param4);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, in T5, TRet>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, TRet>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TRet>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TRet>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8);

    /// <summary>
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    /// <typeparam name="T7"></typeparam>
    /// <typeparam name="T8"></typeparam>
    /// <typeparam name="T9"></typeparam>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    /// <param name="param4"></param>
    /// <param name="param5"></param>
    /// <param name="param6"></param>
    /// <param name="param7"></param>
    /// <param name="param8"></param>
    /// <param name="param9"></param>
    /// <returns></returns>
    public delegate Task<TRet> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TRet>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9);

    #endregion
}