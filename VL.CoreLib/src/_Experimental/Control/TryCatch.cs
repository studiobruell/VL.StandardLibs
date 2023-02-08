﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using VL.Core;

namespace VL.Lib.Control
{
    public static class ExceptionUtils
    {
        public static readonly Exception UnspecifiedException = new Exception("No Exception");

        public static Exception InnermostException(this Exception e)
        {
            if (e == null)
                return e;

            while (e.InnerException != null)
                e = e.InnerException;

            return e;
        }

        public static IEnumerable<Exception> InnerExceptions(this Exception e)
        {
            if (e != null)
            {
                yield return e;
                while (e.InnerException != null)
                {
                    foreach (var inner in e.InnerException.InnerExceptions())
                    {
                        if (inner is AggregateException ae)
                        {
                            yield return ae;
                            foreach (var aggregateInner in ae.InnerExceptions)
                                foreach (var innerAggregateInner in aggregateInner.InnerExceptions())
                                    yield return innerAggregateInner;
                        }
                        else
                            yield return inner;
                    }
                }
                    
            }
        }
    }

    public static partial class TryCatchUtils
    {
        public static void Throw(string message = "Unspecified Exception")
        {
            throw new Exception(message);
        }

        public static void Throw(Exception e)
        {
            throw e;
        }

        /// <summary>
        /// Runs the given stateless patch and returns whether it has been successful or not
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="try"></param>
        /// <param name="defaultOutput"></param>
        /// <param name="success"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static TOutput Try<TOutput>(Func<TOutput> @try, TOutput defaultOutput, out bool success, out string errorMessage)
        {
            success = true;
            errorMessage = "";
            try
            {
                return @try();
            }
            catch (Exception e)
            {
                success = false;
                errorMessage = e.InnermostException().Message;
                return defaultOutput;
            }
        }

        /// <summary>
        /// Runs the given stateless patch, runs catch instead if it has been unsuccessful
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="try"></param>
        /// <param name="catch"></param>
        /// <returns></returns>
        public static TOutput TryCatch<TOutput>(Func<TOutput> @try, Func<Exception, TOutput> @catch)
        {
            try
            {
                return @try();
            }
            catch (Exception e)
            {
                return @catch(e);
            }
        }

        /// <summary>
        /// Runs the given stateless patch, runs Catch instead if it has been unsuccessful. Guarantees to run Finally afterwards
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="try"></param>
        /// <param name="catch"></param>
        /// <param name="finally"></param>
        /// <returns></returns>
        public static TOutput TryCatchFinally<TData, TOutput>(Func<TData> @try, Func<Exception, TData> @catch, Func<TData, TOutput> @finally)
        {
            TData inter = default(TData);
            TOutput final;

            try
            {
                inter = @try();
            }
            catch (Exception e)
            {
                inter = @catch(e);
            }
            finally
            {
                final = @finally(inter);
            }
            return final;
        }
    }

    /// <summary>
    /// Runs the given patch and returns whether it has been successful or not
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class TryStateful<TState> : IDisposable
        where TState : class
    {
        TState FState;

        public TOutput Update<TOutput>(
            Func<TState> create,
            Func<TState, Tuple<TState, TOutput>> @try,
            TOutput defaultOutput,
            bool reInitialize,
            out bool success,
            out string errorMessage
            )
        {
            Tuple<TState, TOutput> t;
            TOutput output;
            try
            {
                if (FState == null || reInitialize)
                    FState = create();

                t = @try(FState);
                FState = t.Item1;
                output = t.Item2;

                success = true;
                errorMessage = "";
            }
            catch (Exception e)
            {
                output = defaultOutput;
                success = false;
                errorMessage = e.InnermostException().Message;
            }
            return output;
        }

        public virtual void Dispose() => (FState as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Runs the given patch, runs catch instead if it has been unsuccessful
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class TryCatchStateful<TState> : IDisposable
        where TState : class
    {
        TState FState;

        public virtual TOutput Update<TOutput>(
            Func<TState> create,
            Func<TState, Tuple<TState, TOutput>> @try,
            Func<TState, Exception, Tuple<TState, TOutput>> @catch,
            bool reInitialize)
        {
            Tuple<TState, TOutput> t;
            TOutput output;
            try
            {
                if (FState == null || reInitialize)
                    FState = create();
                t = @try(FState);
            }
            catch (Exception e)
            {
                t = @catch(FState, e);
            }
            FState = t.Item1;
            output = t.Item2;
            return output;
        }

        public virtual void Dispose() => (FState as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Runs the given patch, runs catch instead if it has been unsuccessful. Guarantees to run Finally afterwards
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class TryCatchFinallyStateful<TState> : IDisposable
        where TState : class
    {
        TState FState;

        public virtual TOutput Update<TData, TOutput>(
            Func<TState> create,
            Func<TState, Tuple<TState, TData>> @try,
            Func<TState, Exception, Tuple<TState, TData>> @catch,
            Func<TState, TData, Tuple<TState, TOutput>> @finally,
            bool reInitialize)
        {
            Tuple<TState, TData> inter = null;
            Tuple<TState, TOutput> output;
            try
            {
                if (FState == null || reInitialize)
                    FState = create();
                inter = @try(FState);
            }
            catch (Exception e)
            {
                inter = @catch(FState, e);
            }
            finally
            {
                output = @finally(inter.Item1, inter.Item2);
            }
            FState = output.Item1;
            return output.Item2;
        }

        public virtual void Dispose() => (FState as IDisposable)?.Dispose();
    }
}