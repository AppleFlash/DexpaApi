// SMSC.RU API (smsc.ru) версия 2.7 (07.04.2013)
using System;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;

public class SMSC
{
	// Константы с параметрами отправки
	string SMSC_LOGIN = "login";		// логин клиента
	string SMSC_PASSWORD = "password";	// пароль или MD5-хеш пароля в нижнем регистре
	const bool SMSC_POST = false;				// использовать метод POST
	const bool SMSC_HTTPS = false;				// использовать HTTPS протокол
	const string SMSC_CHARSET = "utf-8";		// кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8
	const bool SMSC_DEBUG = false;				// флаг отладки

	// Константы для отправки SMS по SMTP
	const string SMTP_FROM = "api@smsc.ru";		// e-mail адрес отправителя
	const string SMTP_SERVER = "send.smsc.ru";	// адрес smtp сервера
	const string SMTP_LOGIN = "";				// логин для smtp сервера
	const string SMTP_PASSWORD = "";			// пароль для smtp сервера

    public SMSC(string sSmscLogin, string sSmscPassword)
    {
        SMSC_LOGIN = sSmscLogin;
        SMSC_PASSWORD = sSmscPassword;
    }

	// Метод отправки SMS
	//
	// обязательные параметры:
	//
	// phones - список телефонов через запятую или точку с запятой
	// message - отправляемое сообщение
	//
	// необязательные параметры:
	//
	// translit - переводить или нет в транслит
	// time - необходимое время доставки в виде строки (DDMMYYhhmm, h1-h2, 0ts, +m)
	// id - идентификатор сообщения. Представляет собой 32-битное число в диапазоне от 1 до 2147483647.
	// format - формат сообщения (0 - обычное sms, 1 - flash-sms, 2 - wap-push, 3 - hlr, 4 - bin, 5 - bin-hex, 6 - ping-sms)
	// sender - имя отправителя (Sender ID). Для отключения Sender ID по умолчанию необходимо в качестве имени
	// передать пустую строку или точку.
	// query - строка дополнительных параметров, добавляемая в URL-запрос ("valid=01:00&maxsms=3")
	//
	// возвращает массив строк (<id>, <количество sms>, <стоимость>, <баланс>) в случае успешной отправки
	// либо массив строк (<id>, -<код ошибки>) в случае ошибки
    public string[] send_sms(string phones, string message, int translit = 0, string time = "", int id = 0, int format = 0, string sender = "", string query = "")
	{
		string[] formats = {"flash=1", "push=1", "hlr=1", "bin=1", "bin=2", "ping=1"};

		string[] m = _smsc_send_cmd("send", "cost=3&phones=" + _urlencode(phones)
						+ "&mes=" + _urlencode(message) + "&id=" + id.ToString() + "&translit=" + translit.ToString()
						+ (format > 0 ? "&" + formats[format-1] : "") + (sender != "" ? "&sender=" + _urlencode(sender) : "")
						+ (time != "" ? "&time=" + _urlencode(time) : "") + (query != "" ? "&" + query : ""));

		// (id, cnt, cost, balance) или (id, -error)

		if (SMSC_DEBUG) {
			if (Convert.ToInt32(m[1]) > 0)
				_print_debug("Сообщение отправлено успешно. ID: " + m[0] + ", всего SMS: " + m[1] + ", стоимость: " + m[2] + ", баланс: " + m[3]);
			else
				_print_debug("Ошибка №" + m[1].Substring(1, 1) + (m[0] != "0" ? ", ID: " + m[0] : ""));
		}

		return m;
	}

	// SMTP версия метода отправки SMS

	public void send_sms_mail(string phones, string message, int translit = 0, string time = "", int id = 0, int format = 0, string sender = "")
	{
		MailMessage mail = new MailMessage();

		mail.To.Add("send@send.smsc.ru");
		mail.From = new MailAddress(SMTP_FROM, "");

		mail.Body = SMSC_LOGIN + ":" + SMSC_PASSWORD + ":" + id.ToString() + ":" + time + ":"
					+ translit.ToString() + "," + format.ToString() + "," + sender
					+ ":" + phones + ":" + message;

		mail.BodyEncoding = Encoding.GetEncoding(SMSC_CHARSET);
		mail.IsBodyHtml = false;

		SmtpClient client = new SmtpClient(SMTP_SERVER, 25);
		client.DeliveryMethod = SmtpDeliveryMethod.Network;
		client.EnableSsl = false;
		client.UseDefaultCredentials = false;

		if (SMTP_LOGIN != "")
			client.Credentials = new NetworkCredential(SMTP_LOGIN, SMTP_PASSWORD);

		client.Send(mail);
	}

	// Метод получения стоимости SMS
	//
	// обязательные параметры:
	//
	// phones - список телефонов через запятую или точку с запятой
	// message - отправляемое сообщение 
	//
	// необязательные параметры:
	//
	// translit - переводить или нет в транслит
	// format - формат сообщения (0 - обычное sms, 1 - flash-sms, 2 - wap-push, 3 - hlr, 4 - bin, 5 - bin-hex, 6 - ping-sms)
	// sender - имя отправителя (Sender ID)
	// query - строка дополнительных параметров, добавляемая в URL-запрос ("list=79999999999:Ваш пароль: 123\n78888888888:Ваш пароль: 456")
	//
	// возвращает массив (<стоимость>, <количество sms>) либо массив (0, -<код ошибки>) в случае ошибки

	public string[] get_sms_cost(string phones, string message, int translit = 0, int format = 0, string sender = "", string query = "")
	{
		string[] formats = {"flash=1", "push=1", "hlr=1", "bin=1", "bin=2", "ping=1"};

		string[] m = _smsc_send_cmd("send", "cost=1&phones=" + _urlencode(phones)
						+ "&mes=" + _urlencode(message) + translit.ToString() + (format > 0 ? "&" + formats[format-1] : "")
						+ (sender != "" ? "&sender=" + _urlencode(sender) : "") + (query != "" ? "&query" : ""));

		// (cost, cnt) или (0, -error)

		if (SMSC_DEBUG) {
			if (Convert.ToInt32(m[1]) > 0)
				_print_debug("Стоимость рассылки: " + m[0] + ". Всего SMS: " + m[1]);
			else
				_print_debug("Ошибка №" + m[1].Substring(1, 1));
		}

		return m;
	}

	// Метод проверки статуса отправленного SMS или HLR-запроса
	//
	// id - ID cообщения
	// phone - номер телефона
	//
	// возвращает массив:
	// для отправленного SMS (<статус>, <время изменения>, <код ошибки sms>)
	// для HLR-запроса (<статус>, <время изменения>, <код ошибки sms>, <код IMSI SIM-карты>, <номер сервис-центра>, <код страны регистрации>,
	// <код оператора абонента>, <название страны регистрации>, <название оператора абонента>, <название роуминговой страны>,
	// <название роумингового оператора>)
	//
	// При all = 1 дополнительно возвращаются элементы в конце массива:
	// (<время отправки>, <номер телефона>, <стоимость>, <sender id>, <название статуса>, <текст сообщения>)
	//
	// либо массив (0, -<код ошибки>) в случае ошибки

	public string[] get_status(int id, string phone, int all = 0)
	{
		string[] m = _smsc_send_cmd("status", "phone=" + _urlencode(phone) + "&id=" + id.ToString() + "&all=" + all.ToString());

		// (status, time, err, ...) или (0, -error)

		if (SMSC_DEBUG)
		{
			if (m[1] != "" && Convert.ToInt32(m[1]) >= 0) {
				int timestamp = Convert.ToInt32(m[1]);
				DateTime offset = new DateTime(1970, 1, 1, 0, 0, 0, 0);
 				DateTime date = offset.AddSeconds(timestamp);

				_print_debug("Статус SMS = " + m[0] + (timestamp > 0 ? ", время изменения статуса - " + date.ToLocalTime() : ""));
			}
			else
				_print_debug("Ошибка №" + m[1].Substring(1, 1));
		}

		if (all == 1 && m.Length > 9 && (m.Length < 14 || m[14] != "HLR"))
			m = String.Join(",", m).Split(",".ToCharArray(), 9);

		return m;
	}

	// Метод получения баланса
	//
	// без параметров
	//
	// возвращает баланс в виде строки или пустую строку в случае ошибки

	public string get_balance()
	{
		string[] m = _smsc_send_cmd("balance", ""); // (balance) или (0, -error)

		if (SMSC_DEBUG) {
			if (m.Length == 1)
				_print_debug("Сумма на счете: " + m[0]);
			else
				_print_debug("Ошибка №" + m[1].Substring(1, 1));
		}

		return m.Length == 1 ? m[0] : "";
	}

	// ПРИВАТНЫЕ МЕТОДЫ

	// Метод вызова запроса. Формирует URL и делает 3 попытки чтения

	private string[] _smsc_send_cmd(string cmd, string arg)
	{
		arg = "login=" + _urlencode(SMSC_LOGIN) + "&psw=" + _urlencode(SMSC_PASSWORD) + "&fmt=1&charset=" + SMSC_CHARSET + "&" + arg;

		string url = (SMSC_HTTPS ? "https" : "http") + "://smsc.ru/sys/" + cmd + ".php" + (SMSC_POST ? "" : "?" + arg);

        string ret;
        int i = 0;
        WebRequest request;
        StreamReader sr;
        HttpWebResponse response;

        do
        {
            if (i > 0)
                System.Threading.Thread.Sleep(2000);

            if (i == 2)
                url = url.Replace("://smsc.ru/", "://www2.smsc.ru/");

    		request = WebRequest.Create(url);

    		if (SMSC_POST) {
	    		request.Method = "POST";
		    	request.ContentType = "application/x-www-form-urlencoded";
			    request.ContentLength = arg.Length;
    			Stream stream = request.GetRequestStream();
	    		stream.Write(Encoding.Default.GetBytes(arg), 0, arg.Length);
		    	stream.Close();
		    }

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                sr = new StreamReader(response.GetResponseStream());
                ret = sr.ReadToEnd();
            }
            catch (WebException e) {
                ret = "";
            }
		}
		while (ret == "" && ++i < 3);

		if (ret == "") {
			if (SMSC_DEBUG)
				_print_debug("Ошибка чтения адреса: " + url);

			ret = ","; // фиктивный ответ
		}

		return ret.Split(',');
	}

	// кодирование параметра в http-запросе
	private string _urlencode(string str) {
		return HttpUtility.UrlEncode(str);
	}

	// вывод отладочной информации
	private void _print_debug(string str) {
		Console.WriteLine(str);
	}
}

// Examples:
// SMSC smsc = new SMSC();
// string[] r = smsc.send_sms("79999999999", "Ваш пароль: 123", 2);
// string[] r = smsc.send_sms("79999999999", "http://smsc.ru\nSMSC.RU", 0, "", 0, 0, "", "maxsms=3");
// string[] r = smsc.send_sms("79999999999", "0605040B8423F0DC0601AE02056A0045C60C036D79736974652E72750001036D7973697465000101", 0, "", 0, 5);
// string[] r = smsc.send_sms("79999999999", "", 0, "", 0, 3);
// string[] r = smsc.get_sms_cost("79999999999", "Вы успешно зарегистрированы!");
// smsc.send_sms_mail("79999999999", "Ваш пароль: 123", 0, "0101121000");
// string[] r = smsc.get_status(12345, "79999999999");
// string balance = smsc.get_balance();
