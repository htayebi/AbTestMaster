using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AbTestMaster.Domain;

namespace AbTestMaster.Services
{
    internal class HttpHelpers
    {
        #region internal methods
        internal static bool IsIn200Family(string statusCode)
        {
            return Acceptedstatuses.Any(a => a.ToString() == statusCode);
        }

        internal static IEnumerable<HttpStatusCode> Acceptedstatuses
        {
            get
            {
                return
                    new List<HttpStatusCode>
                    {
                        HttpStatusCode.OK,
                        HttpStatusCode.Created,
                        HttpStatusCode.Accepted,
                        HttpStatusCode.NonAuthoritativeInformation,
                        HttpStatusCode.NoContent,
                        HttpStatusCode.ResetContent,
                        HttpStatusCode.PartialContent
                    };
            }
        }

        internal static string GetViewTrail(string sequence)
        {
            string viewTrail = string.Empty;

            if (!SessionSplitViews.Any())
            {
                return viewTrail;
            }

            IEnumerable<SplitView> currentSequence = SessionSplitViews.Where(v => v.Sequence == sequence).Take(5);

            viewTrail = currentSequence.Select(FormatSplitViewData).Aggregate(string.Empty, (current, fc) => current + "->" + fc);

            return viewTrail;
        }

        internal static void SaveToCookie(SplitView split)
        {
            var myCookie = new HttpCookie(Constants.AB_TEST_MASTER_COOKIE + "_" + split.SplitGroup);
            myCookie[Constants.SPLIT_GROUP] = split.SplitGroup;
            myCookie[Constants.SPLIT_NAME] = split.SplitViewName;
            myCookie[Constants.SPLIT_SEQUENCE] = split.Sequence;
            myCookie[Constants.ACTION] = split.Action;
            myCookie[Constants.CONTROLLER] = split.Controller;
            myCookie[Constants.AREA] = split.Area;
            myCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        internal static SplitView ReadFromCookie(string splitGroup)
        {
            SplitView split = null;

            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.AB_TEST_MASTER_COOKIE + "_" + splitGroup);

            if (cookie != null)
            {
                split = new SplitView
                {
                    SplitGroup = cookie[Constants.SPLIT_GROUP],
                    SplitViewName = cookie[Constants.SPLIT_NAME],
                    Sequence = cookie[Constants.SPLIT_SEQUENCE],
                    Action = cookie[Constants.ACTION],
                    Controller = cookie[Constants.CONTROLLER],
                    Area = cookie[Constants.AREA]
                };
            }

            return split;
        }

        internal static void SaveToSession(SplitView selectedSplit)
        {
            SessionSplitViews.Add(selectedSplit);
        }

        internal static void RemoveFromSession(SplitGoal splitGoal)
        {
            var viewList = SessionSplitViews;

            var newList = viewList.Where(splitView => splitView.Sequence != splitGoal.Sequence).ToList();

            SessionSplitViews = newList;
        }
        #endregion

        #region private methods
        
        private static string FormatSplitViewData(SplitView splitView)
        {
            string output = string.Empty;

            if (splitView == null)
            {
                return output;
            }

            output = "(" + splitView.SplitViewName + "#" + splitView.SplitGroup + "#" + splitView.Sequence + ")";

            return output;
        }

        private static List<SplitView> SessionSplitViews
        {
            get
            {
                var views = HttpContext.Current.Session[Constants.AB_TEST_MASTER_SESSION] as List<SplitView>;

                if (views == null)
                {
                    views = new List<SplitView>();
                    HttpContext.Current.Session[Constants.AB_TEST_MASTER_SESSION] = views;
                }

                return views;
            }

            set
            {
                HttpContext.Current.Session[Constants.AB_TEST_MASTER_SESSION] = value;
            }
        }
        #endregion
    }
}

