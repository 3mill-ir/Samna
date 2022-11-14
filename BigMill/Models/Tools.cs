using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;

namespace BigMill.Areas.Admin3mill.Models
{
    public static class Tools
    {
        public static string ReturnPath(string ConfigPath, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = string.Format(section[ConfigPath]);
            MakeVaidPath(Path, false);
            return Path;
        }
        public static string ReturnPathPhysicalMode(string ConfigPath)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = HttpContext.Current.Server.MapPath("~" + string.Format(section[ConfigPath]));
            MakeVaidPath(Path, true);
            return Path;
        }

        public static void MakeVaidPath(string Path, bool isPhysicalPath)
        {
            if (!isPhysicalPath)
                Path = HttpContext.Current.Server.MapPath(Path);
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
        }
        public static string ConvertNativeDigits(this string text)
        {

            if (text == null)
                return null;
            if (text.Length == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                    sb.Append(char.GetNumericValue(character));
                else
                    sb.Append(character);
            }
            return sb.ToString();


        }
        private static readonly CultureInfo arabic = new CultureInfo("fa-IR");
        private static readonly CultureInfo latin = new CultureInfo("en-US");

        /// <summary>
        /// in tabe jahate tabdile zabane english be arabic ( ta hududi farsi ) estefade mishavad
        /// </summary>
        /// <param name="input">reshteye morede nazar baraye tabdil</param>
        /// <returns>
        /// string
        /// reshteye tabdil shode
        /// </returns>
        public static string ToArabic(string input)
        {
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(i.ToString(), arabicDigits[i]);
            }
            return input;
        }

        public static string ToLatin(string input)
        {
            var latinDigits = latin.NumberFormat.NativeDigits;
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < latinDigits.Length; i++)
            {
                input = input.Replace(arabicDigits[i], latinDigits[i]);
            }
            return input;
        }

        /// <summary>
        /// in tabe tarikhe miladi ra dar forme datetime gerefte va tarikhe jalali ra dar forme string baz migardanad
        /// </summary>
        /// <param name="date">tarikhe morede nazar jahate tabdil</param>
        /// <returns>
        /// string
        /// tarikhe tabdil shode be jalali
        /// </returns>
        public static string GetDateTimeReturnJalaliDate(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            int Hour = p.GetHour(date);
            int Minute = p.GetMinute(date);
            int Second = p.GetSecond(date);
            string result1 = "";
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());
            if (Hour.ToString().Count() == 2)
                result1 += Tools.ToArabic(Hour.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Hour.ToString()) + ':';
            if (Minute.ToString().Count() == 2)
                result1 += Tools.ToArabic(Minute.ToString()) + ':';
            else
                result1 += '۰' + Tools.ToArabic(Minute.ToString()) + ':';
            if (Second.ToString().Count() == 2)
                result1 += Tools.ToArabic(Second.ToString());
            else
                result1 += '۰' + Tools.ToArabic(Second.ToString());
            string FinalResult = result + " " + result1;

            return FinalResult;
        }
        public static string JalaliDateWithoutHour(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());

            return result;
        }

        /// <summary>
        /// in tabe tarikhe jalali ra dar forme string gerefte va tarikhe miladi ra dar forme datetime baz migardanad
        /// </summary>
        /// <param name="date">tarikhe jalaliye morede nazar dar forme string</param>
        /// <param name="_Date"></param>
        /// <returns>
        /// 
        /// </returns>
        public static bool GetJalaliDateReturnDateTime(string date, out DateTime _Date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                Regex rex = new Regex(@"^[۰-۹0-9]{4}\/[۰-۹0-9]{2}\/[۰-۹0-9]{2} [۰-۹0-9]{2}:[۰-۹0-9]{2}:[۰-۹0-9]{2}$");
                if (rex.Match(date).Success)
                {
                    string firstpart = date.Substring(0, date.IndexOf(':') - 2);
                    string SecondPart = date.Substring(date.IndexOf(':') - 2);
                    string[] persianDatePartsStart = firstpart.Split('/');
                    string[] persianDatePartsStartHour = SecondPart.Split(':');


                    int persianYearStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[0]));
                    int persianMonthStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[1]));
                    int persianDayStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[2]));
                    int persianHourStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[0]));
                    int persianMinStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[1]));
                    int persianSecondStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[2]));

                    string datetimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart);

                    PersianCalendar pc = new PersianCalendar();
                    try
                    {
                        DateTime start = new DateTime(persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart, pc);
                        _Date = start;
                        return true; ;
                    }
                    catch
                    {
                        _Date = DateTime.Now;
                        return false;
                    }
                }
            }
            _Date = DateTime.Now;
            return false; ;
        }

        public static string ImageSave_MaintainAspect(HttpPostedFileBase Content_Two)
        {
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = "~/Content/UplodedImages/PostImages/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));



            string pic = System.IO.Path.GetFileName(RandomValueString + extension);

            WebImage img = new WebImage(Content_Two.InputStream);
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
            img.Save("~/Content/UplodedImages/PostImages/" + pic);
            return RandomValueString + extension;
        }


        public static string ContentFour_Save(string ContentFour, int ID)
        {
            string RandomValueString = "Description_" + ID;
            Random rnd = new Random();
            string extension = ".txt";
            string curFile = HttpContext.Current.Server.MapPath("~/App_Data/ContentFour/") + RandomValueString + extension;
            int i = 1;
            while (File.Exists(curFile))
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = HttpContext.Current.Server.MapPath("~/App_Data/ContentFour/") + RandomValueString + "_" + i + extension;  //Your path
                i++;
            }
            System.IO.File.WriteAllText(curFile, ContentFour);
            return RandomValueString + extension;
        }

        public static void ContentFour_Edit(string ContentFour, string ContentFour_Path)
        {

            string Path = HttpContext.Current.Server.MapPath("~/App_Data/ContentFour/") + ContentFour_Path;
            System.IO.File.WriteAllText(Path, ContentFour);

        }
        public static string ContentFour_Get(string ContentFour)
        {
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/App_Data/ContentFour/" + ContentFour)))
            {
                try
                {
                    return System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/ContentFour/" + ContentFour));
                }
                catch
                {
                    return "خطا در عملیات پردازش متن";
                }
            }
            else
            {
                return "متن مورد نظر یافت نشد.";
            }
        }
        public static int F_UserId()
        {

            using (BigMill.Models.Entities db = new Entities())
            {
                string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var _info = db.UserInformations.FirstOrDefault(u => u.F_ID == userId);
                if (_info != null)
                {
                    return _info.ID;
                }
            }
            return -1;
        }

        public static string SaveFile(HttpPostedFileBase MyFile, string FilePath)
        {
            string path = FilePath;
            string extension = Path.GetExtension(MyFile.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;
            } while (File.Exists(curFile));
            string pic = System.IO.Path.GetFileName(RandomValueString + extension);
            byte[] tempFile = new byte[MyFile.ContentLength];
            MyFile.InputStream.Read(tempFile, 0, MyFile.ContentLength);
            System.IO.File.WriteAllBytes(path + pic, tempFile);
            return RandomValueString + extension;
        }
        public static void SavePasswordFor3mill(string pass)
        {


            string curFile = HttpContext.Current.Server.MapPath("~/App_Data/SecretKey.txt");

            string EncryptedPass = StringCipher.Encrypt(pass, "ParsaDorsa");
            System.IO.File.AppendAllText(curFile, EncryptedPass);
        }
        public static string GetPasswordFor3mill()
        {

            string pass = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/SecretKey.txt"));
            return StringCipher.Decrypt(pass, "ParsaDorsa");

        }
    }

    public class SendingReciveingSMS
    {
        string IP = "http://193.104.22.14:2055/CPSMSService/Access";
        string Number = "10006020";
        string UserName = "ATSIGNCO9";
        string Password = "m@hfye4@5";
        string Company = "ATSIGNCO";

        /// <summary>
        /// ersale taki
        /// </summary>
        /// <param name="msg">matne payam</param>
        /// <param name="dest">adrese magsad</param>
        /// <returns>vaziyate payamake ersal shode ke dars surate ersale movafag true mibashad</returns>
        public string[] Send_single(string msg, string dest)
        {
            string[] status = new string[2];
            if (string.IsNullOrEmpty(msg))
            {
                status[0] = "message is empty";
                return status;
            }

            if (string.IsNullOrEmpty(dest))
            {
                status[0] = "destination is empty";
                return status;
            }

            int msg_part = (int)Math.Ceiling((double)msg.Length / 70);
            int sms_amount;
            if (int.TryParse(SMS_Amount(), out sms_amount))
            {
                if (sms_amount < msg_part)
                    status[0] = "SMS amount is insufficient";
            }

            Cls_SMS.ClsSend sms_Single = new Cls_SMS.ClsSend();
            status = sms_Single.SendSMS_Single(msg, dest, Number, UserName, Password, IP, Company, false);
            return status;
        }
        public string SMS_Amount()
        {
            Cls_SMS.ClsGetRemain sms = new Cls_SMS.ClsGetRemain();
            string amount = sms.GetRemainCredit(UserName, Password, Company, IP);
            //lblRemain.Text = rem + " عدد پيامك  ";
            return amount;
        }

        public static string URL4SEO(string Controller, string action, int ID, string tittle)
        {
            int Maxlenght = 150;

            if (!string.IsNullOrEmpty(tittle))
            {
                string tempExtra = (System.Text.RegularExpressions.Regex.Replace(tittle, @"[^\w\040@-]", "",
                                   System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "/" + Controller + "/" + action + "/" + ID + "/";
                int tempSize = Maxlenght - path.Length;
                if (tempSize > 0)
                {
                    int remain = Maxlenght - tempSize;
                    return tempExtra.Length <= remain ? tempExtra : tempExtra.Substring(0, remain).TrimEnd('-');
                }

            }
            return "";
        }



    }

    public class PageTitle
    {
        public PageTitle(string _Pagetitle, string _ActionName, string _ControllerName, string _HtmlAttribute, string _RouteValue)
        {
            Pagetitle = _Pagetitle;
            ActionName = _ActionName;
            ControllerName = _ControllerName;
            RouteValue = _RouteValue;
            HtmlAttribute = _HtmlAttribute;
        }
        public string Pagetitle { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string RouteValue { get; set; }
        public string HtmlAttribute { get; set; }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }

}