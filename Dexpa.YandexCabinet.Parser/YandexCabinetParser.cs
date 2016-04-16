using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Dexpa.YandexCabinet.Parser
{
    public class YandexCabinetParser
    {
        //private string mUrl;

        //private NetworkCredential mCredential;
        private string mLogin;

        private string mPassword;

        private string mBufferUrl;

        private string mUrl;

        private string mCabId;

        private const string PASSPORT_URL = "https://passport.yandex.ru/passport";

        private const string LOGOUT_URL = "https://passport.yandex.ru/passport?mode=logout&yu=";

        private List<DriverScores> mDriverScores;

        private List<CustomerFeedback> mFeedbacks;

        private WebBrowser mWebBrowser;

        private bool isRatingParser { get; set; }

        private bool isCustomerFeedbackParser { get; set; }

        public YandexCabinetParser(string login, string password, string cabId, string url)
        {
            mLogin = login;
            mPassword = password;
            mCabId = cabId;
            mBufferUrl = url;
            isRatingParser = false;
            isCustomerFeedbackParser = false;
            //login = "kortegavto406429";
            //password = "l8vjrw5vgh";
            //mUrl = "https://taxi-cabinet.mobile.yandex.ru";
            //mCredential = new NetworkCredential(login, password);
        }

        public List<DriverScores> GetRatings()
        {
            isRatingParser = true;
            RunParser();
            return mDriverScores;
            //var request = WebRequest.Create(mUrl + "/drivers");
            //request.Credentials = mCredential;
            //request.PreAuthenticate = true;

            //var response = request.GetResponse();
            //var reader = new StreamReader(response.GetResponseStream());
            //var document = HtmlDocument.HtmlEncode(reader.ReadToEnd());

            //return null;
        }

        public List<CustomerFeedback> GetFeedback()
        {
            isCustomerFeedbackParser = true;
            RunParser();
            return mFeedbacks;
        }

        private void RunParser()
        {
            var thread = new Thread(() =>
            {
                mWebBrowser = new WebBrowser();
                mWebBrowser.ScriptErrorsSuppressed = true;
                mWebBrowser.DocumentCompleted += LogoutDocumentCompleted;
                mWebBrowser.Navigate(PASSPORT_URL);
                Application.Run();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void LogoutDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (mWebBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                Logout();
            }
        }

        private void Logout()
        {
            if (mWebBrowser.Document != null)
            {
                string logoutUrl;
                string cookieString = mWebBrowser.Document.Cookie;
                List<Cookie> cookies = Cookie.GetCookies(cookieString);
                string yandexuid = cookies.Where(x => x.Name == "yandexuid").Select(x => x.Value).FirstOrDefault();
                logoutUrl = LOGOUT_URL + yandexuid;
                mWebBrowser.DocumentCompleted -= LogoutDocumentCompleted;
                mWebBrowser.DocumentCompleted += LogoutSuccess;
                mWebBrowser.Navigate(logoutUrl);
                Console.WriteLine("Yandex Cabinet Parser: Logout");
            }
        }

        private void LogoutSuccess(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            mWebBrowser.DocumentCompleted -= LogoutSuccess;
            mWebBrowser.DocumentCompleted += LoginDocumentCompleted;
            mWebBrowser.Navigate("https://passport.yandex.ru/passport");
            Console.WriteLine("Yandex Cabinet Parser: Login");
        }

        private void LoginDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (mWebBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                var elements = mWebBrowser.Document.All;
                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    if (element.Id == "login")
                    {
                        elements[i].InnerText = mLogin;
                        elements[i].SetAttribute("value", mLogin);
                        //Console.WriteLine(elements[i].GetAttribute("value"));
                    }
                    if (element.Id == "passwd")
                    {
                        elements[i].InnerText = mPassword;
                        elements[i].SetAttribute("value", mPassword);
                        //Console.WriteLine(elements[i].GetAttribute("value"));
                    }
                    if (element.TagName == "BUTTON")
                    {
                        mWebBrowser.DocumentCompleted -= LoginDocumentCompleted;
                        mWebBrowser.DocumentCompleted += LoginSuccess;
                        mWebBrowser.Document.Forms[0].InvokeMember("Submit");
                        break;
                    }
                }
            }
        }

        private void LoginSuccess(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            mWebBrowser.DocumentCompleted -= LoginSuccess;
            mUrl = mBufferUrl;
            if (isRatingParser)
            {
                mUrl += "drivers/";
                mWebBrowser.DocumentCompleted += RatingsDocumentCompleted;
                mWebBrowser.Navigate(new Uri(mUrl));
                if (mCabId != null)
                {
                    List<Cookie> cookie = Cookie.GetCookies(mWebBrowser.Document.Cookie);
                    if (cookie.Where(c => c.Name == "park").Count() == 0)
                    {
                        //mWebBrowser.Document.Cookie += "; park = 1956789018";
                        System.Net.Cookie elem = new System.Net.Cookie("park", mCabId, "/", mWebBrowser.Url.AbsoluteUri);
                        InternetSetCookie(mUrl, null, elem.ToString());
                    }
                }
            }
            if (isCustomerFeedbackParser)
            {
                mUrl += "feedback/?all=1";
                mWebBrowser.DocumentCompleted += FeedbackDocumentCompleted;
                mWebBrowser.Navigate(new Uri(mUrl));
                if (mCabId != null)
                {
                    List<Cookie> cookie = Cookie.GetCookies(mWebBrowser.Document.Cookie);
                    if (cookie.Where(c => c.Name == "park").Count() == 0)
                    {
                        //mWebBrowser.Document.Cookie += "; park = 1956789018";
                        System.Net.Cookie elem = new System.Net.Cookie("park", mCabId, "/", mWebBrowser.Url.AbsoluteUri);
                        InternetSetCookie(mUrl, null, elem.ToString());
                    }
                }
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        private void RatingsDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (mWebBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                mWebBrowser.DocumentCompleted -= RatingsDocumentCompleted;
                string html = mWebBrowser.DocumentText;
                mDriverScores = new List<DriverScores>();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var htmlNodes = document.DocumentNode.ChildNodes.Where(x => x.Name == "html").ToArray();
                var bodyNodes = htmlNodes[0].ChildNodes.Where(x => x.Name == "body").ToArray();
                var childNodes = bodyNodes[0].ChildNodes;
                var divNodes = bodyNodes[0].ChildNodes.Where(x => x.Name == "div").ToArray();
                var tableNodes = divNodes[1].ChildNodes.Where(x => x.Name == "table");
                foreach (var table in tableNodes)
                {
                    var tableBodyNodes = table.ChildNodes.Where(x => x.Name == "tbody").ToArray();
                    var trNodes = tableBodyNodes[0].ChildNodes.Where(x => x.Name == "tr");
                    foreach (var trNode in trNodes)
                    {
                        var tdNodes = trNode.ChildNodes.Where(x => x.Name == "td").ToArray();
                        var item = new DriverScores();
                        item.DriverId = long.Parse(tdNodes[0].InnerText);
                        item.Total = tdNodes[2].InnerText.Trim() == "&mdash;" ? 0 : double.Parse(tdNodes[2].InnerText, CultureInfo.InvariantCulture);

                        var AverageClientScore = tdNodes[3].InnerText.Trim() == "&mdash;"
                            ? new string[] { "&mdash;" }
                            : tdNodes[3].InnerText.Trim().Split(' ');

                        item.AverageClientScore =
                            AverageClientScore[0] == "&mdash;" ? 0 : double.Parse(AverageClientScore[0], CultureInfo.InvariantCulture);
                        item.ClientFeedbacksCount = AverageClientScore[0] == "&mdash;"
                            ? 0
                            : double.Parse(AverageClientScore[1].TrimStart('(').TrimEnd(')'), CultureInfo.InvariantCulture);
                        item.DriverLateScore = tdNodes[4].InnerText.Trim() == "&mdash;" ? 0 : ConvertTextMark(tdNodes[4].InnerText);
                        item.CancelledOrders = tdNodes[5].InnerText.Trim() == "&mdash;" ? 0 : ConvertTextMark(tdNodes[5].InnerText);
                        item.FakeWaitings = tdNodes[6].InnerText.Trim() == "&mdash;" ? 0 : ConvertTextMark(tdNodes[6].InnerText);
                        item.TrackQuality = tdNodes[7].InnerText.Trim() == "&mdash;" ? 0 : ConvertTextMark(tdNodes[7].InnerText);
                        var examResult = tdNodes[9].InnerText.Trim() == "&mdash;"
                        ? new string[] { "&mdash;" }
                        : tdNodes[9].InnerText.Trim().Split(' ');
                        item.ExamResult = examResult[0] == "&mdash;" ? 0 : Convert.ToInt32(examResult[0]);
                        item.ExamDate = examResult[0] == "&mdash;" ? "-" : examResult[1];
                        mDriverScores.Add(item);
                    }
                }
                Console.WriteLine("Yandex Cabinet Parser: Rating parsing completed");
                isRatingParser = false;
                Application.ExitThread();
            }
        }

        private void FeedbackDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (mWebBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                mWebBrowser.DocumentCompleted -= FeedbackDocumentCompleted;
                string html = mWebBrowser.DocumentText;
                mFeedbacks = new List<CustomerFeedback>();
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var htmlNodes = document.DocumentNode.ChildNodes.Where(x => x.Name == "html").ToArray();
                var bodyNodes = htmlNodes[0].ChildNodes.Where(x => x.Name == "body").ToArray();
                var divNodes = bodyNodes[0].ChildNodes.Where(x => x.Name == "div").ToArray();
                var tableNodes = divNodes[1].ChildNodes.Where(x => x.Name == "table");
                foreach (var table in tableNodes)
                {
                    var tableBodyNodes = table.ChildNodes.Where(x => x.Name == "tbody").ToArray();
                    var trNodes = tableBodyNodes[0].ChildNodes.Where(x => x.Name == "tr");
                    foreach (var trNode in trNodes)
                    {
                        var tdNodes = trNode.ChildNodes.Where(x => x.Name == "td").ToArray();
                        var item = new CustomerFeedback();
                        item.SourceOrderId = tdNodes[0].InnerText.Trim();
                        item.Comment = tdNodes[4].InnerText.Trim();
                        var score = tdNodes[5].InnerText.Trim();
                        item.Score = short.Parse(score==""?"0":score);
                        mFeedbacks.Add(item);
                    }
                }
                Console.WriteLine("Yandex Cabinet Parser: Feedback parsing completed");
                isCustomerFeedbackParser = false;
                Application.ExitThread();
            }
        }

        private int ConvertTextMark(string mark)
        {
            mark = mark.Trim();
            switch (mark)
            {
                case "отлично":
                    return 5;
                    break;
                case "хорошо":
                    return 4;
                    break;
                case "нормально":
                    return 3;
                    break;
                case "плохо":
                    return 2;
                    break;
                case "очень плохо":
                    return 1;
                    break;
                default:
                    return 0;
            }
        }
    }
}
